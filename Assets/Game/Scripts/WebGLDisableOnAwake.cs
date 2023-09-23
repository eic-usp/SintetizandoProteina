using UnityEngine;

public class WebGLDisableOnAwake : MonoBehaviour
{
#if UNITY_WEBGL
    private void Awake()
    {
        gameObject.SetActive(false);
    }
#endif
}
