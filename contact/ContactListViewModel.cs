using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Unity;
using RevePbx.modules.core;
using RevePbx.modules.dataModel.buddy;
using RevePbx.modules.services;
using RevePbx.modules.smartCollection;

namespace RevePbx.modules.contact
{
    class ContactListViewModel : BaseUserControlViewModel
    {
        #region dependency

        [Dependency]
        public ProfileService profileService { get; set; }

        #endregion


        #region members

        private readonly BuddyService _buddyService;

        private readonly List<ImBuddyDataModel> _fullContactList =
            new List<ImBuddyDataModel>();

        private SmartCollection<ImBuddyDataModel> _contactList =
            new SmartCollection<ImBuddyDataModel>();

        #endregion


        #region properties

        public SmartCollection<ImBuddyDataModel> ContactList
        {
            get { return _contactList; }
            set { _contactList = value; }
        }

        #endregion


        [InjectionConstructor]
        public ContactListViewModel(BuddyService buddyService)
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
            var buddies = await Task.Run<List<ImBuddyDataModel>>(() => _buddyService.GetBuddies());

            await Task.Run(() =>
            {
                bool isOwnProfileRemoved = false;
                _fullContactList.Clear();
                foreach (var item in buddies)
                {
                    if (!isOwnProfileRemoved && profileService.GetProfile().UserId.Equals(item.UserId))
                    {
                        isOwnProfileRemoved = true;
                        continue;
                    }
                    _fullContactList.Add(item);
                }

                ContactList.ResetGradually(_fullContactList);
            });
        }

        internal async void SearchContact(string searchedText)
        {
            await Task.Run(() =>
            {
                string searchedTextLowered = searchedText.ToLower();

                var searchedContactList = new List<ImBuddyDataModel>(
                    _fullContactList.Where(
                        item => this.IsItemShowable(item, searchedTextLowered)));

                Application.Current.Dispatcher.Invoke(() => { ContactList.Reset(searchedContactList); });
            });
        }

        private bool IsItemShowable(ImBuddyDataModel imBuddyDataModel, string searchedText)
        {
            if (string.IsNullOrEmpty(searchedText)) return true;

            if (imBuddyDataModel == null) return false;
            return (imBuddyDataModel.Name.ToLower().Contains(searchedText) ||
                    imBuddyDataModel.Number.Contains(searchedText));
        }
    }
}