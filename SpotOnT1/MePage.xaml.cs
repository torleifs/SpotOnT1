using System;
using System.Collections.Generic;
using SpotOnT1.ViewModels;
using Xamarin.Forms;
using Autofac;
namespace SpotOnT1
{
    public partial class MePage : ContentPage
    {
        public MePage()
        {
            InitializeComponent();
            BindingContext = App.Container.Resolve<SpotOnT1.ViewModels.UserViewModel>();

        }
     
    }
}
