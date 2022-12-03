using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeltyStars
{
    public class InputObserverMono : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
                EventScheduler.Instance.Dispatch(new InputScheduler.OnKeyDown() { Key = KeyCode.W });
            if (Input.GetKeyDown(KeyCode.S))
                EventScheduler.Instance.Dispatch(new InputScheduler.OnKeyDown() { Key = KeyCode.S });
            if (Input.GetKeyDown(KeyCode.A))
                EventScheduler.Instance.Dispatch(new InputScheduler.OnKeyDown() { Key = KeyCode.A });
            if (Input.GetKeyDown(KeyCode.D))
                EventScheduler.Instance.Dispatch(new InputScheduler.OnKeyDown() { Key = KeyCode.D });
            if (Input.GetKeyUp(KeyCode.W))
                EventScheduler.Instance.Dispatch(new InputScheduler.OnKeyUp() { Key = KeyCode.W });
            if (Input.GetKeyUp(KeyCode.S))
                EventScheduler.Instance.Dispatch(new InputScheduler.OnKeyUp() { Key = KeyCode.S });
            if (Input.GetKeyUp(KeyCode.A))
                EventScheduler.Instance.Dispatch(new InputScheduler.OnKeyUp() { Key = KeyCode.A });
            if (Input.GetKeyUp(KeyCode.D))
                EventScheduler.Instance.Dispatch(new InputScheduler.OnKeyUp() { Key = KeyCode.D });
        }
    }
}
