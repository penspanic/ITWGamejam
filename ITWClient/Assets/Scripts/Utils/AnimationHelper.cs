using UnityEngine;

public static class AnimationHelper
{
    public static Vector2 Arc(Vector2 start, Vector2 end, float ratio, float time)
    {
        float added = Mathf.Sin(time * Mathf.PI) * ratio;
        Vector2 current = EasingUtil.EaseVector2(EasingUtil.linear, start, end, time);
        Vector2 dirVec = (end - start).normalized;
        Vector2 up = new Vector2();
        if(dirVec.x < 0)
        {
            //up = Quaternion.AngleAxis(90, dirVec).eulerAngles.normalized;
            up = (Quaternion.Euler(0, 0, -90) * dirVec).normalized;
            //up = Quaternion.AngleAxis(90, dirVec).eulerAngles.normalized;
        }
        else
        {
            //up = Quaternion.AngleAxis(-90, dirVec).eulerAngles.normalized;
            up = (Quaternion.Euler(0, 0, 90) * dirVec).normalized;
            //up = Quaternion.AngleAxis(-90, dirVec).eulerAngles.normalized;
        }

        //if(dirVec.x < 0 && dirVec.y > 0)
        //{
        //    up = Quaternion.AngleAxis(90, dirVec).eulerAngles.normalized;
        //}
        //else if(dirVec.x >= 0 && dirVec.y > 0)
        //{
        //    up = Quaternion.AngleAxis(-90, dirVec).eulerAngles.normalized;
        //}
        //else if(dirVec.x < 0 && dirVec.y <= 0)
        //{
        //    up = Quaternion.AngleAxis(90, dirVec).eulerAngles.normalized;
        //}
        //else // dirVec.x >= 0 && dirVec.y <= 0)
        //{
        //    up = Quaternion.AngleAxis(-90, dirVec).eulerAngles.normalized;
        //}
        Debug.Log("dir vec : " + dirVec + " up : " + up);
        Debug.Log(up * added);
        current += up * added;
        return current;
    }
}