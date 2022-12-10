using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KuusouEngine
{
    //Life Circle
    /// <summary>
    /// 初始化
    /// </summary>
    public interface IAwake
    {
        void OnAwakeCore();
    }
    /// <summary>
    /// 帧更新
    /// </summary>
    public interface IUpdate
    {
        void OnUpdateCore();
    }
    /// <summary>
    /// Fixed帧更新
    /// </summary>
    public interface IFixedUpdate
    {
        void OnFixedUpdateCore();
    }
    /// <summary>
    /// Late帧更新
    /// </summary>
    public interface ILateUpdate
    {
        void OnLateUpdateCore();
    }
    /// <summary>
    /// 销毁
    /// </summary>
    public interface IDestroy
    {
        void OnDestroyCore();
    }
    //Custom CallBack
    /// <summary>
    /// 当添加组件
    /// </summary>
    public interface IAddComponent
    {
        void OnAddComponentCore(AComponent component);
    }
    /// <summary>
    /// 当移除组件
    /// </summary>
    public interface IRemoveComponent
    {
        void OnRemoveComponentCore(AComponent component);
    }
    /// <summary>
    /// 当作为组件被添加
    /// </summary>
    public interface IAddedAsComponent
    {
        void OnAddedAsComponentCore(AEntity entity);
    }
    /// <summary>
    /// 当作为组件被移除
    /// </summary>
    public interface IRemovedFromComponent
    {
        void OnRemovedFromComponentCore(AEntity entity);
    }
    /// <summary>
    /// 当添加子对象
    /// </summary>
    public interface IAddChild
    {
        void OnAddChildCore(AEntity child);
    }
    /// <summary>
    /// 当移除子对象
    /// </summary>
    public interface IRemoveChild
    {
        void OnRemoveChildCore(AEntity child);
    }
    /// <summary>
    /// 当作为子对象被添加
    /// </summary>
    public interface IAddedAsChild
    {
        void OnAddedAsChildCore(AEntity parent);
    }
    /// <summary>
    /// 当作为子对象被移除
    /// </summary>
    public interface IRemovedFromChild
    {
        void OnRemovedFromChildCore(AEntity parent);
    }
    /// <summary>
    /// 当父级被改变
    /// </summary>
    public interface IParentChanged
    {
        void OnParentChangedCore(AEntity previous, AEntity recent);
    }
}
