﻿using System.Collections.Generic;
using System.Web.Mvc;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Interface.Web.ViewModels
{
    public class FilterSessionsViewModel
    {
        public List<Session> Sessions { get; set; }
        public IEnumerable<SelectListItem> HrPersons { get; set; }

        public HrPerson HrPerson { get; set; }
    }
}