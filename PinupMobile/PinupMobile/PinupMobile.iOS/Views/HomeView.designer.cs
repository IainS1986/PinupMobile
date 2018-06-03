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
	[Register ("HomeView")]
	partial class HomeView
	{
		[Outlet]
		UIKit.UILabel CurrentItemName { get; set; }

		[Outlet]
		UIKit.UIButton NextButton { get; set; }

		[Outlet]
		UIKit.UIButton NextPageButton { get; set; }

		[Outlet]
		UIKit.UIButton PrevButton { get; set; }

		[Outlet]
		UIKit.UIButton PrevPageButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (CurrentItemName != null) {
				CurrentItemName.Dispose ();
				CurrentItemName = null;
			}

			if (NextButton != null) {
				NextButton.Dispose ();
				NextButton = null;
			}

			if (PrevButton != null) {
				PrevButton.Dispose ();
				PrevButton = null;
			}

			if (PrevPageButton != null) {
				PrevPageButton.Dispose ();
				PrevPageButton = null;
			}

			if (NextPageButton != null) {
				NextPageButton.Dispose ();
				NextPageButton = null;
			}
		}
	}
}
