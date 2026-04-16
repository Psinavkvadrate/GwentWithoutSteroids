using GwentWithoutSteroids.Core.Cards;

namespace GwentWithoutSteroids.AI
{
    public enum AiActionType
    {
        PlayCard,
        Pass
    }

    public class AiAction
    {
        public AiActionType Type { get; set; }
        public Card Card { get; set; }

        public static AiAction Pass()
        {
            return new AiAction { Type = AiActionType.Pass };
        }

        public static AiAction Play(Card card)
        {
            return new AiAction
            {
                Type = AiActionType.PlayCard,
                Card = card
            };
        }
    }
}