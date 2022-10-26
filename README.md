
<div align="center">
  <img src="WhatToDo/WhatToDo/WhatToDo/Resources/Images/WhatTodo_Logos/logo_transparent.png" alt="logo" width="200" height="auto" />
  <h1>WhatToDo App</h1>  
  <p>
    Productivity App that provides task tracking and activity suggestions.
  </p>

<p>
  <a href="https://github.com/KindaSorta/WhatToD/graphs/contributors">
    <img src="https://img.shields.io/github/contributors/KindaSorta/WhatToD" alt="contributors" />
  </a>
  <a href="">
    <img src="https://img.shields.io/github/last-commit/KindaSorta/WhatToD" alt="last update" />
  </a>
  <a href="https://github.com/KindaSorta/WhatToD/network/members">
    <img src="https://img.shields.io/github/forks/KindaSorta/WhatToD" alt="forks" />
  </a>
  <a href="https://github.com/KindaSorta/WhatToD/stargazers">
    <img src="https://img.shields.io/github/stars/KindaSorta/WhatToD" alt="stars" />
  </a>
  <a href="https://github.com/KindaSorta/WhatToD/issues/">
    <img src="https://img.shields.io/github/issues/KindaSorta/WhatToD" alt="open issues" />
  </a>
  <a href="https://github.com/KindaSorta/WhatToD/blob/master/LICENSE">
    <img src="https://img.shields.io/github/license/KindaSorta/WhatToD" alt="license" />
  </a>
</p>

<h4>
    <a href="https://github.com/KindaSorta/WhatToD">View Demo</a>
  <span> · </span>
    <a href="https://github.com/KindaSorta/WhatToD">Documentation</a>
  <span> · </span>
    <a href="https://github.com/KindaSorta/WhatToDo/issues/">Report Bug</a>
  <span> · </span>
    <a href="https://github.com/KindaSorta/WhatToDo/issues/">Request Feature</a>
  </h4>
</div>

<br />

## :notebook_with_decorative_cover: Table of Contents

 <details>
   <summary>Table of Contents</summary>
   <ol>
     <li><a href="#information_source-general-information">General Info</a></li>
     <li><a href="#desktop_computer-platforms">Platforms</a></li>
     <li><a href="#toolbox-technologies-used">Technologies Used</a></li>
     <li><a href="#project-status">Project Status</a></li>
     <li><a href="#start2-features">Features</a></li>
     <li><a href="#compass-roadmap">Roadmap</a></li>
     <li><a href="#wave-contributing">Contributing</a></li>
     <li><a href="#scroll-code-of-conduct">Code of Conduct</a></li>
     <li><a href="#warning-room-for-improvement">Room For Improvement</a></li>
     <li><a href="#handshake-contact">Contact</a></li>
     <li><a href="#gem-acknowledgments">Acknowledgments</a></li>
   </ol>
 </details>

 <br />


