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
                .AddMelee("Vernon Roche", 10)
                .AddMelee("John Natalis", 10)
                .AddMelee("Esterad Thyssen", 10)
                .AddMelee("Philippa Eilhart", 10)
                .AddMelee("Redanian Infantry", 1)
                .AddMelee("Redanian Infantry", 1)
                .AddMelee("Poor Infantry", 1)
                .AddMelee("Poor Infantry", 1)
                .AddMelee("Yarpen Zigrin", 2)
                .AddMelee("Roach", 3)
                .AddMelee("Commando", 4)
                .AddMelee("Commando", 4)
                .AddMelee("Sigismund Dijkstra", 4)
                .AddMelee("Prince Stennis", 5)
                .AddMelee("Siegfried", 5)
                .AddMelee("Bianca", 5)
                .AddMelee("Bianca", 5)
                .AddMelee("Yarpen Zigrin", 2)
                .AddRanged("Sheldon Skaggs", 4)
                .AddRanged("Sabrina Glevissig", 4)
                .AddRanged("Crinfrid Reavers", 5)
                .AddRanged("Crinfrid Reavers", 5)
                .AddRanged("Sheala de Tancarville", 5)
                .AddRanged("Keira Metz", 5)
                .AddRanged("Detmold", 6)
                .AddRanged("Detmold", 6)
                .AddSiege("Kaedweni Siege Master", 1)
                .AddSiege("Kaedweni Siege Master", 1)
                .AddSiege("Thaler", 1)
                .AddSiege("Medic of the Blue Stripes", 5)
                .AddSiege("Siege Tower", 6)
                .AddSiege("Ballista", 6)
                .AddSiege("Trebuchet", 6)
                .AddSiege("Catapult", 8)
                .AddSkill("Battle Cry", "boost_row_2")
                .AddSkill("Double Boost", "boost_row_x2")
                .AddSkill("Execute Strongest", "kill_strongest")
                .AddSkill("Execute Weakest", "kill_weakest")
                .AddSkill("Weaken", "weaken_random_3")
                .AddSkill("Mass Execution", "kill_all_strongest")
                .Build();


            var deck2 = builder
                .Reset()
                .AddMelee("Draug", 8)
                .AddMelee("Imlerith", 8)
                .AddMelee("Leshen", 8)
                .AddMelee("Kayran", 6)
                .AddMelee("Ghoul", 1)
                .AddMelee("Ghoul", 1)
                .AddMelee("Nekker", 2)
                .AddMelee("Nekker", 2)
                .AddMelee("Foglet", 2)
                .AddMelee("Arachas", 4)
                .AddMelee("Arachas", 4)
                .AddMelee("Botchling", 4)
                .AddMelee("Bruxa", 4)
                .AddMelee("Garkain", 4)
                .AddMelee("Fleder", 4)
                .AddMelee("Ekimmara", 4)
                .AddMelee("Katakan", 5)
                .AddMelee("Forktail", 5)
                .AddMelee("Werewolf", 5)
                .AddMelee("Griffin", 5)
                .AddRanged("Basilisk", 2)
                .AddRanged("Wyvern", 2)
                .AddRanged("Gargoyle", 2)
                .AddRanged("Endriaga", 2)
                .AddRanged("Grave Hag", 5)
                .AddRanged("Toad Prince", 7)
                .AddRanged("Toad Prince", 7)
                .AddRanged("Grave Hag", 5)
                .AddSiege("Ice Giant", 5)
                .AddSiege("Arachas Behemoth", 4)
                .AddSiege("Earth Elemental", 4)
                .AddSiege("Fire Elemental", 4)
                .AddSiege("Fire Elemental", 4)
                .AddSiege("Arachas Behemoth", 4)
                .AddSkill("Monster Rage", "boost_row_2")
                .AddSkill("Berserk", "boost_row_x2")
                .AddSkill("Devour", "kill_weakest")
                .AddSkill("Hunt", "kill_strongest")
                .AddSkill("Plague", "kill_all_strongest")
                .AddSkill("Curse", "weaken_random_3")
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