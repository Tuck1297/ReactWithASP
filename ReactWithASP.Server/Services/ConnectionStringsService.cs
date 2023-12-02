using AutoMapper;
using ReactWithASP.Server.Data;
using ReactWithASP.Server.Models;
using ReactWithASP.Server.Models.InputModels;
using ReactWithASP.Server.Models.OutputModels;

namespace ReactWithASP.Server.Services
{
    public class ConnectionStringsService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public ConnectionStringsService(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public ConnectionStringOutputModel[] GetAllForUser(Guid id)
        {
            var connectionStrings = _dataContext.ConnectionStrings
            .Where(cs => cs.UserId == id)
            .ToList();

            var mappedModel = _mapper.Map<ConnectionStringOutputModel[]>(connectionStrings);

            return mappedModel.ToArray();
        } 

        public ConnectionStrings? GetByDBName(string dbName, Guid userId) 
        {
            var cString = _dataContext.ConnectionStrings.FirstOrDefault(cs => cs.UserId == userId && cs.dbName == dbName);

            if (cString == null) 
            {
                return null;
            }

            return cString;
        }

        public ConnectionStrings GetById(Guid id)
        {
            var model = _dataContext.ConnectionStrings.FirstOrDefault(cs => cs.Id == id);
            var mappedModel = _mapper.Map<ConnectionStrings>(model);
            return mappedModel;
        }

        public ConnectionStrings Create(ConnectionStrings model)
        {
            model.Id = Guid.NewGuid();

            var connectionStringEntity = _dataContext.ConnectionStrings.FirstOrDefault(cs => cs.dbName == model.dbName && cs.UserId == model.UserId);

            if (connectionStringEntity != null)
            {
                return null;
            }

            var newCSEntity = _dataContext.ConnectionStrings.Add(model);

            _dataContext.SaveChanges();

            return newCSEntity.Entity;
        }

        public ConnectionStrings Update(Guid id, ConnectionStrings model)
        {
            var toUpdate = _dataContext.ConnectionStrings.FirstOrDefault(cs => cs.Id == id);

            if (toUpdate != null)
            {
                toUpdate.dbConnectionString = model.dbConnectionString;
                toUpdate.dbType = model.dbType;
                toUpdate.dbName = model.dbName;
                _dataContext.SaveChanges();
            }

            return toUpdate;
        }

        public bool Delete(Guid id)
        {
            var toDelete = _dataContext.ConnectionStrings.FirstOrDefault(cs => cs.Id == id);

            if (toDelete != null)
            {
                _dataContext.ConnectionStrings.Remove(toDelete);
                _dataContext.SaveChanges();
                return true;
            }

            return false;
        }

        public bool updateTableInteracting(string tableName, Guid csId)
        {
            var toUpdate = _dataContext.ConnectionStrings.FirstOrDefault(cs => cs.Id.Equals(csId));
            if (toUpdate != null)
            {
                toUpdate.currentTableInteracting = tableName;
                _dataContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
