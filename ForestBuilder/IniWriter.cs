//Copyright: ManagedCode
//2015. 07.

using System.Collections.Generic;

namespace ForestBuilder
{
	public static class IniWriter
	{
		public static void Write(string fileName, Generator generator)
		{
			System.Globalization.CultureInfo invariantCulture = System.Globalization.CultureInfo.InvariantCulture;
			List<string> lines = new List<string>();
			lines.Add(string.Format("StartingPoint={0}", generator.StartingPoint.ToString(invariantCulture)));
			lines.Add(string.Format("EndPoint={0}", generator.EndPoint.ToString(invariantCulture)));
			lines.Add(string.Format("Frequency={0}", generator.Frequency.ToString(invariantCulture)));
			for (int i = 0; i < generator.Areas.Count; i++)
			{
				lines.Add("[Area]");
				lines.Add(string.Format("Area.Name={0}", generator.Areas[i].Name));
				lines.Add(string.Format("Area.Frequency={0}", generator.Areas[i].Frequency.ToString(invariantCulture)));
				lines.Add(string.Format("Area.Distance={0}", generator.Areas[i].Distance.ToString(invariantCulture)));
				lines.Add(string.Format("Area.XOffset={0}", generator.Areas[i].XOffset.ToString(invariantCulture)));
				lines.Add(string.Format("Area.ZOffset={0}", generator.Areas[i].ZOffset.ToString(invariantCulture)));
				lines.Add(string.Format("Area.Indices={0}", string.Join(", ", generator.Areas[i].Indices)));
				lines.Add(string.Format("Area.Height={0}", generator.Areas[i].Height.ToString(invariantCulture)));
				lines.Add(string.Format("Area.YRotationMin={0}", generator.Areas[i].YRotationMin.ToString(invariantCulture)));
				lines.Add(string.Format("Area.YRotationMax={0}", generator.Areas[i].YRotationMax.ToString(invariantCulture)));
			}
			System.IO.File.WriteAllLines(fileName, lines);
		}
	}
}
