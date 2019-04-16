using Ryanair.Reservation.Data.Entities;
using Ryanair.Reservation.Data.Interfaces;
using System.Collections.Generic;

namespace Ryanair.Reservation.Infrastructure.Data.Repositories
{
    public class PassengerRepository : IPassengerRepository
    {
        private IList<PassengerEntity> _dataStorage;

        public PassengerRepository()
        {
            _dataStorage = new List<PassengerEntity>();
        }

        public IEnumerable<PassengerEntity> GetAll() => _dataStorage;

        public string Create(PassengerEntity passengerEntity)
        {
            _dataStorage.Add(passengerEntity);

            return passengerEntity.Key;
        }
    }
}
