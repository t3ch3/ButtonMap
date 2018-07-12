using System;

using UIKit;
using CoreGraphics;

namespace JustButtons
{
    /// <summary>
    /// Inherits from the UIButton class
    /// Contains custom properties
    /// </summary>
    public class CustomButton :UIButton
    {
        public int ID { get; set; } //the id of the button. used to identify which button is which and so every button is unique
        public string VidPath { get; set; } // vid path stores the string url to the video in storage
        public string ImgPath { get; set; } // img path stores the string url to the video in storage
        public CGColor BorderColour { get; set; } //stores the colour as rgb for the button border

        //add default data
        public CustomButton()
        {
        }
    }
}
