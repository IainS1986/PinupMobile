// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PinupMobile.iOS.Views
{
	[Register ("RecordDisplayView")]
	partial class RecordDisplayView
	{
		[Outlet]
		UIKit.UIView BGView { get; set; }

		[Outlet]
		UIKit.UILabel DurationLabel { get; set; }

		[Outlet]
		UIKit.UILabel HelpLabel { get; set; }

		[Outlet]
		UIKit.UIButton RecordButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (BGView != null) {
				BGView.Dispose ();
				BGView = null;
			}

			if (DurationLabel != null) {
				DurationLabel.Dispose ();
				DurationLabel = null;
			}

			if (HelpLabel != null) {
				HelpLabel.Dispose ();
				HelpLabel = null;
			}

			if (RecordButton != null) {
				RecordButton.Dispose ();
				RecordButton = null;
			}
		}
	}
}
