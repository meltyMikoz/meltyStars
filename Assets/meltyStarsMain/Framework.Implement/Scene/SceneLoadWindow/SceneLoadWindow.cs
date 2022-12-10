using System;
using Cysharp.Threading.Tasks;
using KuusouEngine;
using KuusouEngine.UI;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;

namespace KuusouEngine
{
    public partial class SceneLoadWindow : AUIWindow
    {
        public override string UIWindowAssetName => "SceneLoadWindow";
        public override Type UIWindowViewType => typeof(SceneLoadWindowView);
        public SceneLoadWindowView View => UIView as SceneLoadWindowView;
        private DG.Tweening.Core.TweenerCore<float, float, DG.Tweening.Plugins.Options.FloatOptions> m_tweenShow;
        protected override void OnAfterHideCore()
        {
            
        }
        protected override async void OnAfterShowCore(object args)
        {
            // bool isKilled = false;
            m_tweenShow = View.uiTransform.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            // m_tweenShow.onComplete = () => isKilled = true;
            // while (!isKilled)
            // {
            //     if (!m_tweenShow.IsComplete())
            //         await UniTask.Yield();
            // }

        }
        protected override async UniTask OnBeforeHideCore()
        {
            // bool isKilled = false;
            m_tweenShow = View.uiTransform.GetComponent<CanvasGroup>().DOFade(0, 0.5f);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            // m_tweenShow.onComplete = () => isKilled = true;
            // while (!isKilled)
            // {
            //     if (!m_tweenShow.IsComplete())
            //         await UniTask.Yield();
            // }
        }
        protected override async UniTask OnBeforeShowCore(object args)
        {
            await UniTask.Yield();
        }
        protected override void OnInitCore()
        {

        }
        protected override void OnUpdateCore()
        {

        }
    }
    public partial class SceneLoadWindow
    {
        //将响应事件写在如下区域或自行新建脚本写在分部类中
        //请将响应事件按照格式声明
        //Tip：请将事件类型声明为public
        //public class TestUIEventHandler : AUIEventHandler<EventType.OnEnterGame>
        //{
        //protected override void Execute(AUIWindow window, EventType.OnEnterGame eventType)
        //{
        //window.OnUpdate(eventType);
        //}
        //}
    }
}