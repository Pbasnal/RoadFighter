using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Scripts.UnityLogic.ScriptableObjects;
using Newtonsoft.Json;
using UnityEngine;

namespace UnityLogic.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Value/FloatValueWithHistory", fileName = "FloatValueWithHistory", order = 51)]
    public class FloatValueWithHistory : FloatValue
    {
        public string filePathToSaveHistory;
        private SortedList<float, DateTime> valueHistory;

        public void Init()
        {
            if (string.IsNullOrWhiteSpace(filePathToSaveHistory))
            {
                return;
            }

            if (!File.Exists(filePathToSaveHistory))
            {
                (new FileInfo(filePathToSaveHistory)).Directory.Create();
                File.WriteAllText(filePathToSaveHistory, string.Empty);
            }
            if (!File.Exists(filePathToSaveHistory))
            {
                throw new UnityException("Unable to create save file");
            }

            valueHistory = JsonConvert.DeserializeObject<SortedList<float, DateTime>>(
                File.ReadAllText(filePathToSaveHistory));

            if (valueHistory == null)
            {
                valueHistory = new SortedList<float, DateTime>();
            }
        }

        public void AddHistoryObject(float value)
        {
            valueHistory.Add(value, DateTime.Now);

            this.value = valueHistory.Keys.Last();

            var valueHistoryTxt = JsonConvert.SerializeObject(valueHistory);
            File.WriteAllText(filePathToSaveHistory, valueHistoryTxt);
        }

    }
}
