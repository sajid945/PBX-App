using System.Windows;
using Microsoft.Practices.Unity;
using Microsoft.Win32;
using RevePbx.modules.core;
using RevePbx.modules.services;

namespace RevePbx.modules.profile
{
    /// <summary>
    /// Interaction logic for ProfileUserControl.xaml
    /// </summary>
    public partial class ProfileUserControl : BaseUserControl
    {
        #region constants

        private const string EditButtonText = "Edit";
        private const string UpdateButtonText = "Update";

        #endregion


        #region members

        private string _profilePictureUploadPath;

        private bool _isUpdateShown = false;

        #endregion


        #region Dependency

        [Dependency]
        public ProfileService profileService { get; set; }

        #endregion


        public ProfileUserControl()
        {
            InitializeComponent();
            this.Loaded += ProfileUserControl_Loaded;
        }

        private void ProfileUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ProfileViewModel profileViewModel = this.ViewModel as ProfileViewModel;
            if (profileViewModel == null) return;

            profileViewModel.SelfInformation = profileService.GetProfile();
        }


        private void PhotoUploadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                _profilePictureUploadPath = openFileDialog.FileName;
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isUpdateShown)
            {
                UpdateProfile();
                EditButton.Content = EditButtonText;
            }
            else
            {
                EditProfile();
                EditButton.Content = UpdateButtonText;
            }

            _isUpdateShown = !_isUpdateShown;
        }


        private void UpdateProfile()
        {
            ProfileViewModel profileViewModel = this.ViewModel as ProfileViewModel;
            if (profileViewModel == null) return;

            if (!string.IsNullOrEmpty(_profilePictureUploadPath))
            {
                profileService.UploadProfilePhoto(_profilePictureUploadPath);
                _profilePictureUploadPath = string.Empty;
            }

            profileViewModel.SelfInformation.StatusNote = StatusBox.Text;
            profileService.UpdateProfile();

            StatusBlock.Visibility = Visibility.Visible;
            StatusBlock.Text = StatusBox.Text;
            StatusBox.Visibility = Visibility.Collapsed;
            //AvailabilityComboBox.Visibility = Visibility.Collapsed;
            PhotoUploadButton.Visibility = Visibility.Collapsed;
        }

        private void EditProfile()
        {
            StatusBlock.Visibility = Visibility.Collapsed;
            StatusBox.Visibility = Visibility.Visible;
            //AvailabilityComboBox.Visibility = Visibility.Visible;
            PhotoUploadButton.Visibility = Visibility.Visible;
        }
    }
}