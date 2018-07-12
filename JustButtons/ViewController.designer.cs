// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace JustButtons
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwipeGestureRecognizer swipeLeft { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwipeGestureRecognizer swipeRight { get; set; }

        [Action ("swipedLeft:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void swipedLeft (UIKit.UISwipeGestureRecognizer sender);

        [Action ("SwipedRight:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void SwipedRight (UIKit.UISwipeGestureRecognizer sender);

        void ReleaseDesignerOutlets ()
        {
            if (swipeLeft != null) {
                swipeLeft.Dispose ();
                swipeLeft = null;
            }

            if (swipeRight != null) {
                swipeRight.Dispose ();
                swipeRight = null;
            }
        }
    }
}