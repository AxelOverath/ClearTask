﻿using ClearTask.Models;

namespace ClearTask.Data;

    public static class UserStorage
    {
        // Properties to hold user data
        public static string Username { get; set; }
        public static string Email { get; set; }
        public static string Password { get; set; }
        public static Role UserRole { get; set; }

        // Method to clear stored user data when the user logs out
        public static void ClearUserData()
        {
            Username = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            UserRole = Role.Employee; // Default role or make it null if needed
        }
    }



