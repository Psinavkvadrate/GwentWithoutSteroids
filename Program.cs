using GwentWithoutSteroids.Patterns.Builder;
using GwentWithoutSteroids.Core.Players;
using GwentWithoutSteroids.Core.GameLogic;
using GwentWithoutSteroids.Patterns.Factory;
using GwentWithoutSteroids.Patterns.Observer;
using GwentWithoutSteroids.Rendering;

namespace GwentWithoutSteroids
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new DefaultCardFactory();
            var builder = new DeckBuilder(factory);

            var deck1 = builder
                .Reset()
                .AddMelee("Infantry", 5)
                .AddMelee("Knight", 7)
                .AddRanged("Archer", 4)
                .AddRanged("Crossbow", 3)
                .AddSiege("Catapult", 6)
                .AddSiege("Trebuchet", 8)
                .AddSkill("Boost Row", "boost_row_2")
                .AddSkill("Kill Strongest", "kill_strongest")
                .AddMelee("Infantry", 5)
                .AddMelee("Infantry", 5)
                .AddMelee("Infantry", 5)
                .AddMelee("Knight", 7)
                .AddMelee("Knight", 7)
                .AddRanged("Archer", 4)
                .AddRanged("Archer", 4)
                .AddRanged("Crossbow", 3)
                .AddSiege("Catapult", 6)
                .AddSiege("Trebuchet", 8)
                .AddMelee("Infantry", 5)
                .AddMelee("Knight", 7)
                .AddRanged("Archer", 4)
                .AddRanged("Crossbow", 3)
                .AddSiege("Catapult", 6)
                .AddSiege("Trebuchet", 8)
                .AddSkill("Boost Row", "boost_row_2")
                .AddSkill("Kill Strongest", "kill_strongest")
                .AddMelee("Infantry", 5)
                .AddMelee("Infantry", 5)
                .AddMelee("Infantry", 5)
                .AddMelee("Knight", 7)
                .AddMelee("Knight", 7)
                .AddRanged("Archer", 4)
                .AddRanged("Archer", 4)
                .AddRanged("Crossbow", 3)
                .AddSiege("Catapult", 6)
                .AddSiege("Trebuchet", 8)
                .Build();

            var deck2 = builder
                .Reset()
                .AddMelee("Infantry", 5)
                .AddRanged("Archer", 4)
                .AddSiege("Catapult", 6)
                .AddSkill("Random Damage", "random_3")
                .AddSiege("Catapult", 6)
                .AddSiege("Trebuchet", 8)
                .AddSkill("Boost Row", "boost_row_2")
                .AddSkill("Kill Strongest", "kill_strongest")
                .AddMelee("Infantry", 5)
                .AddMelee("Infantry", 5)
                .AddMelee("Infantry", 5)
                .AddMelee("Knight", 7)
                .AddMelee("Knight", 7)
                .AddRanged("Archer", 4)
                .AddRanged("Archer", 4)
                .AddRanged("Crossbow", 3)
                .AddSiege("Catapult", 6)
                .AddSiege("Trebuchet", 8)
                .AddMelee("Infantry", 5)
                .AddMelee("Knight", 7)
                .AddRanged("Archer", 4)
                .AddRanged("Crossbow", 3)
                .AddSiege("Catapult", 6)
                .AddSiege("Trebuchet", 8)
                .AddSkill("Boost Row", "boost_row_2")
                .AddSkill("Kill Strongest", "kill_strongest")
                .AddMelee("Infantry", 5)
                .AddMelee("Infantry", 5)
                .AddMelee("Infantry", 5)
                .AddMelee("Knight", 7)
                .AddMelee("Knight", 7)
                .AddRanged("Archer", 4)
                .AddRanged("Archer", 4)
                .AddRanged("Crossbow", 3)
                .AddSiege("Catapult", 6)
                .AddSiege("Trebuchet", 8)
                .AddSiege("Catapult", 6)
                .AddSiege("Trebuchet", 8)
                .AddSkill("Boost Row", "boost_row_2")
                .AddSkill("Kill Strongest", "kill_strongest")
                .AddMelee("Infantry", 5)
                .AddMelee("Infantry", 5)
                .AddMelee("Infantry", 5)
                .AddMelee("Knight", 7)
                .AddMelee("Knight", 7)
                .AddRanged("Archer", 4)
                .AddRanged("Archer", 4)
                .AddRanged("Crossbow", 3)
                .AddSiege("Catapult", 6)
                .AddSiege("Trebuchet", 8)
                .AddMelee("Infantry", 5)
                .AddMelee("Knight", 7)
                .AddRanged("Archer", 4)
                .AddRanged("Crossbow", 3)
                .AddSiege("Catapult", 6)
                .AddSiege("Trebuchet", 8)
                .AddSkill("Boost Row", "boost_row_2")
                .AddSkill("Kill Strongest", "kill_strongest")
                .AddMelee("Infantry", 5)
                .AddMelee("Infantry", 5)
                .AddMelee("Infantry", 5)
                .AddMelee("Knight", 7)
                .AddMelee("Knight", 7)
                .AddRanged("Archer", 4)
                .AddRanged("Archer", 4)
                .AddRanged("Crossbow", 3)
                .AddSiege("Catapult", 6)
                .AddSiege("Trebuchet", 8)
                .Build();

            var player = new Player("Player", deck1);
            var ai = new Player("AI", deck2);

            var game = new Game(player, ai);

            var console = new ConsoleGameLogger();
            var ui = new GameUiObserver();

            game.Init();

            game.Subscribe(console);
            game.Subscribe(ui);
            
            var window = new GameWindow(game, player, ui);
            window.Run();
        }
    }
}