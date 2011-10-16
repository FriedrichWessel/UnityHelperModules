#import <Foundation/Foundation.h>
#import "FBConnect.h"


#define LOGGED_IN 0
#define LOGGED_OUT 1

@interface FacebookProxy : NSObject<FBSessionDelegate, FBRequestDelegate> {
	Facebook *fb;
}

- (id) initWithAppId: (NSString *)appid;
- (void) authorize: (NSArray *)permissions;
- (void) graphRequest: (NSString *)methodname withParameters:(NSMutableDictionary *)dict andHttpMethod: (NSString *)httpmethod;
- (void) logout;

// FBSessionDelegate
- (void) fbDidLogin;
- (void) fbDidNotLogin: (BOOL)cancelled;
- (void) fbDidLogout;

// FBRequestDelegate
- (void) requestLoading: (FBRequest *)request;
- (void) request: (FBRequest *)request didReceiveResponse: (NSURLResponse *)response;
- (void) request: (FBRequest *)request didFailWithError: (NSError *)error;
- (void) request: (FBRequest *)request didLoad: (id)result;
- (void) request: (FBRequest *)request didLoadRawResponse:(NSData *)data;

- (BOOL) handleOpenURL: (NSURL*)url;
@end

extern FacebookProxy *fbp;
