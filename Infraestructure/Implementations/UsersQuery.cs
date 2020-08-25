using Core.Entities.Identity;
using Core.Entities.Shared;
using Core.Exceptions;
using Core.Interfaces;
using Infrastructure.IdentityContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementations
{
    public class UsersQuery:IUsers
    {
        private readonly DatabaseContext _database;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public UsersQuery(DatabaseContext  database , UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _database = database;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async System.Threading.Tasks.Task<IdentityResult> AddUserAsync(User newUser, string password)
        {
            if (_database.Users.Any<User>(s=>s.Document.ToLower()==newUser.Document.ToLower()))
            {
                throw new UserException($"Ya existe un usuario registrado con el número de documento {newUser.Document.ToLower()}");
            }
            if (_database.Users.Any<User>(s => s.Email.ToLower() == newUser.Email.ToLower()))
            {
                throw new UserException($"Ya existe un usuario registrado con el email {newUser.Email.ToLower()}");
            }
            else
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
        }

        public void DeleteUser(string IdUser)
        {
             User usuario = _database.Users.Where(s=>s.Id.ToString().ToLower()== IdUser.ToLower());

            if (usuario!=null)
            {
                try
                {
                  _database.Remove<User>(usuario);
                  _database.SaveChanges();
                }
                catch (Exception e)
                {
                    throw new DatabaseException($"Ocurrió un error al eliminar el usuario con Id {IdUser}");
                }
            }
            else
            {
                throw new UserException($"El usuario con Id {IdUser}, no se encuentra registrado");
            }         
                   
        }

        public List<User> GetAllUsers()
        {
            try
            {
                return  _database.Users.Include(s => s.DocumentType).ToList();
               
            }
            catch (Exception e)
            {
                throw new DatabaseException($"Ocurrió un error al consultar los usuarios");
            }
           
        }

        public DocumentType GetDocumentTypeByEnum(string Enum)
        {
            DocumentType documentType = _database.DocumentType.Where(s=>s.Enum.ToUpper()==Enum.ToUpper()).FirstOrDefault();

            if (documentType!=null)
            {
                return documentType;
            }
            else
            {
                throw new UserException($"Documento {Enum} no encontrado");
            }
        }

        public User GetUserById(string IdUser)
        {
            try
            {
                User usuario = _database.Users.Find(IdUser);

                if (usuario != null)
                {
                    return usuario;
                }
                else
                {
                    throw new UserException($"El usuario con Id {IdUser}, no se encuentra registrado");
                }

            }
            catch (Exception e)
            {
                throw new DatabaseException($"Ocurrió un error al consultar el usuario con Id {IdUser}");
            }
        }

        public User GetUserByIdentification(string Identification)
        {
            try
            {
                User usuario = _database.Users.Where(s => s.Document.ToLower() == Identification.ToLower()).Include(s=>s.DocumentType).FirstOrDefault();

                if (usuario != null)
                {
                    return usuario;
                }
                else
                {
                    throw new UserException($"El usuario con identificación {Identification.ToLower()}, no se encuentra registrado");
                }

            }
            catch (Exception e)
            {
                throw new DatabaseException($"Ocurrió un error al consultar el usuario con identificación {Identification.ToLower()}");
            }
        }

        public async Task<SignInResult> LoginAsync(string identification, string password)
        {
            return await _signInManager.PasswordSignInAsync(identification, password, true, lockoutOnFailure: false);
        }

        public void UpdateUser(User UserModified)
        {
            throw new NotImplementedException();
        }
    }
}
