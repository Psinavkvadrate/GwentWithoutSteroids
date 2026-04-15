using SFML.Graphics;
namespace GwentLikeGame.Rendering.Proxy
{
  public class CardImageProxy : ICardImage
  {
      private string _path;
      private RealCardImage _realImage;

      public CardImageProxy(string path)
      {
          _path = path;
      }

      public Texture GetTexture()
      {
          if (_realImage == null)
              _realImage = new RealCardImage(_path);

          return _realImage.GetTexture();
      }
  }
}