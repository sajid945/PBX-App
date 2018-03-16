using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using RevePbx.modules.core;
using RevePbx.modules.dataModel.history;
using RevePbx.modules.services;
using RevePbx.modules.userControls.searchTextbox;

namespace RevePbx.modules.history
{
    /// <summary>
    /// Interaction logic for HistoryListUserControl.xaml
    /// </summary>
    public partial class HistoryListUserControl : BaseUserControl
    {
        #region events

        public event EventHandler<HistoryContainerDataModel> HistoryListSelectionChangedEvent;

        #endregion


        #region variables

        private bool _isLoaded = false;

        #endregion


        #region dependency

        [Dependency]
        public HistoryContainerService historyContainerService { get; set; }

        #endregion


        public HistoryListUserControl()
        {
            InitializeComponent();
            this.Loaded += HistoryListUserControl_Loaded;
        }

        private void HistoryListUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Initialize();
        }

        public void Initialize()
        {
            if (_isLoaded) return;

            LoadHistoryListViewModel();
            SetupAndAddSearchTextbox();

            _isLoaded = true;
        }

        private void LoadHistoryListViewModel()
        {
            HistoryListViewModel historyListViewModel = this.ViewModel as HistoryListViewModel;
            if (historyListViewModel == null) return;
            historyListViewModel.LoadData();
        }

        private void SetupAndAddSearchTextbox()
        {
            GridSearchTextbox.Children.Clear();

            SearchTextboxUserControl searchTextboxUserControl =
                this.ComponentLoaderInstance.GetUserControl(typeof (SearchTextboxUserControl)) as
                    SearchTextboxUserControl;
            if (searchTextboxUserControl == null) return;

            SearchTextboxViewModel searchTextboxViewModel = searchTextboxUserControl.ViewModel as SearchTextboxViewModel;
            if (searchTextboxViewModel == null) return;

            searchTextboxViewModel.SearchedTextChangedDelayedEvent +=
                SearchTextboxViewModel_SearchedTextChangedDelayedEvent;

            GridSearchTextbox.Children.Add(searchTextboxUserControl);
        }

        private void HistoryList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HistoryContainerDataModel selectedHistoryContainer = HistoryList.SelectedItem as HistoryContainerDataModel;
            if (selectedHistoryContainer == null) return;

            if (HistoryListSelectionChangedEvent == null) return;
            HistoryListSelectionChangedEvent(sender, selectedHistoryContainer);
        }

        private void SearchTextboxViewModel_SearchedTextChangedDelayedEvent(object sender, string searchedText)
        {
            HistoryListViewModel historyListViewModel = this.ViewModel as HistoryListViewModel;
            if (historyListViewModel == null) return;
            historyListViewModel.SearchHistory(searchedText);
        }
    }
}