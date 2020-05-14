using UnityEngine;

public class BaseFactory : MonoBehaviour
{
    protected T CreateItem<T>(T objToClone) where T : UnityEngine.Object
    {
        var rez = Instantiate(objToClone);
        return rez;
    }
}
