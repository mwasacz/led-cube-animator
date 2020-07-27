using LedCubeAnimator.Model.Animations.Data;
using System;
using System.IO;
using System.Linq;
using System.Windows.Media;

namespace LedCubeAnimator.Model.Animations
{
    public static class Exporter
    {
        public static void Export(string path, Animation animation)
        {
            using (var bw = new BinaryWriter(File.Open(path, FileMode.Create)))
            {
                //int repeat = animation.MainGroup.RepeatCount; // ToDo
                int duration = Math.Max(animation.FrameDuration, 1);

                int start = animation.MainGroup.Start * duration;
                int end = animation.MainGroup.End * duration;

                const int step = 25; // ToDo
                const int endStep = 150; // ToDo

                byte[] prevBytes = null;
                int prevT = 0;

                for (int t = start; t <= end - endStep; t++)
                {
                    var voxels = Renderer.Render(animation, t / duration, false);

                    byte max = 0;
                    byte[] bytes = new byte[11];
                    for (int z = 0; z < 4; z++)
                    {
                        byte b;
                        b = PrepareByteHalf(voxels, 0, z, ref max, 0x01);
                        b |= PrepareByteHalf(voxels, 3, z, ref max, 0x10);
                        bytes[z] = b;

                        b = PrepareByteHalf(voxels, 1, z, ref max, 0x01);
                        b |= PrepareByteHalf(voxels, 2, z, ref max, 0x10);
                        bytes[z + 4] = b;
                    }
                    max &= 0xF0;
                    bytes[8] = max;

                    if (t == start)
                    {
                        prevBytes = bytes;
                        prevT = t;
                        t += step - 1;
                    }
                    else if (!bytes.SequenceEqual(prevBytes))
                    {
                        WriteBytes(bw, prevBytes, t - prevT, step);

                        prevBytes = bytes;
                        prevT = t;
                        t += step - 1;
                    }
                }

                WriteBytes(bw, prevBytes, end - prevT, endStep);
            }
        }

        private static byte PrepareByteHalf(Color[,,] voxels, int x, int z, ref byte max, byte m)
        {
            byte b = 0;
            for (int y = 0; y < 4; y++)
            {
                var v = voxels[x, y, z];
                max = Math.Max(max, v.GetBrightness());
                b |= v == Colors.Black ? (byte)0 : m;
                m <<= 1;
            }
            return b;
        }

        private static void WriteBytes(BinaryWriter bw, byte[] prevBytes, int d, int step)
        {
            byte prev8 = prevBytes[8];
            for (int l; d > 0; d -= l)
            {
                l = d <= 0x0FFF ? d : Math.Min(d - step, 0x0FFF);
                prevBytes[8] = (byte)((l >> 8 & 0x0F) | prev8);
                prevBytes[9] = (byte)l;
                prevBytes[10] = 0;

                bw.Write(prevBytes);
            }
        }
    }
}
