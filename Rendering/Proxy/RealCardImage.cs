using SFML.Graphics;

namespace GwentLikeGame.Rendering.Proxy
{
  public class RealCardImage : ICardImage
  {
      private Texture _texture;

      public RealCardImage(string path)
      {
          _texture = new Texture(path);
      }

      public Texture GetTexture() => _texture;
  }
}