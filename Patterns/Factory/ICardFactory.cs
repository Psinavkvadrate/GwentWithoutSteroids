using GwentWithoutSteroids.Core.Cards;
using GwentWithoutSteroids.Core.Board;

namespace GwentWithoutSteroids.Patterns.Factory
{
    public interface ICardFactory
    {
        UnitCard CreateUnit(string name, int power, RowType row);
        SkillCard CreateSkill(string name, string effectId);
    }
}