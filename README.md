# NovaFori

**How long did I spend on my solution?**
Over the course of 3 nights, probably around 5/6 hours.

**How do you build and run your solution?**
Clone this repo into Visual Studio and run from there.
Target Framework is .Net Core v3.1
The Following NugetPackages are needed:
- Microsoft.EntityFrameworkCore.InMemory
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools

**What technical and functional assumptions did you make when implementing
your solution?**
- User should be able to enter a new todo item
- User should be able to mark item as pending/completed - causing it to appear in the relative list
- A mix of knockout.js and ajax will facilate the API calls
- A mix of .NET and Entity Framework will facilate endpoint implementation and data managemeny


**Briefly explain your technical design and why do you think is the best
approach to this problem.**
- In Memory Entity Framework - easy solution to mock a normal DB as I knew the data didn't have to persist
- .Net Web API controllers - perfect for CRUD app, connected to the front end via ajax calls. It then makes necessary changes to in-memory DB
- Knockout.js - could easily connect adding new items to an ajax call that would post to api controller. ObservableArrays also worked well in dynamically updating both lists of items
