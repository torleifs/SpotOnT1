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

        protected override void OnStart()
        {

            Container = CreateContainer();
            if (Application.Current.Resources == null) {
                Application.Current.Resources = new ResourceDictionary();
            }
            this.Resources["Locator"] = new Locator(property => Container.Resolve(Type.GetType($"SpotOnT1.ViewModels.{property}")));
            var rootPage = new Login.OAuthNativeFlowPage();
            var navWrapper = new NavigationPage(rootPage);
            MainPage = navWrapper;
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private IContainer CreateContainer() {
            var builder = new ContainerBuilder();
            builder.Register(c =>
            {
                var spotifyApi = RestService.For<ISpotifyClient>("https://api.spotify.com");
                return spotifyApi;
            }).As<ISpotifyClient>().SingleInstance();
            builder.RegisterType<UserViewModel>();
           // builder.RegisterAssemblyTypes(typeof(App).GetTypeInfo().Assembly).InNamespace("SpotOnT1.ViewModels");
            builder.RegisterType<LoginService>().As<ILoginService>().SingleInstance();
            return builder.Build();
        }
    }
}
