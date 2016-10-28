using System.ComponentModel.DataAnnotations;

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