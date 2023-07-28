using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

namespace D.Unity3dTools
{
    [ExecuteInEditMode]
    public class CopyText : Text
    {
        public int copyNum = 1;
        public List<Text> targetTexts = new List<Text>();
        public override string text
        {
            get { return base.text; }
            set
            {
                base.text = value;
                OnCopy();
            }
        }
        public void OnCopy()
        {
            foreach (Text txt in targetTexts)
            {
                OnCopy(this, txt);
            }
        }
        private void OnCopy(Text source, Text copy)
        {
            if (copy == null) return;
            copy.font = source.font;
            copy.fontSize = source.fontSize;
            copy.fontStyle = source.fontStyle;
            copy.alignment = source.alignment;
            copy.resizeTextForBestFit = source.resizeTextForBestFit;
            copy.text = source.text;
            copy.rectTransform.sizeDelta = source.rectTransform.sizeDelta;
            copy.transform.localScale = source.transform.localScale;
        }
    }
}

