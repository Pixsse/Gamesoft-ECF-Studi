using Microsoft.AspNetCore.Mvc.Rendering;
using static Gamesoft.Services.AccountMgt;

namespace Gamesoft.ViewModels
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }    
}
