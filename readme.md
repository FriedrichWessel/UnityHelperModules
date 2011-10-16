# asdfunity v0.1.1
[Unity] still has a lot of shortcomings. This library is supposed to make life easier
when developing Unity applications - especially mobile ones.

It’s rather empty right now, but it will grow with time.

# Usage

## Installation
* Initialize the submodules

        $ git submodule init
        $ git submodule update

* From the `Asset` folder copy `Plugins`, `Scripts` and `Editor` to your Unity project

## Facebook module
* Create a new, empty Object in Unity’s inspector and name it “Facebook”
* Attach the Facebook script to it
* Set it App Id in the script’s properties
* Before compiling in XCode, replace “[YOUR APP ID]” with your Facebook app id in `Info.plist` inside the `URL Types` key

Right now the Facebook module is kind of awkward. Please note, that I am aware of this fact ;)
You can call any of the below methods on the script instance which you can access
by calling

  Facebook.getInstance()

### Methods

* `compilePermissions(...)`
  Compiles the given permission flags to a single integer which `authorize()` will accept

* `authorize(Permissions, AuthorizeCallback)`
  Requests the given permissions from the user (see `compilePermissions`).
  `AuthorizeCallback` is a delegate taking an `int` and returning `void`.
  The passed `int` is either `Facebook.LOGGED_IN` or `Facebook.LOGGED_OUT`.

* `postToStream(Text, RequestCallback)`
  `postToStream(Text, Image, Link, Name, Caption, Description, RequestCallback)`
  Posts something to the user’s stream. `RequestCallback` is a delegate taking an `int` and a `string` and returning `void`.
  The integer is either `Facebook.REQUEST_SUCCESS` or `Facebook.REQUEST_FAIL`. The string contains the JSON formatted result or error.

* `logout()`

# ToDos
* Get a proper callback infrastructure running. Using `UnitySendMessage()` is not really the nice way
  to get data back to C#.

# License
Under [Creative Commons 3.0 BY-NC-SA][CC] this is for free.
For 10€ you can get a commercial license. [Contact me][mailto:surma@asdf-systems.de].

# Credit
by Alexander Surma <surma@asdf-systems.de>
[JSONKit] by johnezang


[Unity]: http://www.unity3d.com
[JSONKit]: https://github.com/johnezang/JSONKit
[CC]: http://creativecommons.org/licenses/by-nc-sa/3.0/
