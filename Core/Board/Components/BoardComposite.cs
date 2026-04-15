using System.Collections.Generic;
using System.Linq;
using GwentLikeGame.Core.Board;
using GwentLikeGame.Core.Board.Components;

namespace GwentLikeGame.Core.Board.Components
{
    public class BoardComposite : IBoardComponent
    {
        private readonly Dictionary<RowType, Row> _rows = new();

        public BoardComposite()
        {
            _rows[RowType.Melee] = new Row();
            _rows[RowType.Ranged] = new Row();
            _rows[RowType.Siege] = new Row();
        }

        public Row GetRow(RowType type) => _rows[type];

        public Dictionary<RowType, Row> GetRows() => _rows;

        public int GetPower()
        {
            return _rows.Values.Sum(r => r.GetPower());
        }

        public void Clear()
        {
            foreach (var row in _rows.Values)
                row.Clear();
        }
    }
}