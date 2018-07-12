using System;

using UIKit;
using CoreGraphics;
using MediaPlayer;
using Foundation;
using MobileCoreServices;
using AVFoundation;
using CoreMedia;

using Cirrious.FluentLayouts.Touch;

namespace JustButtons
{
    public class ButtonMaintenanceScreen
    {
        //button variables
        public float ButtonBorderWidth = 4.5f;
        public UIColor ButtonBorderColour = UIColor.Gray;
        public UIColor ButtonBackgroundColour = UIColor.White;
        public const int ButtonCornerRadius = 25;

        GeneralMaintenanceScreen GeneralMaintenanceScreen = new GeneralMaintenanceScreen();

        public event EventHandler Saved; //when the save button is clicked
        public event EventHandler GeneralSaved; //when the save button on the general maintenance screen is clicked

        public int ButtonsPerPage = 6;
        public int NumberOfPages = 1;

        public UIViewController Screen; //the screen/view which contains all the controls inside of it

        public ButtonData Button; //button data instance that is equal to the button that is being configured. Used to pass new button data to the main screen

        //navigation buttons
        public UIButton BackButton; //closes button maintenance screen without saving (changes lost)
        public UIButton SaveButton; //save the changes and close the button maintenance screen
        public UIButton GeneralButton; //open the general settings modal

        //colour picker
        public UIView ColourBox; //box contains a solid fill of the colour picked
        public const float ColourBoxBorderWidth = 2f;
        public UISlider RedSlider;
        public UISlider GreenSlider;
        public UISlider BlueSlider;

        //media pickers
        public UIImagePickerController MediaPicker;
        public UIButton ImageButton; 
        public UIButton VideoButton; 
        public UIImageView ImageBox; //displays image picked inside box
        public UIImageView VideoBox; //displays video thumbnail in box

        //movie player - used to get the video thumbnail
        public MPMoviePlayerController MoviePlayer = new MPMoviePlayerController();

        public ButtonMaintenanceScreen()
        {
            GeneralMaintenanceScreen.Saved += SaveGeneral; //

            //create screen
            Screen = new UIViewController();
            Screen.View.BackgroundColor = UIColor.White;

            //create button
            Button = new ButtonData();

            //create media picker
            MediaPicker = new UIImagePickerController();
            MediaPicker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
            MediaPicker.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary);
            MediaPicker.ImageExportPreset = UIImagePickerControllerImageUrlExportPreset.Current;
                       
            MediaPicker.FinishedPickingMedia += Handle_FinishedPickingMedia;
            MediaPicker.Canceled += Handle_Canceled;

            //1. create views
            //create video button
            VideoButton = new UIButton();
            VideoButton.BackgroundColor = UIColor.Green;
            VideoButton.SetTitle("Choose a video for \n the button to play", UIControlState.Normal);
            VideoButton.SetTitleColor(UIColor.Black, UIControlState.Normal);

            //when video button clicked - open the media picker native interface
            VideoButton.TouchUpInside += (s, e) =>
            {
                try
                {
                    //set the media picker to show only videos
                    MediaPicker.MediaTypes = new string[] { UTType.Movie, UTType.Video }; //vids?
                    Screen.PresentViewControllerAsync(MediaPicker, true);
                }
                catch (Exception ex)
                {
                    //if the device doesn't have any videos, pop up an alert box with a message
                    Console.WriteLine(ex.Message);
                    UIAlertView alert = new UIAlertView()
                    {
                        Title = "No Videos Alert",
                        Message = "You don't have any videos to select. To download or record vids..."
                    };

                    alert.AddButton("OK");
                    alert.Show();
                }
            };

            VideoButton.Layer.BorderColor = ButtonBorderColour.CGColor;
            VideoButton.Layer.BorderWidth = ButtonBorderWidth;
            VideoButton.BackgroundColor = ButtonBackgroundColour;
            VideoButton.Layer.CornerRadius = ButtonCornerRadius;

            VideoButton.LineBreakMode = UILineBreakMode.WordWrap;//allow multiple lines for text inside video button
            VideoButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Center; //center text

