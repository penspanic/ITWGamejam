using UnityEngine;
using System.Collections;

/// <summary>
/// PlayerController에서 내린 명령을 수행하기만 함, 게임 로직적인 내용은 담지 말아야 함.
/// </summary>
public class Player : MonoBehaviour
{

    private ICharacter character;

    private void Awake()
    {

    }


    // 플레이 중에 동적으로 캐릭터를 바꿀 수 있도록? 도 할수 있으면 좋을 것 같다.
    public void SetCharacter(ICharacter character)
    {
        this.character = character;
    }
}