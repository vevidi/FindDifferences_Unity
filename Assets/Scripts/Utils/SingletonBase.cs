using UnityEngine;

public class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static bool isShuttingDown = false;
    protected static T instance;

    protected virtual void OnApplicationQuit() => isShuttingDown = true;
    
    protected virtual void OnDestroy() => isShuttingDown = true;
}