﻿using System.Web;
using System.Web.Mvc;

namespace OzElectric_EmployeeManagement
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
