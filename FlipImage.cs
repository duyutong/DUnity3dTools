using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace D.Unity3dTools
{
    [ExecuteInEditMode]
    public class FlipImage : Image
    {
        public bool flip = false;
        private Sprite originalSprite;
        private Texture2D originalTexture;

        private Sprite flippedSprite;
        private Texture2D flippedTexture;
        public void SetSprite(Sprite _sprite)
        {
            base.sprite = _sprite;
            originalSprite = sprite;
            originalTexture = sprite.texture;
            SetFlip();

            if (originalSprite != null) FlipHorizontal();
        }
        public void FlipHorizontal()
        {
            if (sprite == null)
            {
                originalSprite = null;
                originalTexture = null;

                flippedSprite = null;
                flippedTexture = null;
                return;
            }
            if (originalTexture == null || originalSprite == null)
            {
                originalSprite = sprite;
                originalTexture = sprite.texture;
            }
            if (flippedTexture == null || flippedSprite == null) SetFlip();
            sprite = flip ? flippedSprite : originalSprite;
        }

        private void SetFlip()
        {
            // ��ȡԭʼ��������   
            Color[] pixels = originalTexture.GetPixels();

            // ��ȡ����Ŀ�Ⱥ͸߶�
            int width = originalTexture.width;
            int height = originalTexture.height;

            // ˮƽ��ת��������
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width / 2; x++)
                {
                    int leftIndex = y * width + x;
                    int rightIndex = y * width + (width - x - 1);

                    // ����������ɫ
                    Color temp = pixels[leftIndex];
                    pixels[leftIndex] = pixels[rightIndex];
                    pixels[rightIndex] = temp;
                }
            }

            // �����µ�����Ӧ�õ� Image ���
            flippedTexture = new Texture2D(width, height);
            flippedTexture.SetPixels(pixels);
            flippedTexture.Apply();

            // �����µ� Sprite ��Ӧ�õ� Image ���
            flippedSprite = Sprite.Create(flippedTexture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
            sprite = flippedSprite;
        }
        protected override void OnDisable()
        {
            originalSprite = null;
            originalTexture = null;

            flippedSprite = null;
            flippedTexture = null;
        }
    }
}

