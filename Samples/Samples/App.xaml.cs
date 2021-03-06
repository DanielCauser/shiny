﻿using System;
using Shiny;
using Acr.UserDialogs;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Prism;
using Prism.Autofac;
using Prism.Ioc;
using Prism.Mvvm;
using Samples.Jobs;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]


namespace Samples
{
    public partial class App : PrismApplication
    {
        public App() : base(null) { }
        public App(IPlatformInitializer initializer) : base(initializer) { }


        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<IUserDialogs>(UserDialogs.Instance);
            containerRegistry.RegisterForNavigation<NavigationPage>("Nav");
            containerRegistry.RegisterForNavigation<MainPage>("Main");
            containerRegistry.RegisterForNavigation<WelcomePage>("Welcome");
            containerRegistry.RegisterForNavigation<Gps.MainPage>("Gps");

            containerRegistry.RegisterForNavigation<Beacons.MainPage>("Beacons");
            containerRegistry.RegisterForNavigation<BluetoothLE.AdapterPage>("BleCentral");
            containerRegistry.RegisterForNavigation<BluetoothLE.PeripheralPage>("Peripheral");
            containerRegistry.RegisterForNavigation<BlePeripherals.MainPage>("BlePeripherals");

            containerRegistry.RegisterForNavigation<Geofencing.MainPage>("Geofencing");
            containerRegistry.RegisterForNavigation<HttpTransfers.MainPage>("HttpTransfers");
            containerRegistry.RegisterForNavigation<Jobs.MainPage>("Jobs");
            containerRegistry.RegisterForNavigation<Notifications.MainPage>("Notifications");
            containerRegistry.RegisterForNavigation<Sensors.MainPage>("Sensors");
            containerRegistry.RegisterForNavigation<Speech.MainPage>("SpeechRecognition");

            containerRegistry.RegisterForNavigation<AccessPage>("Access");
            containerRegistry.RegisterForNavigation<ErrorLogPage>("ErrorLogs");
            containerRegistry.RegisterForNavigation<FileSystemPage>("FileSystem");
            containerRegistry.RegisterForNavigation<EnvironmentPage>("Environment");
            containerRegistry.RegisterForNavigation<Settings.MainPage>("Settings");
        }


        protected override async void OnInitialized()
        {
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(viewType =>
            {
                var viewModelTypeName = viewType.FullName.Replace("Page", "ViewModel");
                var viewModelType = Type.GetType(viewModelTypeName);
                return viewModelType;
            });
            await this.NavigationService.NavigateAsync("Main/Nav/Welcome");
        }


        protected override IContainerExtension CreateContainerExtension()
        {
            var builder = new ContainerBuilder();
            builder.Populate(ShinyHost.Services);
            builder.RegisterType<GlobalExceptionHandler>().As<IStartable>().AutoActivate().SingleInstance();
            return new AutofacContainerExtension(builder);
        }
    }
}
