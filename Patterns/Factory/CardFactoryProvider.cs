namespace GwentWithoutSteroids.Patterns.Factory
{
    public enum CardStyle
    {
        Default,
        Hero
    }

    public static class CardFactoryProvider
    {
        public static ICardFactory Create(CardStyle style)
        {
            return style switch
            {
                CardStyle.Hero => new HeroCardFactory(),
                _ => new DefaultCardFactory()
            };
        }
    }
}