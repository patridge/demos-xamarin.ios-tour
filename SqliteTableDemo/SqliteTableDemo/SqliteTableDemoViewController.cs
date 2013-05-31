using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MBProgressHUD;
using System.Threading;
using System.Threading.Tasks;

namespace SqliteTableDemo {
    public partial class SqliteTableDemoViewController : DialogViewController {
        MarkerInfoDatabase _Database;

        Section _MainSection;
        public SqliteTableDemoViewController(MarkerInfoDatabase database) : base(null, true) {
            _Database = database;
            Style = UITableViewStyle.Plain;
        }

        MapViewController mapController;
        public void RefreshMarkers() {
            _Database.GetAllMarkersAsync().ContinueWith(task => {
                InvokeOnMainThread(() => {
                    List<MarkerInfo> markerData = task.Result.OrderBy(mi => mi.Name).ToList();
                    _MainSection.Clear();
                    _MainSection.AddAll(markerData.Select(markerInfo => {
                        MarkerInfo currentMarkerInfo = markerInfo;
                        StyledStringElement markerNameElement = new StyledStringElement(currentMarkerInfo.Name, () => {
                            InvokeOnMainThread(() => {
                                mapController.SetCameraPosition(currentMarkerInfo.Latitude, currentMarkerInfo.Longitude);
                                NavigationController.PushViewController(mapController, animated: true);
                            });
                        });
                        markerNameElement.Accessory = UITableViewCellAccessory.DisclosureIndicator;
                        return markerNameElement as Element;
                    }));
                    ReloadData();
                    mapController.RefreshMarkers(markerData);
                });
            });
        }
        public override void ViewDidLoad() {
            base.ViewDidLoad();
            
            _MainSection = new Section() {
                new ActivityElement()
            };
            Root = new RootElement("Markers") {
                _MainSection
            };

            // Simple demo of a progress HUD. Doesn't actually do anything useful.
            // Added after question from audience.
            MTMBProgressHUD progress = new MTMBProgressHUD() {
                DimBackground = true,
                LabelText = "Doing something.",
            };
            View.Add(progress);
            progress.Show(animated: true);
            Task.Factory.StartNew(() => {
                Thread.Sleep(2000);
                InvokeOnMainThread(() => {
                    progress.Hide(animated: true);
                });
            });

            mapController = new MapViewController();
            mapController.NewMarkerCreated += (object sender, MarkerAddedEventArgs e) => {
                InvokeOnMainThread(() => {
                    _Database.SaveMarker(e.MarkerInfo);
                    RefreshMarkers();
                });
            };
            RefreshMarkers();
        }
    }
}