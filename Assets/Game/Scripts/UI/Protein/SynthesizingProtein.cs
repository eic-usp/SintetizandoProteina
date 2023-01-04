using System.Collections.Generic;
using UnityEngine;

namespace UI.Protein.Info
{
    [System.Serializable]
    public class SynthesizingProtein
    {
        [TextArea(15,20)]
        [SerializeField] string descriptionProtein;
        [SerializeField] List<Data> data;

        [System.Serializable]
        private class Data
        {
            [SerializeField] private string key;
            [SerializeField] private List<ExtraDataProtein> meta;

            public string Key => key;
            public List<ExtraDataProtein> Meta => meta;
        }
        
        [System.Serializable]
        private class ExtraDataProtein
        {
            [SerializeField] string nameExtra = default;
            [SerializeField] string descriptionExtra = default;

            public string NameExtra => nameExtra;
            public string DescriptionExtra => descriptionExtra;
        }

        public string GetDescriptionProtein() => descriptionProtein;

        public int GetQtdOfExtras(string key) => data.Find(d => (d.Key == key)).Meta.Count;
        public string GetNameExtra(string key, int index) => data.Find(d => (d.Key == key)).Meta[index].NameExtra;
        public string GetDescriptionTextExtra(string key, int index) => data.Find(d => (d.Key == key)).Meta[index].DescriptionExtra;
    }
}