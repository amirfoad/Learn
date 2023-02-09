using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Core.Genarator
{
    public class NameGenarator
    {
        public static string GenarateUniqCode()
        {
            return Guid.NewGuid().ToString().Replace("-","");
        }
    }
}
