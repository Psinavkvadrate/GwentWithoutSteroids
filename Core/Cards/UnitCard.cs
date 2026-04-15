using GwentLikeGame.Core.Board;
using GwentLikeGame.Core.GameLogic;

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
        }
    }
}