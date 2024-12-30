using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public class CatCtrl : AnimalCtrl
    {
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        protected override void Update()
        {
            speed = 4.0f;
            MoveTowardsPlayer();

            base.Update();
        }
    }
}
