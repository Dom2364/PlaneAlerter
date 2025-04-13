using System;
using System.Collections.Generic;
using System.Text;

namespace PlaneAlerter.Helpers;

public static class GooglePolylineEncodingHelper
{
    // From https://gist.github.com/shinyzhu/4617989
    // https://developers.google.com/maps/documentation/utilities/polylinealgorithm
    public static string Encode(IEnumerable<double[]> points)
    {
        var str = new StringBuilder();

        var encodeDiff = (Action<int>)(diff =>
        {
            var shifted = diff << 1;
            if (diff < 0)
                shifted = ~shifted;

            var rem = shifted;

            while (rem >= 0x20)
            {
                str.Append((char)((0x20 | (rem & 0x1f)) + 63));

                rem >>= 5;
            }

            str.Append((char)(rem + 63));
        });

        var lastLat = 0;
        var lastLng = 0;

        foreach (var point in points)
        {
            var lat = (int)Math.Round(point[0] * 1E5);
            var lng = (int)Math.Round(point[1] * 1E5);

            encodeDiff(lat - lastLat);
            encodeDiff(lng - lastLng);

            lastLat = lat;
            lastLng = lng;
        }

        return str.ToString();
    }
}