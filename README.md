[Xamarin.iOS with Xamarin Studio](https://speakerdeck.com/patridge/xamarin-dot-ios-with-xamarin-studio), Demo Code
======================

This is the final code produced from the talk for attendee reference, not any sort of step-by-step tutorial. That said, I will put down some rough notes of how we got to these results.

*[Slides](https://speakerdeck.com/patridge/xamarin-dot-ios-with-xamarin-studio)*

##Demo 1: UI Creation and Debugging code

Steps followed to get here:

1. Create new project, explained all the project files and what they do.
1. Initial creation of UI element and outlet in Xcode.
1. Tie in to outlet's event in Xamarin Studio.
1. Demo functionality.
1. Creation of UI element (and shift of existing) using C# in Xamarin Studio.
1. Tie in to created element event in Xamarin Studio.
1. Demo functionality.

##Demo 2: Component Store: SQLite.NET and Google Maps[, and Progress HUD]

1. Start from pre-created demo.
1. Walked through Google Maps integration. (Note: To get this to work, you will need your own [Google Maps API key](https://code.google.com/apis/console/) in `AppDelegate.cs`)
1. Walked through SQLite DB creation and initial import.
1. Demo functionality.
1. [Audience question about progress dialog.]
1. Added Progress HUD component and wrote out sample code.
1. Demo functionality.