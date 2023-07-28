using UnityEditor;
using UnityEngine.UI;
using TextEditor = UnityEditor.UI.TextEditor;

namespace D.Unity3dTools.EditorTool 
{
    [CustomEditor(typeof(CopyText), true)]
    public class CopyTextEditor : TextEditor
    {
        private string _lastText;
        private bool foldout = false;
        private CopyText copyText;

        protected override void OnEnable()
        {
            base.OnEnable();
            copyText = target as CopyText;
        }
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PrefixLabel("复制对象数量：");
            copyText.copyNum = EditorGUILayout.IntField(copyText.copyNum);
            foldout = EditorGUILayout.Foldout(foldout, "复制对象：");

            if (foldout)
            {
                if (copyText.targetTexts.Count < copyText.copyNum)
                {
                    for (int i = copyText.targetTexts.Count; i < copyText.copyNum; i++)
                    {
                        copyText.targetTexts.Add(null);
                    }
                }
                else if (copyText.targetTexts.Count > copyText.copyNum)
                {
                    int removeIndex = copyText.copyNum;
                    int removeCount = copyText.targetTexts.Count - copyText.copyNum;
                    copyText.targetTexts.RemoveRange(removeIndex, removeCount);
                }
                for (int i = 0; i < copyText.targetTexts.Count; i++)
                {
                    copyText.targetTexts[i] = EditorGUILayout.ObjectField(copyText.targetTexts[i], typeof(Text), true) as Text;
                }
            }
            if (_lastText != copyText.text)
            {
                _lastText = copyText.text;
                copyText.text = _lastText;
            }

            base.OnInspectorGUI();
        }
    }
}

