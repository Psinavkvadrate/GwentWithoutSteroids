using System.Collections.Generic;

namespace GwentLikeGame.Rendering.Proxy
{
    public static class TextureCache
    {
        private static Dictionary<string, TextureProxy> _cache = new();

        public static TextureProxy Get(string path)
        {
            if (!_cache.ContainsKey(path))
                _cache[path] = new TextureProxy(path);

            return _cache[path];
        }
    }
}