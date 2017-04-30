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
    public bool isSelected = false;

    private int currIdx;
    private float selecterWidth;
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
        selecterWidth = GetComponent<RectTransform>().sizeDelta.x;
        currIdx = startIdx;
        characterArr[0].localPosition = Vector2.one;
        for (int i = 1; i < characterArr.Length; ++i)
        {
            characterArr[i].localPosition = new Vector2(selecterWidth, 0);
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

    public void OnSelected(bool isSelect)
    {
        isSelected = isSelect;
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
        characterArr[currIdx].localPosition = new Vector2(selecterWidth, 0);

        isRotating = true;
        characterArr[prevIdx].DOLocalMoveX(-selecterWidth, 0.4f).SetEase(Ease.InOutBack);
        characterArr[currIdx].DOLocalMoveX(0f, 0.4f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                isRotating = false;
            });

    }

    public int DecideCharacter()
    {
        OnSelected(false);
        return currIdx;
    }
}
