using Core.Entities.Identity;
using Core.Exceptions;
using Core.Interfaces;
using Infrastructure.IdentityContext;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Implementations
{
    public class UsersQuery:IUsers
    {
        private readonly DatabaseContext _database;
        private readonly UserManager<User> _userManager;
        public UsersQuery(DatabaseContext  database , UserManager<User> userManager)
        {
            _database = database;
            _userManager = userManager;
        }

        public async System.Threading.Tasks.Task<IdentityResult> AddUserAsync(User newUser, string password)
        {
            try
            {
                return await _userManager.CreateAsync(newUser, password);
            }
            catch (Exception e)
            {
                throw new DatabaseException("Ocurrió un error al insertar un nuevo usuario");
            }                       
        }

        public void DeleteUser(string IdUser)
        {
            try
            {
                User usuario = _database.Users.Find(IdUser);

                if (usuario!=null)
                {

                }
                else
                {
                    throw new Exception();
                }

            }
            catch (Exception e)
            {
                throw new DatabaseException($"Ocurrió un error al eliminar el usuario con Id {IdUser}");
            }

            throw new NotImplementedException();
        }

        public List<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public User GetUserById(string IdUser)
        {
            throw new NotImplementedException();
        }

        public User GetUserByIdentification(string Identification)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(User User)
        {
            throw new NotImplementedException();
        }
    }
}
