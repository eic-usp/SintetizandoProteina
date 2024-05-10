using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace Game.Scripts.Subtitles
{
    public class Subtitles : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent subtitleText;
        
        private float _actualTime = 0;
        private Queue<SubtitleBlock> _block;
        private TextAsset _srtToParse;
        
        private readonly char[] _charsToTrim = { '\n', '\r', ' ' }; 
        
        private bool _unset = true;
        public void SetSubtitles(TextAsset subtitle)
        {
            _srtToParse = subtitle;
            _block = new SrtParser(_srtToParse).GetSubtitleStack();
            subtitleText.gameObject.SetActive(true);
        }

        public void ResetSubtitles()
        {
            subtitleText.gameObject.SetActive(false);
        }
        public void UpdateSubtitles()
        {
            if (_block.Count == 0) return;
            
            _actualTime += Time.deltaTime;

            SetText();
        }

        private void SetText()
        {
            if (_unset && _block.Peek().From < _actualTime)
            {
                _unset = false;
                SetReferenceToPeek();
                return;
            }

            if (_block.Peek().To > _actualTime) return;
            
            _block.Dequeue();
            _unset = true;
            
            if (_block.Count > 0 && _block.Peek().From > _actualTime)
            {
                SetEmptyText();
            }
        }

        private void SetReferenceToPeek()
        {
            LocalizationManager.SetStringReference(subtitleText, LocalizationManager.Subtitles,_block.Peek().Text.Trim(_charsToTrim));
        }

        private void SetEmptyText()
        {
            LocalizationManager.SetStringReference(subtitleText, LocalizationManager.Subtitles,"id_empty_subtitle");
        }
        
    }
}