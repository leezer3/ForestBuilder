//Copyright: ManagedCode
//2015. 07.

using System.Linq;
using System.Globalization;

namespace ForestBuilder
{
    /// <summary>
    /// Represents an INI data file reader.
    /// </summary>
    public static class IniReader
    {
        /// <summary>
        /// Reads the generator's specific data to a new instance from the specified path.
        /// </summary>
        /// <param name="fileName">The path where the file may be found.</param>
        /// <param name="generator">The generator instance to be loaded.</param>
        public static void Read(string fileName, out Generator generator)
        {
            //initialize a new generator
            generator = new Generator();

            //iterate through the file's lines
            foreach (string line in System.IO.File.ReadAllLines(fileName))
            {
                //trim the line
                string s = line.Trim();

                //remove comment if there is any
                s = s.Contains(';') ? s.Substring(0, s.IndexOf(';')) : s;

                //ignore blank line
                if (string.IsNullOrWhiteSpace(s)) continue;

                //if it is not an assignment
                if (!s.Contains("="))
                {
                    //if there is no indexer to add a new generator, continue
                    if (!s.Contains("[") || !s.Contains("]")) continue;

                    //add a new generator
                    generator.Areas.Add(new Generator.Area());

                    //skip the operations according to assignment
                    continue;
                }

                //found the key for the assignment
                string key = s.Substring(0, s.IndexOf('='));

                //separate the values and parse them
                float[] numbers = s.Substring(s.IndexOf('=') + 1, s.Length - s.IndexOf('=') - 1)
                    .Split(',')
                    .Select(f => float.Parse(f, NumberStyles.Float, CultureInfo.InvariantCulture))
                    .ToArray();

                //switch on the key
                switch (key)
                {
                    case "StartingPoint":
                        generator.StartingPoint = numbers[0];
                        break;
                    case "EndPoint":
                        generator.EndPoint = numbers[0];
                        break;
                    case "Frequency":
                        generator.Frequency = numbers[0];
                        break;
                    case "Area.Distance":
                        generator.Areas[generator.Areas.Count - 1].Distance = numbers[0];
                        break;
                    case "Area.XOffset":
                        generator.Areas[generator.Areas.Count - 1].XOffset = numbers[0];
                        break;
                    case "Area.ZOffset":
                        generator.Areas[generator.Areas.Count - 1].ZOffset = numbers[0];
                        break;
                    case "Area.Indices":
                        generator.Areas[generator.Areas.Count - 1].Indices = numbers.Select(i => (int)i).ToArray();
                        break;
                    case "Area.Height":
                        generator.Areas[generator.Areas.Count - 1].Height = numbers[0];
                        break;
                    case "Area.YRotationMin":
                        generator.Areas[generator.Areas.Count - 1].YRotationMin = numbers[0];
                        break;
                    case "Area.YRotationMax":
                        generator.Areas[generator.Areas.Count - 1].YRotationMax = numbers[0];
                        break;
                }
            }
        }
    }
}
