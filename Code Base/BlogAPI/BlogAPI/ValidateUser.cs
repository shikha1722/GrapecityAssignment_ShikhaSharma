using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccess;

namespace BlogAPI
{
    public class ValidateUser
    {
        public static bool Login(string username, string password)
        {
            using (BlogEntities entities = new BlogEntities())
            {
                return entities.Users.Any(user =>user.Username.Equals(username, StringComparison.OrdinalIgnoreCase)
                                          && user.Password == password);
            }
        }
    }
}