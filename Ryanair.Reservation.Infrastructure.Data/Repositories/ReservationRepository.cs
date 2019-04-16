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

        public bool CheckIfKeyExists(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return _dataStorage.ContainsKey(key);
        }

        public string Create(ReservationEntity reservationItem)
        {
            if (reservationItem == null)
            {
                throw new ArgumentNullException(nameof(reservationItem));
            }

            if (string.IsNullOrWhiteSpace(reservationItem.Key))
            {
                throw new ArgumentException("Reservation should has a key.");
            }

            _dataStorage.Add(reservationItem.Key, reservationItem);

            return reservationItem.Key;
        }

        public ReservationEntity Get(string key)
        {
            if(string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var reservation = _dataStorage.FirstOrDefault(x => string.Equals(x.Value.Key, key, StringComparison.CurrentCultureIgnoreCase));

            if (reservation.Value == null)
            {
                throw new KeyNotFoundException($"Reservation with key: {key} was not found.");
            }

            return reservation.Value;
        }

        public IEnumerable<ReservationEntity> GetAll() => _dataStorage.Select(x => x.Value);
    }
}
