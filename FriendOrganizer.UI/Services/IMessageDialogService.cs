namespace FriendOrganizer.UI.Services
{
    public interface IMessageDialogService
    {
        MessageDialogService.MessageDialogResult ShowOkCancelDialog(string text, string title);
        void ShowInfoDialog(string text);
    }
}