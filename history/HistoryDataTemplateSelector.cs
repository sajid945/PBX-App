using System.Windows;
using System.Windows.Controls;
using RevePbx.modules.dataModel.history;
using RevePbx.modules.@enum;
using RevePbx.modules.services;

namespace RevePbx.modules.history
{
    class HistoryDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ContactHistoryDataTemplate { get; set; }
        public DataTemplate GroupHistoryDataTemplate { get; set; }
        public DataTemplate CallHistoryDataTemplate { get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            HistoryContainerDataModel historyContainerDataModel = item as HistoryContainerDataModel;
            if (historyContainerDataModel == null) return ContactHistoryDataTemplate;

            HistoryDataModel historyDataModel = ServiceLocator.historyContainerService.GetLastHistoryByUserIdOrGroupId(
                historyContainerDataModel.UserIdOrGroupId);
            if (historyDataModel == null) return ContactHistoryDataTemplate;

            switch (historyDataModel.Category)
            {
                case HistoryCategory.Call:
                    return CallHistoryDataTemplate;
                case HistoryCategory.IM:
                {
                    if (historyContainerDataModel.IsGroup)
                        return GroupHistoryDataTemplate;
                    else
                        return ContactHistoryDataTemplate;
                }
                default:
                    return ContactHistoryDataTemplate;
            }
        }
    }
}