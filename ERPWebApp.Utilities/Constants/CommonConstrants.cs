using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Utilities.Constants
{
    public class CommonConstants
    {
        public const string DefaultFooterId = "DefaultFooterId";
        public const string DefaultPW = "@123456";
        public const byte defaultLengthNumberCode = 5;
        public const bool WoodProduct_NotIsSimple = false;

        public const string ProductTag = "Product";
        public const string BlogTag = "Blog";
        public class AppRole
        {
            public const string Directors = "BoardOfDirectors";
            public const string HeadOfProcurement = "HeadOfProcurementDepartment";
            public const string AdminRole = "test";
            public const string HeadOfConstruction = "HeadOfConstruction";
            public const string CaptainOfConstruction = "CaptainOfConstruction";
            public const string SupervisorOfConstruction = "SupervisorOfConstruction";
        }
        public class UserClaims
        {
            public const string Roles = "Role";
            public const string Key = "Id";
            public const string FullName = "FullName";
        }
    }
}
