using CarListApp.Models;
using SQLite;

namespace CarListApp.Services
{
    public class CarDatabaseService
    {
        private SQLiteConnection connection;
        string _dbPath;
        int _result;
        public string StatusMessage;

        public CarDatabaseService(string dbPath)
        {
            _dbPath = dbPath;
        }

        private void Init()
        {
            if(connection != null) { return; }
            connection = new SQLiteConnection(_dbPath);
            connection.CreateTable<Car>();
        }

        public List<Car> GetCars()
        {

            try
            {
                Init();
                return connection.Table<Car>().ToList();
            }
            catch (Exception)
            {
                StatusMessage = "Failed to retrieve data.";
            }

            return new List<Car>();

            //return new List<Car>
            //{
            //    new Car
            //    {
            //        Id = 1, Make = "Honda", Model = "Fit", Vin = "123"
            //    },
            //    new Car
            //    {
            //        Id = 2, Make = "Toyota", Model = "Prado", Vin = "123"
            //    },
            //    new Car
            //    {
            //        Id = 3, Make = "Honda", Model = "Civic", Vin = "123"
            //    },
            //    new Car
            //    {
            //        Id = 4, Make = "Audi", Model = "A5", Vin = "123"
            //    },
            //    new Car
            //    {
            //        Id = 5, Make = "BMW", Model = "M3", Vin = "123"
            //    },
            //    new Car
            //    {
            //        Id = 6, Make = "Nissan", Model = "Note", Vin = "123"
            //    },
            //    new Car
            //    {
            //        Id = 7, Make = "Ferrari", Model = "Spider", Vin = "123"
            //    },
            //};
        }

        public Car GetCar(int id)
        {
            try
            {
                Init();
                return connection.Table<Car>().FirstOrDefault(x => x.Id == id);
            }
            catch (Exception)
            {
                StatusMessage = "Failed to retrieve data";
            }
            return null;
        }

        public void AddCar(Car car)
        {
            try
            {
                if(car == null) {
                    throw new Exception("Invalid Car Record");
                }

                _result = connection.Insert(car);
                StatusMessage = _result == 0 ? "Insert failed" : "Insert succesful";
            }
            catch (Exception)
            {
                StatusMessage = "Failed to insert data";
            }
        }

        public void UpdateCar(Car car)
        {
            try
            {
                if (car == null || car.Id == 0)
                {
                    throw new Exception("Invalid Car Record");
                }

                _result = connection.Update(car);
                StatusMessage = _result == 0 ? "Update failed" : "Update succesful";
            }
            catch (Exception)
            {
                StatusMessage = "Failed to update data";
            }
        }

        public int DeleteCar(int id)
        {
            try
            {
                Init();
                return connection.Table<Car>().Delete(c => c.Id == id);
            }
            catch (Exception)
            {
                StatusMessage = "Failed to delete data";
            }
            return 0;
        }
    }
}
