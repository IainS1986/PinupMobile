using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PinupMobile.Core.Converters;
using PinupMobile.Core.ViewModels;
using UIKit;

namespace PinupMobile.iOS.Views
{
    public partial class HomeView : MvxViewController<HomeViewModel>
    {

        public HomeView() : base("HomeView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<HomeView, HomeViewModel>();
            set.Bind(CurrentItemName).For(v => v.Text).To(vm => vm.CurrentItem).WithConversion<CurrentItemDisplayNameConverter>();
            set.Bind(PrevButton).To(vm => vm.OnGamePrevCommand);
            set.Bind(NextButton).To(vm => vm.OnGameNextCommand);
            set.Bind(PrevPageButton).To(vm => vm.OnPagePrevCommand);
            set.Bind(NextPageButton).To(vm => vm.OnPageNextCommand);
            set.Bind(DisplayButton).To(vm => vm.OnShowDisplayViewCommand);
            set.Apply();
        }
    }
}
