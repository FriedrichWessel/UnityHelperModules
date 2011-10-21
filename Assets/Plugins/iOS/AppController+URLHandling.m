#import "AppController+URLHandling.h"
#import "FacebookProxy.h"



@implementation AppController (URLHandling)

- (BOOL) application: (UIApplication *)application openURL: (NSURL *)url sourceApplication: (NSString *)sourceApplication annotation: (id)annotation {
	return [fbp handleOpenURL: url];
}

- (BOOL) application: (UIApplication *)application handleOpenURL: (NSURL *)url {
	return [self application: application openURL: url sourceApplication: nil annotation: nil];
}

@end
