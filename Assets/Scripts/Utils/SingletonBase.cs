using UnityEngine;

public class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static bool isShuttingDown = false;
    protected static T instance;

    private void OnApplicationQuit()
    {
        isShuttingDown = true;
    }
    
    private void OnDestroy()
    {
        isShuttingDown = true;
    }
}