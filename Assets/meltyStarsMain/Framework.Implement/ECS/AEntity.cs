using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeltyStars
{
    public abstract partial class AEntity
    {
        /// <summary>
        /// 唯一实例Id
        /// </summary>
        /// <returns></returns>
        private readonly Guid _instanceId = Guid.NewGuid();
        /// <summary>
        /// 获取实例Id
        /// </summary>
        /// <returns></returns>
        public string InstanceId => _instanceId.ToString();
        /// <summary>
        /// 父级
        /// </summary>
        private AEntity _parent;
        /// <summary>
        /// 获取父级
        /// </summary>
        /// <value></value>
        public AEntity Parent => _parent;
        /// <summary>
        /// 子对象集
        /// </summary>
        /// <returns></returns>
        protected Dictionary<Type, Dictionary<string, AEntity>> Children;
        /// <summary>
        /// 组件集
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        /// <typeparam name="AComponent"></typeparam>
        /// <returns></returns>
        protected Dictionary<Type, AComponent> Components;
        /// <summary>
        /// 实体或组件是否激活(启用)
        /// </summary>
        /// <value></value>
        public bool Enabled { get; protected set; }
        /// <summary>
        /// 定义隐式转换bool
        /// </summary>
        /// <param name="entity"></param>
        public static implicit operator bool(AEntity entity) => !object.ReferenceEquals(entity, null);
    }
}
