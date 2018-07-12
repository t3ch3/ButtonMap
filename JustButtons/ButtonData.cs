using System;
using CoreGraphics;
using UIKit;

namespace JustButtons
{
    /// <summary>
    /// Holds the button data that is saved to the data file as JSON
    /// </summary>
    public class ButtonData
    {
        public int ID { get; set; }
        public string VidPath { get; set; }
        public string ImgPath { get; set; }
        public BorderColourData BorderColour { get; set; }

        //default stuff
        public ButtonData()
        {
        }
    }
}
