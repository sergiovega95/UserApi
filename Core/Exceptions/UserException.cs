using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Exceptions
{
    [Serializable()]
    public class UserException:Exception
    {        
        public UserException(string description) : base(String.Format(description))
        {

        }
    }
}
