using System.Collections.Generic;
using UnityEngine;

namespace UI.Protein.Info
{
    [System.Serializable]
    public class SynthesizingProtein
    {
        [TextArea(15,20)]
        [SerializeField] string descriptionProtein;
        [SerializeField] List<ExtraDataProtein> meta = default;

        [System.Serializable]

        private class ExtraDataProtein
        {
            [SerializeField] string nameExtra = default;
            [SerializeField] string descriptionExtra = default;

            public string GetNameExtra() => nameExtra;

            public string GetDescriptionExtra() => descriptionExtra;
        }

        public string GetDescriptionProtein() => descriptionProtein;

        public int GetQtdOfExtras() => meta.Count;

        public string GetNameExtra(int index) => meta[index].GetNameExtra();

        public string GetDescriptionTextExtra(int index) => meta[index].GetDescriptionExtra();
    }
}