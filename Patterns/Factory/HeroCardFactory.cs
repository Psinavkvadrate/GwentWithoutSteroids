using GwentWithoutSteroids.Core.Cards;
using GwentWithoutSteroids.Core.Board;
using GwentWithoutSteroids.Rendering.Proxy;

namespace GwentWithoutSteroids.Patterns.Factory
{
  public class HeroCardFactory : ICardFactory
  {
      public UnitCard CreateUnit(string name, int power, RowType row)
      {
          var card = new UnitCard("[HERO] " + name, power + 2, row);
          card.Image = new CardImageProxy("assets/dark.png");
          return card;
      }

      public SkillCard CreateSkill(string name, string effectId)
      {
          var card = new SkillCard("[HERO] " + name, effectId);
          card.Image = new CardImageProxy("assets/dark.png");
          return card;
      }
  }
}
