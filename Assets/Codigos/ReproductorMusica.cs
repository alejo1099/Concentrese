using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReproductorMusica : MonoBehaviour
{

	public static ReproductorMusica Reproductor;

    public AudioClip[] sonidos;

    private AudioSource controladorSonidos;

    //Singleton y refrencias iniciales
    void Awake()
    {
		if(!Reproductor)
		{
			Reproductor = this;
			
		}else
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
		controladorSonidos = GetComponent<AudioSource>();
    }

    void Update()
    {
        ReproducirSonidos();
    }

    //Cambia de clip cuando este termina
    private void ReproducirSonidos()
    {
        if (!controladorSonidos.isPlaying)
        {
            controladorSonidos.clip = sonidos[Random.Range(0, sonidos.Length)];
            controladorSonidos.Play();
        }
    }
}
