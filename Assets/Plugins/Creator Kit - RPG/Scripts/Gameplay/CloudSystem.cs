﻿using UnityEngine;

namespace Plugins.Creator_Kit___RPG.Scripts.Gameplay
{
    /// <summary>
    /// Performs batch translation of cloud transforms, resetting the transform position when
    /// the cloud transform position exceeds the resetRadius distance to the controller.
    /// Automatically collects and animates all it's child transforms.
    /// </summary>
    public class CloudSystem : MonoBehaviour
    {
        public Vector3 windDirection = Vector2.left;
        public float windSpeed = 1;
        public float minSpeed = 0.5f;
        public float resetRadius = 100;
        private Transform[] clouds;
        private float[] speeds;

        private void Start()
        {
            clouds = new Transform[transform.childCount];
            speeds = new float[transform.childCount];
            for (var i = 0; i < transform.childCount; i++)
            {
                clouds[i] = transform.GetChild(i);
                speeds[i] = Random.value;
            }
        }

        private void Update()
        {
            var r2 = resetRadius * resetRadius;
            for (var i = 0; i < speeds.Length; i++)
            {
                var cloud = clouds[i];
                var speed = Mathf.Lerp(minSpeed, windSpeed, speeds[i]);
                cloud.position += windDirection * speed;
                if (cloud.localPosition.sqrMagnitude > r2)
                {
                    cloud.position = -cloud.position;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, resetRadius);
        }
    }
}