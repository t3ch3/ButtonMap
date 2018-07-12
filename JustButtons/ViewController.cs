using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Text;

using UIKit;
using CoreGraphics;
using MediaPlayer;
using Foundation;

namespace JustButtons
{
    public partial class ViewController : UIViewController
    {
        //App data - holds data that is saved
        AppData AppData = new AppData();

        //Button maintenance screen - the button settings screen
        ButtonMaintenanceScreen ButtonMaintenanceScreen = new ButtonMaintenanceScreen();

        //File manager - used to create and save file of app data
        FileManager FileManager = new FileManager();

        //Video player - contains a media player that plays videos
        VideoPlayer VideoPlayer = new VideoPlayer();

        //Temp app data
        public nint PageNum = 1; //current page number
        public nint Cols; //number of cols in the grid
        public nint Rows; //number of rows in the grid
        public bool Hold = false; //true if the user is holding down the secret button to the button maintenance screen

        //Grid and cell dimensions
        public UICollectionView Grid;
        public nfloat GridW;
        public nfloat GridH;
        public const float GridPaddingBottom = 50f; //Creates space for the hold button and page indicators
        public nfloat StatusBarH = UIApplication.SharedApplication.StatusBarFrame.Height; //the height of the status bar

        //Cell variables
        public nfloat CellW;
        public nfloat CellH;
        public const float CellBorderW = 2f;

        //Button variables
        public const float ButtonBorderW = 4.5f;
        public const int ButtonCornerRadius = 25;
        public const float ButtonPadding = 30f;

        //Page indicators
        UIPageControl PageControl;
        public const float PageControlWidth = 300;

        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            //fixes orientation not resizing??//?
            //View.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
            //divide one by 2, multiply other by 2????
            //use autolayout on grid???

            //subscribe to button maintenance or general maintenance screens saving
            ButtonMaintenanceScreen.Saved += SaveButton;
            ButtonMaintenanceScreen.GeneralSaved += SaveSettings;

            Add(VideoPlayer.MoviePlayer.View); //add movie player to view

            //Check if file exists, if no, create default file / dont have to do anything???
            if (!File.Exists(FileManager.FilePath))
            {
                //create file
                Debug.WriteLine("file doesn't exist");
                Debug.WriteLine("[CREATING NEW FILE]");
                FileManager.CreateFileAppData(AppData);
            }

            //load the file app data
            AppData = FileManager.LoadFileAppData();

            //Display page
            CreateGrid();
            CalcRowsAndCols();
            CalcCellsWidthAndHeight();
            CreateCells();
            CreateHoldButton();
            CreatePageControl();

            //change orientation
            Foundation.NSNotificationCenter.DefaultCenter.AddObserver(new NSString("UIDeviceOrientationDidChangeNotification"), UpdateSize);
        }

        /// <summary>
        /// Executes when the devices orientation changes
        /// </summary>
        /// <param name="notification">Notification.</param>
        private void UpdateSize(NSNotification notification)
        {
            Console.WriteLine("oreintation changed");

            // Landscape or Portrait 
            Console.WriteLine(UIDevice.CurrentDevice.Orientation);

            //get the screen width and height
            Console.WriteLine(UIScreen.MainScreen.Bounds.Width);
            Console.WriteLine(UIScreen.MainScreen.Bounds.Height);

            /*
            ClearGrid();
            CreateGrid();
            CalcCellsWidthAndHeight();
            CreateCells();*/
        }

        /// <summary>
        /// Creates the page control.
        /// </summary>
        public void CreatePageControl()
        {
            //create pagecontrol
            CGRect framePageControl = new CGRect((UIScreen.MainScreen.Bounds.Width / 2) - (300 / 2), UIScreen.MainScreen.Bounds.Height - GridPaddingBottom, 300, GridPaddingBottom); //make var
            PageControl = new UIPageControl(framePageControl);

            PageControl.Pages = AppData.NumberOfPages;
            PageControl.PageIndicatorTintColor = UIColor.LightGray;
            PageControl.CurrentPageIndicatorTintColor = UIColor.Black;
            PageControl.UserInteractionEnabled = false; //stops user from changing page control with a tap
            PageControl.CurrentPage = PageNum-1; //-1 because it starts at 0

            Add(PageControl); //add page control to view
        }
         
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        /// <summary>
        /// Creates the grid, sets size, bg colour and adds to view.
        /// Can also recreate grid.
        /// </summary>
        public void CreateGrid()
        {
            //set grid sizes to screen sizes
            GridW = UIScreen.MainScreen.Bounds.Width;
            //height applies padding to add space for hold & page indicators at bottom AND space for status bar at the top so it doesn't overlap
            GridH = UIScreen.MainScreen.Bounds.Height - UIApplication.SharedApplication.StatusBarFrame.Height - GridPaddingBottom;

            CGRect gridFrame = new CGRect(0, StatusBarH, GridW, GridH);
            Grid = new UICollectionView(gridFrame, new UICollectionViewLayout());
            Grid.BackgroundColor = View.BackgroundColor; //set to same bg colour as screen
            //add grid to view
            View.AddSubview(Grid);
        }

