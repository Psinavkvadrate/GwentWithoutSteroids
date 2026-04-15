using GwentLikeGame.Core.Cards;
using SFML.Graphics;
using SFML.System;

namespace GwentLikeGame.Rendering.Models
{
    public class CardView
    {
        public RectangleShape Shape { get; set; }
        public Text NameText { get; set; }
        public Text PowerText { get; set; }
        public Text TypeText { get; set; }

        public Vector2f Position { get; set; }
        public Card Card { get; set; }
        public int HandIndex { get; set; } = -1;
        public Vector2f TargetPosition { get; set; }
        public Vector2f CurrentPosition { get; set; }
        public bool IsAnimating { get; set; } = true;
    }
}