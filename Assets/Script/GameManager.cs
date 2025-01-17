using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton

    public GameObject prefabCarta; // Prefab della carta
    public Transform gridTransform; // Griglia dove posizionare le carte
    public Sprite[] cardFace; // Immagini delle facce delle carte
    public Sprite cardBack; // Immagine del dorso

    [SerializeField] List<Carta> carteScoperte = new List<Carta>(); // Lista delle carte attualmente scoperte
    [SerializeField] int coppieTrovate = 0; // Numero di coppie trovate

    void Awake()
    {
        // Imposta il Singleton
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        GeneraCarte();
    }

    void GeneraCarte()
    {
        Sprite[] immaginiMescolate = MescolaCarte();

        for (int i = 0; i < immaginiMescolate.Length; i++)
        {
            GameObject nuovaCarta = Instantiate(prefabCarta, gridTransform);
            Carta scriptCarta = nuovaCarta.GetComponent<Carta>();

            if (scriptCarta != null)
            {
                scriptCarta.immagineScoperta = immaginiMescolate[i]; // Assegna la faccia
                scriptCarta.SetupDorso(cardBack); // Assegna il dorso
            }
            else
            {
                Debug.LogError("Errore: Lo script 'Carta' non è presente nel prefab.");
            }
        }
    }

    Sprite[] MescolaCarte()
    {
        Sprite[] immaginiDoppie = new Sprite[cardFace.Length * 2];

        for (int i = 0; i < cardFace.Length; i++)
        {
            immaginiDoppie[i * 2] = cardFace[i];
            immaginiDoppie[i * 2 + 1] = cardFace[i];
        }

        for (int i = 0; i < immaginiDoppie.Length; i++)
        {
            Sprite temp = immaginiDoppie[i];
            int randomIndex = Random.Range(0, immaginiDoppie.Length);
            immaginiDoppie[i] = immaginiDoppie[randomIndex];
            immaginiDoppie[randomIndex] = temp;
        }

        return immaginiDoppie;
    }

    public void CartaCliccata(Carta carta)
    {
        if (carteScoperte.Contains(carta) || carta.scoperta || carteScoperte.Count >= 2)
        {
            Debug.Log("La carta è già stata selezionata");
            return;
        }
        else
        {
            carta.Scopri();
            carteScoperte.Add(carta);
            Debug.Log("Carta aggiunta");
        }
        if (carteScoperte.Count == 2)
        {
            // Avvia il controllo delle carte dopo un breve ritardo
            Invoke("ControllaCoppia", 1.0f);
        }
    }

    private void ControllaCoppia()
    {
        // Confronta i nomi delle immagini
        if (carteScoperte[0].immagineScoperta.name == carteScoperte[1].immagineScoperta.name)
        {
            Debug.Log($"Coppia trovata: {carteScoperte[0].immagineScoperta.name} e {carteScoperte[1].immagineScoperta.name}");
            coppieTrovate++;
            carteScoperte.Clear(); // Mantieni le carte scoperte
        }
        else
        {
            Debug.Log($"Carte diverse: {carteScoperte[0].immagineScoperta.name} e {carteScoperte[1].immagineScoperta.name}. Copertura in corso...");
            foreach (var carta in carteScoperte)
            {
                carta.Copri();
            }
            carteScoperte.Clear(); // Copri entrambe le carte
        }

        // Controlla se il gioco è terminato
        if (coppieTrovate == cardFace.Length)
        {
            Debug.Log("Hai trovato tutte le coppie! Hai vinto!");
        }
    }
 

}
