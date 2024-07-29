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
        Rock = 2,
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

            switch(resourceType)
            {
                case CollectResourceType.Tree:
                    {
                        var prefab = PrefabManager.Instance.GetPrefab("TreeItem");
                        var newDropItem = Instantiate(prefab);

                        Vector2 randomCircle = UnityEngine.Random.insideUnitCircle;
                        Vector3 dropPoint = transform.position + Vector3.up + new Vector3(randomCircle.x, 0, randomCircle.y);
                        newDropItem.transform.position = dropPoint;
                    }
                    break;

                case CollectResourceType.Rock:
                    {
                        var prefab = PrefabManager.Instance.GetPrefab("RockItem");
                        var newDropItem = Instantiate(prefab);

                        Vector2 randomCircle = UnityEngine.Random.insideUnitCircle;
                        Vector3 dropPoint = transform.position + Vector3.up + new Vector3(randomCircle.x, 0, randomCircle.y);
                        newDropItem.transform.position = dropPoint;
                    }
                    break;
            }

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
