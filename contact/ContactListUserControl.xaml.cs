using System;
using System.Windows;
using System.Windows.Controls;
using RevePbx.modules.core;
using RevePbx.modules.dataModel.buddy;
using RevePbx.modules.userControls.searchTextbox;

namespace RevePbx.modules.contact
{
    /// <summary>
    /// Interaction logic for ContactListUserControl.xaml
    /// </summary>
    public partial class ContactListUserControl : BaseUserControl
    {
        public event EventHandler<ImBuddyDataModel> ContactListSelectionChangedEvent;

        private bool _isLoaded = false;

        public ContactListUserControl()
        {
            InitializeComponent();
            this.Loaded += ContactListUserControl_Loaded;
        }

        private void ContactListUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Initialize();
        }

        public void Initialize()
        {
            if (_isLoaded) return;

            LoadContactListData();
            SetupAndAddSearchTextbox();

            _isLoaded = true;
        }

        private void LoadContactListData()
        {
            ContactListViewModel contactListViewModel = this.ViewModel as ContactListViewModel;
            if (contactListViewModel == null) return;

            contactListViewModel.LoadData();
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

        private void ContactList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ImBuddyDataModel selectedContact = ContactList.SelectedItem as ImBuddyDataModel;
            if (selectedContact == null) return;

            if (ContactListSelectionChangedEvent == null) return;
            ContactListSelectionChangedEvent(sender, selectedContact);
        }

        private void SearchTextboxViewModel_SearchedTextChangedDelayedEvent(object sender, string searchedText)
        {
            ContactListViewModel contactListViewModel = this.ViewModel as ContactListViewModel;
            if (contactListViewModel == null) return;

            contactListViewModel.SearchContact(searchedText);
        }
    }
}