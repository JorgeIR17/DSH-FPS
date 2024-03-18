using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Rigidbody))]
public class jugador : LivingEntity
{
    CharacterController characterController;
    Rigidbody rb;
    public float speed = 10.0f;
    UnityEngine.Vector3 moveInput, moveVelocity;
    public Camera mainCamera;
    DisparaBala controladorBalas; 

    public delegate void OnDeathJugador();
    public static event OnDeathJugador OnDeathPlayer;

    public GameObject[] comida;
    private int NumComida = 7;
    public Terrain Terreno;
    public float rangoX, rangoZ;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        controladorBalas = GetComponent<DisparaBala>();
        GenerarObjetosAleatorios();
    }
    //Hay que ver que falla aquí
    void GenerarObjetosAleatorios()
    {
        for (int i = 0; i < NumComida; i++)
        {
            // Generar una posición aleatoria dentro del rango especificado
            Vector3 posicionAleatoria = new Vector3(Random.Range(-rangoX, rangoX), 0, Random.Range(-rangoZ, rangoZ));

            // Obtener la altura del terreno en la posición aleatoria
            float alturaTerreno = Terreno.SampleHeight(posicionAleatoria);

            // Ajustar la posición en Y para que el objeto esté en la superficie del terreno
            posicionAleatoria.y = alturaTerreno;

            // Elegir un objeto aleatorio de la lista
            GameObject objetoAleatorio = comida[Random.Range(0, comida.Length)];

            // Instanciar el objeto en la posición ajustada
            Instantiate(objetoAleatorio, posicionAleatoria, Quaternion.identity);
        }
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

        if (Input.GetMouseButtonDown(0)){
            controladorBalas.Dispara();
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
        if(OnDeathPlayer != null)
        {
            OnDeathPlayer();
        }
    }

}