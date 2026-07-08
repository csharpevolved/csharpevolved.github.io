Platform compatibility analysis deserves its own callout because it teaches safer use of OS-specific APIs instead of leaving those rules to tribal knowledge.

```csharp
using Microsoft.Win32;

// Before
public static class BeforePlatformSamples
{
    public static object? ReadInstallPath()
    {
        return Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Contoso", "InstallPath", null);
    }
}

// After
public static class AfterPlatformSamples
{
    public static object? ReadInstallPath()
    {
        if (!System.OperatingSystem.IsWindows())
        {
            return null;
        }

        return Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Contoso", "InstallPath", null);
    }
}
```

`CA1416` is the official rule to watch here, backed by target-framework context, platform attributes, and runtime guards.