        /// <summary>
        /// Calculate number of rows and cols from buttons per page
        /// </summary>
        public void CalcRowsAndCols()
        {
            switch (AppData.ButtonsPerPage)
            {
                case 2:
                    Cols = 1;
                    Rows = 2;
                    break;
                case 4:
                    Cols = 2;
                    Rows = 2;
                    break;
                case 6:
                    Cols = 2;
                    Rows = 3;
                    break;
                case 8:
                    Cols = 2;
                    Rows = 4;
                    break;
                default:
                    Cols = 2;
                    Rows = 3;
                    break;
            }

            Debug.WriteLine("Cols: {0}, Rows: {1}", Cols, Rows);
        }

        /// <summary>
        /// Uses grid size and number of rows and cols to calculate the width and height for the cells
        /// </summary>
        public void CalcCellsWidthAndHeight()
        {
            CellH = GridH / Rows;
            CellW = GridW / Cols;
        }

        /// <summary>
        /// Creates the cells and adds them to grid.
        /// Creates the buttons and adds them to cell.
        /// Creates the image from img path and adds them to button.
        /// </summary>
        public void CreateCells()
        {
            //gets the starting index for button ids using page and number of buttons per page
            nint btnIdCounter = (PageNum - 1) * AppData.ButtonsPerPage;

            for (int c = 0; c < Cols; c++)
            {
                for (int r = 0; r < Rows; r++)
                {
                    //create cell
                    CGRect cellFrame = new CGRect(c * CellW, r * CellH, CellW, CellH);
                    UICollectionViewCell cell = new UICollectionViewCell(cellFrame);
                    Grid.AddSubview(cell); //add cell to grid

                    //create button and button data
                    ButtonData theButtonData;
                    CustomButton theButton = new CustomButton();
                    CGRect btnFrame = new CGRect(0 + (ButtonPadding / 2), 0 + (ButtonPadding / 2), CellW - (ButtonPadding), CellH - (ButtonPadding));

                    //retrives button data obj from list of buttons using id
                    //returns null if no match
                    theButtonData = AppData.Buttons.Find((x) =>
                    {
                        return x.ID == btnIdCounter;
                    });

                    //if no button data obj existed with the button id
                    if(theButtonData == null) //create default button data
                    {
                        //set the button data default values
                        theButtonData = new ButtonData();
                        theButtonData.ID = (int)btnIdCounter;
                        theButtonData.BorderColour = new BorderColourData(){Red = 0, Green = 0, Blue = 105};
                        theButtonData.VidPath = "babyshark.m4v";
                        theButtonData.ImgPath = "shark.jpg";
                    }

                    //fill the button with the button data properties
                    theButton.ID = (int)theButtonData.ID;
                    theButton.BorderColour = UIColor.FromRGB(theButtonData.BorderColour.Red, theButtonData.BorderColour.Green, theButtonData.BorderColour.Blue).CGColor;
                    theButton.VidPath = theButtonData.VidPath;
                    theButton.ImgPath = theButtonData.ImgPath;
                    //set the button styling and settings
                    theButton.Layer.BorderColor = theButton.BorderColour;

                    //set constant properties for every button
                    theButton.Frame = btnFrame;
                    theButton.Layer.BorderWidth = ButtonBorderW;
                    theButton.Layer.CornerRadius = ButtonCornerRadius;

                    //add play vid function to button click
                    theButton.TouchUpInside -= ButtonClickPlayVid; //fixes a saved button playing multiple times!
                    theButton.TouchUpInside += ButtonClickPlayVid;

                    //add button to cell
                    cell.AddSubview(theButton);

                    //create img
                    CGRect imgFrame = new CGRect(0, 0, theButton.Bounds.Width, theButton.Bounds.Height);
                    UIImageView img = new UIImageView(imgFrame);
                    img.Image = new UIImage(theButton.ImgPath); //set the image to the button img path
                    img.UserInteractionEnabled = false; //So user can still click the button
                    img.Layer.CornerRadius = ButtonCornerRadius;
                    img.ClipsToBounds = true; //enables corner radius on img

                    //add img to button
                    theButton.AddSubview(img);

                    btnIdCounter++; //increments the button id counter for the next button
                }
            }
        }

