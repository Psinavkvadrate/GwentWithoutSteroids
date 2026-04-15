using GwentLikeGame.Core.Cards;
using GwentLikeGame.Core.Board;

namespace GwentLikeGame.Patterns.Factory
{
    public interface ICardFactory
    {
        UnitCard CreateUnit(string name, int power, RowType row);
        SkillCard CreateSkill(string name, string effectId);
    }
}