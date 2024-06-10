using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public interface ISelectable
    {
        public GameObject Selection { get; }
        public void Select();
        public void Deselect();
    }
}
