using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfApp1
{
    public class DoubleArrayToPointsConverter : IValueConverter
    {
        private double width = 100;
        private double scale = 1;
        private double offset = 1;

        public double Width
        {
            get { return width; }
            set { width = value; }
        }

        public double Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public double Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double[] values = value as double[];
            if (values == null)
            {
                throw new ArgumentException("value", "Must be double[]");
            }

            PointCollection points = new PointCollection(values.Length);
            for (int i = 0; i < values.Length; i++)
            {
                double x = i * Width / values.Length;
                double y = values[i] * Scale + Offset;
                points.Add(new System.Windows.Point(x, y));
            }
            return points;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }
}
