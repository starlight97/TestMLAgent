using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CooKieRunTest
{
    public class CookierunTestMain : MonoBehaviour
    {
        public CookieRunAgent agent;
        public Renderer groundRenderer;

        public Material redMat;
        public Material groundMat;

        // Start is called before the first frame update
        void Start()
        {
            agent.onDie = () =>
            {
                StartCoroutine(this.ShowRedScreen());
            };
        }

        private IEnumerator ShowRedScreen()
        {
            this.groundRenderer.material = redMat;
            yield return new WaitForSeconds(0.5f);
            this.groundRenderer.material = groundMat;
        }
    }

}
