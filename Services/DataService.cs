using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UniversityManager.Models;  
namespace UniversityManager.Services
{
    public class DataService
    {
        private const string DataFile = "data.json";
        public DataModel Data { get; private set; } = new();

        public void LoadData()
        {
            if (File.Exists(DataFile))
            {
                var json = File.ReadAllText(DataFile);
                Data = JsonConvert.DeserializeObject<DataModel>(json) ?? new DataModel();
            }
        }

        public void SaveData()
        {
            var json = JsonConvert.SerializeObject(Data, Formatting.Indented);
            File.WriteAllText(DataFile, json);
        }
    }

    public class DataModel
    {
        public List<Student> Students { get; set; } = new();
        public List<Teacher> Teachers { get; set; } = new();
        public List<Subject> Subjects { get; set; } = new();
    }
}
