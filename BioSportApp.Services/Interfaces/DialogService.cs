namespace BioSportApp.Services.Interfaces
{
    public class DialogService : IDialogService
    {
        public async Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons)
        {
            return await Application.Current.MainPage.DisplayActionSheet(title, cancel, destruction, buttons);
        }
    }
}
