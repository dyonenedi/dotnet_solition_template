
using System.ComponentModel.DataAnnotations;

namespace app.blazor.UI.ViewModels.Feed
{
    public class FeedViewModel
    {
        [StringLength(200, ErrorMessage = "O post não pode exceder 100 caracteres.")]
        public string text { get; set; } = string.Empty;
    }
}