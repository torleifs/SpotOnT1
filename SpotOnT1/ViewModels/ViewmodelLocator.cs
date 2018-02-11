using Autofac;
using Refit;
using Xamarin.Forms;
using SpotOnT1.Login;
using System;
using System.Globalization;
using System.Reflection;
using System.Diagnostics;

namespace SpotOnT1.ViewModels
{
    public class ViewmodelLocator
    {
        private static IContainer _container;

        public static readonly BindableProperty AutowireViewModelProperty =
            BindableProperty.CreateAttached("AutowireViewModel", typeof(bool), typeof(ViewmodelLocator), 
                                            default(bool), propertyChanged: OnAutoWireViewModelChanged);

        public static bool GetAutowireViewModel(BindableObject bindable)
        {
            return (bool)bindable.GetValue(ViewmodelLocator.AutowireViewModelProperty);
        }

        public static void SetAutowireViewModel(BindableObject bindable, bool value) 
        {
            bindable.SetValue(ViewmodelLocator.AutowireViewModelProperty, value);
        }

        public static void RegisterDependencies() {
            var builder = new ContainerBuilder();
            builder.RegisterType<UserViewModel>();
            builder.RegisterType<PlaylistOverviewViewModel>();
            builder.Register(c =>
            {
                var spotifyApi = RestService.For<ISpotifyClient>("https://api.spotify.com");
                return spotifyApi;
            }).As<ISpotifyClient>().SingleInstance();
            builder.RegisterType<LoginService>().As<ILoginService>().SingleInstance();
            builder.RegisterType<AppInitializer>().SingleInstance();
            if (_container != null) {
                _container.Dispose();
            }
            _container = builder.Build();
        }

        public static T Resolve<T>() {
            return _container.Resolve<T>();
        }

        private static void OnAutoWireViewModelChanged(BindableObject bindable, object oldValue, object newValue) {
            
            var view = bindable as Element;
            Debug.WriteLine("Resolving view for " + view.GetType());
            if (view == null ) {
                return;
            }
            var viewType = view.GetType();
            var viewName = viewType.FullName.Replace(".Views.", ".ViewModels.");
            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}Model, {1}", viewName, viewAssemblyName);
            Debug.WriteLine("Trying to resolve to viewmodel: " + viewModelName);
            var viewModelType = Type.GetType(viewModelName);
            if (viewModelType == null) {
                return;
            }
            var viewModel = _container.Resolve(viewModelType);
            view.BindingContext = viewModel;
            Debug.WriteLine("Bound view " + nameof(viewType) + " to " + nameof(viewModel));
        }
    }
}
