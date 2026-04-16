using System;
using GwentLikeGame.Core.GameLogic;
using GwentLikeGame.Patterns.Interpreter;
using GwentLikeGame.Patterns.Observer;

namespace GwentLikeGame.Core.Cards
{
    public class SkillCard : Card
    {
        public string EffectId { get; }

        public SkillCard(string name, string effectId)
            : base(name, 0)
        {
            EffectId = effectId;
        }

        public override void Play(GameContext context)
        {
            var effect = EffectParser.Parse(EffectId);
            effect.Interpret(context);

            context.Game?.Notify(new GameEvent(
                GameEventType.CardPlayed,
                $"{context.CurrentPlayer.Name} used {Name}"
            ));
        }

        public override Card Clone()
        {
            var clone = new SkillCard(Name, EffectId);
            clone.Image = Image;
            return clone;
        }
    }
}