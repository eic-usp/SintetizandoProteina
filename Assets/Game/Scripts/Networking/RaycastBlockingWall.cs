using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RaycastBlockingWall : MonoBehaviour
{
    [SerializeField] private GameObject raycastBlockingPanel;
    // private Image _raycastBlockingImage;

    private void OnEnable()
    {
        RaycastBlockEvent.Subscribe(BlockRaycast);
    }

    private void OnDisable()
    {
        RaycastBlockEvent.Unsubscribe(BlockRaycast);
    }

    // private void Start()
    // {
    //     _raycastBlockingImage = GetComponent<Image>();
    // }

    private void BlockRaycast(bool shouldBlock)
    {
        if (raycastBlockingPanel)
        {
            StartCoroutine(BlockRaycastEnumerator(shouldBlock));
        }
    }

    private IEnumerator BlockRaycastEnumerator(bool shouldBlock)
    {
        yield return null;
        raycastBlockingPanel.SetActive(shouldBlock);
        // _raycastBlockingImage.enabled = shouldBlock;
    }
}

public static class RaycastBlockEvent
{
    private static event Action<bool> BlockRaycast;

    public static void Subscribe(Action<bool> action)
    {
        BlockRaycast += action;
    }

    public static void Unsubscribe(Action<bool> action)
    {
        BlockRaycast -= action;
    }

    public static void Invoke(bool shouldBlock)
    {
        BlockRaycast?.Invoke(shouldBlock);
    }
}