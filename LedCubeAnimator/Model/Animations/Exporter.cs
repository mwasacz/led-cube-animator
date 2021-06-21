using LedCubeAnimator.Model.Animations;
using LedCubeAnimator.Model.Animations.Data;
using LedCubeAnimator.Utils;
using System;
using System.IO;
using System.Linq;
using System.Windows.Media;

namespace LedCubeAnimator.Model
{
    public static class Exporter
    {
        // FILE FORMAT
        // u16 Version number
        // u16 Header length
        // u08 Bits per frame length
        // u24 Frame length (in us)
        // u24 Frame count
        // u16 Bits per color (xrgb)
        // u24 Size (xyz)
        // For each frame: frame length, voxel data

        public static void Export(string path, Animation animation)
        {
            using (var bw = new BinaryWriter(File.Open(path, FileMode.Create)))
            {
                WriteHeader(animation, bw);

                //int repeat = animation.RepeatCount; // ToDo

                int start = animation.Start;
                int end = animation.End;

                byte[] prevBytes = null;
                int prevT = 0;
                for (int t = start; t <= end; t++)
                {
                    var voxels = Renderer.Render(animation, t, false);

                    int byteCount = voxels.Length;
                    if (animation.ColorMode == ColorMode.Mono)
                    {
                        byteCount = (byteCount + 7) / 8;
                    }
                    else if (animation.ColorMode == ColorMode.RGB)
                    {
                        byteCount *= 3;
                    }

                    byte[] bytes = new byte[byteCount];

                    int i = 0;
                    foreach (var v in voxels)
                    {
                        switch (animation.ColorMode)
                        {
                            case ColorMode.Mono:
                                bytes[i / 8] |= (byte)(v == Colors.Black ? 0 : 1 << (i % 8));
                                break;
                            case ColorMode.MonoBrightness:
                                bytes[i] = v.GetBrightness();
                                break;
                            case ColorMode.RGB:
                                bytes[i * 3] = v.B;
                                bytes[i * 3 + 1] = v.G;
                                bytes[i * 3 + 2] = v.R;
                                break;
                        }
                        i++;
                    }

                    if (t == start)
                    {
                        prevBytes = bytes;
                        prevT = t;
                    }
                    else if (!bytes.SequenceEqual(prevBytes))
                    {
                        WriteBytes(bw, prevBytes, t - prevT);

                        prevBytes = bytes;
                        prevT = t;
                    }
                }

                if (prevBytes != null)
                {
                    WriteBytes(bw, prevBytes, end - prevT);
                }
            }
        }

        private static void WriteBytes(BinaryWriter bw, byte[] prevBytes, int d)
        {
            for (byte l; d > 0; d -= l) // ToDo: set bytesPerFrame instead of repeating frames
            {
                l = (byte)Math.Min(d, 0xFF);

                bw.Write(l);
                bw.Write(prevBytes);
            }
        }

        private static void WriteHeader(Animation animation, BinaryWriter bw)
        {
            const ushort versionNumber = 0;
            const ushort headerLength = 16; // ToDo
            const byte bitsPerFrame = 8; // ToDo

            uint frameLength = (uint)(animation.FrameDuration * 1000); // ToDo: exported frameLength may be different than FrameDuration

            if (animation.End - animation.Start < 0 || animation.End - animation.Start > 0xFFFFFF)
            {
                throw new InvalidOperationException("Animation length out of bounds");
            }
            uint frameCount = (uint)(animation.End - animation.Start);

            ushort bitsPerColor;
            switch (animation.ColorMode)
            {
                case ColorMode.Mono:
                    bitsPerColor = 1;
                    break;
                case ColorMode.MonoBrightness:
                    bitsPerColor = 8;
                    break;
                case ColorMode.RGB:
                    bitsPerColor = 0x888;
                    break;
                default:
                    throw new InvalidOperationException("Invalid ColorMode");
            }

            if (animation.Size > 0xFF)
            {
                throw new InvalidOperationException("Cube size out of bounds");
            }
            byte size = (byte)animation.Size;

            bw.Write(versionNumber);
            bw.Write(headerLength);
            bw.Write(bitsPerFrame);
            WriteUInt24(bw, frameLength);
            WriteUInt24(bw, frameCount);
            bw.Write(bitsPerColor);
            bw.Write(size);
            bw.Write(size);
            bw.Write(size);
        }

        private static void WriteUInt24(BinaryWriter bw, uint value)
        {
            bw.Write((byte)value);
            bw.Write((byte)(value >> 8));
            bw.Write((byte)(value >> 16));
        }

        public static void ExportMW(string path, Animation animation)
        {
            using (var bw = new BinaryWriter(File.Open(path, FileMode.Create)))
            {
                //int repeat = animation.RepeatCount; // ToDo
                int duration = animation.FrameDuration;

                int start = animation.Start * duration;
                int end = animation.End * duration;

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

                if (prevBytes != null)
                {
                    WriteBytes(bw, prevBytes, end - prevT, endStep);
                }
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
