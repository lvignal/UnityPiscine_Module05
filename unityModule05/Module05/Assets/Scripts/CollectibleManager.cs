using System;
using UnityEngine;

namespace Module05.Collectible
{
    public class CollectibleManager : MonoBehaviour
    {
        public int NumberOfCollectibles { get; private set; }

        private void Awake()
        {
            NumberOfCollectibles = transform.childCount;
        }

        public void Reset()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }

        public bool IsCollectibleCollected(int index)
        {
            return !transform.GetChild(index).gameObject.activeSelf;
        }
        
        public void Collect(int index)
        {
            transform.GetChild(index).gameObject.SetActive(false);
        }
    }
}