using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;

namespace TaskApp.Persistence
{
    public interface IOperationRepository
    {
        void Insert(Operation operation);
        void Delete(int id);
        void UpdateOpStatus(Operation operation);
        Operation GetByCurrentId(int id);
        IEnumerable<OperationModel> GetAll();
        OperationModel GetByOpId(int id);
        OperationModel GetById(int id);
        IEnumerable<Operation> GetByMissionId(int missionId);
        IEnumerable<OperationModel> GetOperationsByMissionId(int missionId);
    }
}
