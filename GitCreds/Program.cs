using GitCreds.Settings;
using Meziantou.Framework.Win32;

var settingsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");
if (args.Length != 0)
{
    if (args[0] == "-help" || args[0] == "-h")
    {
        Console.WriteLine("Switch Github credentials.");
        Console.WriteLine("Usage: gitcreds.exe [filePath]");
        Console.WriteLine(@"E.g. gitcreds.exe d:\work\settings.json");
        Console.WriteLine("Otherwise will look in current folder.");

        Console.WriteLine("-help or -h\tDisplay this message.");
        return;
    }

    settingsFile = args[0];
}
else
{
    if (!File.Exists(settingsFile))
    {
        Console.WriteLine($"No settings file found in {AppDomain.CurrentDomain.BaseDirectory}.\tUse -h for help.");
        return;
    }
}

var settings = Settings.Read(settingsFile);

if (settings == null)
{
    Console.WriteLine("Failed to load settings file.");
    return;
}
foreach (var r in from r in settings.TargetsToRemove
                  let creds = CredentialManager.EnumerateCredentials()
                  from c in creds
                  where string.Equals(c.ApplicationName, r.Target)
                  select r)
{
    Console.WriteLine($"Removing target: '{r.Target}' from Windows Credentials");
    CredentialManager.DeleteCredential(applicationName: r.Target);
}

var profiles = settings?.Profiles;
if(profiles == null || !profiles.Any())
{
    Console.WriteLine($"No profiles obtained from {settingsFile}");
    return;
}
var currentProfile = profiles.FirstOrDefault(x => x.Active);
if (currentProfile == null)
{
    Console.WriteLine($"No active profile found in {settingsFile}");
    return;
}

Console.WriteLine($"Switching to profile: {currentProfile.Name} - {currentProfile.Email}");

var gitCmds = new List<string>();
if (!string.IsNullOrEmpty(currentProfile.Name))
{
    gitCmds.Add($"/C git config --global user.name \"{currentProfile.Name}\"");
}
if (!string.IsNullOrEmpty(currentProfile.Email))
{
    gitCmds.Add($"/C git config --global user.email \"{currentProfile.Email}\"");
}

foreach (var c in gitCmds)
{
    Console.WriteLine($"Executing: {c}");
    System.Diagnostics.Process.Start("CMD.exe", c);
}

// if two profiles defined - switch between them each time we run this
if(profiles.Count == 2)
{
    foreach (var p in profiles)
    {
        p.Active = !p.Active;
    }

    settings.Save(settingsFile);

    var next = profiles.First(x => x.Active);
    Console.WriteLine($"Next run active profile will be: {next.Name} - {next.Email}");
}

Console.ReadLine();