        /// <summary>
        /// Fires when a button is clicked
        /// If hold not true, then play the video path. If vid not found then...
        /// If hold true, then open the button maintenace screen for the selected button.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public void ButtonClickPlayVid(object sender, EventArgs e)
        {
            //convert sender to custom button type so can access properties
            CustomButton btn = sender as CustomButton;

            if (btn != null)
            {
                if (Hold == false) //if hold button not pressed down
                {
                    Debug.WriteLine("Button {0} Pressed", btn.ID);
                    Debug.WriteLine("Playing vid: {0}", btn.VidPath);

                    //sets the movie player url to the buttons vid path, and plays the vid in fullscreen
                    VideoPlayer.MoviePlayer.ContentUrl = NSUrl.FromFilename(btn.VidPath);
                    VideoPlayer.MoviePlayer.SetFullscreen(true, false);
                    VideoPlayer.MoviePlayer.Play();
                }
                else //if the hold button is held down / hold is true
                {
                    Debug.WriteLine("open button maintenance");
                    this.PresentModalViewController(ButtonMaintenanceScreen.Screen, false); //open the button maintenace screen modal
                    Hold = false; //reset hold to false otherwise hold stays true after opening button maintenance screen

                    // set button maintenance obj to sender!
                    //create a new button data obj that will be passed to the button mainteance screen
                    //*fixed issure where button maintenance values are changed, but not saved. But it still saved those values because of ref type
                    //set the button data properties to the sender's (button clicked) properties
                    ButtonData buttonData = new ButtonData();
                    buttonData.ID = btn.ID;

                    buttonData.BorderColour = new BorderColourData() 
                    { Red = (float)btn.BorderColour.Components[0], Green = (float)btn.BorderColour.Components[1], Blue = (float)btn.BorderColour.Components[2] };

                    buttonData.ImgPath = btn.ImgPath;
                    buttonData.VidPath = btn.VidPath;

                    //set button maintenance screen button to the button data obj
                    ButtonMaintenanceScreen.Button = buttonData;

                    //set sliders - so it matches the colours of the button clicked
                    ButtonMaintenanceScreen.RedSlider.Value = (float)btn.BorderColour.Components[0];
                    ButtonMaintenanceScreen.GreenSlider.Value = (float)btn.BorderColour.Components[1];
                    ButtonMaintenanceScreen.BlueSlider.Value = (float)btn.BorderColour.Components[2];

                    //set colour and image box
                    ButtonMaintenanceScreen.SetColourBox(); //change to on change
                    ButtonMaintenanceScreen.SetImageBox();

                    //set the video box (thumbnail)
                    //set the movie player url property to the sender's(button clicked) vidpath
                    //so it can display the videos thumbnail in the video box
                    ButtonMaintenanceScreen.MoviePlayer.ContentUrl = NSUrl.FromFilename(btn.VidPath);
                    ButtonMaintenanceScreen.SetVideoBox();

                    //pass some of the app datas values to the button maintenance screen class
                    ButtonMaintenanceScreen.ButtonsPerPage = (int)AppData.ButtonsPerPage;
                    ButtonMaintenanceScreen.NumberOfPages = (int)AppData.NumberOfPages;
                }
            }
        }

        /// <summary>
        /// Creates the hold button.
        /// </summary>
        public void CreateHoldButton()
        {
            //use grid padding bottom so it matches the space left by padding
            CGRect frameHoldButton = new CGRect(0, UIScreen.MainScreen.Bounds.Height - GridPaddingBottom, GridPaddingBottom, GridPaddingBottom);
            UIButton holdButton = new UIButton(frameHoldButton);
            //holdButton.BackgroundColor = UIColor.Red;
            holdButton.TouchDown += HoldButton_TouchDown; //subscribe method to button touched event
            holdButton.TouchUpInside += HoldButton_TouchUpInside; //subscribe method to button released event

            //add hold button to view
            View.AddSubview(holdButton);
        }

