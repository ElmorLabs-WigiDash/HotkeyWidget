using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility {
    public static class Utility {
        public static void LoadXaml(Object obj) {
            var type = obj.GetType();
            var assemblyName = type.Assembly.GetName();
            var uristring = string.Format("/{0};v{1};component/{2}.xaml",
                assemblyName.Name,
                assemblyName.Version,
                type.Name);
            var uri = new Uri(uristring, UriKind.Relative);
            System.Windows.Application.LoadComponent(obj, uri);
        }
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }
    }
}