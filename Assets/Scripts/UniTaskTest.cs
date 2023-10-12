using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class UniTaskTest : MonoBehaviour
{
    Keyboard keyboard;
    KeyControl akey;
    KeyControl skey;
    Subject<Unit> inputSubject = new();
    ReactiveProperty<int> hpProperty = new();
    

    // Start is called before the first frame update
    async void Start()
    {
        keyboard = Keyboard.current;
        akey = keyboard.aKey;
        skey = keyboard.sKey;
        hpProperty.Value = 10;

        inputSubject.Subscribe(_ => print("Aキーが押された時の処理1"));
        inputSubject.Subscribe(_ => print("Aキーが押された時の処理2"));
        inputSubject.Subscribe(B);
        hpProperty.Subscribe(hp => print("現在HP：" + hp));

        await A();
        print("終了");
    }

    // Update is called once per frame
    void Update()
    {
        if (akey.wasPressedThisFrame) inputSubject.OnNext(Unit.Default);
        if (skey.wasPressedThisFrame) hpProperty.Value -= 1;
    }

    private async UniTask A()
    {
        await UniTask.Delay(1000);
        print("待機後");
    }

    private void B(Unit unit)
    {
        print("Unit");
    }
}
