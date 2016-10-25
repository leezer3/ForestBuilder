//Copyright: ManagedCode
//2015. 07.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ForestBuilder
{
    /// <summary>
    /// The class to generate random positions for the objects.
    /// </summary>
    public class Generator
    {
        /// <summary>
        /// Represents an area where indexed objects are to be placed.
        /// </summary>
        public class Area
        {
            /// <summary>
            /// Gets or sets the distance between the area's center line and the rail.
            /// </summary>
            public float Distance { get; set; }

            /// <summary>
            /// Gets or sets the offset to the area's center line in the X axis.
            /// </summary>
            public float XOffset { get; set; }

            /// <summary>
            /// Gets or sets the offset to the area's center line in the Z axis.
            /// </summary>
            public float ZOffset { get; set; }

            /// <summary>
            /// Gets or sets the list of indices to choose a random index from.
            /// </summary>
            public int[] Indices { get; set; }

            /// <summary>
            /// Gets or sets the height of the objects.
            /// </summary>
            public float Height { get; set; }

            /// <summary>
            /// Gets or sets the minimum value of the rotation in the Y axis.
            /// </summary>
            public float YRotationMin { get; set; }

            /// <summary>
            /// Gets or sets the maximum value of the rotation in the Y axis.
            /// </summary>
            public float YRotationMax { get; set; }
        }

        /// <summary>
        /// Gets or sets the point where the first object can be placed.
        /// </summary>
        public float StartingPoint { get; set; } = 0;

        /// <summary>
        /// Gets or sets the point where the last object can be placed.
        /// </summary>
        public float EndPoint { get; set; } = 0;

        /// <summary>
        /// Gets or sets the amount of occurence of the objects.
        /// </summary>
        public float Frequency { get; set; } = 0;

        /// <summary>
        /// Gets or sets the list of the areas.
        /// </summary>
        public List<Area> Areas { get; set; } = new List<Area>();

        /// <summary>
        /// Generates random placements for the objects with the currently set options.
        /// </summary>
        /// <param name="fileName">The path where the file is to be created.</param>
        public void Generate(string fileName)
        {
            List<string> lines = new List<string>();

            //conversion header
            lines.Add(";This header was created by ForestBuilder");
            lines.Add(";Generation time: " + DateTime.Now.ToString());
            lines.Add(";Version: " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            lines.Add(";Copyright: ManagedCode\n");

            //iterate through the positions
            for (float f = StartingPoint; f < EndPoint; f += Frequency)
            {
                //go through all the areas
                foreach(Area area in this.Areas)
                {
                    //distance marker
                    lines.Add(string.Format("{0},", (f + Dispersion(area.ZOffset)).ToString(CultureInfo.InvariantCulture)));

                    //place the object with random parameters
                    lines.Add(string.Format("Track.FreeObj 0;{0};{1};{2};{3};0;0;0,",
                        RandomArrayMember(area.Indices),
                        (area.Distance + Dispersion(area.XOffset)).ToString(CultureInfo.InvariantCulture),
                        area.Height.ToString(CultureInfo.InvariantCulture),
                        RandomBetween(area.YRotationMin, area.YRotationMax).ToString(CultureInfo.InvariantCulture)));
                }
            }

            System.IO.File.WriteAllLines(fileName, lines);
        }

        private static Random random = new Random();

        /// <summary>
        /// Returns a value between -f and f.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        private static float Dispersion(float f)
        {
            return (float)random.NextDouble() * (2 * f) - f;
        }

        /// <summary>
        /// Returns a number between the two specified number.
        /// </summary>
        /// <param name="a">The first specified number.</param>
        /// <param name="b">The second specified number.</param>
        /// <returns></returns>
        private static float RandomBetween(float a, float b)
        {
            float[] numberPair = new float[] { a, b };
            return (float)random.NextDouble() * (numberPair.Max() - numberPair.Min()) + numberPair.Min();
        }

        /// <summary>
        /// Returns a random member from the specified array
        /// </summary>
        /// <param name="Array">The array where the members can be found</param>
        /// <returns></returns>
        private static int RandomArrayMember(int[] Array)
        {
            return Array[random.Next(0, Array.Length)];
        }
    }
}
