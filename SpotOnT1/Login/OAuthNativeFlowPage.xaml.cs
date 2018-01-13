using System;
using System.Collections.Generic;
using Xamarin.Auth;
using Xamarin.Forms;
using Refit;
using System.Net.Http;
using SpotOnT1;
using Autofac;
namespace SpotOnT1.Login
{
    public partial class OAuthNativeFlowPage : ContentPage
    {
        
        public OAuthNativeFlowPage()
        {
            InitializeComponent();
            loginButton.Clicked += async (sender, e) => {
                var loginService = ViewModels.ViewmodelLocator.Resolve<ILoginService>();
              
                    await loginService.Login();

                await Navigation.PushAsync(new Views.UserView());
            };
        }

       
    }
}
