using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Interface.Web.ViewModels
{
    public class EditSessionViewModel
    {
        public int SessionId { get; set; }
        public string NameOfSessionWithActivity { get; set; }

        [Required]
        [Display(Name = "Aktivitet")]
        public Activity Activity { get; set; }
        [Display(Name = "Namn på tillfälle")]
        [Required]
        public string NameOfSession { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [Display(Name = "Startdatum")]
        public DateTime? StartDate { get; set; }
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [Display(Name = "Slutdatum")]
        public DateTime? EndDate { get; set; }

        public string NameOfLocation { get; set; }
        public int? HrPerson { get; set; }

        public List<Tag> AddedTags { get; set; }
    }
}