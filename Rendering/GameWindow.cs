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
        private Vector2f _passPos = new Vector2f(1100, 600);
        private Vector2f _passSize = new Vector2f(120, 50);

        public GameWindow(Game game, Player player, GameUiObserver observer)
        {
            _game = game;
            _player = player;

            _window = new RenderWindow(
                new VideoMode(new SFML.System.Vector2u(1280, 720)),
                "Gwent-like Game"
            );
            _window.MouseButtonPressed += OnMouseClick;

            _window.Closed += (_, __) =>
            {
                _window.Close();
            };

            _renderer = new Renderer(_window, observer);
        }

        public void Run()
        {
            _window.SetFramerateLimit(60);

            while (_window.IsOpen)
            {
                _window.DispatchEvents();

                _game.Tick(); 

                _window.Clear(Color.Black);

                _renderer.BuildFromGame(_player, _game.GetOpponent(_player));
                _renderer.Draw();

                _window.Display();
            }
        }

        private void OnMouseClick(object sender, MouseButtonEventArgs e)
        {
            if (!_game.WaitingForPlayerInput)
                return;

            var mousePos = new Vector2f(e.Position.X, e.Position.Y);

            if (mousePos.X >= _passPos.X &&
                mousePos.X <= _passPos.X + _passSize.X &&
                mousePos.Y >= _passPos.Y &&
                mousePos.Y <= _passPos.Y + _passSize.Y)
            {
                _player.HasPassed = true;
                _game.EndPlayerTurn();
                return;
            }

            var card = _renderer.GetCardAtPosition(mousePos);

            if (card != null)
            {
                PlayPlayerCard(card);
            }
        }

        private void PlayPlayerCard(Card card)
        {
            _player.Hand.Remove(card);

            card.Play(new GameContext
            {
                CurrentPlayer = _player,
                Opponent = _game.GetOpponent(_player),
                Game = _game
            });

            _game.EndPlayerTurn();
        }
    }
}