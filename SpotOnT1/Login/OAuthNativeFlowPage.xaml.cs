using System;
using System.Collections.Generic;
using Xamarin.Auth;
using Xamarin.Forms;
using Refit;
using System.Net.Http;
using SpotOnT1;
using Autofac;
using System.Diagnostics;
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
                Debug.WriteLine("Logged in. Navigationg...");
                Application.Current.MainPage = new Views.MainPage();
                //await Navigation.PushAsync(new Views.MainPage());
            };
        }

       
    }
}