## :information_source: General Information

 #### About
 - The initial intent of the app is to help improve productivity, simplify some processes, and generally improve well being.
 - It will utilize task creation, tracking, and provide metrics
     - Track progress
     - Get task reminders
     - Improve efficient use of time
     - Learn to balance time between needs and wants
 - Provide smart suggestions for tasks and activities
     - Get suggestions regarding when and what to do from ToDo List
     - Get suggestions of other activities to do based on your task history, preferences, interests, and current situation
 - Will have include additional productivity features

 #### :desktop_computer: Platforms
   - Android 
   - Windows (Untested)
   - iOS (Untested)

 #### :toolbox: Technologies Used
   - [.NET Maui](https://github.com/dotnet/maui)
   - [.NET Community Toolkit](https://github.com/CommunityToolkit/dotnet)
   - [LiteDB](https://www.litedb.org)
   - [Fluent Assertions](https://github.com/fluentassertions/fluentassertions)
   - [xUnit](https://github.com/xunit/xunit)
   - [Moq](https://github.com/moq/moq)
   - [AutoFixtures](https://github.com/AutoFixture/AutoFixture)

 #### Project Status
   Project is: _in progress_.

 <br />

## :star2: Features

 - [x] Mobile Shell Navigation
 - [x] Location information with GPS and Weather API

 <p align="left">
     <img src="WhatToDo/WhatToDo/WhatToDo/Resources/Images/ScreenShots/WhatToDo_Demo_Navigation.png" width="250" height="550"> &nbsp;&nbsp;&nbsp; 
     <img src="WhatToDo/WhatToDo/WhatToDo/Resources/Images/ScreenShots/WhatToDoApp_Demo_Weather.gif" width="250" height="550"> 
 </p>
 <br />

 - [x] Task List
     - Task Cards display task title, description, and dates
     - Incremented based on selected filter order
     - Color coded based on priority level
 - [x] Task List Filter
     - Current Filters Options; Recent, Upcoming, and Priority
 - [x] Competing Tasks and Viewing History

 <p align="left">
     <img  src="WhatToDo/WhatToDo/WhatToDo/Resources/Images/ScreenShots/WhatToDo_Demo_TaskList.png" width="250" height="550"> &nbsp;&nbsp;&nbsp; 
     <img  src="WhatToDo/WhatToDo/WhatToDo/Resources/Images/ScreenShots/WhatToDoApp_Demo_Filtering.gif" width="250" height="550"> &nbsp;&nbsp;&nbsp; 
     <img src="WhatToDo/WhatToDo/WhatToDo/Resources/Images/ScreenShots/WhatToDoApp_Demo_Completing.gif" width="250" height="550">
 </p>
 <br />

 - [x] Task Details
     - Task Title Entry (Required)
     - Task Description Editor
     - Optional Due Date and Start Date
         - Defaults to current time
         - Due Date must occur after current time, and after Start Date if present
         - Start Date must occur before Due Date
     - Optional Weather Preference
         - Update desired weather of task to get suggestions and notifications of best time
 - [x] Adding a new Task
 - [x] Editing and Deleting Tasks

 <p align="left">
     <img  src="WhatToDo/WhatToDo/WhatToDo/Resources/Images/ScreenShots/WhatToDo_Demo_TaskDetails.png" width="250" height="550"> &nbsp;&nbsp;&nbsp; 
     <img  src="WhatToDo/WhatToDo/WhatToDo/Resources/Images/ScreenShots/WhatToDoApp_Demo_HalloweenTask.gif" width="250" height="550"> &nbsp;&nbsp;&nbsp; 
     <img src="WhatToDo/WhatToDo/WhatToDo/Resources/Images/ScreenShots/WhatToDoApp_Demo_EditTasks.gif" width="250" height="550">
 </p>

 <br />

## :compass: Roadmap

 - [ ] Add Splash Page
 - [ ] Improve the Task Items
     - [ ] Task dependencies on other tasks
     - [ ] Recurring events
     - [ ] Tagging system for improved lookups and groupings
 - [ ] Task Filter Functionality
     - [ ] Keyword search
     - [ ] Filtering based on Tags
 - [ ] Task List
     - [ ] Limit info displayed on Task Cards
     - [ ] Double tap to expand card details
     - [ ] Press to select multiple items for deletion or completion
 - [ ] Customizable animation / sound for task completion
 - [ ] User Profile Page
     - Metrics, contact Info, credentials
 - [ ] Create App Settings Page
     - Permissions, notification Settings, file storage
 - [ ] Create Note System
     - [ ] Templates
     - [ ] Category System
 - [ ] Create Interval Timer
 - [ ] Create Random Picker
 - [ ] Login Page
 - [ ] Location Based Notifications
     - Get reminders and suggestions based on location
 - [ ] Create ASP.NET Core Server to allow communication and user data backup
 - [ ] Allow creation of groups for users
     - Ex. Your home group could share chores, grocery lists, calendars, etc. 
 - [ ] Allow Task sharing
     - Between users, groups, and privately
 - [ ] Suggestions feature
     - Get activity suggestions based on friends list
 - [ ] Subscribe to activities
     - Makes it easier to setup and coordinate

 <br />


## :warning: Room for Improvement

 #### :heavy_check_mark: Todo List:
 - Need to clean up and standardize my XAML components
 - Looking into the [Prism Library](https://prismlibrary.com/docs/maui/index.html)
 - Going to add the [.NET MAUI Community Toolkit](https://github.com/CommunityToolkit/Maui) for the optimized components and handlers
 - Test coverage
 - Integration tests for key features
 - Utilize Benchmarking to identify performance bottlenecks
 - Implement UI testing, possibly with [Xappium](https://xappium.com)
 - Improve customization of filter criteria

 #### :exclamation: Current Limitations:
 - Limited ability to run background tasks in .NET Maui
     - Needed for tracking task info while app is closed
         - Location tracking
         - System Notifications
         - Timers
 - No native support for Notifications in .NET Maui
     - Possible solution using third party library [Plugin.LocalNotification](https://github.com/thudugala/Plugin.LocalNotification)
 - Focus and Unfocused functionality a bit buggy according to GitHub ticket
 - No access to Apple emulators
  
 <br />


## :wave: Contributing

 Contributions are always welcome!
 See `contributing.md` for ways to get started. 

 #### :scroll: Code of Conduct 
   Please read the [Code of Conduct](https://github.com/Louis3797/awesome-readme-template/blob/master/CODE_OF_CONDUCT.md)

 <br />


## :handshake: Contact

 Created by [@KindaSorta](https://www.linkedin.com/in/gregory-salopek0/)
 <br />


## :gem: Acknowledgements

 - This project was inspired by several tutorials by [James Montemagno](https://montemagno.com).
 - Many thanks to [Gerald Versluis](https://blog.verslu.is) for the detailed [video tutorials](https://youtube.com/playlist?list=PLfbOp004UaYWu-meDkRN6_Y1verl96npI) and support offered in his discord channel
 - All app Icons are from [Icon8](https://icons8.com)
 - [Shields.io](https://shields.io/)
 - [Awesome README](https://github.com/matiassingers/awesome-readme)
 - [Emoji Cheat Sheet](https://github.com/ikatyang/emoji-cheat-sheet/blob/master/README.md#travel--places)
 - [Readme Template](https://github.com/othneildrew/Best-README-Template)

 <br />

