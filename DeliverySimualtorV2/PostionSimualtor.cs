using NetTopologySuite.Geometries;
using PolylineEncoder.Net.Models;
using System.Text;
using Decoder = PolylineEncoder.Net.Utility.Decoders.Decoder;


namespace DeliverySimualtorV2;

public static class PositionSimulator
{
    private static readonly Decoder _decoder = new Decoder();
    public static async Task<List<IGeoCoordinate>> GetNewComplexCoordinates(Point vehicleLocation, Point userLocation)
    {
        var polylineToUser = await HttpClientService.GetPolyline(vehicleLocation, userLocation);
        return await GetNewCoordinates(polylineToUser);
    }

    private static async Task<List<IGeoCoordinate>> GetNewCoordinates(string polylineHash)
    {
        var polylineCheckpoints = _decoder.Decode(polylineHash).ToList();

        var distance = 0.087;

        if (polylineCheckpoints.Count < 2)
            throw new ArgumentException("At least two points are required.");

        GeoCoordinate main = new();
        GeoCoordinate referent = new();
        List<IGeoCoordinate> _newCordinates = new();

        main = (GeoCoordinate)polylineCheckpoints[0];

        double newDistance;

        _newCordinates.Add(polylineCheckpoints.First());

        for (int i = 1; i < polylineCheckpoints.Count; i++)
        {
            while (true)
            {
                referent = (GeoCoordinate)polylineCheckpoints[i];

                var newdistance = CalculateDistance(main, referent);

                if (newdistance < distance)
                {
                    main = referent;
                    distance = distance - newdistance;
                    break;
                }


                double distanceBettwen = CalculateDistance(main, referent);
                if (distanceBettwen > distance)
                {
                    main = CreateNewCord(main, referent, distance);
                    _newCordinates.Add(main);
                    if (distance != 0.087)
                        distance = 0.087;

                    if (i == polylineCheckpoints.Count() - 1)
                    {
                        i++;
                        _newCordinates.Add(referent);
                        break;
                    }

                }

            }

        }
        return _newCordinates;

    }

    private static GeoCoordinate CreateNewCord(GeoCoordinate cord1, GeoCoordinate cord2, double? distance)
    {
        if (distance == null)
            distance = 0.027;

        var dist = CalculateDistance(cord1, cord2);

        double angle = CalculateBearing(cord1, cord2);

        (double newX, double newY) = CalculateDestinationPoint(cord1, angle, distance);

        return new GeoCoordinate(newY, newX);
    }

    private static double CalculateDistance(IGeoCoordinate point1, IGeoCoordinate point2)
    {
        double R = 6371.0;
        double lat1 = DegreesToRadians(point1.Latitude);
        double lat2 = DegreesToRadians(point2.Latitude);
        double deltaLat = lat2 - lat1;
        double deltaLon = DegreesToRadians(point2.Longitude) - DegreesToRadians(point1.Longitude);

        double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                   Math.Cos(lat1) * Math.Cos(lat2) *
                   Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        double distance = R * c;

        return distance;
    }

    private static double CalculateBearing(IGeoCoordinate start, IGeoCoordinate end)
    {
        double x = Math.Cos(DegreesToRadians(start.Latitude)) * Math.Sin(DegreesToRadians(end.Latitude)) - Math.Sin(DegreesToRadians(start.Latitude)) * Math.Cos(DegreesToRadians(end.Latitude)) * Math.Cos(DegreesToRadians(end.Longitude - start.Longitude));
        double y = Math.Sin(DegreesToRadians(end.Longitude - start.Longitude)) * Math.Cos(DegreesToRadians(end.Latitude));

        return (Math.Atan2(y, x) + Math.PI * 2) % (Math.PI * 2);
    }

    private static (double, double) CalculateDestinationPoint(IGeoCoordinate start, double bearing, double? distance)
    {
        double radiusEarthKm = 6371.01; // Earth's radius in km
        double distRatio = (double)(distance / radiusEarthKm);
        double distRatioSine = Math.Sin(distRatio);
        double distRatioCosine = Math.Cos(distRatio);

        double startLatRad = DegreesToRadians(start.Latitude);
        double startLonRad = DegreesToRadians(start.Longitude);

        double startLatCos = Math.Cos(startLatRad);
        double startLatSin = Math.Sin(startLatRad);

        double endLatRads = Math.Asin((startLatSin * distRatioCosine) + (startLatCos * distRatioSine * Math.Cos(bearing)));

        double endLonRads = startLonRad + Math.Atan2(Math.Sin(bearing) * distRatioSine * startLatCos, distRatioCosine - startLatSin * Math.Sin(endLatRads));

        return (RadiansToDegrees(endLonRads), RadiansToDegrees(endLatRads));
    }

    private static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }

    private static double RadiansToDegrees(double radians)
    {
        return radians * 180.0 / Math.PI;
    }

}