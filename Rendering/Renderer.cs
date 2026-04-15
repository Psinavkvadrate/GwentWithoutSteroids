using SFML.Graphics;
using SFML.System;
using GwentLikeGame.Core.Players;
using GwentLikeGame.Rendering.Models;
using System.Collections.Generic;

namespace GwentLikeGame.Rendering
{
    public class Renderer
    {
        private RenderWindow _window;
        private List<CardView> _views = new();

        public Renderer(RenderWindow window)
        {
            _window = window;
        }

        public void BuildFromGame(Player player)
        {
            _views.Clear();

            float x = 50;
            float y = 600;

            foreach (var card in player.Hand)
            {
                var rect = new RectangleShape(new Vector2f(80, 120))
                {
                    Position = new Vector2f(x, y),
                    FillColor = SFML.Graphics.Color.Blue
                };

                var view = new CardView
                {
                    Shape = rect,
                    Position = rect.Position
                };

                _views.Add(view);
                x += 100;
            }
        }

        public void Draw()
        {
            foreach (var v in _views)
            {
                _window.Draw(v.Shape);
            }
        }
    }
}