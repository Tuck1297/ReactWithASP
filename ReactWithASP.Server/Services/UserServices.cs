using AutoMapper;
using ReactWithASP.Server.Data;
using ReactWithASP.Server.Models;
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

        public User Update(Guid id, User model) {
            var toUpdate = _dataContext.Users.FirstOrDefault(user => user.UserId == id);

            if (toUpdate != null)
            {
                if (model.PasswordHash != null)
                {
                    toUpdate.PasswordHash = BC.HashPassword(model.PasswordHash);
                }
                toUpdate.FirstName = model.FirstName;
                toUpdate.LastName = model.LastName;
                toUpdate.Email = model.Email;
                _dataContext.SaveChanges();
            }

            return toUpdate;
        }

        public bool Delete(Guid id)
        {
            var toDelete = _dataContext.Users.FirstOrDefault(user => user.UserId == id);

            if (toDelete != null)
            {
                _dataContext.Users.Remove(toDelete);
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

        // update role only be able to do this by admin and superuser
    }
}
