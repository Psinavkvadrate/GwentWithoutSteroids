using SFML.Graphics;

namespace GwentLikeGame.Rendering.Proxy
{
    public class TextureProxy
    {
        private Texture _realTexture;
        private string _path;

        public TextureProxy(string path)
        {
            _path = path;
        }

        public Texture GetTexture()
        {
            if (_realTexture == null)
            {
                _realTexture = new Texture(_path);
            }

            return _realTexture;
        }
    }
}