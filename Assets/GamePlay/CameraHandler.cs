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
            float moveScroll = -Input.GetAxis("Mouse ScrollWheel") * 500 * Time.deltaTime;
            float ytemp = cam.transform.localPosition.y;
            if ((moveScroll < 0 && ytemp < 10) || (moveScroll > 0 && ytemp > 50))
            {
                moveScroll = 0;
            }
            float moveVertical = Input.GetAxis("Vertical") * 5 * Time.deltaTime;
            float moveHorizontal = Input.GetAxis("Horizontal") * 5 * Time.deltaTime;
            if (Application.platform == RuntimePlatform.Android)
            {
                moveVertical = Input.acceleration.y * 0.35F;
                moveHorizontal = Input.acceleration.x * 0.35F;
            }
            Vector3 move = new Vector3(moveHorizontal * 4F * (ytemp / 10F), moveScroll * 1.5F, moveVertical * 4F * (ytemp / 10F));
            cam.transform.Translate(move, Space.World);

            // automove camera
            if (Input.mousePosition.x < 2)
            {
                cam.transform.Translate(new Vector3(-0.3F, 0, 0), Space.World);
            }
            else if (Input.mousePosition.x > Screen.width - 2)
            {
                cam.transform.Translate(new Vector3(0.3F, 0, 0), Space.World);
            }
            if (Input.mousePosition.y < 2)
            {
                cam.transform.Translate(new Vector3(0, 0, -0.3F), Space.World);
            }
            else if (Input.mousePosition.y > Screen.height - 2)
            {
                cam.transform.Translate(new Vector3(0, 0, 0.3F), Space.World);
            }
        }
    }
}