            //create image button
            ImageButton = new UIButton();
            ImageButton.BackgroundColor = UIColor.Green;
            ImageButton.SetTitle("Choose an image for \n the button thumbnail", UIControlState.Normal);
            ImageButton.SetTitleColor(UIColor.Black, UIControlState.Normal);
            //when image button clicked - open the media picker native interface
            ImageButton.TouchUpInside += (s, e) =>
            {
                try
                {
                    //set the media picker to show only images
                    MediaPicker.MediaTypes = new string[] { UTType.Image };
                    Screen.PresentViewControllerAsync(MediaPicker, true);
                }
                catch (Exception ex)
                {
                    //if the device doesn't have any images, pop up an alert box with a message
                    Console.WriteLine(ex.Message);
                    UIAlertView alert = new UIAlertView()
                    {
                        Title = "No Images Alert",
                        Message = "You don't have any images to select. To download or take photos..."
                    };

                    alert.AddButton("OK");
                    alert.Show();
                }
            };
            ImageButton.Layer.BorderColor = ButtonBorderColour.CGColor;
            ImageButton.Layer.BorderWidth = ButtonBorderWidth;
            ImageButton.BackgroundColor = ButtonBackgroundColour;
            ImageButton.LineBreakMode = UILineBreakMode.WordWrap;//allow multiple lines for text inside video button
            ImageButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Center; //center text
            ImageButton.Layer.CornerRadius = ButtonCornerRadius;

            //create image box
            ImageBox = new UIImageView();
            ImageBox.Layer.BorderWidth = ColourBoxBorderWidth;
            ImageBox.Layer.CornerRadius = ButtonCornerRadius;
            ImageBox.ClipsToBounds = true;

            //create video box
            VideoBox = new UIImageView();
            VideoBox.Layer.BorderWidth = ColourBoxBorderWidth;
            VideoBox.Layer.CornerRadius = ButtonCornerRadius;
            VideoBox.ClipsToBounds = true;

            //create back button
            BackButton = new UIButton();
            BackButton.TouchUpInside += CloseScreen;
            BackButton.SetTitle("Back", UIControlState.Normal);
            BackButton.SetTitleColor(UIColor.Black, UIControlState.Normal);
            Screen.Add(BackButton); //add back button to screen
            BackButton.Layer.BorderColor = ButtonBorderColour.CGColor;
            BackButton.Layer.BorderWidth = ButtonBorderWidth;
            BackButton.BackgroundColor = ButtonBackgroundColour;
            BackButton.Layer.CornerRadius = ButtonCornerRadius;

            //create save button
            SaveButton = new UIButton();
            SaveButton.BackgroundColor = UIColor.Red;
            SaveButton.TouchUpInside += Save;
            SaveButton.SetTitle("Save", UIControlState.Normal);
            SaveButton.SetTitleColor(UIColor.Black, UIControlState.Normal);
            Screen.Add(SaveButton); //add save button to screen
            SaveButton.Layer.BorderColor = ButtonBorderColour.CGColor;
            SaveButton.Layer.BorderWidth = ButtonBorderWidth;
            SaveButton.BackgroundColor = ButtonBackgroundColour;
            SaveButton.Layer.CornerRadius = ButtonCornerRadius;

            //create colour box
            ColourBox = new UIView();
            ColourBox.Layer.BorderWidth = ColourBoxBorderWidth;
            ColourBox.Layer.CornerRadius = ButtonCornerRadius;

            //create general button
            GeneralButton = new UIButton();
            GeneralButton.BackgroundColor = UIColor.Green;
            //open the general settings modal.
            //pass some app data
            GeneralButton.TouchUpInside += (o, s) =>
            {
                //remove?
                GeneralMaintenanceScreen.ButtonsPerPage = ButtonsPerPage;
                GeneralMaintenanceScreen.NumberOfPages = NumberOfPages;
                Screen.PresentModalViewController(GeneralMaintenanceScreen.Screen, false); //

                GeneralMaintenanceScreen.NumberOfPages = this.NumberOfPages;
                GeneralMaintenanceScreen.ButtonsPerPage = this.ButtonsPerPage;
                GeneralMaintenanceScreen.SetDropDowns();
            };
            GeneralButton.SetTitle("JustButtons Settings", UIControlState.Normal);
            GeneralButton.SetTitleColor(UIColor.Black, UIControlState.Normal);
            GeneralButton.Layer.BorderColor = ButtonBorderColour.CGColor;
            GeneralButton.Layer.BorderWidth = ButtonBorderWidth;
            GeneralButton.BackgroundColor = ButtonBackgroundColour;
            GeneralButton.Layer.CornerRadius = ButtonCornerRadius;

