﻿namespace CarListApp.Models
{
    public class LoginModel
    {
        public LoginModel(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public string UserName { get; set; }
        public string Password { get; set; }
    }
}