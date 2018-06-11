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
	[Register ("SetupPopperView")]
	partial class SetupPopperView
	{
		[Outlet]
		UIKit.UIButton ConnectButton { get; set; }

		[Outlet]
		UIKit.UIActivityIndicatorView ConnectingSpinner { get; set; }

		[Outlet]
		UIKit.UILabel ErrorLabel { get; set; }

		[Outlet]
		UIKit.UILabel HeaderLabel { get; set; }

		[Outlet]
		UIKit.UITextView HelpText { get; set; }

		[Outlet]
		UIKit.UITextField UrlInput { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ConnectButton != null) {
				ConnectButton.Dispose ();
				ConnectButton = null;
			}

			if (ConnectingSpinner != null) {
				ConnectingSpinner.Dispose ();
				ConnectingSpinner = null;
			}

			if (ErrorLabel != null) {
				ErrorLabel.Dispose ();
				ErrorLabel = null;
			}

			if (HeaderLabel != null) {
				HeaderLabel.Dispose ();
				HeaderLabel = null;
			}

			if (UrlInput != null) {
				UrlInput.Dispose ();
				UrlInput = null;
			}

			if (HelpText != null) {
				HelpText.Dispose ();
				HelpText = null;
			}
		}
	}
}
