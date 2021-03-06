using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Shiny.Infrastructure;
using CoreLocation;
using UIKit;


namespace Shiny.Locations
{
    public class GeofenceManagerImpl : AbstractGeofenceManager
    {
        readonly CLLocationManager locationManager;
        readonly GeofenceManagerDelegate gdelegate;


        public GeofenceManagerImpl(IRepository repository) : base(repository)
        {
            this.gdelegate = new GeofenceManagerDelegate();
            this.locationManager = new CLLocationManager { Delegate = this.gdelegate };
        }


        public override Task<AccessState> RequestAccess() => this.locationManager.RequestGeofenceAccess();
        public override AccessState Status => this.locationManager.CurrentAccessStatus();


        public override async Task<GeofenceState> RequestState(GeofenceRegion region, CancellationToken cancelToken = default)
        {
            var task = Observable
                .FromEventPattern<CLRegionStateDeterminedEventArgs>(
                    x => this.gdelegate.DeterminedState += x,
                    x => this.gdelegate.DeterminedState -= x
                )
                .Timeout(TimeSpan.FromSeconds(20))
                .Take(1)
                .Select(x => x.EventArgs)
                .Where(x => x.Region is CLRegion native && native.Identifier.Equals(region.Identifier))
                .ToTask(cancelToken);

            this.locationManager.RequestState(region.ToNative());
            var args = await task;
            return args.State.FromNative();
        }


        public override async Task StartMonitoring(GeofenceRegion region)
        {
            var native = region.ToNative();
            var tcs = new TaskCompletionSource<object>();
            UIApplication.SharedApplication.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    this.locationManager.StartMonitoring(native);
                    tcs.SetResult(null);
                }
                catch (Exception ex)
                {
                    this.locationManager.StopMonitoring(native);
                    tcs.SetException(ex);
                }
            });
            await tcs.Task.ConfigureAwait(false);
            await this.Repository.Set(region.Identifier, region);
        }


        public override async Task StopMonitoring(GeofenceRegion region)
        {
            await this.Repository.Remove(region.Identifier);
            this.locationManager.StopMonitoring(region.ToNative());
        }


        public override async Task StopAllMonitoring()
        {
            await this.Repository.Clear();
            var natives = this
                .locationManager
                .MonitoredRegions
                .OfType<CLCircularRegion>()
                .ToList();

            foreach (var native in natives)
                this.locationManager.StopMonitoring(native);
        }
    }
}