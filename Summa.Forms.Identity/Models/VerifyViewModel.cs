using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OpenIddict.Abstractions;

namespace Summa.Forms.Identity.Models
{
    public class VerifyViewModel
    {
        [Display(Name = "Application")]
        public string ApplicationName { get; set; }

        [BindNever, Display(Name = "Error")]
        public string Error { get; set; }
        
        [BindNever, Display(Name = "Error description")]
        public string ErrorDescription { get; set; }

        [Display(Name = "Scope")]
        public string Scope { get; set; }

        [FromQuery(Name = OpenIddictConstants.Parameters.UserCode)]
        [Display(Name = "User code")]
        public string UserCode { get; set; }
    }
}
