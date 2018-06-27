# <img src="https://github.com/IainS1986/PinupMobile/blob/master/PinupMobile/PinupMobile/PinupMobile.Droid/Resources/mipmap-xhdpi/ic_launcher.png" width="60"> Pinup Remote 
Mobile frontend for the Remote Control of PinupPopper

This is a super basic mobile app to interact with the web api created by Nailbuster in PinupPopper.

If you don't know what PinupPopper is then this project will be no use to you! 

You also have to ensure you've followed all the steps with setting up the Web Remote feature of Pinup explained here http://www.nailbuster.com/wikipinup/doku.php?id=web_remote_control

Soon(ish) I'll be getting a copy of both the Android and iOS apps up onto their respective app stores for free so people can grab copied and use it with their VPinCab.

# Architecure Details

* Cross platform app developed in Xamarin, targetting Android and iOS devices.
* MvvmCross 6.0
* PhraseApp used for Translation/Localisation/Strings
* AppCenter for builds and test distribution
* AppCenter Crash Logging
* AppCenter Analytics

Soon will be available on iTunes App Store and Google Play Store


# Features

Currently works with version 1.3.3 of Pinup Popper

* Connects to popper via URL/IP. User friendly setup view. (Popper URL persisted between sessions)
* Displays current wheel icon in app
* Displays name of current game, can tap name to hide/show (persisted between sessions)
* Play, Prev, Next, Page Prev, Page Next controls
* Home button
* Power Button - Supports, Reboot, Shutdown and System Exit
* Exit Emulator button
* Start Game button
* Select Button (allows navigation of playlists)
* Display View - Will play the current Playfield/Backglass/Topper/DMD video on your device
* Record View - Supports start/stop recording Playfield/Backglass/Topper/DMD. Can either start from a game already playing, or launch the game in "Normal" or "Record" mode.

# Icons Explained

Ideally, I went for a minimalist, material design but I'm not sure how well the icons represent what they need too...

<img src="https://github.com/IainS1986/PinupMobile/blob/master/PinupMobile/PinupMobile/PinupMobile.iOS/Assets.xcassets/ic_power.imageset/round_power_settings_new_black_48pt_1x.png"> - Power (System Menu)

<img src="https://github.com/IainS1986/PinupMobile/blob/master/PinupMobile/PinupMobile/PinupMobile.iOS/Assets.xcassets/ic_select.imageset/round_check_circle_black_48pt_1x.png"> - Select

<img src="https://github.com/IainS1986/PinupMobile/blob/master/PinupMobile/PinupMobile/PinupMobile.iOS/Assets.xcassets/ic_back.imageset/round_keyboard_return_black_48pt_1x.png"> - Menu Return

<img src="https://github.com/IainS1986/PinupMobile/blob/master/PinupMobile/PinupMobile/PinupMobile.iOS/Assets.xcassets/ic_page_prev.imageset/round_fast_rewind_black_48pt_1x.png"> - Page Previous

<img src="https://github.com/IainS1986/PinupMobile/blob/master/PinupMobile/PinupMobile/PinupMobile.iOS/Assets.xcassets/ic_prev.imageset/round_skip_previous_black_48pt_1x.png"> - Previous

<img src="https://github.com/IainS1986/PinupMobile/blob/master/PinupMobile/PinupMobile/PinupMobile.iOS/Assets.xcassets/ic_next.imageset/round_skip_next_black_48pt_1x.png"> - Next

<img src="https://github.com/IainS1986/PinupMobile/blob/master/PinupMobile/PinupMobile/PinupMobile.iOS/Assets.xcassets/ic_page_next.imageset/round_fast_forward_black_48pt_1x.png"> - Page Next

<img src="https://github.com/IainS1986/PinupMobile/blob/master/PinupMobile/PinupMobile/PinupMobile.iOS/Assets.xcassets/ic_home.imageset/round_home_black_48pt_1x.png"> - Home

<img src="https://github.com/IainS1986/PinupMobile/blob/master/PinupMobile/PinupMobile/PinupMobile.iOS/Assets.xcassets/ic_play.imageset/round_play_circle_filled_white_black_48pt_1x.png"> - Start Game

<img src="https://github.com/IainS1986/PinupMobile/blob/master/PinupMobile/PinupMobile/PinupMobile.iOS/Assets.xcassets/ic_exit.imageset/round_exit_to_app_black_48pt_1x.png"> - Exit Emulator


# iOS Screenshots

<img src="https://github.com/IainS1986/PinupMobile/blob/master/Wiki/iOS/Screen%20Shot%202018-06-25%20at%2021.51.13.png" width="300"><img src="https://github.com/IainS1986/PinupMobile/blob/master/Wiki/iOS/Screen%20Shot%202018-06-25%20at%2021.51.28.png" width="300"><img src="https://github.com/IainS1986/PinupMobile/blob/master/Wiki/iOS/Screen%20Shot%202018-06-25%20at%2021.51.35.png" width="300"><img src="https://github.com/IainS1986/PinupMobile/blob/master/Wiki/iOS/Screen%20Shot%202018-06-25%20at%2021.51.44.png" width="300"><img src="https://github.com/IainS1986/PinupMobile/blob/master/Wiki/iOS/Screen%20Shot%202018-06-25%20at%2021.51.50.png" width="300">

# Android Screenshots

<img src="https://github.com/IainS1986/PinupMobile/blob/master/Wiki/Android/Screen%20Shot%202018-06-25%20at%2021.58.32.png" width="300"><img src="https://github.com/IainS1986/PinupMobile/blob/master/Wiki/Android/Screen%20Shot%202018-06-25%20at%2021.58.42.png" width="300"><img src="https://github.com/IainS1986/PinupMobile/blob/master/Wiki/Android/Screen%20Shot%202018-06-25%20at%2021.58.56.png" width="300"><img src="https://github.com/IainS1986/PinupMobile/blob/master/Wiki/Android/Screen%20Shot%202018-06-25%20at%2021.59.04.png" width="300"><img src="https://github.com/IainS1986/PinupMobile/blob/master/Wiki/Android/Screen%20Shot%202018-06-25%20at%2021.59.25.png" width="300">
