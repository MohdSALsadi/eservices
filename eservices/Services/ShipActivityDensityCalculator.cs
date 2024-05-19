using Pattern_of_life.Models.Entity;
using Pattern_of_life.Repository;
using System;
using System.Collections.Generic;
namespace Pattern_of_life.Services
{


    public class ShipActivityDensityCalculator
    {

        public ShipActivityDensityCalculator()
        {
        }
        //حساب الشفافية
        // Function to calculate density based on distance between data points
        //distanceThreshold=لتحديد الحد الأقصى للمسافة التي يعتبرها التطبيق بعيدة بما فيه الكفاية ليُظهرها أو يأخذها بعين الاعتبار في الحسابات.

        public Dictionary<Tuple<double, double>, double> CalculateDensity(IEnumerable<ShipActivity> shipActivities, double distanceThreshold)
        {
            Dictionary<Tuple<double, double>, double> densityMap = new Dictionary<Tuple<double, double>, double>();

            // Iterate over each ship activity
            foreach (var activity in shipActivities)
            {
                // Iterate over all other ship activities to calculate distance
                foreach (var otherActivity in shipActivities)
                {
                    if (activity != otherActivity)
                    {
                        // Calculate distance between two points
                        double distance = CalculateDistance(activity.Latitude, activity.Longitude, otherActivity.Latitude, otherActivity.Longitude);

                        // If distance is less than or equal to the threshold, increase density for both points
                        if (distance <= distanceThreshold)
                        {
                            IncreaseDensity(densityMap, activity.Latitude, activity.Longitude);
                            IncreaseDensity(densityMap, otherActivity.Latitude, otherActivity.Longitude);
                        }
                    }
                }
            }

            return densityMap;
        }

        // Function to calculate distance between two points using Haversine formula
        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Earth radius in kilometers
            var dLat = (lat2 - lat1).ToRadians();
            var dLon = (lon2 - lon1).ToRadians();
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1.ToRadians()) * Math.Cos(lat2.ToRadians()) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // Distance in kilometers
            return d;
        }

        // Function to increase density for a given point in the density map
        private void IncreaseDensity(Dictionary<Tuple<double, double>, double> densityMap, double latitude, double longitude)
        {
            Tuple<double, double> point = Tuple.Create(latitude, longitude);
            if (densityMap.ContainsKey(point))
            {
                densityMap[point]++;
            }
            else
            {
                densityMap.Add(point, 0.1);
            }
            //Tuple<double, double> point = Tuple.Create(latitude, longitude);
            double intensity = densityMap.ContainsKey(point) ? densityMap[point] : 0;

            if (intensity <= 0.3)
            {
                intensity += 0.1; // Increase intensity by 0.1 for the yellow range
            }
            else if (intensity <= 0.7)
            {
                intensity += 0.4; // Increase intensity by 0.4 for the orange range
            }
            else
            {
                intensity += 0.3; // Increase intensity by 0.3 for the red range
            }

            densityMap[point] = intensity;
        }
    }

    // Extension method to convert degrees to radians
    public static class DoubleExtensions
    {
        public static double ToRadians(this double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }

}


