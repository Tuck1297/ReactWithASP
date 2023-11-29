using AutoMapper;
using ReactWithASP.Server.Data;
using ReactWithASP.Server.Models;
using ReactWithASP.Server.Models.InputModels;
using ReactWithASP.Server.Models.OutputModels;
using BC = BCrypt.Net.BCrypt;

namespace ReactWithASP.Server.Services
{
    public class UserServices
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public UserServices(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public UserOutputModel[] getAllUsers()
        {
            var users = _dataContext.Users.ToList();
            var mappedModel = _mapper.Map<UserOutputModel[]>(users);
            return mappedModel.ToArray();
        }

        public UserAccount Update(Guid id, UpdateUserModel model) {
            var toUpdateUserAccount = _dataContext.UserAccount.FirstOrDefault(user => user.UserId == id);
            var toUpdateUser = _dataContext.Users.FirstOrDefault(user => user.UserId == id);

            if (toUpdateUserAccount != null && toUpdateUser != null)
            {
                if (model.PasswordHash != null)
                {
                    toUpdateUserAccount.PasswordHash = BC.HashPassword(model.PasswordHash);
                }
                toUpdateUserAccount.Email = model.Email.ToLower();
                toUpdateUser.Email = model.Email.ToLower();
                toUpdateUser.FirstName = model.FirstName;
                toUpdateUser.LastName = model.LastName;
                _dataContext.SaveChanges();
            }

            return toUpdateUserAccount;
        }

        public bool Delete(UserAccount model)
        {
            var toDeleteUser = _dataContext.Users.FirstOrDefault(user => user.UserId == model.UserId);
            if (toDeleteUser != null && model != null)
            {
                _dataContext.Users.Remove(toDeleteUser);
                _dataContext.UserAccount.Remove(model);
                _dataContext.SaveChanges();
                return true;
            }

            return false;
        }

        public UserOutputModel GetById(Guid id)
        {
            var user = _dataContext.Users.FirstOrDefault(user => user.UserId == id);

            var mappedModel = _mapper.Map<UserOutputModel>(user);

            return mappedModel;
        }

        public User GetByEmail(string email)
        {
            return _dataContext.Users.FirstOrDefault(user => user.Email == email);
        }

        public bool UpdateRole(string email, string Role)
        {
            var confirmedRole = "";
            if (string.Equals(Role, "user", StringComparison.OrdinalIgnoreCase))
            {
                confirmedRole = "User";
            }
            else if (string.Equals(Role, "admin", StringComparison.OrdinalIgnoreCase))
            {
                confirmedRole = "Admin";
            }
            else
            {
                return false;
            }

            var userToUpdate = _dataContext.Users.FirstOrDefault(user => user.Email.ToLower() == email.ToLower());
            if (userToUpdate != null)
            {
                userToUpdate.Role = confirmedRole;
                _dataContext.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
