using System;
using BusinessObjects.Models;

namespace Services
{
    public static class UserSession
    {
        public static User? CurrentUser { get; set; }

        public static int CurrentUserId => CurrentUser?.UserId ?? 1;

        public static string FullName => CurrentUser?.FullName ?? "Người Dùng";

        public static string Email => CurrentUser?.Email ?? "user@example.com";

        public static string GetInitials(string? fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return "U";
            var parts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1) return parts[0][0].ToString().ToUpper();
            return (parts[0][0].ToString() + parts[^1][0].ToString()).ToUpper();
        }
    }
}
