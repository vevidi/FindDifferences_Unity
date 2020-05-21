using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vevidi.FindDiff.GameLogic
{
    public class MissTapCheckmark : MonoBehaviour
    {
        private Animator checkmarkAnimator;

        private void Awake()
        {
            checkmarkAnimator = GetComponent<Animator>();
            checkmarkAnimator.SetTrigger(GameVariables.MissAnimStart);
        }

        public void OnAnimationEnded() => Destroy(gameObject);
    }
}