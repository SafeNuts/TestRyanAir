using Ryanair.Reservation.Data.Entities;
using Ryanair.Reservation.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ryanair.Reservation.Infrastructure.Data.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private IDictionary<string, ReservationEntity> _dataStorage;

        public ReservationRepository()
        {
            _dataStorage = new Dictionary<string, ReservationEntity>();
        }

        public bool CheckIfKeyExists(string key) => _dataStorage.ContainsKey(key);

        public string Create(ReservationEntity item)
        {
            _dataStorage.Add(item.Key, item);

            return item.Key;
        }

        public ReservationEntity Get(string key)
        {
            if(string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var reservation = _dataStorage.FirstOrDefault(x => x.Value.Key.ToUpper() == key.ToUpper());
            return reservation.Value;
        }

        public IEnumerable<ReservationEntity> GetAll() => _dataStorage.Select(x => x.Value);
    }
}
