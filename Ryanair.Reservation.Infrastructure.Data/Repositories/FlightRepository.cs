using Newtonsoft.Json;
using Ryanair.Reservation.Data.Entities;
using Ryanair.Reservation.Data.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ryanair.Reservation.Infrastructure.Data.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private IList<FlightEntity> _dataStorage;

        public FlightRepository()
        {
            Seed();
        }

        private void Seed()
        {
            using (StreamReader streamReader = new StreamReader("../../InitialState.json"))
            {
                string json = streamReader.ReadToEnd();
                _dataStorage = JsonConvert.DeserializeObject<List<FlightEntity>>(json);
            }
        }

        public IEnumerable<FlightEntity> GetAll() => _dataStorage;

        public FlightEntity Get(string key) => _dataStorage.FirstOrDefault(x => x.Key == key);
    }
}
