using BioSportApp.Common.Messaging;
using BioSportApp.Common.Security;
using BioSportApp.Domain.Core;
using BioSportApp.Models.User;
using Domain.Core;
using Mapster;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;

namespace BioSportApp.Services
{
    public class UserService(BioSportContext bioSportContext, SessionService sessionService)
    {
        private readonly SQLiteAsyncConnection connection = bioSportContext.GetConnectionAsync();
        private readonly SessionService sessionService = sessionService;

        /// <summary>
        /// CreateUser
        /// </summary>
        /// <param name="userToAdd"></param>
        /// <returns></returns>
        public async Task<Response<BaseAddResponse>> CreateUser(UserAddModel userToAdd)
        {
            try
            {
                var user = userToAdd.Adapt<User>();

                user.Id = Guid.NewGuid();

                var passwordHash = PasswordUtils.PasswordHash(user.Password);
                user.Password = passwordHash.Hash;
                user.Salt = passwordHash.Salt;

                await connection.InsertAsync(user);

                sessionService.Login(user);

                return new Response<BaseAddResponse>
                {
                    Data = new BaseAddResponse { Id = user.Id },
                    IsValid = true
                };
            }
            catch (SQLiteException)
            {
                return new Response<BaseAddResponse>
                {
                    Data = null,
                    IsValid = false,
                    Message = "No se pudo guardar los datos, debido a un error con la base de datos."
                };
            }
            catch (Exception)
            {
                return new Response<BaseAddResponse>
                {
                    Data = null,
                    IsValid = false,
                    Message = "No se pudo guardar los datos, debido a un error desconocido."
                };
            }
        }

        /// <summary>
        /// LoadUser
        /// </summary>
        /// <returns></returns>
        public async Task<Response<UserAddModel>> LoadUser()
        {
            try
            {
                var users = await connection.GetAllWithChildrenAsync<User>();

                return new Response<UserAddModel>
                {
                    Data = users.FirstOrDefault().Adapt<UserAddModel>(),
                    IsValid = true
                };
            }
            catch (SQLiteException)
            {
                return new Response<UserAddModel>
                {
                    Data = null,
                    IsValid = false,
                    Message = "No se pudo recuperar los datos del usuario, debido a un error con la base de datos."
                };
            }
            catch (Exception)
            {
                return new Response<UserAddModel>
                {
                    Data = null,
                    IsValid = false,
                    Message = "No se pudo recuperar los datos del usuario, debido a un error desconocido."
                };
            }
        }

        /// <summary>
        /// GetUserById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Response<UserAddModel>> GetUserById(Guid id)
        {
            try
            {
                var user = await connection.GetAsync<User>(id);

                return new Response<UserAddModel> 
                { 
                    Data = user.Adapt<UserAddModel>() ?? null, 
                    IsValid = true 
                };
            }
            catch (SQLiteException)
            {
                return new Response<UserAddModel>
                {
                    Data = null,
                    IsValid = false,
                    Message = "No se pudo recuperar el usuario, debido a un error con la base de datos."
                };
            }
            catch (Exception)
            {
                return new Response<UserAddModel>
                {
                    Data = null,
                    IsValid = false,
                    Message = "No se pudo recuperar el usuario, debido a un error desconocido."
                };
            }
        }
    }
}
