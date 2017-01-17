using DapperApp.Models;
using System.Collections.Generic;

namespace DapperApp.Repository {
    public interface IRepository<T> where T : BaseEntity {
        void Add(T item);
        T FindByID(int id);
        IEnumerable<T> FindAll(); 
    }
}