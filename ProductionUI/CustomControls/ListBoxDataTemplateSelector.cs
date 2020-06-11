using Microsoft.VisualBasic;
using ProductionUI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ProductionUI.CustomControls
{
    public class ListBoxDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ListItemFocus { get; set; }
        public DataTemplate ListItemRightOfFocus { get; set; }
        public DataTemplate ListItemLeftOfFocus { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if(item == null)
            {
                return null;
            }

            var card = item as ProductionCardModel;
            
            if(card.IsActiveStep)
            {
                return ListItemFocus;
            }
            else if(card.StepPassed)
            {
                return ListItemLeftOfFocus;
            }

            return ListItemRightOfFocus;

        }

    }
}
