using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Interface.Web.ViewModels
{
    public class ActivityViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Namn på aktivitet krävs")]
        [Display(Name = "Namn")]
        public string Name { get; set; }
    }

    //public class IndexActivityViewModel
    //{
    //    public List<Activity> Type { get; set; }
    //}
}