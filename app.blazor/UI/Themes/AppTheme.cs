using MudBlazor;

namespace app.blazor.UI.Themes;

public static class AppTheme
{
    public static MudTheme Theme = new MudTheme()
    {
        PaletteLight = new PaletteLight()
        {
            Primary = "50048c",
            Secondary = Colors.Gray.Darken3,
            Background = "#202436",
            Surface = Colors.Shades.White,
            AppbarBackground = "#050A1E",
            AppbarText = Colors.Shades.White,
            DrawerBackground = "#050A1E",
            DrawerText = Colors.Shades.White,
            DrawerIcon = Colors.Shades.White,
            TextPrimary = Colors.Gray.Darken4,
            TextSecondary = Colors.Gray.Darken3,

            Success = Colors.Green.Default,
            Error = Colors.Red.Default,
            Warning = Colors.Orange.Default,
            Info = Colors.Blue.Default,
        },
        PaletteDark = new PaletteDark()
        {
            Primary = "50048c",
            Secondary = Colors.Gray.Darken2,
            Background = "#202436",
            Surface = Colors.Shades.White,
            AppbarBackground = "#050A1E",
            AppbarText = Colors.Shades.White,
            DrawerBackground = "#050A1E",
            DrawerText = Colors.Shades.White,
            DrawerIcon = Colors.Shades.White,
            TextPrimary = Colors.Gray.Darken4,
            TextSecondary = Colors.Gray.Darken3,

            Success = Colors.Green.Default,
            Error = Colors.Red.Default,
            Warning = Colors.Orange.Default,
            Info = Colors.Blue.Default,
        }
    };
}
