using System;
using System.Collections;
using UnityEngine;

namespace _Project.Codebase
{
    public class Gun : MonoBehaviour
    {
        public float damage;
        public float fireDelay;
        public float range;
        public int magSize;
        public float reloadTime;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] public Transform _projectileSource;
        public Vector2 targetPos;
        public GameObject _projectileFab;
        [HideInInspector] public int bulletsInMag;
        public bool reloading;
        public bool inFireDelay;
        public float ReloadProgress { get; private set; }
        public float FireDelayProgress { get; private set; }

        private float _reloadStartTime;
        private float _lastFireTime;
        private Coroutine _reloadRoutine;
        private Coroutine _fireDelayRoutine;

        private void Start()
        {
            bulletsInMag = magSize;
            ReloadProgress = 1f;
            FireDelayProgress = 1f;
        }

        private void Update()
        {
            transform.right = (targetPos - (Vector2)transform.position).normalized;
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPositions(new Vector3[]{_projectileSource.position, (Vector2)_projectileSource.position
                                                                        + (Vector2)transform.right * range});
        }

        public void StartReload()
        {
            if (reloading || bulletsInMag == magSize) return;
            
            _reloadRoutine = StartCoroutine(ReloadRoutine());
        }

        private IEnumerator ReloadRoutine()
        {
            reloading = true;

            ReloadProgress = 0f;
            
            float t = 0;
            while (t < reloadTime)
            {
                yield return null;
                t += Time.deltaTime;
                ReloadProgress = t / reloadTime;
            }

            bulletsInMag = magSize;
            reloading = false;
            _reloadRoutine = null;
        }
        
        private IEnumerator FireDelayRoutine()
        {
            inFireDelay = true;
            
            float t = 0;
            while (t < fireDelay)
            {
                yield return null;
                t += Time.deltaTime;
                FireDelayProgress = t / fireDelay;
            }

            inFireDelay = false;
            _fireDelayRoutine = null;
        }

        public void Fire()
        {
            if (!reloading && bulletsInMag == 0)
            {
                StartReload();
            }
            
            if (inFireDelay || reloading || bulletsInMag == 0)
                return;

            if (_projectileFab == null) return;

            _fireDelayRoutine = StartCoroutine(FireDelayRoutine());
            
            bulletsInMag--;
            _lastFireTime = Time.time;
            Projectile.FireProjectile(_projectileFab, _projectileSource.position, _projectileSource.right);
        }
    }
}