            //color sliders
            RedSlider = new UISlider();
            RedSlider.ValueChanged += UpdateBorderColor;

            GreenSlider = new UISlider();
            GreenSlider.ValueChanged += UpdateBorderColor;

            BlueSlider = new UISlider();
            BlueSlider.ValueChanged += UpdateBorderColor;

            //2. add views to parent view
            Screen.View.Add(VideoButton);
            Screen.View.Add(ImageButton);
            Screen.View.Add(ImageBox);
            Screen.View.Add(VideoBox);
            Screen.View.Add(BackButton);
            Screen.View.Add(SaveButton);
            Screen.Add(ColourBox);
            Screen.Add(GeneralButton);
            Screen.Add(RedSlider);
            Screen.Add(GreenSlider);
            Screen.Add(BlueSlider);

            //3. call method on parent view
            Screen.View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            //4. add constraints
            Screen.View.AddConstraints(
                VideoButton.AtTopOf(Screen.View, UIApplication.SharedApplication.StatusBarFrame.Height + 70),
                VideoButton.AtLeftOf(Screen.View, 70),
                VideoButton.WithRelativeWidth(Screen.View, 0.2f),
                VideoButton.Height().EqualTo(100),

                ImageButton.Below(VideoButton, 80),
                ImageButton.WithSameLeft(VideoButton),
                ImageButton.WithSameWidth(VideoButton),
                ImageButton.WithSameHeight(VideoButton),

                VideoBox.WithSameTop(VideoButton),
                VideoBox.Left().EqualTo().RightOf(VideoButton).Plus(35),
                VideoBox.WithRelativeWidth(Screen.View, 0.17f),
                VideoBox.Height().EqualTo(100),

                ImageBox.WithSameTop(ImageButton),
                ImageBox.WithSameLeft(VideoBox),
                ImageBox.WithSameWidth(VideoBox),
                ImageBox.WithSameHeight(VideoBox),

                BackButton.WithSameTop(VideoButton),
                BackButton.AtRightOf(Screen.View, 70),
                BackButton.WithSameWidth(VideoButton),
                BackButton.WithSameHeight(VideoButton),

                SaveButton.WithSameTop(ImageButton),
                SaveButton.AtRightOf(Screen.View, 70),
                SaveButton.WithSameWidth(VideoButton),
                SaveButton.WithSameHeight(VideoButton),

                ColourBox.Below(ImageButton, 80),
                ColourBox.WithSameLeft(VideoButton),
                ColourBox.WithSameWidth(VideoButton),
                ColourBox.WithSameHeight(VideoBox),

                GeneralButton.Below(ColourBox, 80),
                GeneralButton.WithSameLeft(VideoButton),
                GeneralButton.WithSameWidth(VideoButton),
                GeneralButton.WithSameHeight(VideoButton),

                RedSlider.WithSameTop(ColourBox),
                RedSlider.Left().EqualTo().RightOf(ColourBox).Plus(35),
                RedSlider.WithRelativeWidth(Screen.View, 0.11f),
                RedSlider.WithSameHeight(ColourBox),

                GreenSlider.WithSameTop(ColourBox),
                GreenSlider.Left().EqualTo().RightOf(RedSlider).Plus(30),
                GreenSlider.WithSameWidth(RedSlider),
                GreenSlider.WithSameHeight(ColourBox),

                BlueSlider.WithSameTop(ColourBox),
                BlueSlider.Left().EqualTo().RightOf(GreenSlider).Plus(30),
                BlueSlider.WithSameWidth(RedSlider),
                BlueSlider.WithSameHeight(ColourBox)
            );






        }

