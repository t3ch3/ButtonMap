using System;
using System.Diagnostics;

using CoreGraphics;
using Foundation;
using MediaPlayer;
using UIKit;

namespace JustButtons
{
    /// <summary>
    /// Uses the MPMoviePlayerController to play videos
    /// </summary>
    public class VideoPlayer
    {
        public MPMoviePlayerController MoviePlayer; //the video player

        /// <summary>
        /// Initializes a new instance of the <see cref="T:JustButtons.VideoPlayer"/> class.
        /// Set the movie player properties and events
        /// </summary>
        public VideoPlayer()
        {
            MoviePlayer = new MPMoviePlayerController();
            MoviePlayer.ControlStyle = MPMovieControlStyle.Fullscreen;
            MoviePlayer.ShouldAutoplay = true;
            MoviePlayer.View.BackgroundColor = UIColor.Clear; //???
            MoviePlayer.View.Frame = new CGRect(0, 0, 0, 0); // size 0 because full screen only

            //events = video finished or user clicked done = stops the vid
            NSNotificationCenter.DefaultCenter.AddObserver(MPMoviePlayerController.PlaybackDidFinishNotification, VideoFinished);
            NSNotificationCenter.DefaultCenter.AddObserver(MPMoviePlayerController.DidExitFullscreenNotification, VideoFinished);
        }

        /// <summary>
        /// Stops the movie player
        /// </summary>
        /// <param name="notification">Notification.</param>
        public void VideoFinished(NSNotification notification)
        {
            //firing twice???
            Debug.WriteLine("video finished or user hit done!");

            MoviePlayer.SetFullscreen(false, false);
            MoviePlayer.Stop();
        }
    }
}
