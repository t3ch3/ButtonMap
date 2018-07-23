using System;

using UIKit;
using CoreGraphics;
using MediaPlayer;
using Foundation;
using System.Diagnostics;

using Cirrious.FluentLayouts.Touch;

namespace JustButtons
{
    public class GeneralMaintenanceScreen
    {
        public event EventHandler Saved;

        public UIViewController Screen;

        public int ButtonsPerPage = 6;
        public int NumberOfPages = 3;
        public float BordersThickness = 5.5f;
        string[] ButtonsNum = { "2", "4", "6", "8" }; //the options for number of buttons per page
        string[] PagesNum = { "1", "2", "3", "4", "5", "6", "7", "8", "9" }; //the options for number of pages
        string[] BordersNum = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" }; //the options for border thickness

        //navigation buttons
        public UIButton BackButton; //doesn't save changes
        public UIButton SaveButton; //saves changes

        //button variables
        public float ButtonBorderWidth = 5.5f;
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
        //thickness of button border, picker
        UILabel BordersLabel; // the text for the picker
        PickerModel BordersPickerModel; //contains the model for the view
        UIPickerView BordersPickerView; //the view for the picker

        public GeneralMaintenanceScreen()
        {
            //create screen
            Screen = new UIViewController();
            Screen.View.BackgroundColor = UIColor.White;

            //1. create view
            //pages label
            PagesLabel = new UILabel();
            PagesLabel.Text = "Number of pages:";
            PagesLabel.TextColor = UIColor.Black;
            PagesLabel.TextAlignment = UITextAlignment.Left;

            //buttons label
            ButtonsLabel = new UILabel();
            ButtonsLabel.Text = "Number of buttons:";
            ButtonsLabel.TextColor = UIColor.Black;
            ButtonsLabel.TextAlignment = UITextAlignment.Left;

            //borders label
            BordersLabel = new UILabel();
            BordersLabel.Text = "Borders thickness:";
            BordersLabel.TextColor = UIColor.Black;
            BordersLabel.TextAlignment = UITextAlignment.Left;

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
            //ButtonsPickerView.Frame = new CoreGraphics.CGRect(250, 300, 60, 60);
            ButtonsPickerView.BackgroundColor = UIColor.White;
            ButtonsPickerView.Layer.BorderColor = UIColor.Gray.CGColor;
            ButtonsPickerView.Layer.BorderWidth = 4.5f;

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
            //PagesPickerView.Frame = new CoreGraphics.CGRect(250, 150, 60, 60);
            PagesPickerView.BackgroundColor = UIColor.White;
            PagesPickerView.Layer.BorderColor = UIColor.Gray.CGColor;
            PagesPickerView.Layer.BorderWidth = 4.5f;

            //borders dropdown
            //create picker model, and add function event when it changes
            //function will change the text fields value
            BordersPickerModel = new PickerModel(BordersNum);
            BordersPickerModel.PickerChanged += (object sender, PickerChangedEventArgs e) =>
            {
                Debug.WriteLine("picker changed");
                Debug.WriteLine("value selected" + e.SelectedValue);
                BordersThickness = Int32.Parse(e.SelectedValue.ToString());
            };
            //add picker model to ui picker and some styling
            BordersPickerView = new UIPickerView();
            BordersPickerView.Model = BordersPickerModel;
            BordersPickerView.ShowSelectionIndicator = true;
            BordersPickerView.BackgroundColor = UIColor.White;
            BordersPickerView.Layer.BorderColor = UIColor.Gray.CGColor;
            BordersPickerView.Layer.BorderWidth = 4.5f;

            //create back button
            BackButton = new UIButton();
            BackButton.BackgroundColor = UIColor.Red;
            BackButton.TouchUpInside += CloseScreen;
            BackButton.SetTitle("Back", UIControlState.Normal);
            BackButton.SetTitleColor(UIColor.Black, UIControlState.Normal);
            BackButton.Layer.BorderColor = ButtonBorderColour.CGColor;
            BackButton.Layer.BorderWidth = BordersThickness;
            BackButton.BackgroundColor = ButtonBackgroundColour;
            BackButton.Layer.CornerRadius = ButtonCornerRadius;

            //create save button
            SaveButton = new UIButton();
            SaveButton.BackgroundColor = UIColor.Red;
            SaveButton.TouchUpInside += Save;
            SaveButton.SetTitle("Save", UIControlState.Normal);
            SaveButton.SetTitleColor(UIColor.Black, UIControlState.Normal);
            SaveButton.Layer.BorderColor = ButtonBorderColour.CGColor;
            SaveButton.Layer.BorderWidth = BordersThickness;
            SaveButton.BackgroundColor = ButtonBackgroundColour;
            SaveButton.Layer.CornerRadius = ButtonCornerRadius;

            //2. add view to parent view
            Screen.View.Add(PagesLabel);
            Screen.View.Add(ButtonsLabel);
            Screen.View.Add(ButtonsPickerView);
            Screen.View.Add(PagesPickerView);
            Screen.View.Add(BackButton);
            Screen.View.Add(SaveButton);
            Screen.View.Add(BordersLabel);
            Screen.View.Add(BordersPickerView);

            //3. call method on parent view
            Screen.View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            //4. add constraints
            Screen.View.AddConstraints(
                PagesLabel.WithSameTop(BackButton),
                PagesLabel.AtLeftOf(Screen.View, 70),
                PagesLabel.WithRelativeWidth(Screen.View, 0.2f),
                //PagesLabel.Width().EqualTo(150),
                PagesLabel.Height().EqualTo(45),

                ButtonsLabel.WithSameTop(SaveButton),
                ButtonsLabel.WithSameLeft(PagesLabel),
                ButtonsLabel.WithSameWidth(PagesLabel),
                ButtonsLabel.WithSameHeight(PagesLabel),

                BordersLabel.Below(SaveButton, 80),//
                BordersLabel.WithSameLeft(PagesLabel),
                BordersLabel.WithSameWidth(PagesLabel),
                BordersLabel.WithSameHeight(PagesLabel),

                ButtonsPickerView.WithSameTop(ButtonsLabel),
                ButtonsPickerView.Left().EqualTo().RightOf(ButtonsLabel).Plus(30),
                ButtonsPickerView.Width().EqualTo(60),
                ButtonsPickerView.Height().EqualTo(60),

                PagesPickerView.WithSameTop(PagesLabel),
                PagesPickerView.WithSameLeft(ButtonsPickerView),
                PagesPickerView.WithSameWidth(ButtonsPickerView),
                PagesPickerView.WithSameHeight(ButtonsPickerView),

                BordersPickerView.WithSameTop(BordersLabel),//
                BordersPickerView.WithSameLeft(ButtonsPickerView),
                BordersPickerView.WithSameWidth(ButtonsPickerView),
                BordersPickerView.WithSameHeight(ButtonsPickerView),

                BackButton.AtTopOf(Screen.View, UIApplication.SharedApplication.StatusBarFrame.Height + 70),
                BackButton.AtRightOf(Screen.View, 70),
                BackButton.WithRelativeWidth(Screen.View, 0.2f),
                BackButton.Height().EqualTo(100),

                SaveButton.Below(BackButton, 80),
                SaveButton.WithSameRight(BackButton),
                SaveButton.WithSameWidth(BackButton),
                SaveButton.WithSameHeight(BackButton)
            );
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

            //set the border width dropdown value
            string ButtonWidthIndexString = BordersThickness.ToString();
            nint ButtonWidthIndex = BordersPickerModel.values.IndexOf(ButtonWidthIndexString);
            BordersPickerView.Select(ButtonWidthIndex, 0, false);
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
