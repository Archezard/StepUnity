using System.Collections.Generic;
using UnityEngine;

namespace BasketballCards.Utilities
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _initialSize = 10;
        
        private Queue<GameObject> _pool = new Queue<GameObject>();
        
        private void Start()
        {
            InitializePool();
        }
        
        private void InitializePool()
        {
            for (int i = 0; i < _initialSize; i++)
            {
                GameObject obj = Instantiate(_prefab, transform);
                obj.SetActive(false);
                _pool.Enqueue(obj);
            }
        }
        
        public GameObject GetObject()
        {
            if (_pool.Count > 0)
            {
                GameObject obj = _pool.Dequeue();
                obj.SetActive(true);
                return obj;
            }
            else
            {
                GameObject obj = Instantiate(_prefab, transform);
                return obj;
            }
        }
        
        public void ReturnObject(GameObject obj)
        {
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }
        
        public void ReturnAll()
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.activeSelf)
                {
                    ReturnObject(child.gameObject);
                }
            }
        }
    }
}