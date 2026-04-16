using GwentLikeGame.Core.Board;
using GwentLikeGame.Core.GameLogic;
using GwentLikeGame.Patterns.Observer;

namespace GwentLikeGame.Core.Cards
{
    public class UnitCard : Card
    {
        public RowType Row { get; }

        public UnitCard(string name, int power, RowType row)
            : base(name, power)
        {
            Row = row;
        }

        public override void Play(GameContext context)
        {
            context.CurrentPlayer.PlayCard(this);

            context.Game?.Notify(new GameEvent(
                GameEventType.CardPlayed,
                $"{context.CurrentPlayer.Name} played {Name} ({Power})"
            ));
        }

        public override Card Clone()
        {
            var clone = new UnitCard(Name, Power, Row);
            clone.Image = Image;
            return clone;
        }
    }
}