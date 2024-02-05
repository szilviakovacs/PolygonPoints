using System;
using System.Collections.Generic;
using System.Xml;

namespace PolygonAreaCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello!");
            List<Point> polygonPoints = ReadPolygonFromXml("C:/work/polygonpoints/polygon.xml");

            Point testPoint = GetUserInputPoint();

            bool isInside = IsPointInsidePolygon(testPoint, polygonPoints);

            Console.WriteLine(isInside ? "The point is inside the polygon area." : "The point is outside the polygon area.");
        }

        static List<Point> ReadPolygonFromXml(string filePath)
        {
            List<Point> points = new List<Point>();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);

                XmlNodeList pointNodes = doc.SelectNodes("/ConturPoints/Point");
                foreach (XmlNode node in pointNodes)
                {
                    double x = Convert.ToDouble(node.SelectSingleNode("X").InnerText);
                    double y = Convert.ToDouble(node.SelectSingleNode("Y").InnerText);
                    points.Add(new Point(x, y));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error reading XML: {e.Message}");
            }

            return points;
        }

        static Point GetUserInputPoint()
        {
            Console.Write("Enter the X coordinate of the test point: ");
            double x = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter the Y coordinate of the test point: ");
            double y = Convert.ToDouble(Console.ReadLine());

            return new Point(x, y);
        }

        static bool IsPointInsidePolygon(Point testPoint, List<Point> polygonPoints)
        {
            //Ray Casting algorithm (Winding Number ? )
            int count = 0;
            int n = polygonPoints.Count;

            for (int i = 0, j = n - 1; i < n; j = i++)
            {
                if (((polygonPoints[i].Y > testPoint.Y) != (polygonPoints[j].Y > testPoint.Y)) &&
                    (testPoint.X < (polygonPoints[j].X - polygonPoints[i].X) * (testPoint.Y - polygonPoints[i].Y) / (polygonPoints[j].Y - polygonPoints[i].Y) + polygonPoints[i].X))
                {
                    count++;
                }
            }

            return count % 2 == 1;
        }
    }

    class Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
