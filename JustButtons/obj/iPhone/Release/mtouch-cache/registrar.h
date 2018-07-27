#pragma clang diagnostic ignored "-Wdeprecated-declarations"
#pragma clang diagnostic ignored "-Wtypedef-redefinition"
#pragma clang diagnostic ignored "-Wobjc-designated-initializers"
#include <stdarg.h>
#include <objc/objc.h>
#include <objc/runtime.h>
#include <objc/message.h>
#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <MediaPlayer/MediaPlayer.h>
#import <QuartzCore/QuartzCore.h>
#import <AVFoundation/AVFoundation.h>
#import <CoreGraphics/CoreGraphics.h>

@class UIApplicationDelegate;
@class UIPickerViewModel;
@class UIKit_UIControlEventProxy;
@class UIImagePickerControllerDelegate;
@class Foundation_InternalNSNotificationHandler;
@class __MonoMac_NSActionDispatcher;
@class __Xamarin_NSTimerActionDispatcher;
@class __MonoMac_NSAsyncActionDispatcher;
@class AppDelegate;
@class JustButtons_CustomButton;
@class JustButtons_PickerModel;
@class ViewController;
@class UIKit_UIImagePickerController__UIImagePickerControllerDelegate;
@class __NSObject_Disposer;
@class OpenTK_Platform_iPhoneOS_CADisplayLinkTimeSource;
@class OpenTK_Platform_iPhoneOS_iPhoneOSGameView;

@interface UIApplicationDelegate : NSObject<UIApplicationDelegate> {
}
	-(id) init;
@end

@interface UIPickerViewModel : NSObject<UIPickerViewDataSource, UIPickerViewDelegate> {
}
	-(id) init;
@end

@interface UIImagePickerControllerDelegate : NSObject<UINavigationControllerDelegate, UIImagePickerControllerDelegate, UINavigationControllerDelegate, UINavigationControllerDelegate> {
}
	-(id) init;
@end

@interface AppDelegate : NSObject<UIApplicationDelegate, UIApplicationDelegate> {
}
	-(void) release;
	-(id) retain;
	-(int) xamarinGetGCHandle;
	-(void) xamarinSetGCHandle: (int) gchandle;
	-(UIWindow *) window;
	-(void) setWindow:(UIWindow *)p0;
	-(BOOL) application:(UIApplication *)p0 didFinishLaunchingWithOptions:(NSDictionary *)p1;
	-(void) applicationWillResignActive:(UIApplication *)p0;
	-(void) applicationDidEnterBackground:(UIApplication *)p0;
	-(void) applicationWillEnterForeground:(UIApplication *)p0;
	-(void) applicationDidBecomeActive:(UIApplication *)p0;
	-(void) applicationWillTerminate:(UIApplication *)p0;
	-(BOOL) conformsToProtocol:(void *)p0;
	-(id) init;
@end

@interface JustButtons_CustomButton : UIButton {
}
	-(void) release;
	-(id) retain;
	-(int) xamarinGetGCHandle;
	-(void) xamarinSetGCHandle: (int) gchandle;
	-(BOOL) conformsToProtocol:(void *)p0;
	-(id) init;
@end

@interface JustButtons_PickerModel : NSObject<UIPickerViewDelegate, UIPickerViewDataSource, UIPickerViewDelegate> {
}
	-(void) release;
	-(id) retain;
	-(int) xamarinGetGCHandle;
	-(void) xamarinSetGCHandle: (int) gchandle;
	-(NSInteger) numberOfComponentsInPickerView:(UIPickerView *)p0;
	-(NSInteger) pickerView:(UIPickerView *)p0 numberOfRowsInComponent:(NSInteger)p1;
	-(NSString *) pickerView:(UIPickerView *)p0 titleForRow:(NSInteger)p1 forComponent:(NSInteger)p2;
	-(CGFloat) pickerView:(UIPickerView *)p0 rowHeightForComponent:(NSInteger)p1;
	-(UIView *) pickerView:(UIPickerView *)p0 viewForRow:(NSInteger)p1 forComponent:(NSInteger)p2 reusingView:(UIView *)p3;
	-(void) pickerView:(UIPickerView *)p0 didSelectRow:(NSInteger)p1 inComponent:(NSInteger)p2;
	-(BOOL) conformsToProtocol:(void *)p0;
@end

@interface ViewController : UIViewController {
}
	@property (nonatomic, assign) UISwipeGestureRecognizer * swipeLeft;
	@property (nonatomic, assign) UISwipeGestureRecognizer * swipeRight;
	-(void) release;
	-(id) retain;
	-(int) xamarinGetGCHandle;
	-(void) xamarinSetGCHandle: (int) gchandle;
	-(UISwipeGestureRecognizer *) swipeLeft;
	-(void) setSwipeLeft:(UISwipeGestureRecognizer *)p0;
	-(UISwipeGestureRecognizer *) swipeRight;
	-(void) setSwipeRight:(UISwipeGestureRecognizer *)p0;
	-(void) viewDidLoad;
	-(void) didReceiveMemoryWarning;
	-(void) viewDidLayoutSubviews;
	-(void) swipedLeft:(UISwipeGestureRecognizer *)p0;
	-(void) SwipedRight:(UISwipeGestureRecognizer *)p0;
	-(BOOL) conformsToProtocol:(void *)p0;
@end

@interface OpenTK_Platform_iPhoneOS_iPhoneOSGameView : UIView {
}
	-(void) release;
	-(id) retain;
	-(int) xamarinGetGCHandle;
	-(void) xamarinSetGCHandle: (int) gchandle;
	+(Class) layerClass;
	-(void) layoutSubviews;
	-(void) willMoveToWindow:(UIWindow *)p0;
	-(BOOL) conformsToProtocol:(void *)p0;
	-(id) initWithCoder:(NSCoder *)p0;
	-(id) initWithFrame:(CGRect)p0;
@end


