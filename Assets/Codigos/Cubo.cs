using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Cubo : MonoBehaviour
{

    private Camera camara;
    private Transform transformCamara;

    //Inicializador de variables
    void Awake()
    {
        camara = Camera.main;
        transformCamara = transform;
    }

    private void Update()
    {
        LanzarRaycast();
    }

    //Lanzador de raycast a los cubos
    private void LanzarRaycast()
    {
        if (ControladorJuego.GameController.Jugando)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Vector3 o = Input.mousePosition;
                o.z = Mathf.Abs(transformCamara.position.z);
                Vector3 posRel = camara.ScreenToWorldPoint(o) - transformCamara.position;
                Ray rayo = new Ray(transformCamara.position, posRel);
                RaycastHit infoGolpe;
                if (Physics.Raycast(rayo, out infoGolpe, 3 + Mathf.Abs(transformCamara.position.z)))
                {
                    VerificarSeleccion(infoGolpe.transform.gameObject);
                }
            }
        }
    }

    //Verificar refrencias de cubos pares
    private void VerificarSeleccion(GameObject objetoGolpeado)
    {
        if (!ControladorJuego.GameController.PrimerCuboSeleccionado)
        {
            ControladorJuego.GameController.PrimerCuboSeleccionado = objetoGolpeado;
            objetoGolpeado.GetComponent<Animator>().SetBool("Girar", true);
        }
        else if (!ControladorJuego.GameController.SegundoCuboSeleccionado && ControladorJuego.GameController.PrimerCuboSeleccionado != objetoGolpeado)
        {
            ControladorJuego.GameController.SegundoCuboSeleccionado = objetoGolpeado;
            objetoGolpeado.GetComponent<Animator>().SetBool("Girar", true);
            ControladorJuego.GameController.VerificarCoincidencias();
        }
    }
}