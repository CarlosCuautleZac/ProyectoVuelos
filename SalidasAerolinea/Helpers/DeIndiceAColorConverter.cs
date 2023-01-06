using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SalidasAerolinea.Helpers
{
    internal class DeIndiceAColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string color = "";
            int valor = (int)value;
            if (valor % 2 == 0)
            {
                color = "#81C6E8";
            }
            else
                color = "#E3E3E3";

            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
