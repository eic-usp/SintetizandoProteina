using System.Collections.Generic;

namespace Phases.Destination
{
    [System.Serializable]
    public struct QuizData
    {
        public List<QuizDataItem> glucagon;
        public List<QuizDataItem> insulina;
        public List<QuizDataItem> hidrolas;
        public List<QuizDataItem> tirosinas;
    }

    [System.Serializable]
    public struct QuizDataItem
    {
        public string question;
        public QuizDataOption[] options;
    }

    [System.Serializable]
    public struct QuizDataOption
    {
        public string answer;
        public bool correct;
    }
}