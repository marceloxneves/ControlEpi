using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace TitansMVC.Helpers
{
    public static class JsonExtensions
    {
        public static string ToJson(this Object obj)
        {
            return new JavaScriptSerializer().Serialize(obj);
        }
    }
}