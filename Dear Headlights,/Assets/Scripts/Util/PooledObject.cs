using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Den {
    public class PooledObject : MonoBehaviour {

        [HideInInspector] public ObjectPooler m_ObjectPooler;

        public void ReturnToPool() {
            m_ObjectPooler.ReturnObject(gameObject);
        }
    }
}
