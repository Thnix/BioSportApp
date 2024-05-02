namespace BioSportApp.Common.Security
{
    public class PasswordHash
    {
        public string Hash { get; set; } = null!;
        public string Salt { get; set; } = null!;
    }
}