namespace Kitchen
{
    [System.Flags]
    public enum DiseaseName
    {
        HeadAche = (1 << 0), 
        ToothAche = (1 << 1), 
        StomachAche = (1 << 2), 
        Appendicitis = (1 << 3), 
    }
}
