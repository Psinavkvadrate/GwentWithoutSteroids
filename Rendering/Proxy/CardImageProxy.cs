using SFML.Graphics;

namespace GwentWithoutSteroids.Rendering.Proxy
{
    public class CardImageProxy : ICardImage
    {
        private readonly string _path;

        public CardImageProxy(string path)
        {
            _path = path;
        }

        public Texture GetTexture()
        {
            return TextureCache.Get(_path);
        }
    }
}