public class CardBuilder
{
    private Card _card;

    public CardBuilder CreateUnit(string name, int power, RowType row)
    {
        _card = new UnitCard(name, power, row);

        string path = row switch
        {
            RowType.Melee => "assets/melee.png",
            RowType.Ranged => "assets/ranged.png",
            RowType.Siege => "assets/siege.png",
            _ => "assets/default.png"
        };

        _card.Image = new CardImageProxy(path);

        return this;
    }

    public CardBuilder CreateSpell(string name)
    {
        _card = new SpecialCard(name);

        _card.Image = new CardImageProxy("assets/cards/spell.png");

        return this;
    }

    public Card Build() => _card;
}