using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorJuego : MonoBehaviour
{
    public static ControladorJuego GameController;

    public GameObject prefabCubo;
    public GameObject particula;
    public List<GameObject> cubos;

    public Material[] materialesCubos;

    public Transform PadreInstanciados { get; set; }
    private Transform transformCamara;

    private Vector3 posicionActualCubo;

    private List<int> listaIndices;
    private int randomCubo1;
    private int randomCubo2;
    private int randomMaterial;
    private int indiceCubo1;
    private int indiceCubo2;
    public int IntentosIniciales { get; set; }
    private int cantidadCubosNivel;
    private int intentosNivel;
    public int IntentosNivel
    {
        get
        {
            return intentosNivel;
        }
        set
        {
            intentosNivel = value;
            ControladorInterfaz.ControladorUI.textoIntentos.text = "Intentos : " + intentosNivel;
        }
    }
    public int CantidadCubosNivel
    {
        get
        {
            return cantidadCubosNivel;
        }
        set
        {
            cantidadCubosNivel += 2;
            ValoresIniciales();
        }
    }

    public bool Jugando { get; set; }

    public GameObject PrimerCuboSeleccionado { get; set; }
    public GameObject SegundoCuboSeleccionado { get; set; }


    //inicializador de variables
    void Awake()
    {
        if (!GameController)
        {
            GameController = this;
        }
        else
        {
            Destroy(this);
        }
        transformCamara = Camera.main.transform;
        PadreInstanciados = transform.GetChild(0);
        IntentosIniciales = 30;
    }

    void Start()
    {
        IntentosNivel = IntentosIniciales;
        cantidadCubosNivel = 4;
        ValoresIniciales();
    }

    //Valores dados para iniciar una partida de juego
    public void ValoresIniciales()
    {
        Jugando = true;
        InstanciarPrefabs();
        AñadirElementosIndices();
        Cubos();
        AgregarElementosCubos();
    }

    //Valores para el reincio del juego
    public void ReiniciarJuego()
    {
        IntentosNivel = 30;
        cantidadCubosNivel = 4;
        ValoresIniciales();
    }

    //Instancia de cubos
    private void InstanciarPrefabs()
    {
        cubos = new List<GameObject>();
        listaIndices = new List<int>();
        for (int i = 0; i < CantidadCubosNivel; i++)
        {
            for (int j = 0; j < CantidadCubosNivel; j++)
            {
                posicionActualCubo = new Vector3(0.5f + i, 0.5f + j, 0);
                cubos.Add(Instantiate(prefabCubo, posicionActualCubo, Quaternion.identity));
            }
        }
        transformCamara.position = new Vector3(CantidadCubosNivel / 2, CantidadCubosNivel / 2, -CantidadCubosNivel);
    }

    //Añadir cubos a lista
    private void AñadirElementosIndices()
    {
        for (int i = 0; i < cubos.Count; i++)
        {
            listaIndices.Add(i);
        }
    }

    //Emparentar cubos a gameobject
    private void AgregarElementosCubos()
    {
        for (int i = 0; i < cubos.Count; i++)
        {
            cubos[i].transform.SetParent(PadreInstanciados);
        }
    }

    //Determinar cbos con materiales pares
    private void Cubos()
    {
        for (int i = 0; i < cubos.Count / 2; i++)
        {
            randomMaterial = Random.Range(0, materialesCubos.Length);
            randomCubo1 = Random.Range(0, listaIndices.Count);
            EliminarIndice(randomCubo1, ref indiceCubo1);
            randomCubo2 = Random.Range(0, listaIndices.Count);
            EliminarIndice(randomCubo2, ref indiceCubo2);
            cubos[indiceCubo1].GetComponentInChildren<MeshRenderer>().material = materialesCubos[randomMaterial];
            cubos[indiceCubo2].GetComponentInChildren<MeshRenderer>().material = materialesCubos[randomMaterial];
        }
    }

    //Eliminar cubos con material ya asignado
    private void EliminarIndice(int numero, ref int indiceCubo)
    {
        indiceCubo = listaIndices[numero];
        listaIndices.Remove(listaIndices[numero]);
    }

    //Comparar cubos seleccionados
    public void VerificarCoincidencias()
    {
        IntentosNivel--;
        if (PrimerCuboSeleccionado.GetComponentInChildren<MeshRenderer>().material.name == SegundoCuboSeleccionado.GetComponentInChildren<MeshRenderer>().material.name)
        {
            Jugando = false;
            Destroy(Instantiate(particula,PrimerCuboSeleccionado.transform.position,particula.transform.rotation),2);
            Destroy(Instantiate(particula,SegundoCuboSeleccionado.transform.position,particula.transform.rotation),2);
            StartCoroutine(DesactivarCubos());
        }
        else
        {
            Jugando = false;
            StartCoroutine(GirarCubos());
        }
        if (IntentosNivel <= 0)
        {
            ControladorInterfaz.ControladorUI.JuegoPerdido();
        }
    }

    //Activar animaciones
    private IEnumerator GirarCubos()
    {
        yield return new WaitForSeconds(1);
        Jugando = true;
        if (PrimerCuboSeleccionado && SegundoCuboSeleccionado)
        {
            PrimerCuboSeleccionado.GetComponent<Animator>().SetBool("Girar", false);
            SegundoCuboSeleccionado.GetComponent<Animator>().SetBool("Girar", false);
        }
        PrimerCuboSeleccionado = null;
        SegundoCuboSeleccionado = null;
    }

    //Destruye los cubos si son pares
    private IEnumerator DesactivarCubos()
    {
        yield return new WaitForSeconds(1);
        Jugando = true;
        cubos.Remove(PrimerCuboSeleccionado);
        cubos.Remove(SegundoCuboSeleccionado);
        Destroy(PrimerCuboSeleccionado);
        Destroy(SegundoCuboSeleccionado);
        PrimerCuboSeleccionado = null;
        SegundoCuboSeleccionado = null;
        ControladorInterfaz.ControladorUI.VerificarEstadoJuego();
    }
}