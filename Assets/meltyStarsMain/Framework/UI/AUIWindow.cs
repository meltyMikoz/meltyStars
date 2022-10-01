using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace MeltyStars.UI
{
    public abstract partial class AUIWindow
    {
        public AUIWindowView UIView;
        public abstract string UIWindowAssetName { get; }
        public abstract Type UIWindowViewType { get; }
    }
    public abstract partial class AUIWindow
    {
        protected abstract void OnInitCore();
        protected abstract void OnUpdateCore();
        protected abstract void OnAfterShowCore(object args);
        protected abstract void OnAfterHideCore();
        protected abstract UniTask OnBeforeShowCore(object args);
        protected abstract UniTask OnBeforeHideCore();
    }
    public abstract partial class AUIWindow : IUIControllable
    {
        public void OnInit()
        {
            UIView = GetWindowView();
            OnInitCore();
        }
        public async UniTask OnShow(object args)
        {
            await OnBeforeShowCore(args);
            UIView.uiTransform.gameObject.SetActive(true);
            OnAfterShowCore(args);
        }
        public async UniTask OnHide()
        {
            await OnBeforeHideCore();
            UIView.uiTransform.gameObject.SetActive(false);
            OnAfterHideCore();
        }
        public void OnUpdate()
        {
            OnUpdateCore();
        }
        public void SetParent(Transform root)
        {
            UIView!.uiTransform.SetParent(root, false);
        }
        public void SetAsFirstSibling()
        {
            UIView!.uiTransform.SetAsFirstSibling();
        }
        public void SetAsLastSibling()
        {
            UIView!.uiTransform.SetAsLastSibling();
        }
        public void SetSiblingIndex(int index)
        {
            UIView!.uiTransform.SetSiblingIndex(index);
        }
        private AUIWindowView GetWindowView()
        {
            if (object.ReferenceEquals(UIWindowViewType, null))
            {
                Debug.LogError($"{this.GetType()}.UIWindowViewType is null!");
                return null;
            }
            if (!UIWindowViewType.IsSubclassOf(typeof(AUIWindowView)))
            {
                Debug.LogError($"{this.GetType()}.UIWindowViewType is not a sub class of AUIWindowView!");
                return null;
            }
            var uiWindowGO = ObjectScheduler.Instance.GetGameObject(UIWindowAssetName, UIWindowAssetName);
            AUIWindowView aUIWindowView =
            Activator.CreateInstance(type: UIWindowViewType, args: uiWindowGO.transform) as AUIWindowView;
            return aUIWindowView;
        }
    }
}
