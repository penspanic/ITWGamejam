using UnityEngine;
using System.Collections;

public class Engineer : ICharacter
{
    [SerializeField]
    private GameObject steamShotPrefab;

    protected override void Awake()
    {
        base.Awake();
        CharacterType = CharacterType.Engineer;
    }

    protected override void UseSkill()
    {
        base.UseSkill();
        animator.Play("skill", 0);
        State = CharacterState.SkillActivated;

        SteamShot newSteamShot = Instantiate(steamShotPrefab).GetComponent<SteamShot>();
        newSteamShot.SetOwner(this);
        newSteamShot.gameObject.layer = this.gameObject.layer;
        Vector2 additionalPos = new Vector2(0.3f, 0.1f);
        newSteamShot.transform.position = this.transform.position;
        additionalPos.x *= IsFacingRight ? 1 : -1;
        newSteamShot.transform.Translate(additionalPos);

        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y = IsFacingRight == true ? 180 : 0;
        newSteamShot.transform.rotation = Quaternion.Euler(rotation);

        StartCoroutine(SkillProcess());
    }
    
    private IEnumerator SkillProcess()
    {
        yield return new WaitForSeconds(0.5f);
        if(State == CharacterState.SkillActivated)
        {
            OnSkillEnd();
        }
    }

    protected override void OnSkillEnd()
    {
        State = CharacterState.Idle;
        animator.Play("idle", 0);
    }
}