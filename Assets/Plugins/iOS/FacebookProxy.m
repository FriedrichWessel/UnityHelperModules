#import "FacebookProxy.h"
#import "JSONKit.h"

FacebookProxy *fbp = NULL;

static NSArray *decompilePermissions(int permissions);
static NSMutableDictionary *ArrayToDictionary(char **arr);
@implementation FacebookProxy

- (id) initWithAppId: (NSString *)appid {
	self = [super init];
	fb = [[Facebook alloc] initWithAppId: appid andDelegate: self];
	return self;
}

- (void) authorize: (NSArray *)permissions {
	NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
	if ([defaults objectForKey:@"FBAccessTokenKey"] && [defaults objectForKey:@"FBExpirationDateKey"]) {
        fb.accessToken = [defaults objectForKey:@"FBAccessTokenKey"];
        fb.expirationDate = [defaults objectForKey:@"FBExpirationDateKey"];
	}
	if (![fb isSessionValid])  {
	    [fb authorize: permissions];
	} else {
		[self fbDidLogin];
	}
}

- (void) graphRequest: (NSString *)methodname withParameters:(NSMutableDictionary *)dict andHttpMethod: (NSString *)httpmethod {
	[fb requestWithGraphPath: methodname andParams: dict andHttpMethod: httpmethod andDelegate: self];
}

- (void) logout {
	[fb logout: self];
}

- (void) fbDidLogin {
    NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
    [defaults setObject:[fb accessToken] forKey:@"FBAccessTokenKey"];
    [defaults setObject:[fb expirationDate] forKey:@"FBExpirationDateKey"];
    [defaults synchronize];
	UnitySendMessage("Facebook", "loggedIn", "");
}

- (void) fbDidNotLogin: (BOOL)cancelled {
	UnitySendMessage("Facebook", "loggedOut", cancelled?"y":"n");
}

- (void) fbDidLogout {
	UnitySendMessage("Facebook", "loggedOut", "");
}

- (void) requestLoading: (FBRequest *)request {
}

- (void) request: (FBRequest *)request didReceiveResponse: (NSURLResponse *)response {
}

- (void) request: (FBRequest *)request didFailWithError: (NSError *)error {
	UnitySendMessage("Facebook", "requestFailed", [[[error userInfo] JSONString] UTF8String]);
}

- (void) request: (FBRequest *)request didLoad: (id)result {
}

- (void) request: (FBRequest *)request didLoadRawResponse:(NSData *)data {
	UnitySendMessage("Facebook", "requestSucceeded", (char*)[data bytes]);
}

- (BOOL) handleOpenURL: (NSURL*)url {
	return [fb handleOpenURL: url];
}
@end

void _init(char *appid) {
	if(fbp != NULL) {
		return;
	}
	NSString *nsappid = [NSString stringWithUTF8String: appid];
	fbp = [[FacebookProxy alloc] initWithAppId: nsappid];
	// [nsappid release];
}

void _authorize(int permissions) {
	if(fbp == NULL) {
		return;
	}
	NSArray *perms = decompilePermissions(permissions);
	[fbp authorize: perms];
	// [perms release];
}

void _graphRequest(char *methodname, char **param, char *httpmethod) {
	if(fbp == NULL) {
		return;
	}
	NSMutableDictionary *dict = ArrayToDictionary(param);
	NSString *nsmethodname = [NSString stringWithUTF8String: methodname];
	NSString *nshttpmethod = [NSString stringWithUTF8String: httpmethod];
	[fbp graphRequest: nsmethodname withParameters: dict andHttpMethod: nshttpmethod];
}

void _logout() {
	if(fbp == NULL) {
		return;
	}
	[fbp logout];
}

void _deleteSession() {
    NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
	[defaults removeObjectForKey: @"FBAccessTokenKey"];
	[defaults removeObjectForKey: @"FBExpirationDateKey"];
    [defaults synchronize];
}

static NSArray *decompilePermissions(int permissions) {
	static NSString *values[3] = {
		@"publish_stream",
	};
	int i;
	NSMutableArray *res = [NSMutableArray array];
	for(i = 0; i < 3; i++) {
		// If the i-th bit is set...
		if((permissions&(1<<i)) != 0) {
			NSString* perm = values[i];
			[res addObject: perm];
		}
	}
	return res;
}

static NSMutableDictionary* ArrayToDictionary(char** arr) {
	NSMutableDictionary *dict = [NSMutableDictionary dictionary];
	for(int i = 0; arr[2*i] != NULL; i++) {
		NSString *key = [NSString stringWithUTF8String: arr[2*i]];
		NSString *val = [NSString stringWithUTF8String: arr[2*i+1]];
		[dict setObject: val forKey:key];
	}
	return dict;

}
