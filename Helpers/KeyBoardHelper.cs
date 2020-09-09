using ExpressBase.Mobile.Helpers;
using ExpressBase.Mobile.iOS.Helpers;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(KeyBoardHelper))]

namespace ExpressBase.Mobile.iOS.Helpers
{
    public class KeyBoardHelper : IKeyboardHelper
    {
        public void HideKeyboard()
        {
            UIApplication.SharedApplication.KeyWindow.EndEditing(true);
        }
    }
}