using SFML.Graphics;
using SFML.System;
using GwentWithoutSteroids.Core.Players;
using GwentWithoutSteroids.Rendering.Models;
using System.Collections.Generic;
using GwentWithoutSteroids.Core.Cards;
using GwentWithoutSteroids.Core.Board;
using SFML.Window;
using GwentWithoutSteroids.Core.GameLogic;

namespace GwentWithoutSteroids.Rendering
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
        private const float FIELD_X = 0;
        private const float FIELD_WIDTH = 1000;
        private const float CARD_WIDTH = 80f;
        private const float CARD_HEIGHT = 120f;

        private const float ROW_HEIGHT = 130f; 
        private const float ROW_PADDING = 10f; 
        private const float ENEMY_ROW_OFFSET = -CARD_HEIGHT * 2f / 3f;
        private const float GLOBAL_Y_OFFSET = 120f;
        private FloatRect _passBounds;
        private bool _passHovered;
        private bool _passPressed;
        private float _passScale = 1f;
        private FloatRect _restartBounds;
        private bool _restartHovered;

        public Renderer(RenderWindow window, GameUiObserver observer, Game game)
        {
            _window = window;
            _uiObserver = observer;
            _game = game;
            _font = new Font("assets/gwentFont.ttf");
            var texture = new Texture("assets/gwentBg.jpg");
            _background = new Sprite(texture);

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

            return new Color(200, 200, 80); 
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

            BuildBoard(opponent, 40 + GLOBAL_Y_OFFSET, true);
            BuildBoard(player, 350 + GLOBAL_Y_OFFSET, false);
            BuildHand(player);

            _playerPower = player.Board.GetPower();
            _aiPower = opponent.Board.GetPower();
        }

        private void BuildBoard(Player player, float startY, bool isEnemy)
        {
            var rows = player.Board.GetRows().ToList();

            if (isEnemy)
                rows.Reverse();

            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                var cards = row.Value.GetCards().ToList();

                int count = Math.Max(1, cards.Count);

                float minSpacing = CARD_WIDTH + 10;

                float cardSpacing = Math.Max(
                    minSpacing,
                    FIELD_WIDTH / count
                );

                float totalWidth = (count - 1) * cardSpacing + CARD_WIDTH;
                float startX = FIELD_X + (FIELD_WIDTH - totalWidth) / 2;

                float x = startX;

                float y = startY + i * ROW_HEIGHT + ROW_PADDING;

                if (isEnemy)
                    y += ENEMY_ROW_OFFSET;

                foreach (var card in cards)
                {
                    var view = CreateCardView(card, new Vector2f(x, y));
                    _views.Add(view);

                    x += cardSpacing;
                }
            }
        }

        private void DrawFieldLines()
        {
            float fieldWidth = FIELD_WIDTH;

            float topStart = 40 + GLOBAL_Y_OFFSET;
            float bottomStart = 350 + GLOBAL_Y_OFFSET;

            for (int i = 0; i < 3; i++)
            {
                float y = topStart + i * ROW_HEIGHT;

                var line = new RectangleShape(new Vector2f(fieldWidth, 2))
                {
                    Position = new Vector2f(FIELD_X, y),
                    FillColor = Color.Black
                };

                _window.Draw(line);
            }

            var middle = new RectangleShape(new Vector2f(fieldWidth, 5))
            {
                Position = new Vector2f(FIELD_X, 330 + GLOBAL_Y_OFFSET),
                FillColor = Color.Black
            };
            _window.Draw(middle);

            for (int i = 0; i < 3; i++)
            {
                float y = bottomStart + i * ROW_HEIGHT;

                var line = new RectangleShape(new Vector2f(fieldWidth, 2))
                {
                    Position = new Vector2f(FIELD_X, y),
                    FillColor = Color.Black
                };

                _window.Draw(line);
            }
        }

        private void BuildHand(Player player)
        {
            int count = player.Hand.Count;

            if (count == 0)
                return;

            float spacing = CARD_WIDTH + 10;

            float totalWidth = (count - 1) * spacing + CARD_WIDTH;

            if (totalWidth > FIELD_WIDTH)
            {
                spacing = (FIELD_WIDTH - CARD_WIDTH) / (count - 1);
                totalWidth = FIELD_WIDTH;
            }

            float startX = FIELD_X + (FIELD_WIDTH - totalWidth) / 2;
            float y = 780 + GLOBAL_Y_OFFSET;

            float x = startX;

            for (int i = 0; i < player.Hand.Count; i++)
            {
                var card = player.Hand[i];
                var view = CreateCardView(card, new Vector2f(x, y));

                view.HandIndex = i;

                _views.Add(view);

                x += spacing;
            }
        }

        public void Draw()
        {

            foreach (var v in _views)
            {
                if (v.IsAnimating)
                {
                    var diff = v.TargetPosition - v.CurrentPosition;

                    if (Math.Abs(diff.X) < 1 && Math.Abs(diff.Y) < 1)
                    {
                        v.CurrentPosition = v.TargetPosition;
                        v.IsAnimating = false;
                    }
                    else
                    {
                        float speed = 0.15f;
                        var delta = v.TargetPosition - v.CurrentPosition;

                        v.CurrentPosition += delta * speed;
                        if (delta.X * delta.X + delta.Y * delta.Y < 4f)
                        {
                            v.CurrentPosition = v.TargetPosition;
                            v.IsAnimating = false;
                        }
                    }

                    v.Shape.Position = v.CurrentPosition;
                    v.NameText.Position = v.CurrentPosition + new Vector2f(5, 5);
                    v.PowerText.Position = v.CurrentPosition + new Vector2f(65, 95);
                    v.TypeText.Position = v.CurrentPosition + new Vector2f(5, 105);
                }
            }
            _window.Draw(_background);

            DrawFieldLines();

            var mouse = Mouse.GetPosition(_window);
            var mousePos = new Vector2f(mouse.X, mouse.Y);

            foreach (var v in _views)
            {
                if (v.Shape.GetGlobalBounds().Contains(mousePos))
                    v.Shape.Scale = new Vector2f(1.1f, 1.1f);
                else
                    v.Shape.Scale = new Vector2f(1f, 1f);
}

            foreach (var v in _views)
            {
                _window.Draw(v.Shape);
                _window.Draw(v.NameText);
                _window.Draw(v.PowerText);
                _window.Draw(v.TypeText);
            }

            foreach (var v in _views)
            {
                if (v.Shape.GetGlobalBounds().Contains(mousePos))
                {
                    v.Shape.Scale = new Vector2f(1.1f, 1.1f);
                }
                else
                {
                    v.Shape.Scale = new Vector2f(1f, 1f);
                }
            }

            if (_game.State == GameState.Mulligan)
            {
                var text = new Text(_font, "MULLIGAN PHASE (RMB to finish)", 20)
                {
                    Position = new Vector2f(400, 20)
                };
                _window.Draw(text);
            }

            if (!_game.WaitingForPlayerInput && _game.State == GameState.Playing)
            {
                var overlay = new RectangleShape(new Vector2f(1000, 800))
                {
                    Position = new Vector2f(0, 0),
                    FillColor = new Color(0, 0, 0, 100)
                };

                _window.Draw(overlay);
            }

            if (_game.IsFinished)
            {
                DrawEndGame();
            }

            DrawUI();
        }

        private void DrawUI()
        {
            float uiX = 1150;

            var playerScore = new Text(_font, $"Player: {_playerPower}", 26)
            {
                Position = new Vector2f(uiX, 700 + GLOBAL_Y_OFFSET)
            };

            var aiScore = new Text(_font, $"AI: {_aiPower}", 26)
            {
                Position = new Vector2f(uiX, 20 + GLOBAL_Y_OFFSET)
            };

            _window.Draw(playerScore);
            _window.Draw(aiScore);

            var roundsText = new Text(_font,
                $"ROUNDS\nPlayer: {_game.PlayerRounds}\nAI: {_game.AiRounds}",
                22)
            {
                Position = new Vector2f(uiX, 80 + GLOBAL_Y_OFFSET)
            };

            _window.Draw(roundsText);

            _passBounds = new FloatRect(
                new Vector2f(uiX, 600 + GLOBAL_Y_OFFSET),
                new Vector2f(140, 60)
            );

            var mouse = Mouse.GetPosition(_window);
            var mousePos = new Vector2f(mouse.X, mouse.Y);

            _passHovered = _passBounds.Contains(mousePos);

            float targetScale = _passHovered ? 1.1f : 1f;
            _passScale += (targetScale - _passScale) * 0.2f;

            var pass = new RectangleShape(new Vector2f(140, 60))
            {
                Position = new Vector2f(uiX + 70, 600 + GLOBAL_Y_OFFSET + 30),
                Origin = new Vector2f(70, 30),
                FillColor = _passHovered ? new Color(220, 70, 70) : new Color(180, 50, 50),
                Scale = new Vector2f(_passScale, _passScale)
            };

            var passText = new Text(_font, "PASS", 18)
            {
                Position = new Vector2f(uiX + 20, 610 + GLOBAL_Y_OFFSET)
            };

            _window.Draw(pass);
            _window.Draw(passText);

            float y = 150 + GLOBAL_Y_OFFSET;

            foreach (var line in _uiObserver.GetLog())
            {
                var text = new Text(_font, line, 14)
                {
                    Position = new Vector2f(uiX, y)
                };

                _window.Draw(text);
                y += 20;
            }

            var turnText = new Text(_font,
                _game.WaitingForPlayerInput ? "YOUR TURN" : "AI THINKING...",
                24)
            {
                Position = new Vector2f(1150, 500)
            };

            turnText.FillColor = _game.WaitingForPlayerInput
                ? Color.Green
                : Color.Red;

            _window.Draw(turnText);

            var aiCards = new Text(_font,
                $"Cards: {_game.GetAiHandCount()}",
                20)
            {
                Position = new Vector2f(uiX, 50 + GLOBAL_Y_OFFSET)
            };

            _window.Draw(aiCards);
        }

        private void DrawEndGame()
        {
            var overlay = new RectangleShape(new Vector2f(_window.Size.X, _window.Size.Y))
            {
                Position = new Vector2f(0, 0),
                FillColor = new Color(0, 0, 0, 180)
            };
            _window.Draw(overlay);

            string winner = _game.PlayerRounds > _game.AiRounds
                ? "PLAYER WINS"
                : "AI WINS";

            var winText = new Text(_font, winner, 50)
            {
                Position = new Vector2f(600, 400),
                FillColor = Color.White
            };

            _window.Draw(winText);

            _restartBounds = new FloatRect(
                new Vector2f(650, 550),
                new Vector2f(300, 80)
            );

            var mouse = Mouse.GetPosition(_window);
            var mousePos = new Vector2f(mouse.X, mouse.Y);

            _restartHovered = _restartBounds.Contains(mousePos);

            var btn = new RectangleShape(new Vector2f(300, 80))
            {
                Position = new Vector2f(650, 550),
                FillColor = _restartHovered
                    ? new Color(100, 200, 100)
                    : new Color(70, 150, 70)
            };

            var text = new Text(_font, "RESTART", 30)
            {
                Position = new Vector2f(710, 570)
            };

            _window.Draw(btn);
            _window.Draw(text);
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
            for (int i = _views.Count - 1; i >= 0; i--)
            {
                var v = _views[i];

                if (v.Shape.GetGlobalBounds().Contains(pos))
                    return v;
            }

            return null;
        }

        public CardView GetHandCardAtPosition(Vector2f pos)
        {
            for (int i = _views.Count - 1; i >= 0; i--)
            {
                var v = _views[i];

                if (v.HandIndex >= 0 &&
                    v.Shape.Position.Y > 700 &&  
                    v.Shape.GetGlobalBounds().Contains(pos))
                {
                    return v;
                }
            }

            return null;
        }

        private CardView CreateCardView(Card card, Vector2f position)
        {
            var texture = card.Image?.GetTexture()
              ?? new Texture("assets/default.png");

            var rect = new RectangleShape(new Vector2f(80, 120))
            {
                Position = position,
                Texture = texture,
                OutlineColor = Color.Black,
                OutlineThickness = 2
            };

            var name = new Text(_font, card.Name, 12)
            {
                Position = position + new Vector2f(5, 5)
            };

            var power = new Text(_font, card.Power.ToString(), 22)
            {
                Position = position + new Vector2f(CARD_WIDTH - 15, CARD_HEIGHT - 25)
            };

            string type = card switch
            {
                UnitCard u => u.Row.ToString(),
                _ => "Skill"
            };

            var typeText = new Text(_font, type, 10)
            {
                Position = position + new Vector2f(5, CARD_HEIGHT - 15)
            };

            return new CardView
            {
                Shape = rect,
                NameText = name,
                PowerText = power,
                TypeText = typeText,
                Position = position,

                CurrentPosition = position + new Vector2f(0, -50), 
                TargetPosition = position,
                IsAnimating = true,

                Card = card
            };
        }

        public void TriggerPassClick()
        {
            _passScale = 0.85f;
            _passPressed = true;
        }
    }
}