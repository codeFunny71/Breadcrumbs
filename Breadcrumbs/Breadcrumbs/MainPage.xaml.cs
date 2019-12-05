using Java.Lang;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Breadcrumbs
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private Xamarin.Forms.Maps.Position position;

        public MainPage()
        {
            InitializeComponent();

            MainMap.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromMiles(1)));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            IGeolocator locator = CrossGeolocator.Current;

            Task.Run(async () =>
            {
                try
                {
                    Plugin.Geolocator.Abstractions.Position currentPosition = await locator.GetLastKnownLocationAsync();

                    if (currentPosition == null)
                    {
                        currentPosition = await locator.GetPositionAsync(TimeSpan.FromSeconds(5));
                    }
                    else
                    {
                        Console.WriteLine("initial Cords: {0} -- {1}", currentPosition.Latitude, currentPosition.Longitude);
                    }

                    Console.WriteLine("final Cords: {0} -- {1}", currentPosition.Latitude, currentPosition.Longitude);
                    this.position = new Xamarin.Forms.Maps.Position(currentPosition.Latitude, currentPosition.Longitude);
                }
                catch (FeatureNotSupportedException fnsEx)
                {
                    // Handle not supported on device exception
                    Console.WriteLine("Unsupported");
                }
                catch (FeatureNotEnabledException fneEx)
                {
                    // Handle not enabled on device exception
                    Console.WriteLine("not enabled");
                }
                catch (PermissionException pEx)
                {
                    // Handle permission exception
                    Console.WriteLine("not permitted");
                }
                catch (SecurityException secEx)
                {
                    Console.WriteLine("security----------------------------------------------------------------------");
                }
                catch (System.Exception ex)
                {
                    // Unable to get location
                    Console.WriteLine("Hi---------------------------------- \n{0}", ex.StackTrace);
                }
            });
        }
    }
}
