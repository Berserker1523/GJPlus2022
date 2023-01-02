namespace Kitchen
{
    [System.Flags]
    public enum CookingToolName
    {
        Mortar = (1<<0),
        Stove = (1 << 1),
        None = (1 << 2)
    }

    public enum CookingToolOnlyMortar { Mortar = 0,}
    public enum CookingToolOnlyStove { Stove = 1,}
    public enum CookingToolOnlyNone { None = 2,}
    public enum CookingToolBoth { Mortar = 0, Stove =1}
}
