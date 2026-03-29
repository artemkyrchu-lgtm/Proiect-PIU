namespace GestionareaEnum
{
    [Flags]
    public enum Rolu    {
        Dps = 1 << 0,
        Tank = 1 << 1,
        Healer = 1 << 2,
    }
    public enum Ranku {         
        Bronze = 1,
        Silver = 2,
        Gold = 3,
        Platinum = 4,
        Diamond = 5,
        Grandmaster = 6,
        Celestial = 7,
        Eternity = 8,
        One_Above_All = 9

    }
}
