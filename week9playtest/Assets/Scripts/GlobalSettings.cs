public static class GlobalSettings
{
    public static Difficulty SelectedDifficulty { get; set; } = Difficulty.Medium;
}

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}