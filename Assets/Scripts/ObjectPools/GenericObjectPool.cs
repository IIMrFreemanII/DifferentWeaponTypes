using System.Collections.Generic;
using UnityEngine;

namespace ObjectPools
{
    public abstract class GenericObjectPool<T> : MonoBehaviour where T : Component
    {
        [SerializeField] private T prefab = null;
        [SerializeField] private int currentInstancesAmount;

        public static GenericObjectPool<T> Instance { get; private set; }
        private Queue<T> _objects = new Queue<T>();
    
        private void Awake()
        {
            Instance = this;
        }

        public T Get()
        {
            if (_objects.Count == 0)
            {
                AddObjects(1);
                currentInstancesAmount++;
            }

            return _objects.Dequeue();
        }

        private void AddObjects(int count)
        {
            for (int i = 0; i < count; i++)
            {
                T newObject = Instantiate(prefab, transform);
                newObject.gameObject.SetActive(false);
                _objects.Enqueue(newObject);
            }
        }

        public void ReturnToPool(T objectToReturn)
        {
            objectToReturn.gameObject.SetActive(false);
            Transform currentObjTransform = objectToReturn.transform;
            currentObjTransform.SetParent(transform);

            _objects.Enqueue(objectToReturn);
        }
    }
}