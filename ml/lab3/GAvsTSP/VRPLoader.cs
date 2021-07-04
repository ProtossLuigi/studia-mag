using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GAvsTSP
{
    class VRPLoader
    {
        public static TSP readTSP(string filename)
        {
            var lines = File.ReadAllLines(filename);
            int size = 0;
            List<(int, int, int)> coords = new List<(int, int, int)>();
            bool readingCoords = false;
            foreach (string line in lines)
            {
                string[] words = line.Split(' ');
                if (readingCoords)
                {
                    try
                    {
                        coords.Add((int.Parse(words[1]), int.Parse(words[2]), int.Parse(words[3])));
                    }
                    catch (FormatException)
                    {
                        readingCoords = false;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        readingCoords = false;
                    }
                }
                else if (words[0] == "DIMENSION")
                {
                    size = int.Parse(words[2]);
                }
                else if (words[0] == "NODE_COORD_SECTION")
                {
                    readingCoords = true;
                }
            }
            if (size == 0)
            {
                throw new FormatException();
            }
            return TSP.FromEuclid2D(size, coords);
        }
    }
}
