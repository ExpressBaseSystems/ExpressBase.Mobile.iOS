using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ExpressBase.Mobile.CustomControls;
using ExpressBase.Mobile.iOS.CustomRenderer;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TextBox), typeof(TextBoxRenderer))]
[assembly: ExportRenderer(typeof(TextArea), typeof(TextAreaRenderer))]
[assembly: ExportRenderer(typeof(CustomDatePicker), typeof(CustomDatePickerRenderer))]
[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomSelectRenderer))]
[assembly: ExportRenderer(typeof(CustomSearchBar), typeof(CustomSearchRenderer))]
[assembly: ExportRenderer(typeof(ComboBoxLabel), typeof(ComboLabelRenderer))]
namespace ExpressBase.Mobile.iOS.CustomRenderer
{
    class TextBoxRenderer : EntryRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }
    }

    class TextAreaRenderer : EditorRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }
    }

    public class CustomDatePickerRenderer : DatePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
        {
            base.OnElementChanged(e);
        }
    }

    public class CustomSelectRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Picker> e)
        {
            base.OnElementChanged(e);
        }
    }

    public class CustomSearchRenderer : SearchBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.SearchBar> e)
        {
            base.OnElementChanged(e);
        }
    }

    public class ComboLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Label> e)
        {
            base.OnElementChanged(e);
        }
    }
}