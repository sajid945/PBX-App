using RevePbx.modules.core;
using RevePbx.modules.dataModel.profile;

namespace RevePbx.modules.profile
{
    class ProfileViewModel : BaseUserControlViewModel
    {
        #region variables

        private ProfileDataModel _selfInformation;

        #endregion


        #region properties

        public ProfileDataModel SelfInformation
        {
            get { return _selfInformation; }
            set
            {
                _selfInformation = value;
                OnPropertyChanged("SelfInformation");
            }
        }

        #endregion
    }
}