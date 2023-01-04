namespace Kitchen
{
    [System.Flags]
    public enum CookingToolName
    {
        Mortar = (1<<0),
        Stove = (1 << 1),
        None = (1 << 2)
    }
}
