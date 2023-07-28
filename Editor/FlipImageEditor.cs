using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace D.Unity3dTools.EditorTool
{
    [CustomEditor(typeof(FlipImage), true)]
    public class FlipImageEditor : ImageEditor
    {
        private bool lastFlip = false;
        private FlipImage mFlipImage;
        protected override void OnEnable()
        {
            base.OnEnable();
            mFlipImage = target as FlipImage;
            mFlipImage.flip = lastFlip;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            mFlipImage.flip = EditorGUILayout.ToggleLeft("ÊÇ·ñ·­×ªÍ¼Ïñ", mFlipImage.flip);
            if (lastFlip != mFlipImage.flip)
            {
                mFlipImage.FlipHorizontal();
                lastFlip = mFlipImage.flip;
            }
        }
    }
}

