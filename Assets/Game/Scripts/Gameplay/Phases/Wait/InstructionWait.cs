using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    There is a significant interaction with the Close Button
*/

namespace Phases.Wait
{
    public class InstructionWait : MonoBehaviour
    {
        [SerializeField] Transform childInstruction = default; 

        private int actualChildInstruction = 0;
        private int childCnt;
        private GameplayManager _gameplayManager;
        private bool _onStartup = true;
        private bool _skipTutorialOnStartUp = true;

        private void Start()
        {
            _skipTutorialOnStartUp = (PlayerPrefs.GetInt("SkipTutorialOnStartUp") == 1);
            _gameplayManager = FindObjectOfType<GameplayManager>();

            Reset();

            if (_skipTutorialOnStartUp)
            {
                _gameplayManager.StartGame();
                gameObject.SetActive(false);
            }
        }

        public void Reset()
        {
            childCnt = childInstruction.childCount;

            childInstruction.GetChild(0).gameObject.SetActive(true);

            for (var i = 1; i < childCnt; i++)
            {
                childInstruction.GetChild(i).gameObject.SetActive(false);
            }

            actualChildInstruction = 0;
        }

        public void IncreaceChildInstruction()
        {
            if (actualChildInstruction == childCnt - 1)
            {
                Close();
                return;
            }

            childInstruction.GetChild(actualChildInstruction).gameObject.SetActive(false);
            actualChildInstruction++;
            childInstruction.GetChild(actualChildInstruction).gameObject.SetActive(true);
        }

        public void DecreaceChildInstruction()
        {
            if (actualChildInstruction == 0)
            {
                return;
            }

            childInstruction.GetChild(actualChildInstruction).gameObject.SetActive(false);
            actualChildInstruction--;
            childInstruction.GetChild(actualChildInstruction).gameObject.SetActive(true);
        }

        public void SetOnStartup(bool set) => _onStartup = set;

        //The button has this method
        public void Close()
        {
            if (_onStartup)
            {
                _gameplayManager.StartGame();
                _skipTutorialOnStartUp = true;
                PlayerPrefs.SetInt("SkipTutorialOnStartUp", 1);
            }

            gameObject.SetActive(false);
        }
    }
}