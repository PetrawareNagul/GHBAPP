﻿using Silverlake.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MimzyCaptureSyncServer.Custom
{
    public class APIUser
    {
        public static User user;

        public static void SetUser(User obj)
        {
            user = obj;
        }

        public static User GetUser()
        {
            return user;
        }
    }
}
