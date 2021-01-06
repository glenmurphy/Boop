# Boop
Uses the North Loop to send key events to Windows; default setup is for Chrome/Google Meet:
- fwd/back cycle tabs
- up/down scrolls
- center press sends Ctrl+D (Mute in Hangouts Meet)

### Build instructions
- Install .NET SDK
- `git clone https://github.com/glenmurphy/boop.git`
- `dotnet add package System.Runtime.WindowsRuntime --version 4.7.0`
- Make sure the Windows.winmd path in Boop.csproj points to the right place
- `dotnet build` or `dotnet run`