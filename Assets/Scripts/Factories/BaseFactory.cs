using UnityEngine;

public class BaseFactory : MonoBehaviour
{
    protected T CreateItem<T>(T objToClone) where T : UnityEngine.Object
    {
        var rez = Instantiate(objToClone);
        return rez;
    }

    protected T CreateItem<T>(T objToClone, Transform parent) where T : MonoBehaviour
    {
        var rez = Instantiate(objToClone);
        rez.transform.SetParent(parent);
        rez.transform.localScale = Vector3.one;
        return rez;
    }
}
