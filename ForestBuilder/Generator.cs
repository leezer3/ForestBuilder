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
			/// Gets or sets the area's textual name placed.
			/// </summary>
			public string Name { get; set; }

			/// <summary>
			/// Gets or sets the frequency at which objects are placed.
			/// </summary>
			public float Frequency { get; set; }

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
        public float StartingPoint { get; set; }
        /// <summary>
        /// Gets or sets the point where the last object can be placed.
        /// </summary>
        public float EndPoint { get; set; }
        /// <summary>
        /// Gets or sets the amount of occurence of the objects.
        /// </summary>
        public float Frequency { get; set; }

        /// <summary>
        /// Gets or sets the list of the areas.
        /// </summary>
        public List<Area> Areas { get; set; }

	    public Generator()
	    {
		    StartingPoint = 0;
		    EndPoint = 0;
		    Frequency = 0;
			Areas = new List<Area>();
	    }

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

			List<ObjectEntry> ObjectList = new List<ObjectEntry>();
                //go through all the areas
                foreach(Area area in this.Areas)
                {
					//iterate through the positions
					for (float f = StartingPoint; f < EndPoint; f += area.Frequency)
					{
						ObjectList.Add(new ObjectEntry(f + Dispersion(area.ZOffset), string.Format("Track.FreeObj 0;{0};{1};{2};{3};0;0;0,",
						RandomArrayMember(area.Indices),
						(area.Distance + Dispersion(area.XOffset)).ToString(CultureInfo.InvariantCulture),
						area.Height.ToString(CultureInfo.InvariantCulture),
						RandomBetween(area.YRotationMin, area.YRotationMax).ToString(CultureInfo.InvariantCulture))));
						
					}
				

            }
			//Order track positions
			ObjectList = ObjectList.OrderBy(x => x.TrackPosition).ToList();
			double tpos = 0;
			//Write object list out to file
			foreach(ObjectEntry entry in ObjectList)
			{
				if(entry.TrackPosition != tpos)
				{
					tpos = entry.TrackPosition;
				}
				lines.Add(entry.TrackPosition.ToString(CultureInfo.InvariantCulture) + ", " + entry.Object);
				
			}
			System.IO.File.WriteAllLines(fileName, lines);
        }

		internal struct ObjectEntry
		{
			internal double TrackPosition;
			internal string Object;
			internal ObjectEntry(double Pos, string Object)
			{
				this.TrackPosition = Pos;
				this.Object = Object;
			}
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
