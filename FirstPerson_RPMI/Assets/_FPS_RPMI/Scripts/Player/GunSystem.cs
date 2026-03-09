using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunSystem : MonoBehaviour
{
    #region
    [Header("General References")]
    [SerializeField] Camera fpsCam; //Ref si disparamos desde el centro de la cam
    [SerializeField] Transform shootPoint; //Ref si disparamos desde la punta del cañón
    [SerializeField] LayerMask impactLayer; //Layer con la q interactua el raycast
    RaycastHit hit; //Almacen de la informacion de los objetos.

    [Header("Weapon Parameters")]
    [SerializeField] int damage = 10; //Daño del arma por bala
    [SerializeField] float range = 100; //Distancia máxima de disparo
    [SerializeField] float spread = 0; //Radio de dispersión del disparo
    [SerializeField] float shootingCooldown = 0.2f; //Tiempo entre disparos
    [SerializeField] float reloadTime = 1.5f; //Tiempo de recarga en segundos
    [SerializeField] bool allowButtonHold = false; //Si el disparo se ejecuta por click (false) o por mantener (true)

    [Header("Bullet Management")]
    [SerializeField] int ammoSize = 30; //Cantidad máxima de balas por cargador
    [SerializeField] int bulletsPerTap = 1; //Cantidad de balas disparadas por cada ejecución de disparo
    [SerializeField] int bulletsLeft; //Cantidad de balas dentro del cargador

    [Header("Feedback References")]
    [SerializeField] GameObject impactEffect; //Referencia al VFX de impacto de bala

    [Header("Dev - Gun State Bools")]
    [SerializeField] bool shooting; //Indica si estamos disparando
    [SerializeField] bool canShoot; //Indica si podemos disparar en X momento del juego
    [SerializeField] bool reloading; //Indica si estamos en proceso de recarga

    #endregion

    private void Awake()
    {
        bulletsLeft = ammoSize; //Al iniciar partida, tenemos el cargador lleno
        canShoot = true; //Al iniciar partida, podemos disparar
    }

    // Update is called once per frame
    void Update()
    {
        if (canShoot && shooting && !reloading && bulletsLeft > 0)
        {
            //inicializar el proceso de disparo
            StartCoroutine(ShootRoutine());
        }
    }

    IEnumerator ShootRoutine()
    {
        canShoot = false;   //primera capa de seguridad que evita que apilemos disparos
        if (!allowButtonHold) shooting = false;// configuracion del disparo por tap
        for (int i = 0; i < bulletsPerTap; i++)
        {
            if (bulletsLeft <= 0) break;
            Shoot();
            bulletsLeft--;
        }
        yield return new WaitForSeconds(shootingCooldown);
        canShoot = true;
    }
    


    void Shoot()
    {

        //ESTE ES EL METODO MÁS IMPORTANTE
        //SE DEFINE DISPARO POR RAYCAST -> UTILIZABLE POR CUALQUIER MECÁNICA

        //Almacenar la información de disparo y modificarla en caso de haber dispersión
        Vector3 direction = fpsCam.transform.forward;


        //Añadir dispersión aleatoria según el valor de spread
        direction.x += Random.Range(-spread, spread);
        direction.y += Random.Range(-spread, spread);

        //DECLARACIÓN DEL RAYCAST
        // Physics. Raycast(origen del rayo, dirección, almacén de la info del impacto, longitud del rayo y layer con la que impacta el rayo)
        if (Physics.Raycast(fpsCam.transform.position, direction, out hit, range, impactLayer))
        {
            //Aquí podemos codear todos los efectos que quiero para la interacción
            Debug.Log(hit.collider.name);
            if (hit.collider.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
                enemyHealth.TakeDamage(damage);
            }
        }
    }

    IEnumerator ReloadRoutine()
    {
        reloading = true;
        //aqui iria la llamada a la animacion de recarga
        yield return new WaitForSeconds(reloadTime);
        bulletsLeft = ammoSize;
        reloading = false;
    }

    void Reload()
    {
        if (bulletsLeft < ammoSize && !reloading)
        {
            StartCoroutine(ReloadRoutine());
        }
    }




    #region Input Methods

    public void OnShoot(InputAction.CallbackContext context)
    {
        // el sistema de input debe comprobar de si el disparo es por tap o por mantener
        if (allowButtonHold)
        {
            //Modo mantener on
            shooting = context.ReadValueAsButton();
        }
        else
        {
            //modo tap on
            if (context.performed) shooting = true;
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.performed) Reload();
    }

    #endregion
}
