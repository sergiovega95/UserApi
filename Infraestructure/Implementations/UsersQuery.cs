﻿using Core.Entities.Identity;
using Core.Entities.Shared;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Infrastructure.IdentityContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
                    newUser.DocumentType = _database.DocumentType.Find(newUser.DocumentType.Id);
                    return await _userManager.CreateAsync(newUser, password);

                }
                catch (Exception e)
                {
                    throw new DatabaseException("Ocurrió un error al insertar un nuevo usuario");
                }
            }        
        }

        public void DeleteUser(int  IdUser)
        {
           
            User usuario = _database.Users.Where(s=>s.IdUser == IdUser).FirstOrDefault();

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

        public List<DocumentType> GetDocumentTypes()
        {
            return _database.DocumentType.ToList();
        }

        public User GetUserById(int IdUser)
        {
            
            User usuario = _database.Users.Where(s=>s.IdUser== IdUser).Include(s=>s.DocumentType).FirstOrDefault();

            if (usuario != null)
            {
                return usuario;
            }
            else
            {
                throw new UserException($"El usuario con Id {IdUser}, no se encuentra registrado");
            }
          
        }

        public User GetUserByIdentification(string Identification)
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

        public async Task<SignInResult> LoginAsync(string identification, string password)
        {
            return await _signInManager.PasswordSignInAsync(identification, password, true, lockoutOnFailure: false);
        }

        public void UpdateUser(string changes)
        {
            UserUpdate  data = JsonConvert.DeserializeObject<UserUpdate>(changes);

            if (_database.Users.Any(s=>s.IdUser==data.IdUser))
            {
                if (!string.IsNullOrEmpty(data.Email))
                {
                    if (_database.Users.Any(s => s.Email.ToUpper() == data.Email.ToUpper()))
                    {
                        throw new UserException($"Ya existe un usuario registrado con el correo {data.Email}");
                    }
                }
               
                User usuario = _database.Users.Where(s => s.IdUser == data.IdUser).FirstOrDefault();
                JsonConvert.PopulateObject(changes, usuario);              

                _database.SaveChanges();
                
            }
            else
            {
                throw new UserException($"Usuario con Id {data.IdUser} no encontrado");
            }
        }
    }
}
