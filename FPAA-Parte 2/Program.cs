using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ConvexHull
{
    class Program
    {
        static void Main(string[] args)
        {
            int initialPoints = 10000;
            int maxTimeSeconds = 10;
            int numSets = 50;

            Console.WriteLine("Executando algoritmo da envoltória convexa...");
            Console.WriteLine("Quantidade de pontos inicial: " + initialPoints);

            for (int i = 0; i < numSets; i++)
            {
                int points = initialPoints * (int)Math.Pow(2, i);
                List<Point> pointSet = GerarPontos(points);

                Stopwatch stopwatch = Stopwatch.StartNew();
                List<Point> convexHull = OrdenarPontos(pointSet);
                int convexHullPoints = convexHull.Count; // Quantidade de pontos no envoltório convexo
                stopwatch.Stop();
                long elapsedTime = stopwatch.ElapsedMilliseconds;

                Console.WriteLine("\nConjunto " + (i + 1) + ":");
                Console.WriteLine("Quantidade de pontos: " + points);
                Console.WriteLine("Tempo de execução: " + elapsedTime + " ms");
                Console.WriteLine("Quantidade de pontos no envoltório convexo: " + convexHullPoints);

                if (elapsedTime >= maxTimeSeconds * 1000)
                {
                    Console.WriteLine("O algoritmo excedeu o tempo máximo de " + maxTimeSeconds + " segundos. Encerrando.");
                    break;
                }
            }

            Console.ReadLine();
        }

        static List<Point> GerarPontos(int quantity)
        {
            Random random = new Random();
            List<Point> points = new List<Point>();

            for (int i = 0; i < quantity; i++)
            {
                double x = random.NextDouble() * 100;
                double y = random.NextDouble() * 100;
                points.Add(new Point(x, y));
            }

            return points;
        }

        static List<Point> OrdenarPontos(List<Point> points)
        {
            points.Sort((p1, p2) =>
            {
                int cmpX = p1.X.CompareTo(p2.X);
                return cmpX != 0 ? cmpX : p1.Y.CompareTo(p2.Y);
            });

            List<Point> envoltoriaInferior = EnvoltoriaInferiorSuperior(points);
            List<Point> envoltoriaSuperior = EnvoltoriaInferiorSuperior(points.AsEnumerable().Reverse().ToList());

            envoltoriaInferior.AddRange(envoltoriaSuperior.GetRange(1, envoltoriaSuperior.Count - 2));
            return envoltoriaInferior;
        }

        static List<Point> EnvoltoriaInferiorSuperior(List<Point> points)
        {
            List<Point> hull = new List<Point>();

            foreach (Point point in points)
            {
                while (hull.Count >= 2 && CCW(hull[hull.Count - 2], hull[hull.Count - 1], point) <= 0)
                {
                    hull.RemoveAt(hull.Count - 1);
                }

                hull.Add(point);
            }

            return hull;
        }

        static double CCW(Point p1, Point p2, Point p3)
        {
            return (p2.X - p1.X) * (p3.Y - p1.Y) - (p2.Y - p1.Y) * (p3.X - p1.X);
        }
    }

    class Point
    {
        public double X { get; }
        public double Y { get; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
