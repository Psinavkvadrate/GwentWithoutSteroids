using SFML.Graphics;
using SFML.Window;
using GwentLikeGame.Core.GameLogic;
using GwentLikeGame.Core.Players;
using GwentLikeGame.Core.Cards;
using SFML.System;

namespace GwentLikeGame.Rendering
{
    public class GameWindow
    {
        private RenderWindow _window;
        private Renderer _renderer;

        private Game _game;
        private Player _player;
        private Vector2f _passPos = new Vector2f(1100, 600 + 120f);
        private Vector2f _passSize = new Vector2f(140, 60);

        public GameWindow(Game game, Player player, GameUiObserver observer)
        {
            _game = game;
            _player = player;

            _window = new RenderWindow(
                new VideoMode(new Vector2u(1600, 1400)),
                "Gwent without steroids"
            );
            _window.MouseButtonPressed += OnMouseClick;

            _window.Closed += (_, __) =>
            {
                _window.Close();
            };

            _renderer = new Renderer(_window, observer, _game);
        }

        public void Run()
        {
            _window.SetFramerateLimit(60);

            while (_window.IsOpen)
            {
                _window.DispatchEvents();

                _game.Tick();

                _renderer.BuildFromGame(_player, _game.GetOpponent(_player)); // ← ПЕРЕД отрисовкой

                _window.Clear(Color.Black);
                _renderer.Draw();
                _window.Display();
            }
        }

        private void OnMouseClick(object sender, MouseButtonEventArgs e)
        {
            var mousePos = new Vector2f(e.Position.X, e.Position.Y);

            // ================= MULLIGAN =================
            if (_game.State == GameState.Mulligan)
            {
                var viewCard = _renderer.GetHandCardAtPosition(mousePos);
                Console.WriteLine($"Clicked hand index: {viewCard?.HandIndex}");
                if (viewCard != null)
                {
                    _game.ReplaceCard(viewCard.HandIndex);
                    return;
                }

                if (e.Button == Mouse.Button.Right)
                {
                    _game.EndMulligan();
                    return;
                }

                return;
            }

            // ================= PLAYING =================
            if (!_game.WaitingForPlayerInput)
                return;

            // PASS
            if (mousePos.X >= _passPos.X &&
                mousePos.X <= _passPos.X + _passSize.X &&
                mousePos.Y >= _passPos.Y &&
                mousePos.Y <= _passPos.Y + _passSize.Y)
            {
                _game.PassTurn();
                return;
            }

            var view = _renderer.GetCardViewAtPosition(mousePos);

            if (view != null && view.HandIndex >= 0)
            {
                var card = _player.Hand[view.HandIndex];
                _game.PlayCard(card);
                return;
            }
        }

        private void PlayPlayerCard(Card card)
        {
            _game.PlayCard(card);
        }
    }
}