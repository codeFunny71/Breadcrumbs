using Java.Lang;
using Plugin.Geolocator;
using System;
using System.ComponentModel;
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
        public MainPage()
        {
            Console.WriteLine("Beginning of main page");

            InitializeComponent();

            Console.WriteLine("pre-location");
            Position currentPosition = getCurrentLocation();
            Console.WriteLine("post-location");
            Console.WriteLine("Position: {0} - {1}", currentPosition.Latitude, currentPosition.Longitude);
            MainMap.MoveToRegion(MapSpan.FromCenterAndRadius(currentPosition, Distance.FromMiles(1)));
        }

        private Position getCurrentLocation()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                Console.WriteLine("pre-Async");
                var position = locator.GetPositionAsync(TimeSpan.FromSeconds(10)).Result;
                Console.WriteLine("post-Async");

                //var request = new GeolocationRequest(GeolocationAccuracy.Best);
                //Location location = Geolocation.GetLocationAsync(request).Result;
                Position currentPosition = new Position(position.Latitude, position.Longitude);

                return currentPosition;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                Console.WriteLine("Unsupported");
                return new Position(0, 0);
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                Console.WriteLine("not enabled");
                return new Position(0, 0);
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                Console.WriteLine("not permitted");
                return new Position(0, 0);
            }
            catch(SecurityException secEx)
            {
                Console.WriteLine("security----------------------------------------------------------------------");
                return new Position(0, 0);
            }
            catch (System.Exception ex)
            {
                // Unable to get location
                Console.WriteLine("Hi---------------------------------- \n{0}", ex.StackTrace);
                return new Position(0, 0);
            }
        }
    }
}
