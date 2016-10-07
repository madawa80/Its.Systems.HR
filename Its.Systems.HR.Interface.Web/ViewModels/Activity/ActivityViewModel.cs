using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Its.Systems.HR.Interface.Web.ViewModels
{
    public class ActivityViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Namn på aktivitet krävs")]
        [Display(Name = "Namn")]
        public string Name { get; set; }
    }
    
}