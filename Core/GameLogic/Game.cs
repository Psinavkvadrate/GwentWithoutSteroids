using System;
using System.Linq;
using GwentLikeGame.Core.Players;
using GwentLikeGame.Core.Cards;
using System.Collections.Generic;
using GwentLikeGame.Patterns.Observer;
using GwentLikeGame.AI;

namespace GwentLikeGame.Core.GameLogic
{
    public class Game : ISubject
    {
        private Player _player;
        private Player _ai;
        private List<IObserver> _observers = new();
        private AiPlayer _aiBrain = new();
        public bool WaitingForPlayerInput { get; private set; }

        private int _playerRounds = 0;
        private int _aiRounds = 0;

        public bool IsFinished { get; private set; }
        private bool _roundStarted = false;

        private Player _current;
        private Player _opponent;

        public Game(Player player, Player ai)
        {
            _player = player;
            _ai = ai;
        }

        public void Start()
        {
            _player.Draw(10);
            _ai.Draw(10);

            Mulligan(_player);
            MulliganAI(_ai);

            PlayGame();
        }

        private void PlayGame()
        {
            while (_playerRounds < 2 && _aiRounds < 2)
            {
                Console.WriteLine($"\n=== NEW ROUND ===");
                ResetRound();

                PlayRound();

                int playerPower = _player.Board.GetPower();
                int aiPower = _ai.Board.GetPower();

                Console.WriteLine($"\nResult: Player {playerPower} - AI {aiPower}");

                if (playerPower > aiPower)
                {
                    _playerRounds++;
                    Console.WriteLine("Player wins round!");
                }
                else if (aiPower > playerPower)
                {
                    _aiRounds++;
                    Console.WriteLine("AI wins round!");
                }
                else
                {
                    Console.WriteLine("Draw round!");
                }

                Console.WriteLine($"Score: Player {_playerRounds} - AI {_aiRounds}");
            }

            Console.WriteLine("\n=== GAME OVER ===");
            Console.WriteLine(_playerRounds > _aiRounds ? "PLAYER WINS!" : "AI WINS!");
        }

        private void ResetRound()
        {
            _player.HasPassed = false;
            _ai.HasPassed = false;

            _player.Board.Clear();
            _ai.Board.Clear();
        }

        private void PlayRound()
        {
            var current = _player;
            var opponent = _ai;

            // монетка (рандом)
            if (new Random().Next(2) == 1)
                (current, opponent) = (opponent, current);

            Console.WriteLine($"{current.Name} starts first");

            while (true)
            {
                if (!current.HasPassed)
                {
                    Console.WriteLine($"\n{current.Name} turn");

                    if (current == _player)
                        PlayerTurn(current, opponent);
                    else
                        AITurn(current, opponent);
                }

                // если оба спасовали — конец раунда
                if (current.HasPassed && opponent.HasPassed)
                    break;

                // если один спасовал — второй доигрывает
                if (current.HasPassed && !opponent.HasPassed)
                {
                    (current, opponent) = (opponent, current);
                    continue;
                }

                (current, opponent) = (opponent, current);
            }
        }

        private void PlayerTurn(Player player, Player opponent)
        {
            Console.WriteLine("Hand:");
            for (int i = 0; i < player.Hand.Count; i++)
            {
                Console.WriteLine($"{i}: {player.Hand[i].Name} ({player.Hand[i].Power})");
            }

            Console.WriteLine("Enter index or -1 to pass:");
            int input = int.Parse(Console.ReadLine());

            if (input == -1)
            {
                player.HasPassed = true;
                return;
            }

            var card = player.Hand[input];
            player.Hand.RemoveAt(input);

            card.Play(new GameContext
            {
                CurrentPlayer = player,
                Opponent = opponent
            });
        }

        private void AITurn(Player ai, Player opponent)
        {
            _aiBrain.MakeMove(ai, opponent);
        }

