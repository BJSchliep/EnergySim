using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace EnergySim
{
    public static class Images
    {
        public readonly static ImageSource Empty = LoadImage("Empty.png");
        public readonly static ImageSource Biomass = LoadImage("Biomass.png");
        public readonly static ImageSource Business = LoadImage("Business.png");
        public readonly static ImageSource Geothermal = LoadImage("Geothermal.png");
        public readonly static ImageSource House = LoadImage("House.png");
        public readonly static ImageSource Hydroelectric = LoadImage("Hydroelectric.png");
        public readonly static ImageSource Nuclear = LoadImage("Nuclear.png");
        public readonly static ImageSource Solar = LoadImage("Solar.png");
        public readonly static ImageSource Turbine = LoadImage("Turbine.png");

        public static ImageSource LoadImage(string path)
        {
            return new BitmapImage(new Uri($"Assets/{path}", UriKind.Relative));
        }
    }
}
