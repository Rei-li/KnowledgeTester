using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KnowledgeTester.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KnowlageTester.JsonDAL
{
    public class Repository<T> 
    {
        private string file = "{0}.json";
        private readonly string _directoryPath = ConfigurationManager.AppSettings["RepositoryPath"];
        private readonly IList<T> _items = new List<T>();
        private readonly string _collectionPath;

        public Repository(string collectionPath)
        {
            _collectionPath = collectionPath;
        }

        public  IList<T> GetAllItems()
        {
            if (Directory.Exists(_directoryPath))
            {
                string[] fileEntries = Directory.GetFiles(_directoryPath + _collectionPath);
                foreach (string fileName in fileEntries)
                {
                   _items.Add(FormItem(fileName));
                }
            }
            return _items;

        }

        public T GetItem(Guid id)
        {
            var filePath = _directoryPath + _collectionPath + String.Format(file, id);
            return FormItem(filePath);
        }

        private T FormItem(string filePath)
        {
            if (!File.Exists(filePath)) return default(T);
            var text = File.ReadAllText(filePath);
            return JObject.Parse(text).ToObject<T>();
        }

        public void SaveItem(T item)
        {
            var path = _directoryPath + _collectionPath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var jason = JsonConvert.SerializeObject(item);
            var baseItem = item as BaseItem;
            if (baseItem != null)
            {
                File.WriteAllText(_directoryPath + String.Format(file, baseItem.Id), jason);
            }
        }
    }
}
