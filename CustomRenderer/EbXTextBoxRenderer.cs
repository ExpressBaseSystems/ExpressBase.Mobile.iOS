using System.ComponentModel;
using ExpressBase.Mobile.CustomControls;
using ExpressBase.Mobile.iOS.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(EbXTextBox), typeof(EbXTextBoxRenderer))]

namespace ExpressBase.Mobile.iOS.CustomRenderer
{
    public class EbXTextBoxRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }
    }
}