using Core.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interface
{
    public interface IUserServices
    {
        IRestResponse SignInUser(string identification, string password);
        IRestResponse GetUsers();
        IRestResponse GetUserById(int Iduser);
        IRestResponse UpdateUser(UserUpdate user);
        IRestResponse DeleteUser(int Iduser);
        IRestResponse AddUser(object user);
        IRestResponse GetDocumentTypes();
    }
}
