using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using System.Threading.Tasks;

namespace SqliteTableDemo {
    public class MarkerInfoDatabase : SQLiteAsyncConnection {
        static MarkerInfo[] GetInitialData() {
            return new[] {
                new MarkerInfo() { Name = "Arizona Island", Latitude = 43.96611, Longitude = -110.65278 },
                new MarkerInfo() { Name = "Badger Island", Latitude = 43.87167, Longitude = -110.65167 },
                new MarkerInfo() { Name = "Boulder Island", Latitude = 43.8, Longitude = -110.72694 },
                new MarkerInfo() { Name = "Bush Island", Latitude = 43.48889, Longitude = -109.57167 },
                new MarkerInfo() { Name = "Carrington Island", Latitude = 44.4539, Longitude = -110.55472 },
                new MarkerInfo() { Name = "Cow Island", Latitude = 43.93194, Longitude = -110.645 },
                new MarkerInfo() { Name = "Dollar Island", Latitude = 43.86972, Longitude = -110.66389 },
                new MarkerInfo() { Name = "Donoho Point", Latitude = 44.45194, Longitude = -110.62917 },
                new MarkerInfo() { Name = "Dot Island", Latitude = 43.86139, Longitude = -110.40972 },
                new MarkerInfo() { Name = "Elk Island", Latitude = 42.52361, Longitude = -110.6775 },
                new MarkerInfo() { Name = "Ferry Island", Latitude = 44.42528, Longitude = -110.05583 },
                new MarkerInfo() { Name = "Frank Island", Latitude = 44.42528, Longitude = -110.36778 },
                new MarkerInfo() { Name = "Freezeout Island", Latitude = 41.84333, Longitude = -109.78861 },
                new MarkerInfo() { Name = "Grassy Island", Latitude = 43.85917, Longitude = -110.73167 },
                new MarkerInfo() { Name = "Indian Island", Latitude = 43.96111, Longitude = -110.65917 },
                new MarkerInfo() { Name = "Johnson Island", Latitude = 41.57333, Longitude = -106.96778 },
                new MarkerInfo() { Name = "Long Island", Latitude = 42.38611, Longitude = -110.11583 },
                new MarkerInfo() { Name = "Marie Island", Latitude = 43.84667, Longitude = -110.65528 },
                new MarkerInfo() { Name = "Molly Islands", Latitude = 44.31361, Longitude = -110.265 },
                new MarkerInfo() { Name = "Moose Island", Latitude = 43.925, Longitude = -110.65028 },
                new MarkerInfo() { Name = "Mullison Island", Latitude = 41.46583, Longitude = -106.80806 },
                new MarkerInfo() { Name = "Mystic Isle", Latitude = 43.81972, Longitude = -110.7244 },
                new MarkerInfo() { Name = "Oxbow Bend", Latitude = 43.86278, Longitude = -110.54806 },
                new MarkerInfo() { Name = "Peale Island", Latitude = 44.28806, Longitude = -110.31639 },
                new MarkerInfo() { Name = "Pelican Roost", Latitude = 44.52222, Longitude = -110.30556 },
                new MarkerInfo() { Name = "Sheffield Island", Latitude = 43.87594, Longitude = -110.655 },
                new MarkerInfo() { Name = "Stevenson Island", Latitude = 44.51528, Longitude = -110.38444 },
                new MarkerInfo() { Name = "Tarters Island", Latitude = 42.48389, Longitude = -110.07667 },
                new MarkerInfo() { Name = "Telephone Island", Latitude = 41.78667, Longitude = -110.76694 },
                new MarkerInfo() { Name = "Treasure Island", Latitude = 44.24306, Longitude = -110.94 },
                new MarkerInfo() { Name = "Willow Island", Latitude = 42.88639, Longitude = -109.85472 },
            };
        }
        public MarkerInfoDatabase(string path) : base(path) {
            // Normally, CreateTableAsync would only create if needed and could be the only call here.
            // Since I only want to inject initial data the time it is created, this had to be a bit more obtuse.
            ExecuteScalarAsync<string>("SELECT name FROM sqlite_master WHERE type='table' AND name='MarkerInfo';").ContinueWith(task => {
                string foundName = task.Result;
                if (foundName == null) {
                    CreateTableAsync<MarkerInfo>().ContinueWith(_ => {
                        // TODO: Insert initial info.
                        InsertAllAsync(GetInitialData());
                    });
                }
            });
        }

        public Task<List<MarkerInfo>> GetAllMarkersAsync() {
            return Table<MarkerInfo>().OrderBy(mi => mi.Name).ToListAsync();
        }

        public Task<MarkerInfo> GetMarkerAsync(int id) {
            var query = Table<MarkerInfo>().Where(mi => mi.Id == id);
            return query.FirstOrDefaultAsync();
        }

        public Task<int> SaveMarker(MarkerInfo item) {
            if (item.Id != 0) {
                return UpdateAsync(item);
            }
            else {
                return InsertAsync(item);
            }
        }

        public Task<int> DeleteMarker(int id) {
            return DeleteAsync(new MarkerInfo() { Id = id });
        }
    }
}