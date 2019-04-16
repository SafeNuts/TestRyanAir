using Ryanair.Reservation.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Ryanair.Reservation.Data.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll();
    }
}
