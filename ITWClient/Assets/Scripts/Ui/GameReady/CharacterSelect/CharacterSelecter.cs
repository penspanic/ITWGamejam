using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum PlayerType
{
    None,
    PL1,
    PL2,
    CPU,
}

public class CharacterSelecter : MonoBehaviour {
    public Transform[] characterArr;
    public PlayerType plType;
    public GameObject selectObj;
    public Text selectObjText;
    public bool isSelected = false;

    public int currIdx { get; private set; }
    private float selecterHeight;
    private bool isRotating = false;


    public void InitCharacterSelecter(int startIdx, PlayerType plType) 
    {
        plType = plType;
        if (startIdx == 99)
        {
            // X 처리
            return;
        }
        isRotating = false;
        selecterHeight = GetComponent<RectTransform>().sizeDelta.y;
        currIdx = startIdx;
        characterArr[0].localPosition = Vector2.one;
        for (int i = 1; i < characterArr.Length; ++i)
        {
            characterArr[i].gameObject.SetActive(true);
            characterArr[i].localPosition = new Vector2(0, selecterHeight);
        }



    }

    void Update()
    {
        if (plType == PlayerType.PL1)
        {
            //if(Input.GetKeyDown(KeyCode.Arr
        }
    }

    public void SetDisable()
    {
        
    }

    public void OnSelected(bool isSelect, string text)
    {
        isSelected = isSelect;
        selectObj.SetActive(isSelect);

        if (isSelect == true)
        {            
            selectObjText.text = text;
        }
        else
        {
            selectObjText.text = "";
            selectObjText.gameObject.SetActive(isSelect);
        }
    }

    public void MoveNext()
    {
        if (isSelected == false || isRotating == true)
        {
            return;
        }

        var prevIdx = currIdx++;
        if (currIdx >= characterArr.Length)
        {
            currIdx = 0;
        }
        characterArr[currIdx].localPosition = new Vector2(0, selecterHeight);

        isRotating = true;
        characterArr[prevIdx].DOLocalMoveY(-selecterHeight, 0.4f).SetEase(Ease.InOutBack);
        characterArr[currIdx].DOLocalMoveY(0f, 0.4f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                isRotating = false;
            });

    }

    public int DecideCharacter()
    {
        OnSelected(false, "");
        return currIdx;
    }
}
