using UnityEngine;
using System.Collections;

public class SpriteAnimator : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer[] targetRenderers = null;

    public void SetColor(Color color)
    {
        for(int i = 0; i < targetRenderers.Length; ++i)
        {
            targetRenderers[i].color = color;
        }
    }

    public IEnumerator Twinkle(float time, float speed)
    {
        float elapsedTime = 0f;
        bool isRendererOn = false;
        while(elapsedTime < time)
        {
            for(int i = 0; i < targetRenderers.Length; ++i)
            {
                targetRenderers[i].enabled = isRendererOn;
            }
            isRendererOn = !isRendererOn;
            yield return new WaitForSeconds(speed / 2f);
            elapsedTime += speed / 2f;
        }
        for(int i = 0; i < targetRenderers.Length;++i)
        {
            targetRenderers[i].enabled = true;
        }
    }
}