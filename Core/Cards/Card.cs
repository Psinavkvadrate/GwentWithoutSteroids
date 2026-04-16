using GwentWithoutSteroids.Core.GameLogic;
using GwentWithoutSteroids.Rendering.Proxy;
namespace GwentWithoutSteroids.Core.Cards
{
    public abstract class Card
    {
        public string Name { get; }
        public int Power { get; set; }
        public ICardImage Image { get; set; }

        protected Card(string name, int power)
        {
            Name = name;
            Power = power;
        }

        public abstract void Play(GameContext context);

        public abstract Card Clone();
    }
}