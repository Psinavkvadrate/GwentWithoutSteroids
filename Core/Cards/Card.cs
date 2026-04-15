using GwentLikeGame.Core.GameLogic;

namespace GwentLikeGame.Core.Cards
{
    public abstract class Card
    {
        public string Name { get; }
        public int Power { get; set; }

        protected Card(string name, int power)
        {
            Name = name;
            Power = power;
        }

        public abstract void Play(GameContext context);
    }
}