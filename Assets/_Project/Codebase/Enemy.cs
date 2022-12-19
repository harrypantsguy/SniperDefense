using _Project.Codebase.UI;
using UnityEngine;

namespace _Project.Codebase
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private FillableBarUI _healthBar;
        private int _health;

        private void Start()
        {
            _health = MaxHealth;
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

        public void TakeDamage(float damage)
        {
            Health -= (int)damage;
            Health = Mathf.Max(Health, 0);

            if (Health == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}