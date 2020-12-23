using ExpressBase.Mobile.CustomControls;
using ExpressBase.Mobile.iOS.CustomRenderer;
using System.Drawing;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(EbXFrame), typeof(EbXFrameRederer))]

namespace ExpressBase.Mobile.iOS.CustomRenderer
{
    public class EbXFrameRederer : FrameRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Frame> element)
        {
            base.OnElementChanged(element);

            var elem = (EbXFrame)Element;
        }
    }
}