        /// <summary>
        /// When hold button pressed set hold to true
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public void HoldButton_TouchDown(object sender, EventArgs e)
        {
            Debug.WriteLine("Pressed hold button");
            Hold = true;
        }

        /// <summary>
        /// When hold button released set hold to false
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public void HoldButton_TouchUpInside(object sender, EventArgs e)
        {
            Debug.WriteLine("Released hold button");
            Hold = false;
        }

        /// <summary>
        /// Disposes of grid and cells.
        /// </summary>
        public void ClearGrid()
        {
            Grid.RemoveFromSuperview(); //remove grid from view
            Grid.Dispose();
        }

        /// <summary>
        /// Fires when user swipes to left.
        /// It checks if already on last page, if yes doesn't do anything
        /// If not, it changes page num and and updates the page.
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void swipedLeft(UISwipeGestureRecognizer sender)
        {
            if(!(PageNum+1 > AppData.NumberOfPages))
            {
                PageNum++;
                Debug.WriteLine("Page " + PageNum);

                //recreate page with new buttons
                ClearGrid();
                CreateGrid();
                CreateCells();

                //increment page control
                PageControl.CurrentPage++;

                //fixes ...
                Add(PageControl); //add page indicator to view
                CreateHoldButton();
            }
            else
            {
                Debug.WriteLine("No more pages!");
            }
        }

        /// <summary>
        /// Fires when user swipes to left.
        /// It checks if already on first page, if yes doesn't do anything
        /// If not, it changes page num and and updates the page.
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void SwipedRight(UISwipeGestureRecognizer sender)
        {
            if (!(PageNum - 1 <= 0))
            {
                PageNum--;
                Debug.WriteLine("Page " + PageNum);

                ClearGrid();
                CreateGrid();
                CreateCells();

                //change page control
                PageControl.CurrentPage--;

                //fixes ...
                Add(PageControl); //this brings back the page control but why is it REMOVED?!?!?!?!!
                CreateHoldButton();
            }
            else
            {
                Debug.WriteLine("No more pages!");
            }
        }

        /// <summary>
        /// Executes when save button is clicked on button maintenance screen
        /// Saves the changes to the button img, vid and border colour to memory
        /// Finally it saves the memory to a file
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public void SaveButton(object sender, EventArgs e)
        {
            Console.WriteLine("saving button inside view controller");
            //this sender??

            //returns true if button being saved already exists in data (been saved before) else false
            bool buttonExists = AppData.Buttons.Exists((x) =>
            {
                return x.ID == ButtonMaintenanceScreen.Button.ID;
            });

            //if the button being saved doesn't exist yet then add it to the app data memory
            if(!buttonExists)
            {
                AppData.Buttons.Add(ButtonMaintenanceScreen.Button);
                Console.WriteLine("added btn");
            }
            else//update the already existed button in the list. *dont need cause when set button maintenace button it is reference type
            {
                //get the index of where the button is in the list
                int indexButton = AppData.Buttons.FindIndex((x) =>
                {
                    return x.ID == ButtonMaintenanceScreen.Button.ID;
                });

                //replace the button with the new saved button
                Debug.WriteLine("index of button:" + indexButton);
                AppData.Buttons[indexButton] = ButtonMaintenanceScreen.Button;
            }

            //save the new memory app data to data file in storage
            FileManager.CreateFileAppData(AppData);

            ClearGrid();
            CreateGrid();
            CreateCells();
        }

        /// <summary>
        /// Executed when general maintenance screen save button is clicked
        /// Saves the new app data to memory
        /// Then saves the memory to the data file in storage
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public void SaveSettings(object sender, EventArgs e)
        {
            Console.WriteLine("saving settings");
            //change settings
            AppData.ButtonsPerPage = ButtonMaintenanceScreen.ButtonsPerPage;
            AppData.NumberOfPages = ButtonMaintenanceScreen.NumberOfPages;

            //Set current page number to 1 incase user was on a page higher than what the new nubmer of pages is
            PageNum = 1;

            //save the new app data to the data file
            FileManager.CreateFileAppData(AppData);

            //recreate buttons
            CalcRowsAndCols();
            CalcCellsWidthAndHeight();
            ClearGrid();
            CreateGrid();
            CreateCells();

            //update page control
            PageControl.Pages = AppData.NumberOfPages;
            PageControl.CurrentPage = PageNum-1; //reset back to page 1, minus 1 because starts at 0
        }

    }
}
