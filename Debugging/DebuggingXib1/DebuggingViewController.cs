using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Debugging {
    public partial class DebuggingViewController : UIViewController {
        public DebuggingViewController() : base ("DebuggingViewController", null) { }

        int count;
        void UpdateCountLabel() {
            countLabel.Text = string.Format("{0} clicks", count);
        }
        UIButton decrementButton;

        public override void ViewDidLoad() {
            base.ViewDidLoad();

            // * Demo starts with XIB layout of incrementButton.

            incrementButton.TouchUpInside += (object sender, EventArgs e) => {
                count += 1;
                UpdateCountLabel();
            };

            // * Follow-up shows code-based layout of decrementButton.

            incrementButton.Frame = new RectangleF(incrementButton.Frame.Left + (incrementButton.Frame.Width / 2), incrementButton.Frame.Top, incrementButton.Frame.Width, incrementButton.Frame.Height);
            decrementButton = new UIButton(UIButtonType.RoundedRect) {
                Frame = new RectangleF(incrementButton.Frame.Left - incrementButton.Frame.Width, incrementButton.Frame.Top, incrementButton.Frame.Width, incrementButton.Frame.Height),
            };
            decrementButton.SetTitle("-", UIControlState.Normal);
            decrementButton.TouchUpInside += (object sender, EventArgs e) => {
                count -= 1;
                UpdateCountLabel();
            };
            View.Add(decrementButton);
        }
    }
}

