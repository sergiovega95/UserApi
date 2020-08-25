using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Exceptions
{
    public class UserException:Exception
    {
        public UserException(string description) : base(String.Format("Database error: {0}", description))
        {

        }
    }
}
