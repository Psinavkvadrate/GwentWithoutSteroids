using GwentLikeGame.Core.Cards;
using GwentLikeGame.Core.Board;

namespace GwentLikeGame.Patterns.Factory
{
    public class DefaultCardFactory : ICardFactory
    {
        public UnitCard CreateUnit(string name, int power, RowType row)
        {
            return new UnitCard(name, power, row);
        }

        public SkillCard CreateSkill(string name, string effectId)
        {
            return new SkillCard(name, effectId);
        }
    }
}