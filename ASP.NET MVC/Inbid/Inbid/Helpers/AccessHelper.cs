using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Inbid.Helpers
{
    public class AccessHelper
    {


        public static bool YouCan(Guid userId)
        {
            MembershipUser mu = Membership.GetUser();
            Guid currentUserId = (Guid)mu.ProviderUserKey;
            if (userId == currentUserId)
                return true;
            else
                return false; 
        }

    }
}