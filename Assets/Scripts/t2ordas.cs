using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class t2ordas : MonoBehaviour
{
    public valoresenemigos[] valoresEnemigos;
    public valoresenemigos enemigoActual;
    float tiempoEspera = 0.0f;
    int numOrdaActual = 0;
    int enemigosporCrear = 0;
    int enemigosporMatar = 0;

    Vector3 posicionSpawn = new Vector3(142.62f, 10.44f, 219.72f);
    // Start is called before the first frame update
    void Start()
    {
        NextOrda();
        LivingEntity.onDeathAnother += EnemigoMuerto;
    }

    void NextOrda()
    {
        numOrdaActual++;
        enemigoActual = valoresEnemigos[numOrdaActual - 1];
        enemigosporCrear = enemigoActual.numeroEnemigos;
        enemigosporMatar = enemigoActual.numeroEnemigos;
    }

    void EnemigoMuerto()
    {
        enemigosporMatar --;
        if(enemigosporMatar  <= 0)
        {
            NextOrda();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(enemigosporCrear > 0 && tiempoEspera <=0)
        {
            Instantiate(enemigoActual.tipoEnemigo, posicionSpawn , Quaternion.identity);
            enemigosporCrear--;
            tiempoEspera = enemigoActual.tiempoEntreEnemigos;
        } 
        else
        {
            tiempoEspera -= Time.deltaTime;
        }
    }
}
