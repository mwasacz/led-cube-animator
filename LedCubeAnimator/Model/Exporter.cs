using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LedCubeAnimator.Model
{
    public static class Exporter
    {
        public static void Export(string path, Animation animation)
        {
            using (var bw = new BinaryWriter(File.Open(path, FileMode.Create)))
            {
                int start = animation.MainGroup.Start;
                int end = animation.MainGroup.End;
                int step = 25; // ToDo
                int endStep = 250; // ToDo

                byte[] prevBytes = null;
                int s = 0;

                for (int t = start; t <= end; t += step)
                {
                    var voxels = Renderer.Render(animation, t, false);

                    byte max = 0;
                    byte[] bytes = new byte[11];
                    for (int z = 0; z < 4; z++)
                    {
                        byte b = 0;
                        byte m = 1;
                        for (int y = 0; y < 4; y++)
                        {
                            var v = voxels[0, y, z];
                            max = Max(max, v);
                            b |= v == Colors.Black ? (byte)0 : m;
                            m <<= 1;
                        }
                        for (int y = 0; y < 4; y++)
                        {
                            var v = voxels[3, y, z];
                            max = Max(max, v);
                            b |= v == Colors.Black ? (byte)0 : m;
                            m <<= 1;
                        }
                        bytes[z] = b;
                    }
                    for (int z = 0; z < 4; z++)
                    {
                        byte b = 0;
                        byte m = 1;
                        for (int y = 0; y < 4; y++)
                        {
                            var v = voxels[1, y, z];
                            max = Max(max, v);
                            b |= v == Colors.Black ? (byte)0 : m;
                            m <<= 1;
                        }
                        for (int y = 0; y < 4; y++)
                        {
                            var v = voxels[2, y, z];
                            max = Max(max, v);
                            b |= v == Colors.Black ? (byte)0 : m;
                            m <<= 1;
                        }
                        bytes[z + 4] = b;
                    }
                    max &= 0xF0;
                    bytes[8] = max;

                    if (t == start || (bytes.SequenceEqual(prevBytes) && s <= 0x0FFF - step))
                    {
                        s += step;
                    }
                    else
                    {
                        prevBytes[8] |= (byte)(s >> 8 & 0x0F);
                        prevBytes[9] = (byte)s;
                        prevBytes[10] = 0;

                        bw.Write(prevBytes);

                        s = step;
                    }

                    prevBytes = bytes;
                }

                s = Math.Max(s, endStep);

                prevBytes[8] |= (byte)(s >> 8);
                prevBytes[9] = (byte)s;
                prevBytes[10] = 0;

                bw.Write(prevBytes);
            }
        }

        private static byte Max(byte b, Color c)
        {
            b = Math.Max(b, c.R);
            b = Math.Max(b, c.G);
            b = Math.Max(b, c.B);
            return b;
        }
    }
}
