using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CooKieRunTest
{
    public class Floors : MonoBehaviour
    {
        public float moveSpeed;
        public GameObject[] floorGos;

        private void Update()
        {
            this.transform.Translate(Vector3.left * this.moveSpeed * Time.deltaTime);
            if (this.transform.localPosition.x <= -28f)
            {
                var pos = this.transform.localPosition;
                pos.x = 28f;
                this.transform.localPosition = pos;

                ShowFloors();
                HideFloor();
            }
        }

        private void ShowFloors()
        {
            foreach (var floorGo in floorGos)
            {
                floorGo.SetActive(true);
            }
        }
        private void HideFloor()
        {
            var rand = Random.Range(0, floorGos.Length);
            floorGos[rand].SetActive(false);
            if(rand+1 != floorGos.Length)
            {
                floorGos[rand+1].SetActive(false);
            }
            else
            {
                floorGos[rand - 1].SetActive(false);
            }
        }
    }

}
