using SFML.Graphics;
using SFML.System;

namespace GwentLikeGame.Rendering.Models
{
    public class CardView
    {
        public RectangleShape Shape { get; set; }
        public Text Text { get; set; }
        public Vector2f Position { get; set; }
    }
}