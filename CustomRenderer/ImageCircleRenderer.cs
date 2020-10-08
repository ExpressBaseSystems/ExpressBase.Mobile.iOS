using System;
using System.ComponentModel;
using ExpressBase.Mobile.CustomControls;
using ExpressBase.Mobile.iOS.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ImageCircle), typeof(ImageCircleRenderer))]

namespace ExpressBase.Mobile.iOS.CustomRenderer
{
    public class ImageCircleRenderer : ImageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || Element == null) return;
            CreateCircle();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == VisualElement.HeightProperty.PropertyName || e.PropertyName == VisualElement.WidthProperty.PropertyName)
            {
                CreateCircle();
            }
        }

        private void CreateCircle()
        {
            try
            {
                double min = Math.Min(Element.Width, Element.Height);
                Layer.CornerRadius = (float)(min / 2.0);
                Layer.MasksToBounds = false;
                Layer.BorderColor = Color.White.ToCGColor();
                Layer.BorderWidth = 1;
                ClipsToBounds = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to create circle image: " + ex);
            }
        }
    }
}