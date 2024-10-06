// This file is part of LED Cube Animator
// Copyright (C) 2019-2021, 2024 Mikolaj Wasacz
// SPDX-License-Identifier: GPL-3.0-only WITH GPL-3.0-linking-source-exception

using LedCubeAnimator.Model.Animations.Data;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace LedCubeAnimator.Model.Animations
{
    public static class FileReaderWriter
    {
        public static void Save(string path, Animation animation)
        {
            using (var sw = new StreamWriter(path))
            {
                sw.Write(SerializeJson(animation));
            }
        }

        public static Animation Open(string path)
        {
            try
            {
                using (var sr = new StreamReader(path))
                {
                    return DeserializeJson<Animation>(sr.ReadToEnd());
                }
            }
            catch
            {
                return null;
            }
        }

        public static string Serialize(ICollection<Tile> tiles) => SerializeJson(tiles);

        public static ICollection<Tile> Deserialize(string str) => DeserializeJson<ICollection<Tile>>(str);

        private static string SerializeJson<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
        }

        private static T DeserializeJson<T>(string str)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(str, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
            }
            catch
            {
                return default;
            }
        }
    }
}
