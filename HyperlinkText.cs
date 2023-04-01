using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace D.Unity3dTools
{
    public class HrefClickEvent : UnityEvent<string, Vector3> { }
    public class HyperlinkText : Text, IPointerClickHandler
    {
        public HrefClickEvent OnClick = new HrefClickEvent();
        private Regex hrefRegex = new Regex(@"<a href=([^>\n\s]+)>(.*?)(</a>)", RegexOptions.Singleline);
        private Regex colorRegex = new Regex(@"<color=([^>\n\s]+)>(.*?)(</color>)", RegexOptions.Singleline);
        private List<HyperlinkInfo> hyperlinkInfos = new List<HyperlinkInfo>();
        private string _hyperlinkInfo = "";
        public string hyperlinkInfo
        {
            set
            {
                _hyperlinkInfo = value;
                text = value;
                SetClickRect();
            }
            get { return _hyperlinkInfo; }
        }
        IEnumerator SetClickRectE()
        {
            yield return new WaitForEndOfFrame();
            hyperlinkInfos.Clear();
            StringBuilder checkBuilder = new StringBuilder();
            int indexText = 0;
            int textBuilderLength = 0;
            string noColorText = RemoveColor(_hyperlinkInfo);
            foreach (Match match in hrefRegex.Matches(noColorText))
            {
                checkBuilder.Append(noColorText.Substring(indexText, match.Index - indexText));
                string infoStr = match.Groups[1].Value;
                string showStr = match.Groups[2].Value;
                HyperlinkInfo hyperlinkInfo = new HyperlinkInfo()
                {
                    startIndex = checkBuilder.Length,
                    endIndex = checkBuilder.Length + match.Groups[2].Value.Length,
                    hyperInfo = match.Groups[1].Value,
                    showInfo = match.Groups[2].Value,
                };
                hyperlinkInfos.Add(hyperlinkInfo);
                checkBuilder.Append(match.Groups[2].Value);
                textBuilderLength += match.Groups[2].Value.Length;
                indexText = match.Index + match.Length;
            }
            checkBuilder.Append(noColorText.Substring(indexText, noColorText.Length - indexText));
            string checkTxt = checkBuilder.ToString();
            TextGenerator textGen = new TextGenerator(checkTxt.Length);
            Vector2 extents = GetComponent<RectTransform>().rect.size;
            textGen.Populate(checkTxt, GetGenerationSettings(extents));
            yield return new WaitForEndOfFrame();
            foreach (HyperlinkInfo _checkInfo in hyperlinkInfos)
            {
                for (int charIndex = _checkInfo.startIndex; charIndex < _checkInfo.endIndex; charIndex++)
                {
                    char cha = checkTxt[charIndex];
                    int indexOfTextQuad = charIndex * 4;
                    Vector3 charPos = (textGen.verts[indexOfTextQuad].position +
                           textGen.verts[indexOfTextQuad + 1].position +
                           textGen.verts[indexOfTextQuad + 2].position +
                           textGen.verts[indexOfTextQuad + 3].position) / 4f;
                    charPos /= canvas.scaleFactor;//适应不同分辨率的屏幕
                    Vector2 charSize = new Vector2(Mathf.Abs(textGen.verts[indexOfTextQuad].position.x - textGen.verts[indexOfTextQuad + 2].position.x),
                        Mathf.Abs(textGen.verts[indexOfTextQuad].position.y - textGen.verts[indexOfTextQuad + 2].position.y));
                    _checkInfo.boxes.Add(new Rect(charPos, charSize));

                    GameObject go = new GameObject(cha.ToString());
                    go.AddComponent<RectTransform>();
                    go.transform.SetParent(transform);
                    go.GetComponent<RectTransform>().localScale = Vector3.one;
                    go.GetComponent<RectTransform>().sizeDelta = charSize;
                    go.GetComponent<RectTransform>().localPosition = charPos;

                }
            }
        }
        private void SetClickRect()
        {
            StartCoroutine(SetClickRectE());
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Vector2 lp = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out lp);
            foreach (HyperlinkInfo hrefInfo in hyperlinkInfos)
            {
                var boxes = hrefInfo.boxes;
                for (var i = 0; i < boxes.Count; ++i)
                {
                    Rect rect = boxes[i];
                    float maxX = rect.x + 0.5f * rect.width;
                    float minX = rect.x - 0.5f * rect.width;
                    float maxY = rect.y + 0.5f * rect.height;
                    float minY = rect.y - 0.5f * rect.height;
                    if (lp.x < minX) continue;
                    if (lp.x > maxX) continue;
                    if (lp.y < minY) continue;
                    if (lp.y > maxY) continue;

                    Vector2 vec2;
                    RectTransform rectTrans = canvas.GetComponent<RectTransform>();
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTrans, eventData.position, eventData.pressEventCamera, out vec2);
                    OnHyperlinkTextInfo(hrefInfo.hyperInfo, vec2);
                    return;
                }
            }
        }
        private void OnHyperlinkTextInfo(string info, Vector2 pos)
        {
            //Debug.Log("超链接信息：" + info);
            OnClick?.Invoke(info, pos);
        }
        private string RemoveColor(string check)
        {
            StringBuilder result = new StringBuilder();
            int indexText = 0;
            foreach (Match match in colorRegex.Matches(check))
            {
                result.Append(check.Substring(indexText, match.Index - indexText));
                result.Append(match.Groups[2].Value);
                indexText = match.Index + match.Length;
            }
            if (colorRegex.IsMatch(check))
            {
                result.Append(check.Substring(indexText, check.Length - indexText));
                return result.ToString();
            }
            else return check;
        }
    }
}

