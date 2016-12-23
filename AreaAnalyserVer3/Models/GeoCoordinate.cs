using System.Data.Entity.Spatial;

namespace AreaAnalyserVer3.ViewModels
{
    internal class GeoCoordinate
    {
        private float latitude;
        private float longitude;

        public GeoCoordinate(float latitude, float longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }

        public static DbGeography CreatePoint(double latitude, double longitude)
        {
            var text = string.Format(System.Globalization.CultureInfo.InvariantCulture.NumberFormat,
                                     "POINT({0} {1})", longitude, latitude);
            // 4326 is most common coordinate system used by GPS/Maps
            return DbGeography.PointFromText(text, 4326);
        }
    }
}