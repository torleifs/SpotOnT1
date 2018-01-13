using Xamarin.Forms;
using Autofac;
using Refit;
using SpotOnT1.ViewModels;
using SpotOnT1.Login;
using System.Reflection;
using System;

namespace SpotOnT1
{
    public partial class App : Application
    {
        public static IContainer Container { get; set; }

        public App()
        {
            InitializeComponent();
        }

        protected override async void OnStart()
        {
            ViewmodelLocator.RegisterDependencies();

            var loginService = ViewmodelLocator.Resolve<ILoginService>();

            bool isLoggedIn = loginService.IsLoggedIn;
            

          
            if (isLoggedIn)
            {
                var rootPage = new Views.UserView();
                var navWrapper = new NavigationPage(rootPage);
                MainPage = navWrapper;
            }
            else
            {
                var rootPage = new Login.OAuthNativeFlowPage();
                var navWrapper = new NavigationPage(rootPage);
                MainPage = navWrapper;
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

      
        private void initializeServices() {
            
        }
    }
}
