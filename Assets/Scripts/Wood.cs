using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    public List<GameObject> woods;
    public int woodValue = 10;
    public void Collect()
    {
        var destroyWood = woods[^1];
        woods.Remove(destroyWood);
        Destroy(destroyWood);
        if (woods.Count<=0)
        {
            Destroy(gameObject);
        }
    }
}
