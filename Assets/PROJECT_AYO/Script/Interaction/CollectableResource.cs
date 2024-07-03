using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYO
{
    public enum CollectResourceType //���������� �ڿ� ����
    {
        None = 0,
        Tree = 1,
        Stone = 2,
    }

    public class CollectableResource : MonoBehaviour
    {
        public static CollectableResource Instance { get; private set; } = null;    // Instance�� �ʿ��Ѱ�? ����

        public CollectResourceType ResourceType => resourceType;    //�Ŀ� �ν����� â�� ���̵���

        [SerializeField] private CollectResourceType resourceType;  //�ܺο��� ���� ���ϵ��� private

        public float resourceHp = 100.0f;

        public bool isDestroy = false;

        public Action<CollectableResource> OnResourceDestroy;

        private void Awake()
        {
            Instance = this;
        }

        public void Damage(float damage)
        {
            resourceHp -= damage;
            if (resourceHp <= 0)
            {
                //if (OnResourceDestroy != null)
                //{
                //    OnResourceDestroy();
                //}
                OnResourceDestroy?.Invoke(this);
                Destroy(gameObject);
            }
        }

        public void OnDestroy()
        {
            //if (resourceHp == 0)
            //{
            //    Instance = null;
            //    Destroy(gameObject);
            //    isDestroy = true;
            //}
        }


    }
}
