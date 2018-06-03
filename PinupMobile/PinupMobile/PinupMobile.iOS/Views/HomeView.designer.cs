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
		
		void ReleaseDesignerOutlets ()
		{
			if (CurrentItemName != null) {
				CurrentItemName.Dispose ();
				CurrentItemName = null;
			}
		}
	}
}
