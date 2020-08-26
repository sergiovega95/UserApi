using Core.Entities.Identity;
using Core.Entities.Shared;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
        User GetUserById(int IdUser);

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
        void DeleteUser (int IdUser);

        /// <summary>
        /// Get document type by enum
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        DocumentType GetDocumentTypeByEnum(string Enum);

        /// <summary>
        ///   SignIn
        /// </summary>
        /// <param name="identification"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<SignInResult> LoginAsync(string identification, string password);

        /// <summary>
        /// Get documents
        /// </summary>
        /// <returns></returns>
        List<DocumentType> GetDocumentTypes();
    }
}
