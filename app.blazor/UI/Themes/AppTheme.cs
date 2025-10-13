using MudBlazor;

namespace app.blazor.UI.Themes;

public static class AppTheme
{
    public static MudTheme Theme = new MudTheme()
    {
        PaletteLight = new PaletteLight()
        {
            Primary = "#FF3E8F",
            Secondary = Colors.Gray.Darken3,
            Background = "#202436",
            Surface = "#131631",
            Dark = Colors.Shades.Black,
            AppbarBackground = "#050A1E",
            AppbarText = Colors.Shades.White,
            DrawerBackground = "#050A1E",
            DrawerText = Colors.Shades.White,
            DrawerIcon = Colors.Shades.White,
            TextPrimary = Colors.Shades.White,
            TextSecondary = Colors.Gray.Darken3,
            TextDisabled = Colors.BlueGray.Lighten4,

            Success = Colors.Green.Default,
            Error = Colors.Red.Default,
            Warning = Colors.Orange.Default,
            Info = Colors.Blue.Default,
        },
        PaletteDark = new PaletteDark()
        {
            Primary = "#50048c",
            Secondary = Colors.Gray.Darken2,
            Background = "#202436",
            Surface = "#FFFFFF",
            Dark = Colors.Shades.Black,
            AppbarBackground = "#050A1E",
            AppbarText = Colors.Shades.White,
            DrawerBackground = "#050A1E",
            DrawerText = Colors.Shades.White,
            DrawerIcon = Colors.Shades.White,
            TextPrimary = Colors.Gray.Darken4,
            TextSecondary = Colors.Gray.Darken3,
            TextDisabled = Colors.BlueGray.Lighten4,

            Success = Colors.Green.Default,
            Error = Colors.Red.Default,
            Warning = Colors.Orange.Default,
            Info = Colors.Blue.Default,
        }
    };
}
