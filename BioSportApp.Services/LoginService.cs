using BioSportApp.Common.Messaging;
using BioSportApp.Common.Security;
using BioSportApp.Domain.Core;
using BioSportApp.Models.Login;
using Domain.Core;
using SQLite;

namespace BioSportApp.Services
{
    public class LoginService(BioSportContext context, SessionService sessionService)
    {
        private readonly SQLiteAsyncConnection connectionAsync = context.GetConnectionAsync();
        private readonly SessionService sessionService = sessionService;


        /// <summary>
        /// AutenticateUser
        /// </summary>
        /// <param name="userToCheck"></param>
        /// <returns></returns>
        public async Task<Response<BaseResponse>> AutenticateUser(LoginModel userToCheck)
        {
            try
            {
                var user = await connectionAsync
                    .Table<User>()
                    .Where(user => user.Email == userToCheck.Email)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return new Response<BaseResponse>
                    {
                        Data = null,
                        IsValid = false,
                        Message = "Correo no encontrado."
                    };
                }
                else
                {
                    var hashPasswordUser = PasswordUtils.PasswordHash(userToCheck.Password, user.Salt);

                    if (!hashPasswordUser.Equals(user.Password))
                    {
                        return new Response<BaseResponse>
                        {
                            Data = null,
                            IsValid = false,
                            Message = "Contraseña incorrecta."
                        };
                    }
                }

                sessionService.Login(user);

                return new Response<BaseResponse>
                { 
                    IsValid = true 
                };
            }
            catch (SQLiteException)
            {
                return new Response<BaseResponse>
                {
                    Data = null,
                    IsValid = false,
                    Message = "No se pudo autenticar, debido a  un error con la base de datos."
                };
            }
            catch (Exception)
            {
                return new Response<BaseResponse>
                {
                    Data = null,
                    IsValid = false,
                    Message = "No se pudo autenticar, debido a  un error desconocido."
                };
            }
        }
    }
}
