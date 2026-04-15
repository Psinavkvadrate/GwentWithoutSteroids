using System.Collections.Generic;
using SFML.Graphics;

namespace GwentLikeGame.Rendering.Proxy
{
    public class TextureCache
    {
        private static Dictionary<string, Texture> _cache = new();

        public static Texture Get(string path)
        {
            if (!_cache.ContainsKey(path))
                _cache[path] = new Texture(path);

            return _cache[path];
        }
    }
}