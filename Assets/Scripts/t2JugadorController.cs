using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Rigidbody))]
public class jugador : LivingEntity
{
    CharacterController characterController;
    Rigidbody rb;
    public float speed = 10.0f;
    Vector3 moveInput, moveVelocity;
    public Camera mainCamera;
    DisparaBala controladorBalas; 

    public delegate void OnDeathJugador();
    public static event OnDeathJugador OnDeathPlayer;



    private float nextFoodSpawnTime = 0f;
    public float foodSpawnInterval = 10f;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        controladorBalas = GetComponent<DisparaBala>();
<<<<<<< Updated upstream
=======
        GenerarObjetosAleatorios();
    }

    void GenerarObjetosAleatorios()
    {
        for (int i = 0; i < NumComida; i++)
        {
            Vector3 posicionAleatoria = new Vector3(Random.Range(-rangoX, rangoX), 0, Random.Range(-rangoZ, rangoZ));

            float alturaTerreno = Terreno.SampleHeight(posicionAleatoria);

            posicionAleatoria.y = alturaTerreno;

            GameObject objetoAleatorio = comida[Random.Range(0, comida.Length)];

            Instantiate(objetoAleatorio, posicionAleatoria, Quaternion.identity);
        }
>>>>>>> Stashed changes
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundplane = new Plane(Vector3.up, Vector3.zero);
        if (groundplane.Raycast(ray, out float rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            Debug.DrawLine(ray.origin, point, Color.red);
            transform.LookAt(new UnityEngine.Vector3(point.x, transform.position.y, point.z));
        }

        if (Input.GetMouseButtonDown(0))
        {
            controladorBalas.Dispara();
        }

        if (Time.time >= nextFoodSpawnTime)
        {
            SpawnFoodNearPlayer();
            nextFoodSpawnTime = Time.time + foodSpawnInterval;
        }
    }

    void FixedUpdate()
    {
        moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveVelocity = moveInput.normalized * speed;
        characterController.Move(moveVelocity * Time.fixedDeltaTime);
    }

    void OnDestroy()
    {
        if (OnDeathPlayer != null)
        {
            OnDeathPlayer();
            SceneManager.LoadScene("Game Over");
        }
    }

    // Método para generar comida cerca del jugador
    void SpawnFoodNearPlayer()
    {
        Vector3 playerPosition = transform.position;
        Vector3 posicionAleatoria = new Vector3(Random.Range(playerPosition.x - 5f, playerPosition.x + 5f), 0f, Random.Range(playerPosition.z - 5f, playerPosition.z + 5f));

        float alturaTerreno = Terreno.SampleHeight(posicionAleatoria);

        posicionAleatoria.y = alturaTerreno;

        GameObject objetoAleatorio = comida[Random.Range(0, comida.Length)];

        Instantiate(objetoAleatorio, posicionAleatoria, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Comida"))
        {
            Destroy(other.gameObject);

        }
    }
}
