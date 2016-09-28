using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Its.Systems.HR.Interface.Web.ViewModels
{
    public class ActivitySummaryViewModel
    {
        public IEnumerable<SelectListItem> Activities { get; set; }
        public SelectList Sessions { get; set; }

    }

    
}