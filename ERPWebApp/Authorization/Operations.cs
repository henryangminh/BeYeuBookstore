﻿using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPWebApp.Authorization
{
    public static class Operations
    {
        public static OperationAuthorizationRequirement Read =
          new OperationAuthorizationRequirement { Name = nameof(Read) };
        public static OperationAuthorizationRequirement Create =
      new OperationAuthorizationRequirement { Name = nameof(Create) };
        public static OperationAuthorizationRequirement Delete =
          new OperationAuthorizationRequirement { Name = nameof(Delete) };
        public static OperationAuthorizationRequirement Update =
            new OperationAuthorizationRequirement { Name = nameof(Update) };
        public static OperationAuthorizationRequirement Confirm =
          new OperationAuthorizationRequirement { Name = nameof(Confirm) };

    }
}
