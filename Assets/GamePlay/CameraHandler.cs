using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.GamePlay
{
    class CameraHandler
    {
        GameObject cam;
        public CameraHandler(GameObject cam)
        {
            this.cam = cam;
        }

        public void update()
        {
            // move the camera
            float moveVertical = Input.GetAxis("Vertical") * 4 * Time.deltaTime;
            //float moveVertical2 = Input.GetAxis("Vertical2") * 3 * Time.deltaTime;
            float moveHorizontal = Input.GetAxis("Horizontal") * 3 * Time.deltaTime;
            Vector3 move = new Vector3(moveHorizontal * 4F, 0, moveVertical * 3F);
            cam.transform.Translate(move, Space.World);

            // automove camera
            if (Input.mousePosition.x < 1)
            {
                cam.transform.Translate(new Vector3(-0.25F, 0, 0), Space.World);
            }
            else if (Input.mousePosition.x > Screen.width - 1)
            {
                cam.transform.Translate(new Vector3(0.25F, 0, 0), Space.World);
            }
            if (Input.mousePosition.y < 1)
            {
                cam.transform.Translate(new Vector3(0, 0, -0.25F), Space.World);
            }
            else if (Input.mousePosition.y > Screen.height - 1)
            {
                cam.transform.Translate(new Vector3(0, 0, 0.25F), Space.World);
            }
        }
    }
}
