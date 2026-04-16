using SFML.Graphics;
using System.Collections.Generic;

namespace GwentWithoutSteroids.Rendering.Proxy
{
    public static class TextureCache
    {
        private static readonly Dictionary<string, Texture> _cache = new();

        public static Texture Get(string path)
        {
            if (!_cache.TryGetValue(path, out var texture))
            {
                texture = new Texture(path);
                _cache[path] = texture;
                Console.WriteLine($"[Cache] NEW texture: {path} | ID: {texture.NativeHandle}");
            }
            else
            {
                Console.WriteLine($"[Cache] HIT: {path} | ID: {texture.NativeHandle}");
            }

            return texture;
        }
    }
}