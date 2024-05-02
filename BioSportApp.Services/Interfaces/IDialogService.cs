namespace BioSportApp.Services.Interfaces
{
    public interface IDialogService
    {
        Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons);
    }
}
