using System;
using System.Drawing;
using System.Linq;
using Google.Maps;
using MonoTouch.CoreLocation;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;

namespace SqliteTableDemo {
    public class MarkerAddedEventArgs : EventArgs {
        public MarkerInfo MarkerInfo { get; set; }
        public MarkerAddedEventArgs(MarkerInfo markerInfo) {
            MarkerInfo = markerInfo;
        }
    }
    public class MappedMarkerInfo : Marker {
        public MarkerInfo MarkerInfo { get; set; }
        public MappedMarkerInfo(MarkerInfo markerInfo) {
            MarkerInfo = markerInfo;
            Position = new CLLocationCoordinate2D(MarkerInfo.Latitude, MarkerInfo.Longitude);
            Title = MarkerInfo.Name;
        }
    }
    public class MapViewController : UIViewController {
        public event EventHandler<MarkerAddedEventArgs> NewMarkerCreated = delegate { };
        MapView _MapView;
        double cameraLatitude;
        double cameraLongitude;

        public void SetCameraPosition(double latitude, double longitude) {
            cameraLatitude = latitude;
            cameraLongitude = longitude;
        }
        public void RefreshMarkers(List<MarkerInfo> markers) {
            _Markers.ForEach(mmi => {
                mmi.Map = null;
            });
            _Markers = markers.Select(m => {
                MappedMarkerInfo locationMarker = new MappedMarkerInfo(m);
                locationMarker.Map = _MapView;
                return locationMarker;
            }).ToList();
        }

        List<MappedMarkerInfo> _Markers = new List<MappedMarkerInfo>();
        public MapViewController() { }

        public override void LoadView() {
            base.LoadView();

            _MapView = new MapView() {
                MyLocationEnabled = true,
            };
            _Markers.ForEach(mmi => mmi.Map = _MapView);
            _MapView.Settings.RotateGestures = false;
            _MapView.Tapped += (object tapSender, GMSCoordEventArgs coordEventArgs) => {
                UIAlertView nameInputAlertView = new UIAlertView("Add New Marker", "", null, "Cancel", new[] { "Add" }) {
                    AlertViewStyle = UIAlertViewStyle.PlainTextInput,
                };
                UITextField nameTextField = nameInputAlertView.GetTextField(0);
                nameTextField.Placeholder = "Marker name";
                nameTextField.BecomeFirstResponder();
                nameInputAlertView.Dismissed += (sender, e) => {
                    if (e.ButtonIndex != nameInputAlertView.CancelButtonIndex) {
                        MarkerInfo newMarkerInfo = new MarkerInfo() {
                            Latitude = coordEventArgs.Coordinate.Latitude,
                            Longitude = coordEventArgs.Coordinate.Longitude,
                            Name = nameTextField.Text,
                        };
                        NewMarkerCreated(this, new MarkerAddedEventArgs(newMarkerInfo));
                    }
                };
                nameInputAlertView.Show();
            };
            View = _MapView;
        }

        public override void ViewWillAppear(bool animated) {
            base.ViewWillAppear(animated);

            var camera = CameraPosition.FromCamera(latitude: cameraLatitude,
                                                   longitude: cameraLongitude,
                                                   zoom: 12);
            _MapView.Camera = camera;
            _MapView.StartRendering();
        }

        public override void ViewWillDisappear(bool animated) {   
            _MapView.StopRendering();
            base.ViewWillDisappear(animated);
        }
    }
}