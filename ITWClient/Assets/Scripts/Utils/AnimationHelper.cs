using UnityEngine;

public static class AnimationHelper
{
    public static Vector2 Arc(Vector2 start, Vector2 end, float max, float time)
    {
        float added = Mathf.Sin(time * Mathf.PI);
        Vector2 current = EasingUtil.EaseVector2(EasingUtil.linear, start, end, time);
        Vector2 dirVec = (end - start).normalized;
        Vector2 up = new Vector2();
        if(dirVec.x < 0 && dirVec.y > 0)
        {
            up = Quaternion.AngleAxis(90, dirVec).eulerAngles.normalized;
        }
        else if(dirVec.x >= 0 && dirVec.y > 0)
        {
            up = Quaternion.AngleAxis(-90, dirVec).eulerAngles.normalized;
        }
        else if(dirVec.x < 0 && dirVec.y <= 0)
        {
            up = Quaternion.AngleAxis(90, dirVec).eulerAngles.normalized;
        }
        else // dirVec.x >= 0 && dirVec.y <= 0)
        {
            up = Quaternion.AngleAxis(-90, dirVec).eulerAngles.normalized;
        }
        current += up * added;
        return current;
    }
}