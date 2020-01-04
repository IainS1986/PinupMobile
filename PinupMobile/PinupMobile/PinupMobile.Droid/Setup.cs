using Android.Content;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MvvmCross;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Presenters;
using PinupMobile.Core;
using PinupMobile.Core.Alerts;
using PinupMobile.Core.Settings;
using PinupMobile.Droid.Settings;
using PinupMobile.Droid.Alerts;
using PinupMobile.Droid.Controls;
using PinupMobile.Core.Strings;

namespace PinupMobile.Droid
{
    public class Setup : MvxAndroidSetup<App>
    {      
        protected override void InitializeIoC()
        {
            base.InitializeIoC();

            Mvx.LazyConstructAndRegisterSingleton<ILocalisation>(() => new Localisation(typeof(Resource.String)));
            Mvx.LazyConstructAndRegisterSingleton<IDialog, Dialog>();
            Mvx.LazyConstructAndRegisterSingleton<IUserSettings, UserSettings>();
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);
            MvxAppCompatSetupHelper.FillTargetFactories(registry);
        }

        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            //AppCenter.Start("YOUR KEY", typeof(Analytics), typeof(Crashes));
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter() => new MvxAppCompatViewPresenter(AndroidViewAssemblies);

        //protected override IMvxAndroidViewPresenter CreateViewPresenter() => new CustomPresenter(AndroidViewAssemblies);
    }
}
