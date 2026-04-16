using System;
using GwentWithoutSteroids.Core.Players;
using GwentWithoutSteroids.Core.Cards;
using System.Collections.Generic;
using GwentWithoutSteroids.Patterns.Observer;
using GwentWithoutSteroids.AI;

namespace GwentWithoutSteroids.Core.GameLogic
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

        private Player _current;
        private Player _opponent;

        private GameState _state = GameState.CoinFlip;
        private int _mulligansLeft = 2;

        public int MulligansLeft => _mulligansLeft;
        public GameState State => _state;
        private DateTime _aiTurnTime;
        private int _aiDelayMs;
        private bool _aiThinking = false;
        public int PlayerRounds => _playerRounds;
        public int AiRounds => _aiRounds;

        public Game(Player player, Player ai)
        {
            _player = player;
            _ai = ai;
        }

        public void Init()
        {
            _player.Draw(10);
            _ai.Draw(10);
        }

        public void Tick()
        {
            switch (_state)
            {
                case GameState.CoinFlip:
                    StartCoinFlip();
                    break;

                case GameState.Mulligan:
                    WaitingForPlayerInput = true;
                    if (_mulligansLeft <= 0)
                    {
                        EndMulligan();
                    }
                    break;

                case GameState.Playing:
                    UpdateRound();
                    break;

                case GameState.RoundEnd:
                    EndRound();
                    break;
            }
        }

        private void StartCoinFlip()
        {
            var rnd = new Random();

            if (rnd.Next(2) == 0)
            {
                _current = _player;
                _opponent = _ai;
            }
            else
            {
                _current = _ai;
                _opponent = _player;
            }

            _mulligansLeft = 2;

            Notify(new GameEvent(
                GameEventType.RoundStarted,
                $"{_current.Name} starts!"
            ));

            _state = GameState.Mulligan;
        }

        public void ReplaceCard(int index)
        {
            if (_state != GameState.Mulligan || _mulligansLeft <= 0)
                return;

            var player = _player;

            if (index < 0 || index >= player.Hand.Count)
                return;

            var deck = player.Deck.GetCards();
            if (deck.Count == 0)
                return;

            var rnd = new Random();

            var oldCard = player.Hand[index];
            player.Hand.RemoveAt(index);

            var newCardTemplate = deck[rnd.Next(deck.Count)];

            var newCard = newCardTemplate.Clone();

            player.Hand.Insert(index, newCard);

            _mulligansLeft--;

            Notify(new GameEvent(
                GameEventType.CardReplaced,
                $"Replaced {oldCard.Name} with {newCard.Name}"
            ));
        }

        public void EndMulligan()
        {
            if (_state != GameState.Mulligan)
                return;

            Notify(new GameEvent(
                GameEventType.Info,
                "Mulligan finished"
            ));

            StartRound();
            _state = GameState.Playing;
        }

        private void MulliganAI()
        {
            var deck = _ai.Deck.GetCards();
            var rnd = new Random();

            for (int i = 0; i < 2 && deck.Count > 0 && _ai.Hand.Count > 0; i++)
            {
                int index = rnd.Next(_ai.Hand.Count);

                var oldCard = _ai.Hand[index];
                _ai.Hand.RemoveAt(index);

                var newCard = deck[rnd.Next(deck.Count)];

                deck.Remove(newCard);
                deck.Add(oldCard);

                _ai.Hand.Add(newCard);
            }
        }

        private void StartRound()
        {
            ResetRound();
            MulliganAI();

            Notify(new GameEvent(
                GameEventType.Info,
                $"Round started. {_current.Name} goes first"
            ));
        }

        private void ResetRound()
        {
            _player.HasPassed = false;
            _ai.HasPassed = false;

            _player.Board.Clear();
            _ai.Board.Clear();
        }

        private void UpdateRound()
        {
            if (_player.HasPassed && _ai.HasPassed)
            {
                _state = GameState.RoundEnd;
                return;
            }

            if (_current.HasPassed)
            {
                SwapTurns();
                return;
            }

            if (_current == _player)
            {
                WaitingForPlayerInput = true;
                return;
            }

            if (_current == _ai)
            {
                WaitingForPlayerInput = false;

                if (!_aiThinking)
                {
                    _aiThinking = true;

                    var rnd = new Random();
                    _aiDelayMs = rnd.Next(1000, 4000); 
                    _aiTurnTime = DateTime.Now;
                }

                var elapsed = (DateTime.Now - _aiTurnTime).TotalMilliseconds;

                if (elapsed >= _aiDelayMs)
                {
                    _aiBrain.MakeMove(_ai, _player, this);
                    _aiThinking = false;
                    SwapTurns();
                }
            }
        }

        public void PlayCard(Card card)
        {
            if (_state != GameState.Playing || _current != _player)
                return;

            if (!_player.Hand.Contains(card))
                return;

            _player.Hand.Remove(card);

            card.Play(new GameContext
            {
                CurrentPlayer = _player,
                Opponent = _ai,
                Game = this
            });

            WaitingForPlayerInput = false;
            SwapTurns();
        }

        public void PassTurn()
        {
            if (_current != _player)
                return;

            _player.HasPassed = true;

            Notify(new GameEvent(
                GameEventType.PlayerPassed,
                "Player passed"
            ));

            WaitingForPlayerInput = false;
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
                $"Round: {playerPower} - {aiPower}",
                _playerRounds,
                _aiRounds
            ));

            _player.Board.Clear();
            _ai.Board.Clear();

            if (_playerRounds >= 2 || _aiRounds >= 2)
            {
                IsFinished = true;

                Notify(new GameEvent(
                    GameEventType.GameEnded,
                    _playerRounds > _aiRounds ? "PLAYER WINS" : "AI WINS",
                    _playerRounds,
                    _aiRounds
                ));

                return;
            }
            _player.Board.Clear();
            _ai.Board.Clear();

            DrawAfterRound();

            _state = GameState.CoinFlip;
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

        public Player GetOpponent(Player p)
        {
            return p == _player ? _ai : _player;
        }

        private void DrawAfterRound()
        {
            _player.Draw(3);
            _ai.Draw(3);

            Notify(new GameEvent(
                GameEventType.Info,
                "Both players draw 3 cards"
            ));
        }

        public int GetAiHandCount()
        {
            return _ai.Hand.Count;
        }

        public void Reset()
        {
            _playerRounds = 0;
            _aiRounds = 0;

            _player.Board.Clear();
            _ai.Board.Clear();

            _player.Hand.Clear();
            _ai.Hand.Clear();

            _player.Deck.Reset();
            _ai.Deck.Reset();     

            IsFinished = false;
            _state = GameState.CoinFlip;

            Init();
        }
    }
}