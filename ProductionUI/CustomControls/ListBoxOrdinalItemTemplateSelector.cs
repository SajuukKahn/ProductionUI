using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ProductionUI.CustomControls
{
    public class ListBoxOrdinalItemTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {

            ListBoxItem listBoxItem = item as ListBoxItem;
            ListView listView = container as ListView;

            var currentSelection = listView.SelectedItem;

            foreach (var listItem in listView.Items)
            {
                if(currentSelection == listItem)
                {

                }
            }
            
            var alternationCount = listView.AlternationCount;

            return null;

        }

    }
}
