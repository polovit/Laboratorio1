using UnityEngine;
using UnityEngine.UI;

public class Carta : MonoBehaviour
{
    public Sprite immagineScoperta; // Immagine della faccia della carta
    private Sprite immagineCoperta; // Immagine del dorso
    public bool scoperta = false;  // Stato della carta

    [SerializeField] Image _image; // Componente Image della carta

    void Start()
    {
        _image = GetComponent<Image>();
        if (_image == null)
        {
            Debug.LogError($"Errore: Componente Image non trovato sull'oggetto {gameObject.name}");
            return;
        }

        if (immagineCoperta != null)
        {
            _image.sprite = immagineCoperta; // Imposta il dorso iniziale
        }
    }

    public void SetupDorso(Sprite dorso)
    {
        immagineCoperta = dorso;

        if (_image == null)
        {
            Debug.LogError($"Errore: Componente Image non trovato sull'oggetto {gameObject.name}");
            return;
        }

        _image.sprite = immagineCoperta; // Mostra il dorso
    }

    public void OnCartaClick()
    {
        if (!scoperta)
        {
            Scopri();
        }
        else
        {
            Copri();
        }
    }

    public void Scopri()
    {
        scoperta = true;
        _image.sprite = immagineScoperta; // Mostra la faccia
    }

    public void Copri()
    {
        scoperta = false;
        _image.sprite = immagineCoperta; // Mostra il dorso
    }
}

