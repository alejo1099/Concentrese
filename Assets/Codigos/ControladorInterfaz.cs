using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControladorInterfaz : MonoBehaviour
{
    public static ControladorInterfaz ControladorUI;

    public Image imagenCanvas;
    public Sprite[] imagenesInterfaz;

    public Text textoIntentos;
    public Text ganador;

    public GameObject botonReinicio;
    public GameObject botonMenuPrincipal;
    public GameObject botonContinuar;

    private int vecesGanadas;
    private int nivelActual;

    //Singleton e inicializacion de variables
    void Awake()
    {
        if (!ControladorUI)
        {
            ControladorUI = this;
        }
        else
        {
            Destroy(this);
        }
        textoIntentos.text = "Intentos : 30";
    }

    //Verificar cuando se gana el nivel y determinar lo que sigue
    public void VerificarEstadoJuego()
    {
        if (ControladorJuego.GameController.cubos.Count != 0)
        {
            return;
        }
        if (nivelActual < 5)
        {
            imagenCanvas.sprite = imagenesInterfaz[0];
            imagenCanvas.enabled = true;
            StartCoroutine(AumentarNivelJuego());
        }else if(nivelActual == 5)
        {
            imagenCanvas.sprite = imagenesInterfaz[2];
            imagenCanvas.enabled = true;
            ganador.enabled = true;
            StartCoroutine(CargarMenuPrincipal());

        }
    }

    //Cargar escena
    private IEnumerator CargarMenuPrincipal()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
    }

    //Aumentar escena nivel juego
    private IEnumerator AumentarNivelJuego()
    {
        yield return new WaitForSeconds(3);
        imagenCanvas.enabled = false;
        imagenCanvas.sprite = null;
        ControladorJuego.GameController.CantidadCubosNivel++;
        nivelActual++;
        vecesGanadas += 10;
        ControladorJuego.GameController.IntentosNivel = ControladorJuego.GameController.IntentosIniciales + vecesGanadas;
    }

    //Activador de variables cuando se pierde el juego
    public void JuegoPerdido()
    {
        imagenCanvas.sprite = imagenesInterfaz[1];
        imagenCanvas.enabled = true;
        botonReinicio.SetActive(true);
        botonMenuPrincipal.SetActive(true);
        Destroy(ControladorJuego.GameController.PadreInstanciados.gameObject);
        ControladorJuego.GameController.PadreInstanciados = new GameObject("PadreInstancias").transform;
        ControladorJuego.GameController.PadreInstanciados.SetParent(ControladorJuego.GameController.transform);
    }

    //Metodo que reinicia el nivel al primero
    public void ReiniciarJuego()
    {
        botonReinicio.SetActive(false);
        botonMenuPrincipal.SetActive(false);
        imagenCanvas.sprite = null;
        imagenCanvas.enabled = false;
        ControladorJuego.GameController.ReiniciarJuego();
    }

    //cargar menu principal
    public void IrAlMenuPrincipal()
    {
        botonReinicio.SetActive(false);
        botonMenuPrincipal.SetActive(false);
        imagenCanvas.sprite = null;
        SceneManager.LoadScene(0);
    }
}