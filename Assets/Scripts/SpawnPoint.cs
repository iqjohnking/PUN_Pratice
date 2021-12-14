using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tanks
{
    public class SpawnPoint : MonoBehaviour
    {
        void Awake()
        {
            //一開始不要出現坦克
            gameObject.SetActive(false);
        }
    }
}
