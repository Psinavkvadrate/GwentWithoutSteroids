using System;
using System.Linq;
using GwentLikeGame.Core.GameLogic;
using GwentLikeGame.Core.Players;
using GwentLikeGame.Core.Cards;

namespace GwentLikeGame.AI
{
    public class AiEvaluator
    {
        private Random _rnd = new();

        public AiAction Evaluate(Player ai, Player opponent)
        {
            int aiPower = ai.Board.GetPower();
            int enemyPower = opponent.Board.GetPower();
            int diff = aiPower - enemyPower;

            if (diff > 12)
                return AiAction.Pass();

            if (diff < -15 && ai.Hand.Count <= 2)
                return AiAction.Pass();

            if (!ai.Hand.Any())
                return AiAction.Pass();

            var bestCard = ai.Hand
                .OrderByDescending(c => EvaluateCard(c, ai, opponent))
                .First();

            return AiAction.Play(bestCard);
        }

        private float EvaluateCard(Card card, Player ai, Player opponent)
        {
            float score = 0;

            int aiPower = ai.Board.GetPower();
            int enemyPower = opponent.Board.GetPower();
            int diff = aiPower - enemyPower;

            if (card is UnitCard unit)
            {
                score += unit.Power;

                if (diff < 0)
                    score += unit.Power * 0.5f;

                if (diff > 5)
                    score -= unit.Power * 0.3f;
            }

            if (card is SkillCard skill)
            {
                score += EvaluateSkill(skill, ai, opponent);
            }

            score += (float)_rnd.NextDouble();

            return score;
        }

        private float EvaluateSkill(SkillCard skill, Player ai, Player opponent)
        {
            int enemyPower = opponent.Board.GetPower();
            int aiPower = ai.Board.GetPower();

            return skill.EffectId switch
            {
                "boost_row_2" => aiPower * 0.5f,

                "boost_row_x2" => aiPower,

                "kill_strongest" => GetStrongestEnemy(opponent),

                "kill_weakest" => GetWeakestEnemySum(opponent),

                "kill_all_strongest" => GetStrongestEnemy(opponent) * 1.5f,

                "weaken_random_3" => enemyPower * 0.3f,

                _ => 1
            };
        }

        private int GetStrongestEnemy(Player opponent)
        {
            return opponent.Board.GetRows()
                .SelectMany(r => r.Value.GetCards())
                .Select(c => c.Power)
                .DefaultIfEmpty(0)
                .Max();
        }

        private int GetWeakestEnemySum(Player opponent)
        {
            return opponent.Board.GetRows()
                .SelectMany(r => r.Value.GetCards())
                .OrderBy(c => c.Power)
                .Take(2)
                .Sum(c => c.Power);
        }
    }
}