using Engine.UI;
using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class HudWindowPresenter : IDisposable, IAttachableUi, IInitializable
{
    private readonly HudWindowView _view;

    public HudWindowPresenter(HudWindowView view)
    {
        _view = view;
    }

    public void Attach(Transform parent)
    {
        _view.Attach(parent);
    }

    public void Initialize()
    {
        _view.Subscribe(Test);
    }

    private void Test()
    {
        _view.SetTestValue(Random.Range(0, 1001).ToString());
    }


    public void Dispose()
    {
        _view.Unsubscribe();
    }
}