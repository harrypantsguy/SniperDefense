using System;
using System.Collections.Generic;
using _Project.Codebase.UI;
using UnityEngine;

namespace _Project.Codebase
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private FillableBarUI _healthBar;
        private int _health;

        public static readonly List<Enemy> enemies = new List<Enemy>();
        public static Action<Enemy> NewEnemyEvent;
        public static Action<Enemy> RemoveEnemyEvent;
        
        private void Start()
        {
            _health = MaxHealth;
            enemies.Add(this);
            NewEnemyEvent.Invoke(this);
        }

        private void Update()
        {
            transform.position -= new Vector3(5f * Time.deltaTime, 0f, 0f);
        }

        public int MaxHealth { get; set; } = 25;

        public int Health
        {
            get => _health;
            set
            {
                _health = value;
                _healthBar.SetFillAmount((float)Health / MaxHealth);
            }
        }

        public void TakeDamage(DamageReport damageReport)
        {
            int damage = damageReport.damage;
            Health -= damage;
            Health = Mathf.Max(Health, 0);

            FlyingText.SpawnFlyingText(damage.ToString(), damageReport.impactLocation, damageReport.direction, false);
            
            if (Health == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Destroy(gameObject);
            enemies.Remove(this);
            RemoveEnemyEvent.Invoke(this);
        }
    }
}