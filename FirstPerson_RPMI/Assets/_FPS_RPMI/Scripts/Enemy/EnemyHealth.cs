using UnityEngine;

public class EnemyHealth : MonoBehaviour



{

    [Header("Health System Managemt")]
    [SerializeField] int maxHealth = 100;
    [SerializeField] int health;



    [Header("FeedBack Configuration")]
    [SerializeField] Material damagedMat;
    [SerializeField] GameObject deathVfx;
    [SerializeField] MeshRenderer enemyRend;
    Material baseMat;


    private void Awake()
    {
        
        health = maxHealth;
        baseMat = enemyRend.material;
    }
    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            health = 0;
            gameObject.SetActive(false);
            deathVfx.SetActive(true);
            gameObject.SetActive(false);
        }
    }


    public void TakeDamage(int damage)
    {
        health -= damage;
        enemyRend.material = damagedMat;
        Invoke(nameof(ResetEnemyMaterial), 0.1f);
    }
    void ResetEnemyMaterial()
    {
        enemyRend.material= baseMat;
    }
}
