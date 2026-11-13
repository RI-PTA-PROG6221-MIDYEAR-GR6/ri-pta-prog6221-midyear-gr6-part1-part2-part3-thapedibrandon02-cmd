namespace CybersecurityAwarenessBot2
{
    /// <summary>Generic memory container — stores user details across turns.</summary>
    public class UserMemory
    {
        public string Name { get; set; } = string.Empty;
        public string FavouriteTopic { get; set; } = string.Empty;
        public string LastTopic { get; set; } = string.Empty;
    }
}