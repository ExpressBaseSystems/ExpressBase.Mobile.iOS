﻿using System;
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
[assembly: ExportRenderer(typeof(XButton), typeof(ButtonRenderer))]
namespace ExpressBase.Mobile.iOS.CustomRenderer
{
    class TextBoxRenderer : EntryRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }
    }

    public class XButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
        }
    }
}