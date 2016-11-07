using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Interface.Web.ViewModels
{
    public class CreateSessionViewModel
    {
        [Display(Name = "Namn på tillfälle")]
        [Required]
        public string Name { get; set; }
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [Display(Name = "Startdatum")]
        public DateTime? StartDate { get; set; }
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [Display(Name = "Slutdatum")]
        public DateTime? EndDate { get; set; }
        [AllowHtml]
        [Display(Name = "Beskrivning (html tillåtet)")]
        public string Description { get; set; }
        [Display(Name = "Är tillfället öppet för intresseanmälningar?")]
        public bool IsOpenForExpressionOfInterest { get; set; }
        public string NameOfLocation { get; set; }
        public int? HrPerson { get; set; }
        [Required]
        public Activity Activity { get; set; }
        public List<SessionParticipant> SessionParticipants { get; set; }
        public string AddedParticipants { get; set; }
        public string AddedTags { get; set; }

        public List<Tag> GenerateSessionTags
        {
            get
            {
                if (AddedTags == null)
                    return new List<Tag>();


                var sessionTags = AddedTags.Split(',');
                for (var i = 0; i < sessionTags.Length; i++)
                    sessionTags[i] = sessionTags[i].Trim().ToLower();

                var result = new List<Tag>();
                foreach (var tag in sessionTags.Distinct())
                {
                    if (tag.Length < 1) continue;

                    result.Add(new Tag
                    {
                        Name = tag.ToLower()
                    });
                }

                return result;
            }
        }
    }
}