using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorEscenas : MonoBehaviour
{

    //Carga escena del juego
    public void CargarEscenaPrincipal()
    {
        SceneManager.LoadScene(1);
    }

    //Quita la aplicacion
    public void QuitarAplicaion()
    {
        Application.Quit();
    }
}