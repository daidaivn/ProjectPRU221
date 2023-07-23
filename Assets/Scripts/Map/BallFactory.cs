using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class BallFactory
    {
        private GameObject prefabBall;

        public BallFactory(GameObject ballPrefab)
        {
            prefabBall = ballPrefab;
        }

        // Phương thức này tạo một đối tượng Ball mới và trả về nó
        public GameObject CreateBall(Transform parent, Vector3 position)
        {
            GameObject ball = GameObject.Instantiate(prefabBall, position, Quaternion.identity);
            ball.transform.SetParent(parent);
            return ball;
        }
    }
}