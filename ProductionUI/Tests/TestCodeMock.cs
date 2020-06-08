using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionUI
{
    class TestCodeMock
    {

        //TODO:
        /*
         *
         * pull in the random time length code from Ryan and utilize it to mock path running - iterate for(size of collection)
         * 
         * 
         * due: July 1
         * 
         * functional playback 
         * multithreaded timers
         * MVVM relationship clarification and clearer separation of concerns
         * datatemplates for left / right / selected
         * -- clean up usercontrol to be replaced by datatemplate
         X move path image to separate control
         X change the card layout to match original mock
         * 
         * A few things that are mostly just notes to help you make your code look closer to what we've been doing in the codebase thus far:
         * As a general convention, x:Key and x:Name declarations in XAML should have the type they are referencing in them "3DListBoxStyle" for a Style key, "CardListBox" for a Listbox name, etc.
         * If a model (ProductionCardDeckModel) basically has nothing in it you can just get rid of it. It's perfectly fine to have no model for a View/ViewModel pair, and in fact that tends to happen more often than not.
         * Get in the habit of creating an interface for all your Views and ViewModels as well. You'll need those for registration later when doing injection.
         * 
         * later:
         * prism integration
         * telerik controls
         */

    }
}
