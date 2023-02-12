using JetBrains.Annotations;

namespace Model
{
    public class LeaderboardEntry
    {
        public LeaderboardEntry(string name, string score, string id = null)
        {
            Name = name;
            Score = score;
            Id = id;
        }

        public string Name { get; set; }
        public string Score { get; set; }
        [CanBeNull] public string Id { get; set; }
    }
}