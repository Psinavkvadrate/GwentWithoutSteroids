using GwentLikeGame.Core.Cards;
using GwentLikeGame.Core.Board;
using GwentLikeGame.Rendering.Proxy;

namespace GwentLikeGame.Patterns.Factory
{
    public class DefaultCardFactory : ICardFactory
    {
        public UnitCard CreateUnit(string name, int power, RowType row)
        {
            var card = new UnitCard(name, power, row);

            card.Image = new CardImageProxy(GetPath(row));

            return card;
        }

        public SkillCard CreateSkill(string name, string effectId)
        {
            var card = new SkillCard(name, effectId);

            card.Image = new CardImageProxy("assets/default.png");

            return card;
        }

        private string GetPath(RowType row)
        {
            return row switch
            {
                RowType.Melee => "assets/melee.png",
                RowType.Ranged => "assets/ranged.png",
                RowType.Siege => "assets/siege.png",
                _ => "assets/default.png"
            };
        }
    }
}