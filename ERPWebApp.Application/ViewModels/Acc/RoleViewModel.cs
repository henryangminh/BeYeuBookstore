using System;
using System.Collections.Generic;
using System.Text;

namespace ERPWebApp.Application.ViewModels.System
{
    public class RoleViewModel
    {
        public Guid? Id { set; get; }

        public string Name { set; get; }

        public string Description { set; get; }
    }
}
