using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Subtitles
{
  public class SrtParser
  {
    private readonly List<SubtitleBlock> _subtitles;
  
    public SrtParser(string textAssetResourcePath)
    {
      var text = Resources.Load<TextAsset>(textAssetResourcePath);
      Load(text);
    }

    public SrtParser(TextAsset textAsset)
    {
      this._subtitles = Load(textAsset);
    }

    public Queue<SubtitleBlock> GetSubtitleStack()
    {
        return new Queue<SubtitleBlock>(_subtitles);
    }

    private static List<SubtitleBlock> Load(TextAsset textAsset)
    {
      if (textAsset == null)
      {
        Debug.LogError("Subtitle file is null");
        return null;
      }

      var lines = textAsset.text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

      var currentState = EReadState.Index;

      var subs = new List<SubtitleBlock>();

      var currentIndex = 0;
      double currentFrom = 0, currentTo = 0;
      var currentText = string.Empty;
      for (var l = 0; l < lines.Length; l++)
      {
        var line = lines[l];

        switch (currentState)
        {
          case EReadState.Index:
          {
            int index;
            if (int.TryParse(line, out index))
            {
              currentIndex = index;
              currentState = EReadState.Time;
            }
          }
            break;
          case EReadState.Time:
          {
            line = line.Replace(',', '.');
            var parts = line.Split(new[] { "-->" }, StringSplitOptions.RemoveEmptyEntries);

            // Parse the timestamps
            if (parts.Length == 2)
              if (TimeSpan.TryParse(parts[0], out var fromTime))
                if (TimeSpan.TryParse(parts[1], out var toTime))
                {
                  currentFrom = fromTime.TotalSeconds;
                  currentTo = toTime.TotalSeconds;
                  currentState = EReadState.Text;
                }
          }
            break;
          case EReadState.Text:
          {
            if (currentText != string.Empty) currentText += "\r\n";

            currentText += line;

            // When we hit an empty line, consider it the end of the text
            if (string.IsNullOrEmpty(line) || l == lines.Length - 1)
            {
              // Create the SubtitleBlock with the data we've aquired 
              subs.Add(new SubtitleBlock(currentIndex, currentFrom, currentTo, currentText));

              // Reset stuff so we can start again for the next block
              currentText = string.Empty;
              currentState = EReadState.Index;
            }
          }
            break;
        }
      }

      return subs;
    }

    private enum EReadState
    {
      Index,
      Time,
      Text
    }
  }

  public class SubtitleBlock
  {
    private static SubtitleBlock blank;
    public static SubtitleBlock Blank => blank ??= new SubtitleBlock(0, 0, 0, string.Empty);
    public int Index { get; }
    public double Length { get; }
    public double From { get; }
    public double To { get; }
    public string Text { get; }

    public SubtitleBlock(int index, double from, double to, string text)
    {
      Index = index;
      From = from;
      To = to;
      Length = to - from;
      Text = text;
    }
  }
}