using System;

namespace EventEaseApp.Services
{
    public class UserSessionState
    {
        public string CurrentUserEmail { get; private set; } = string.Empty;
        public bool IsLoggedIn => !string.IsNullOrEmpty(CurrentUserEmail);

        public event Action? OnStateChanged;

        public void Login(string email)
        {
            CurrentUserEmail = email;
            NotifyStateChanged();
        }

        public void Logout()
        {
            CurrentUserEmail = string.Empty;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnStateChanged?.Invoke();
    }
}