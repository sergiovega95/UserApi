using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    public interface IUsers
    {
        /// <summary>
        /// List all user
        /// </summary>
        /// <returns></returns>
        public List<User> GetAllUsers();

        /// <summary>
       /// Get User by Id Column
       /// </summary>
       /// <param name="IdUser"></param>
       /// <returns></returns>
        User GetUserById(string IdUser);

        /// <summary>
        /// Get User by Identification
        /// </summary>
        /// <param name="IdUser"></param>
        /// <returns></returns>
        User GetUserByIdentification(string Identification);

        /// <summary>
        /// Add new user to database
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task<IdentityResult> AddUserAsync(User newUser, string password);

        /// <summary>
        /// Update user information
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        void UpdateUser(User User);

        /// <summary>
        /// Delete User with specific Id
        /// </summary>
        /// <param name="IdUser"></param>
        void DeleteUser (string IdUser);

    }
}
