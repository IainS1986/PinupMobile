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
	[Register ("DisplayView")]
	partial class DisplayView
	{
		[Outlet]
		UIKit.UIActivityIndicatorView LoadingSpinner { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (LoadingSpinner != null) {
				LoadingSpinner.Dispose ();
				LoadingSpinner = null;
			}
		}
	}
}
