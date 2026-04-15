using SFML.Graphics;
using SFML.Window;
using GwentLikeGame.Core.GameLogic;
using GwentLikeGame.Core.Players;

namespace GwentLikeGame.Rendering
{
    public class GameWindow
    {
        private RenderWindow _window;
        private Renderer _renderer;

        private Game _game;
        private Player _player;

        public GameWindow(Game game, Player player)
        {
            _game = game;
            _player = player;

            _window = new RenderWindow(
                new VideoMode(new SFML.System.Vector2u(1280, 720)),
                "Gwent-like Game"
            );

            _window.Closed += (_, __) =>
            {
                _window.Close();
            };

            _renderer = new Renderer(_window);
        }

        public void Run()
        {
            _window.SetFramerateLimit(60);

            while (_window.IsOpen)
            {
                _window.DispatchEvents();

                _game.Tick(); 

                _window.Clear(Color.Black);

                _renderer.BuildFromGame(_player);
                _renderer.Draw();

                _window.Display();
            }
        }
    }
}