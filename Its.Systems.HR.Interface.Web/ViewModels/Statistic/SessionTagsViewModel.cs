﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Interface.Web.ViewModels.Statistic
{
    public class SessionTagsViewModel
    {
        public String tagName { get; set; }
        public List<Session> Sessions { get; set; }

        //public Tag tag { get; set; }

        public IEnumerable<SelectListItem> Tags { get; set; }

    }
}