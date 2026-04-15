using SFML.Graphics;
using SFML.System;
using GwentLikeGame.Core.Players;
using GwentLikeGame.Rendering.Models;
using System.Collections.Generic;
using GwentLikeGame.Core.Cards;

namespace GwentLikeGame.Rendering
{
    public class Renderer
    {
        private RenderWindow _window;
        private GameUiObserver _uiObserver;
        private List<CardView> _views = new();
        private Font _font;
        private int _playerPower;
        private int _aiPower;

        public Renderer(RenderWindow window, GameUiObserver observer)
        {
            _window = window;
            _uiObserver = observer;
            _font = new Font("gwentFont.ttf");
        }
        
        private void DrawBoard(Player player, float startY)
        {
            float x = 50;

            foreach (var row in player.Board.GetRows())
            {
                foreach (var card in row.Value.GetCards())
                {
                    var rect = new RectangleShape(new Vector2f(60, 90))
                    {
                        Position = new Vector2f(x, startY),
                        FillColor = Color.Green
                    };

                    _window.Draw(rect);

                    x += 70;
                }

                startY += 100;
                x = 50;
            }
        }

        public void BuildFromGame(Player player, Player opponent)
        {
            _views.Clear();

            // --- РУКА ---
            float x = 50;
            float y = 550;

            foreach (var card in player.Hand)
            {
                var rect = new RectangleShape(new Vector2f(80, 120))
                {
                    Position = new Vector2f(x, y),
                    FillColor = Color.Blue
                };

                _views.Add(new CardView
                {
                    Shape = rect,
                    Position = rect.Position,
                    Card = card
                });

                x += 100;
            }

            // --- СТОЛ ИГРОКА ---
            DrawBoard(player, 350);

            // --- СТОЛ AI ---
            DrawBoard(opponent, 100);
            _playerPower = player.Board.GetPower();
            _aiPower = opponent.Board.GetPower();
        }

        public void Draw()
        {
            foreach (var v in _views)
            {
                _window.Draw(v.Shape);

                var name = new Text(_font, v.Card.Name, 12)
                {
                    Position = v.Position + new Vector2f(5, 5)
                };

                var power = new Text(_font, v.Card.Power.ToString(), 16)
                {
                    Position = v.Position + new Vector2f(5, 90)
                };

                _window.Draw(name);
                _window.Draw(power);
            }

            // PASS кнопка
            var pass = new RectangleShape(new Vector2f(120, 50))
            {
                Position = new Vector2f(1100, 600),
                FillColor = Color.Red
            };

            var playerScore = new Text(_font, $"Player: {_playerPower}", 24)
            {
                Position = new Vector2f(50, 500)
            };

            var aiScore = new Text(_font, $"AI: {_aiPower}", 24)
            {
                Position = new Vector2f(50, 20)
            };

            _window.Draw(playerScore);
            _window.Draw(aiScore);

            _window.Draw(pass);

            float y = 200;

            foreach (var line in _uiObserver.GetLog())
            {
                var text = new Text(_font, line, 14)
                {
                    Position = new Vector2f(900, y)
                };

                _window.Draw(text);
                y += 20;
            }
        }

        public Card GetCardAtPosition(Vector2f pos)
        {
            foreach (var v in _views)
            {
                if (v.Shape.GetGlobalBounds().Contains(pos))
                    return v.Card;
            }
            return null;
        }
    }
}