using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MenuButton : MonoBehaviour {
    [SerializeField]
    private Transform selectTrs;
    private GameObject unSelectObj;
    private GameObject selectObj;

    private Vector2 selectOriPos;


    public void InitMenuButton(bool isSelect = false) 
    {
        selectObj = selectTrs.FindChild("Select").gameObject;
        unSelectObj = selectTrs.FindChild("UnSelect").gameObject;

        selectOriPos = selectTrs.position;

        OnSelected(isSelect);
    }


    public void OnSelected(bool isPress)
    {
        if (isPress)
        {
            selectObj.SetActive(true);
            unSelectObj.SetActive(false);
            selectTrs.DOMoveY(selectOriPos.y + 0.2f, 0.2f).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            selectTrs.DOKill(true);
            selectTrs.position = selectOriPos;
            selectObj.SetActive(false);
            unSelectObj.SetActive(true);
        }
    }


}
