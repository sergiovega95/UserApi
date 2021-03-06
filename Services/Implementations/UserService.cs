﻿using Core.Models;
using Newtonsoft.Json;
using RestSharp;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Implementations
{
    public class UserService : IUserServices
    {
        private readonly string BASEURL = "http://murmullo95-001-site1.atempurl.com";

        public IRestResponse AddUser(object user)
        {          
           
            RestClient client = new RestClient(BASEURL + "/api/v1/Identity/RegisterUser");
            client.Timeout = 3000;
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", JsonConvert.SerializeObject(user), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return response;
        }

        public IRestResponse DeleteUser(int IdUser)
        {
            RestClient client = new RestClient(BASEURL + $"/api/v1/Identity/DeleteUser?IdUser={IdUser}");
            client.Timeout = 3000;
            RestRequest request = new RestRequest(Method.DELETE);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            return response;
        }

        public IRestResponse GetUserById(int IdUser)
        {
            RestClient client = new RestClient(BASEURL + "");
            client.Timeout = 3000;
            RestRequest request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            return response;
        }

        public IRestResponse GetUsers()
        {
            RestClient client = new RestClient(BASEURL + "/api/v1/Identity/Users");
            client.Timeout = 3000;
            RestRequest request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            return response;
        }

        public IRestResponse SignInUser(string identification, string password)
        {
            RestClient client = new RestClient(BASEURL + $"/api/v1/Identity/SignInUser?document={identification}&password={password}");
            client.Timeout = 3000;
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            return response;
        }

        public IRestResponse UpdateUser(UserUpdate user)
        {
            RestClient client = new RestClient(BASEURL + "/api/v1/Identity/UpdateUser");
            client.Timeout = 3000;
            RestRequest request = new RestRequest(Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", JsonConvert.SerializeObject(user), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return response;
        }               

        public IRestResponse GetDocumentTypes()
        {
            RestClient client = new RestClient(BASEURL + "/api/v1/Identity/DocumentsType");
            client.Timeout = 3000;
            RestRequest request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            return response;
        }
    }
}
