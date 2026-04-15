using SFML.Graphics;
using SFML.System;
using GwentLikeGame.Core.Players;
using GwentLikeGame.Rendering.Models;
using System.Collections.Generic;
using GwentLikeGame.Core.Cards;
using GwentLikeGame.Core.Board;
using SFML.Window;
using GwentLikeGame.Core.GameLogic;

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

        private Sprite _background;

        private Game _game;

        public Renderer(RenderWindow window, GameUiObserver observer, Game game)
        {
            _window = window;
            _uiObserver = observer;
            _game = game;
            _font = new Font("assets/gwentFont.ttf");
            var texture = new Texture("assets/gwentBg.jpg");
            _background = new Sprite(texture);

            // растянуть на окно
            var size = _window.Size;
            _background.Scale = new Vector2f(
                size.X / (float)texture.Size.X,
                size.Y / (float)texture.Size.Y
            );
        }
        
        private Color GetCardColor(Card card)
        {
            if (card is UnitCard unit)
            {
                return unit.Row switch
                {
                    RowType.Melee => new Color(200, 80, 80),
                    RowType.Ranged => new Color(80, 200, 80),
                    RowType.Siege => new Color(80, 80, 200),
                    _ => Color.White
                };
            }

            return new Color(200, 200, 80); // Skill
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

            BuildHand(player);
            BuildBoard(opponent, 50, true);
            BuildBoard(player, 300, false);

            _playerPower = player.Board.GetPower();
            _aiPower = opponent.Board.GetPower();
        }

        private void BuildHand(Player player)
        {
            float x = 100;
            float y = 580; 

            for (int i = 0; i < player.Hand.Count; i++)
            {
                var card = player.Hand[i];
                var view = CreateCardView(card, new Vector2f(x, y));
                
                view.HandIndex = i; 
                
                _views.Add(view);

                x += 110;
            }
        }

        private void BuildBoard(Player player, float startY, bool isEnemy)
        {
            float rowSpacing = 120;
            float cardSpacing = 110;

            var rows = player.Board.GetRows().ToList();

            if (isEnemy)
                rows.Reverse(); // ← ключевая строка

            foreach (var row in rows)
            {
                float x = 150;
                float y = startY;

                foreach (var card in row.Value.GetCards())
                {
                    var view = CreateCardView(card, new Vector2f(x, y));
                    _views.Add(view);

                    x += cardSpacing;
                }

                var label = new Text(_font, row.Key.ToString(), 14)
                {
                    Position = new Vector2f(20, y + 40)
                };

                _window.Draw(label);

                startY += rowSpacing;
            }
        }

        public void Draw()
        {
            _window.Draw(_background); 

            foreach (var v in _views)
            {
                _window.Draw(v.Shape);
                _window.Draw(v.NameText);
                _window.Draw(v.PowerText);
                _window.Draw(v.TypeText);
            }

            var mouse = Mouse.GetPosition(_window);
            var mousePos = new Vector2f(mouse.X, mouse.Y);

            foreach (var v in _views)
            {
                Console.WriteLine(v.Shape.GetGlobalBounds());
                if (v.Shape.GetGlobalBounds().Contains(mousePos))
                    v.Shape.OutlineColor = Color.Yellow;
                else
                    v.Shape.OutlineColor = Color.Black;
            }

            if (_game.State == GameState.Mulligan)
            {
                var text = new Text(_font, "MULLIGAN PHASE (RMB to finish)", 20)
                {
                    Position = new Vector2f(400, 20)
                };
                _window.Draw(text);
            }

            DrawUI();
        }

        private void DrawUI()
        {
            // Scores
            var playerScore = new Text(_font, $"Player: {_playerPower}", 26)
            {
                Position = new Vector2f(50, 650)
            };

            var aiScore = new Text(_font, $"AI: {_aiPower}", 26)
            {
                Position = new Vector2f(50, 10)
            };

            _window.Draw(playerScore);
            _window.Draw(aiScore);

            // PASS кнопка
            var pass = new RectangleShape(new Vector2f(140, 60))
            {
                Position = new Vector2f(1100, 600),
                FillColor = new Color(180, 50, 50)
            };

            var passText = new Text(_font, "PASS", 18)
            {
                Position = new Vector2f(1120, 610)
            };

            _window.Draw(pass);
            _window.Draw(passText);

            // Лог
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

        public CardView GetCardViewAtPosition(Vector2f pos)
        {
            foreach (var v in _views)
            {
                if (v.Shape.GetGlobalBounds().Contains(pos))
                    return v;
            }
            return null;
        }

        private CardView CreateCardView(Card card, Vector2f position)
        {
            var rect = new RectangleShape(new Vector2f(90, 140))
            {
                Position = position,
                FillColor = GetCardColor(card),
                OutlineColor = Color.Black,
                OutlineThickness = 2
            };

            var name = new Text(_font, card.Name, 12)
            {
                Position = position + new Vector2f(5, 5)
            };

            var power = new Text(_font, card.Power.ToString(), 22)
            {
                Position = position + new Vector2f(5, 100)
            };

            string type = card switch
            {
                UnitCard u => u.Row.ToString(),
                _ => "Skill"
            };

            var typeText = new Text(_font, type, 10)
            {
                Position = position + new Vector2f(5, 120)
            };

            return new CardView
            {
                Shape = rect,
                NameText = name,
                PowerText = power,
                TypeText = typeText,
                Position = position,
                Card = card
            };
        }
    }
}