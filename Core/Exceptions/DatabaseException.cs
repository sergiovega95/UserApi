using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Exceptions
{
    public class DatabaseException: Exception
    {
        public DatabaseException(string description )
        : base(String.Format("Database error: {0}", description))
        {

        }

    }
}
