using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;

namespace D.Unity3dTools
{
    /// <summary>
    /// 组件扩展类
    /// </summary>
    public static class ComponentEx
    {
        /// <summary>
        /// 将Texture2D转换成Sprite
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Sprite ToSprite(this Texture2D self)
        {
            Rect rect = new Rect(0, 0, self.width, self.height);
            Vector2 pivot = Vector2.one * 0.5f;
            return Sprite.Create(self, rect, pivot);
        }
        /// <summary>
        /// 通过代码添加EventTrigger事件（只能做unity已经定义的事件）
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="eventID"></param>
        /// <param name="call"></param>
        public static void AddTrggerEventListener(this EventTrigger trigger, EventTriggerType eventID, Action<PointerEventData> call)
        {
            TriggerEvent callback = new TriggerEvent();
            callback.AddListener(data => call(data as PointerEventData));
            Entry entry = new Entry() { callback = callback, eventID = eventID };
            trigger.triggers.Add(entry);
        }
        /// <summary>
        /// 通过代码删除EventTrigger事件（只能做unity已经定义的事件）
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="eventID"></param>
        public static void RemoveEventListener(this EventTrigger trigger, EventTriggerType eventID)
        {
            List<Entry> newTriggers = new List<Entry>();
            foreach (Entry entry in trigger.triggers)
            {
                if (entry.eventID == eventID) continue;
                newTriggers.Add(entry);
            }
            trigger.triggers = newTriggers;
        }
        /// <summary>
        /// 通过代码删除全部EventTrigger事件
        /// </summary>
        /// <param name="trigger"></param>
        public static void RemoveAllEventListener(this EventTrigger trigger)
        {
            trigger.triggers.RemoveAll((item) => { return item != null; });
        }
        /// <summary>
        /// 重置Transform的本地尺寸，角度以及位置
        /// </summary>
        /// <param name="value"></param>
        public static void Reset(this Transform value)
        {
            value.position = Vector3.zero;
            value.localPosition = Vector3.zero;
            value.localRotation = Quaternion.identity;
            value.localScale = Vector3.one;
        }
        /// <summary>
        /// 删除所有子物体
        /// </summary>
        /// <param name="value"></param>
        public static void RemoveAllChildren(this Transform value)
        {
            for (int i = value.childCount - 1; i >= 0; i--)
            {
                Transform child = value.GetChild(i);
                GameObject.Destroy(child.gameObject);
            }
        }
        /// <summary>
        /// 获取组件，如果没有组件，则添加组件并返回新组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this Transform value) where T : Component
        {
            T component;
            bool isExist = value.gameObject.TryGetComponent<T>(out component);
            if (!isExist) component = value.gameObject.AddComponent<T>();
            return component;
        }
        /// <summary>
        /// 获得字典的某一项
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static KeyValuePair<TKey, TValue> GetPair<TKey, TValue>(this Dictionary<TKey, TValue> dic, int index)
        {
            KeyValuePair<TKey, TValue> pair = new KeyValuePair<TKey, TValue>();
            int count = 0;
            foreach (KeyValuePair<TKey, TValue> keyValuePair in dic)
            {
                if (count == index) { pair = keyValuePair; break; }
                else count++;
            }
            return pair;
        }
        /// <summary>
        /// 将键和值添加或替换到字典中
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void AddOrReplace<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, TValue value)
        {
            if (dic.ContainsKey(key) == false) dic.Add(key, value);
            else dic[key] = value;
        }
    }
}