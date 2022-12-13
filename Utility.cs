using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility {
    public class Utility {
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
    }
}