        /// <summary>
        /// Executes when the save button on the general modal is clicked
        /// It passes over the new app data values
        /// And fires the general saved event
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public void SaveGeneral(object sender, EventArgs e)
        {
            ButtonsPerPage = GeneralMaintenanceScreen.ButtonsPerPage;
            NumberOfPages = GeneralMaintenanceScreen.NumberOfPages;
            if (GeneralSaved != null)
                GeneralSaved(this, EventArgs.Empty);
        }

        /// <summary>
        /// Updates the border colour to the slider values
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public void UpdateBorderColor(object sender, EventArgs e)
        {
            Button.BorderColour = new BorderColourData() { Red = RedSlider.Value, Green = GreenSlider.Value, Blue = BlueSlider.Value };
            SetColourBox();
            SetImageBox();
        }

        /// <summary>
        /// Sets the colour box fill to slider values
        /// </summary>
        public void SetColourBox()
        {
            ColourBox.BackgroundColor = UIColor.FromRGB(Button.BorderColour.Red, Button.BorderColour.Green, Button.BorderColour.Blue);
        }


        /// <summary>
        /// Sets the video box image to the movie players video thumbnail
        /// </summary>
        public void SetVideoBox()
        {
            //gets the thumbnail from the movie player property
            //VideoBox.Image = MoviePlayer.ThumbnailImageAt(0.0, MPMovieTimeOption.NearestKeyFrame); //stops code flow here
            VideoBox.Image = GetVideoThumbnail(Button.VidPath);
        }

        UIImage GetVideoThumbnail(string path)
        {
            try
            {
                CMTime actualTime;
                NSError outError;
                using (var asset = AVAsset.FromUrl(NSUrl.FromFilename(path)))
                using (var imageGen = new AVAssetImageGenerator(asset))
                using (var imageRef = imageGen.CopyCGImageAtTime(new CMTime(1, 1), out actualTime, out outError))
                {
                    return UIImage.FromImage(imageRef);
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Used to update the image box to the button's image path
        /// </summary>
        public void SetImageBox()
        {
            ImageBox.Image = new UIImage(Button.ImgPath);
            ImageBox.Layer.BorderColor = UIColor.FromRGB(Button.BorderColour.Red, Button.BorderColour.Green, Button.BorderColour.Blue).CGColor;
        }

        /// <summary>
        /// Dismisses the media picker interface
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void Handle_Canceled(object sender, EventArgs e)
        {
            MediaPicker.DismissModalViewController(true);
        }

        /// <summary>
        /// Handles the finished picking media
        /// Gets the item selected from the media picker
        /// If it is an image it sets the Button's image path to the path of the item
        /// If it is a video, it sets the Button's vid path to the path of the item
        /// Finally it closes the interface back to the button settings modal
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected void Handle_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
        {
            switch (e.Info[UIImagePickerController.MediaType].ToString())
            {
                case "public.image": //if item is image
                    Console.WriteLine("Image selected");
                    Button.ImgPath = e.ImageUrl.Path;
                    Console.WriteLine("img:" + e.ImageUrl);
                    SetImageBox();
                    break;
                case "public.movie": //if item is video or movie
                    Console.WriteLine("Video selected");
                    Button.VidPath = e.MediaUrl.Path; //assign item path to button vid path
                    Console.WriteLine("media:" + e.MediaUrl);
                    MoviePlayer.ContentUrl = NSUrl.FromFilename(Button.VidPath); //assign movie player url to item path. so can retrieve the video thumbnail
                    SetVideoBox();
                    break;
                default:
                    Console.WriteLine(e.Info[UIImagePickerController.MediaType]+" selected");
                    break;
            }

            // dismiss the picker
            MediaPicker.DismissModalViewController(true);
        }

        /// <summary>
        /// closes the screen.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void CloseScreen(object sender, EventArgs e)
        {
            //close the button settings modal back to the main screen
            Screen.DismissModalViewController(false);
        }

        /// <summary>
        /// invokes the saved event and closes the button settings screen back to the main screen
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void Save(object sender, EventArgs e)
        {
            Console.WriteLine("saved event firing");
            if (Saved != null)
                Saved(this, EventArgs.Empty);

            //close the button settings modal back to the main screen
            Screen.DismissModalViewController(false);
        }
    }
}