        private void Mulligan(Player player)
        {
            Console.WriteLine($"\n{player.Name} Mulligan phase");

            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine("Hand:");
                for (int j = 0; j < player.Hand.Count; j++)
                {
                    Console.WriteLine($"{j}: {player.Hand[j].Name} ({player.Hand[j].Power})");
                }

                Console.WriteLine("Enter index to replace or -1 to skip:");
                int input = int.Parse(Console.ReadLine());

                if (input == -1)
                    continue;

                var oldCard = player.Hand[input];
                player.Hand.RemoveAt(input);

                var deckCards = player.Deck.GetCards();
                if (deckCards.Count == 0)
                    continue;

                var rnd = new Random();
                var newCard = deckCards[rnd.Next(deckCards.Count)];

                deckCards.Remove(newCard);
                deckCards.Add(oldCard);

                player.Hand.Add(newCard);

                Console.WriteLine($"Replaced {oldCard.Name} with {newCard.Name}");
            }
        }
        
        private void MulliganAI(Player ai)
        {
            var deckCards = ai.Deck.GetCards();
            var rnd = new Random();

            for (int i = 0; i < 2 && ai.Hand.Count > 0 && deckCards.Count > 0; i++)
            {
                var index = rnd.Next(ai.Hand.Count);

                var oldCard = ai.Hand[index];
                ai.Hand.RemoveAt(index);

                var newCard = deckCards[rnd.Next(deckCards.Count)];

                deckCards.Remove(newCard);
                deckCards.Add(oldCard);

                ai.Hand.Add(newCard);
            }
        }

        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(GameEvent gameEvent)
        {
            foreach (var o in _observers)
                o.OnEvent(gameEvent);
        }

        public void Init()
        {
            _player.Draw(10);
            _ai.Draw(10);
        }

        public void Tick()
        {
            if (IsFinished)
                return;

            if (!_roundStarted)
            {
                StartRound();
                _roundStarted = true;
            }

            UpdateRound();
        }

        private void StartRound()
        {
            ResetRound();

            var current = _player;
            var opponent = _ai;

            if (new Random().Next(2) == 1)
                (current, opponent) = (opponent, current);

            _current = current;
            _opponent = opponent;

            Notify(new GameEvent(
                GameEventType.RoundStarted,
                $"{current.Name} starts first"
            ));
        }

        private void UpdateRound()
        {
            if (_player.HasPassed && _ai.HasPassed)
            {
                EndRound();
                _roundStarted = false;
                return;
            }

            if (_current.HasPassed)
            {
                SwapTurns();
                return;
            }

            if (_current == _player && !_player.HasPassed)
            {
                WaitingForPlayerInput = true;
                return;
            }

            if (_current == _ai && !_ai.HasPassed)
            {
                _aiBrain.MakeMove(_ai, _player);
            }

            SwapTurns();
        }

        private void SwapTurns()
        {
            (_current, _opponent) = (_opponent, _current);

            Notify(new GameEvent(
                GameEventType.TurnChanged,
                $"Turn: {_current.Name}"
            ));
        }

        private void EndRound()
        {
            int playerPower = _player.Board.GetPower();
            int aiPower = _ai.Board.GetPower();

            if (playerPower > aiPower)
                _playerRounds++;
            else if (aiPower > playerPower)
                _aiRounds++;

            Notify(new GameEvent(
                GameEventType.RoundEnded,
                $"Round ended: {playerPower} - {aiPower}",
                _playerRounds,
                _aiRounds
            ));

            if (_playerRounds >= 2 || _aiRounds >= 2)
            {
                IsFinished = true;

                Notify(new GameEvent(
                    GameEventType.GameEnded,
                    _playerRounds > _aiRounds ? "PLAYER WINS" : "AI WINS",
                    _playerRounds,
                    _aiRounds
                ));
            }
        }
        public Player GetOpponent(Player p)
        {
            return p == _player ? _ai : _player;
        }

        public void EndPlayerTurn()
        {
            WaitingForPlayerInput = false;
            SwapTurns();
        }
    }
}