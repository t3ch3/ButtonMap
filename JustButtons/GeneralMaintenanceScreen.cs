using System;

using UIKit;
using CoreGraphics;
using MediaPlayer;
using Foundation;
using System.Diagnostics;

namespace JustButtons
{
    public class GeneralMaintenanceScreen
    {
        public event EventHandler Saved;

        public UIViewController Screen;

        public int ButtonsPerPage = 6;
        public int NumberOfPages = 3;
        string[] ButtonsNum = { "2", "4", "6", "8" }; //the options for number of buttons per page
        string[] PagesNum = { "1", "2", "3", "4", "5", "6", "7", "8", "9" }; //the options for number of pages

        //navigation buttons
        public UIButton BackButton; //doesn't save changes
        public UIButton SaveButton; //saves changes

        //button variables
        public float ButtonBorderWidth = 4.5f;
        public UIColor ButtonBorderColour = UIColor.Gray;
        public UIColor ButtonBackgroundColour = UIColor.White;
        public const int ButtonCornerRadius = 25;

        //dropdowns
        //number of buttons per page, picker
        UILabel ButtonsLabel; // the text for the picker
        PickerModel ButtonsPickerModel; //contains the model for the view
        UIPickerView ButtonsPickerView; //the view for the picker
        //number of pages, picker
        UILabel PagesLabel; // the text for the picker
        PickerModel PagesPickerModel; //contains the model for the view
        UIPickerView PagesPickerView; //the view for the picker

        public GeneralMaintenanceScreen()
        {
            //create screen
            Screen = new UIViewController();
            Screen.View.BackgroundColor = UIColor.White;

            //create dropdown
            //list of values drop down contains
            //ButtonsNum = { "2", "4", "6", "8" };
            //PagesNum = { "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            //buttons per page dropdown
            ButtonsPickerModel = new PickerModel(ButtonsNum);
            ButtonsPickerModel.PickerChanged += (object sender, PickerChangedEventArgs e) =>
            {
                Debug.WriteLine("picker changed");
                Debug.WriteLine("value selected" + e.SelectedValue);
                ButtonsPerPage = Int32.Parse(e.SelectedValue.ToString());
            };
            //add picker model to ui picker and some styling
            ButtonsPickerView = new UIPickerView();
            ButtonsPickerView.Model = ButtonsPickerModel;
            ButtonsPickerView.ShowSelectionIndicator = true;
            ButtonsPickerView.Frame = new CoreGraphics.CGRect(250, 300, 60, 60);
            ButtonsPickerView.BackgroundColor = UIColor.White;
            ButtonsPickerView.Layer.BorderColor = UIColor.Gray.CGColor;
            ButtonsPickerView.Layer.BorderWidth = 4.5f;
            Screen.Add(ButtonsPickerView); //add picker to view

            //label
            CGRect frameButtonLabel = new CGRect(50, 280, 150, 100);
            ButtonsLabel = new UILabel(frameButtonLabel);
            ButtonsLabel.Text = "Number of buttons:";
            ButtonsLabel.TextColor = UIColor.Black;
            ButtonsLabel.TextAlignment = UITextAlignment.Right;
            Screen.Add(ButtonsLabel); //add label to view

            //pages dropdown
            //create picker model, and add function event when it changes
            //function will change the text fields value
            PagesPickerModel = new PickerModel(PagesNum);
            PagesPickerModel.PickerChanged += (object sender, PickerChangedEventArgs e) =>
            {
                Debug.WriteLine("picker changed");
                Debug.WriteLine("value selected" + e.SelectedValue);
                NumberOfPages = Int32.Parse(e.SelectedValue.ToString());
            };
            //add picker model to ui picker and some styling
            PagesPickerView = new UIPickerView();
            PagesPickerView.Model = PagesPickerModel;
            PagesPickerView.ShowSelectionIndicator = true;
            PagesPickerView.Frame = new CoreGraphics.CGRect(250, 150, 60, 60);
            PagesPickerView.BackgroundColor = UIColor.White;
            PagesPickerView.Layer.BorderColor = UIColor.Gray.CGColor;
            PagesPickerView.Layer.BorderWidth = 4.5f;
            Screen.Add(PagesPickerView); //add picker to view
            //label
            CGRect framePagesLabel = new CGRect(50, 120, 150, 100);
            PagesLabel = new UILabel(framePagesLabel);
            PagesLabel.Text = "Number of pages:";
            PagesLabel.TextColor = UIColor.Black;
            PagesLabel.TextAlignment = UITextAlignment.Right;
            Screen.Add(PagesLabel); //add label to view

            //create back button
            CGRect frameBackButton = new CGRect(700, 100, 250, 100);
            BackButton = new UIButton(frameBackButton);
            BackButton.BackgroundColor = UIColor.Red;
            BackButton.TouchUpInside += CloseScreen;
            BackButton.SetTitle("Back", UIControlState.Normal);
            BackButton.SetTitleColor(UIColor.Black, UIControlState.Normal);
            Screen.Add(BackButton); //add back button to screen
            BackButton.Layer.BorderColor = ButtonBorderColour.CGColor;
            BackButton.Layer.BorderWidth = ButtonBorderWidth;
            BackButton.BackgroundColor = ButtonBackgroundColour;
            BackButton.Layer.CornerRadius = ButtonCornerRadius;

            //create save button
            CGRect frameSaveButton = new CGRect(700, 250, 250, 100);
            SaveButton = new UIButton(frameSaveButton);
            SaveButton.BackgroundColor = UIColor.Red;
            SaveButton.TouchUpInside += Save;
            SaveButton.SetTitle("Save", UIControlState.Normal);
            SaveButton.SetTitleColor(UIColor.Black, UIControlState.Normal);
            Screen.Add(SaveButton); //add save button to screen
            SaveButton.Layer.BorderColor = ButtonBorderColour.CGColor;
            SaveButton.Layer.BorderWidth = ButtonBorderWidth;
            SaveButton.BackgroundColor = ButtonBackgroundColour;
            SaveButton.Layer.CornerRadius = ButtonCornerRadius;
        }

        public void SetDropDowns()
        {
            //set the pages dropdown value
            string PageNumIndexString = NumberOfPages.ToString();
            nint PageNumIndex = PagesPickerModel.values.IndexOf(PageNumIndexString);
            PagesPickerView.Select(PageNumIndex, 0, false);

            //set the buttons dropdown value
            string ButtonIndexString = ButtonsPerPage.ToString();
            nint ButtonIndex = ButtonsPickerModel.values.IndexOf(ButtonIndexString);
            ButtonsPickerView.Select(ButtonIndex, 0, false);
        }

        /// <summary>
        /// closes the screen.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void CloseScreen(object sender, EventArgs e)
        {
            Screen.DismissModalViewController(false);
        }

        /// <summary>
        /// closes the screen and invokes the Saved event if not null.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void Save(object sender, EventArgs e)
        {
            Console.WriteLine("saved event firing");
            if (Saved != null)
                Saved(this, EventArgs.Empty);

            Screen.DismissModalViewController(false);
        }
    }
}
