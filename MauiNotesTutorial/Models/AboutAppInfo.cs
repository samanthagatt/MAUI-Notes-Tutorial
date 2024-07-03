namespace MauiNotesTutorial.Models;

public class AboutAppInfo
{
    public string Title => AppInfo.Name;
    public string Version => AppInfo.VersionString;
    public string MoreInfoUrl => "https://apple.com";
    public string Message => "This app is written in XAML and C# with .NET MAUI.";
}
