using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    private AudioController _AudioController; 
    private Camera mainCam;
    private FadeController _FadeController;
    public Color bgColorCam;
    public float timeToFadeOut;
    public float timeStep;
    public float timeStepFury;
    private float timeStepPadrao;
    public SpriteRenderer spriteRenderer;
    public GameObject transition;
    public GameObject canvasText;
    public GameObject btnSpace;
    public GameObject cena;
    public GameObject title;
    public Sprite[] cenas;
    public Dialogo[] dialogo;
    public List<string> falas;
    public Text txt;
    public bool isFinishWord;
    private string falaAtual;
    private int idFala = 0;
    private int idDialogo = 0;
    private bool isStartStep;
    private bool isLastScene;
    private bool isFirstNext;

    private void Start() {
        _AudioController = FindObjectOfType(typeof(AudioController)) as AudioController;
        transition.SetActive(true);
        mainCam = Camera.main;
        _FadeController = FindObjectOfType(typeof(FadeController)) as FadeController;
        cena.SetActive(true);
        title.SetActive(false);
        timeStepPadrao = timeStep;
        txt.text = "";
        UpdateFalasList();
        falaAtual = falas[idFala];
        spriteRenderer.sprite = cenas[idDialogo];
    }

    private void Update() {

        if(_FadeController.isFadeComplete == true && isLastScene == false)
        {
            if(isStartStep == false)
            {
                isStartStep = true;
                StartCoroutine("FalaStep");
            }

            if(Input.GetButtonDown("Jump") && isFinishWord == true)
            {   
                NextFala();
                _AudioController.PlayFX(_AudioController.uiClick, 1f);
            }

            if(Input.GetButton("Jump") && isFinishWord == false)
            {
                btnSpace.SetActive(false);
                timeStep = timeStepFury;
                _AudioController.PlayFX(_AudioController.uiClick, 1f);
            }
            else if(Input.GetButton("Jump") && isFinishWord == true && btnSpace.activeSelf == false)
            {
                btnSpace.SetActive(true);
            }

            if(Input.GetButtonUp("Jump") && isFinishWord == false)
            {
                btnSpace.SetActive(true);
                timeStep = timeStepPadrao;
            }
        }
    }

    void UpdateFalasList()
    {
        if(isLastScene == true) {return;}
        falas.Clear();

        foreach(string fala in dialogo[idDialogo].falas)
        {
            falas.Add(fala);
        }
    }

    void NextFala()
    {
        if(isFirstNext == true)
        {     
            isFirstNext = false;
            idFala = 0;
        }
        else
        {
            idFala++;
        }

        txt.text = "";
        print(falas.Count);
        if(idFala >= falas.Count)
        {
            UpdateFalasList();
            NextDialogo();
        }
        else
        {
            falaAtual = falas[idFala];
            StartCoroutine("FalaStep");
        }
       
    }

    void NextDialogo()
    {
        isFirstNext = true;
        idFala = 0;
        idDialogo ++;
        if(idDialogo >= dialogo.Length) //ultima cena
        {
            isLastScene = true;
            canvasText.SetActive(false);
            _FadeController.FadeIn();
            StartCoroutine("WaitFadeOut");
        }
        else
        {
            _FadeController.FadeIn();
            StartCoroutine("WaitFadeOut");
           UpdateFalasList();
        }
    }

    IEnumerator WaitFadeOut()
    {
        yield return new WaitForSeconds(timeToFadeOut);
        
        if(isLastScene == true)
        {
            mainCam.backgroundColor = bgColorCam;
            cena.SetActive(false);
            title.SetActive(true);
            StartCoroutine("WaitFadeIn");
        }
        else
        {
            mainCam.backgroundColor = Color.black;
            spriteRenderer.sprite = cenas[idDialogo];
           
        }
         _FadeController.FadeOut();
    }

    IEnumerator WaitFadeIn()
    {
        _FadeController.FadeIn();
        yield return new WaitForSeconds(timeToFadeOut * 2);
     
        if(_FadeController.isFadeComplete == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    IEnumerator FalaStep()
    {
        isFinishWord = false;
        char[] slip = falaAtual.ToCharArray();

        for(int i = 0; i < slip.Length; i++)
        {
            txt.text += slip[i];
            yield return new WaitForSeconds(timeStep);
        }
        isFinishWord = true;
    }
}
