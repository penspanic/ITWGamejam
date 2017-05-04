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
    public Image selectObj;
    public Text selectObjText;
    public bool isSelected = false;

    public int currIdx { get; private set; }
    private float selecterHeight;
    private bool isRotating = false;

    private void Awake()
    {
        selecterHeight = GetComponent<RectTransform>().sizeDelta.y;
        for (int i = 0; i < characterArr.Length; ++i)
        {
            characterArr[i].gameObject.SetActive(true);
            characterArr[i].localPosition = new Vector2(0, selecterHeight);
        }
    }

    public void InitCharacterSelecter(int startIdx, PlayerType plType) 
    {
        this.plType = plType;
        if (startIdx == 99)
        {
            // X 처리
            return;
        }
        isRotating = false;
        currIdx = startIdx;

        // characterArr[startIdx].localPosition = Vector2.zero;
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

    public void OnSelected(bool isSelect, string text, Color roundColor = default(Color))
    {
        isSelected = isSelect;
        selectObj.gameObject.SetActive(isSelect);

        if (isSelect == true)
        {            
            selectObjText.text = text;
            isRotating = true;
            characterArr[currIdx].DOLocalMoveY(0f, 0.4f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                isRotating = false;
            });
            selectObj.color = roundColor;
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
        return currIdx + 1; // 0번은 ICharacterType의 Unkown이라 +1을 해준다.
    }
}
