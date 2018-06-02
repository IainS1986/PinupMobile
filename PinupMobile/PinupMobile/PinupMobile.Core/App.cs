using MvvmCross.IoC;
using MvvmCross.ViewModels;
using PinupMobile.Core.ViewModels;

namespace PinupMobile.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Setup.Register();

            RegisterAppStart<AppStartupViewModel>();
        }
    }
}
