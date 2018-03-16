using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Unity;
using RevePbx.modules.core;
using RevePbx.modules.dataModel.buddy;
using RevePbx.modules.dataModel.@group;
using RevePbx.modules.dataModel.history;
using RevePbx.modules.services;
using RevePbx.modules.smartCollection;

namespace RevePbx.modules.history
{
    class HistoryListViewModel : BaseUserControlViewModel
    {
        #region events

        public event EventHandler<HistoryContainerDataModel> HistoryContainerUpdatedEvent;

        #endregion


        #region dependency

        [Dependency]
        public HistoryContainerService historyContainerService { get; set; }

        [Dependency]
        public HistoryService historyService { get; set; }

        [Dependency]
        public GroupService groupService { get; set; }

        #endregion


        #region members

        private readonly BuddyService _buddyService;

        private List<HistoryContainerDataModel> _fullHistoryContainerList
            = new List<HistoryContainerDataModel>();

        private SmartCollection<HistoryContainerDataModel> _historyContainerList
            = new SmartCollection<HistoryContainerDataModel>();

        private string _searchedText = string.Empty;

        #endregion


        #region properties

        public SmartCollection<HistoryContainerDataModel> HistoryContainerList
        {
            get { return _historyContainerList; }
            set { _historyContainerList = value; }
        }

        #endregion


        [InjectionConstructor]
        public HistoryListViewModel(BuddyService buddyService)
        {
            _buddyService = buddyService;
            buddyService.ContactFetchEvent -= BuddyService_ContactFetchEvent;
            buddyService.ContactFetchEvent += BuddyService_ContactFetchEvent;
        }

        private void BuddyService_ContactFetchEvent(object sender, List<ImBuddyDataModel> e)
        {
            LoadData();
        }

        internal async void LoadData()
        {
            var historyContainers =
                await Task.Run<List<HistoryContainerDataModel>>(() => historyContainerService.GetHistoryContainer());

            await Task.Run(() =>
            {
                _fullHistoryContainerList.Clear();
                _fullHistoryContainerList.AddRange(historyContainers);

                HistoryContainerList.ResetGradually(_fullHistoryContainerList);
            });

            historyContainerService.HistoryContainerAddedEvent -= HistoryContainerService_HistoryContainerAddedEvent;
            historyContainerService.HistoryContainerAddedEvent += HistoryContainerService_HistoryContainerAddedEvent;

            historyContainerService.HistoryContainerUpdatedForHistoryAddedEvent -=
                HistoryContainerService_HistoryContainerUpdatedForHistoryAddedEvent;
            historyContainerService.HistoryContainerUpdatedForHistoryAddedEvent +=
                HistoryContainerService_HistoryContainerUpdatedForHistoryAddedEvent;
        }

        internal async void SearchHistory(string searchedText)
        {
            _searchedText = searchedText;

            await Task.Run(() =>
            {
                string searchedTextLowered = searchedText.ToLower();

                var historyContainerList = new List<HistoryContainerDataModel>(
                    _fullHistoryContainerList.Where(
                        item => this.IsItemShowable(item, searchedTextLowered)));

                Application.Current.Dispatcher.Invoke(() => { HistoryContainerList.Reset(historyContainerList); });
            });
        }

        private bool IsItemShowable(HistoryContainerDataModel item, string searchedText)
        {
            if (string.IsNullOrEmpty(searchedText)) return true;

            if (item.IsGroup)
            {
                ImGroupDataModel imGroupDataModel = groupService.GetGroupByGroupId(item.GroupId);
                if (imGroupDataModel == null) return false;
                return imGroupDataModel.GroupName.ToLower().Contains(searchedText);
            }
            else
            {
                ImBuddyDataModel imBuddyDataModel = _buddyService.GetBuddyByUserId(item.UserId);
                if (imBuddyDataModel == null) return false;
                return (imBuddyDataModel.Name.ToLower().Contains(searchedText) ||
                        imBuddyDataModel.Number.Contains(searchedText));
            }
        }

        private void UpdateFullHistoryContainerList(HistoryContainerDataModel historyContainerDataModel)
        {
            int foundIndex =
                _fullHistoryContainerList.FindIndex(
                    item => (item.UserIdOrGroupId == historyContainerDataModel.UserIdOrGroupId));

            if (foundIndex == 0) return;

            if (foundIndex == -1)
            {
                _fullHistoryContainerList.Insert(0, historyContainerDataModel);
            }
            else
            {
                _fullHistoryContainerList.RemoveAt(foundIndex);
                _fullHistoryContainerList.Insert(0, historyContainerDataModel);
            }
        }

        private void UpdateAllForHistoryContainer(HistoryContainerDataModel historyContainerDataModel)
        {
            UpdateFullHistoryContainerList(historyContainerDataModel);
            SearchHistory(_searchedText);

            if (HistoryContainerUpdatedEvent == null) return;
            HistoryContainerUpdatedEvent(this, historyContainerDataModel);
        }

        private void HistoryContainerService_HistoryContainerAddedEvent(object sender,
            HistoryContainerDataModel historyContainerDataModel)
        {
            UpdateAllForHistoryContainer(historyContainerDataModel);
        }

        private void HistoryContainerService_HistoryContainerUpdatedForHistoryAddedEvent(object sender,
            HistoryContainerDataModel historyContainerDataModel)
        {
            UpdateAllForHistoryContainer(historyContainerDataModel);
        }
    }
}