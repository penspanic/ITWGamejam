using UnityEngine;
using System.Collections;


public class IMap : MonoBehaviour
{
    private IObstacle[] obstacles;

    public virtual void InitMap()
    {
        obstacles = transform.GetComponentsInChildren<IObstacle>(true);

        if (obstacles != null)
        {
            for (int i = 0; i < obstacles.Length; ++i)
            {
                obstacles[i].InitObstacle();
                SortingLayerController.Instance.AddTarget(obstacles[i]);
            }
        }

    }

}