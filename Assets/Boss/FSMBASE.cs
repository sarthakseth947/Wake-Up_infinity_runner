using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMBASE : StateMachineBehaviour {
    public GameObject NPC;
    public GameObject enemy;
    public float speed=200.0f;
    public float accuracy=1.0f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        NPC = animator.gameObject;
        enemy = NPC.GetComponent<BossAI>().GetPlayer();
    }
}
