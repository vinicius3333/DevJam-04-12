using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
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

    private void Start() {
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

        if(_FadeController.isFadeComplete == true)
        {
            if(isStartStep == false)
            {
                isStartStep = true;
                StartCoroutine("FalaStep");
            }

            if(Input.GetButtonDown("Jump") && isFinishWord == true)
            {   
                NextFala();
            }

            if(Input.GetButton("Jump") && isFinishWord == false)
            {
                btnSpace.SetActive(false);
                timeStep = timeStepFury;
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
        falas.Clear();

        foreach(string fala in dialogo[idDialogo].falas)
        {
            falas.Add(fala);
        }
    }

    void NextFala()
    {
        idFala ++;
        print(idFala);
        txt.text = "";

        if(idFala >= falas.Count)
        {
            NextDialogo();
            idFala = 0;
        }
        else
        {
            falaAtual = falas[idFala];
            StartCoroutine("FalaStep");
        }
    }

    void NextDialogo()
    {
        idDialogo ++;
        print(idDialogo);
        if(idDialogo >= dialogo.Length)
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
           
        }

        UpdateFalasList();
    }

    IEnumerator WaitFadeOut()
    {
        yield return new WaitForSeconds(timeToFadeOut);
        
        if(isLastScene == true)
        {
            mainCam.backgroundColor = bgColorCam;
            cena.SetActive(false);
            title.SetActive(true);
        }
        else
        {
            mainCam.backgroundColor = Color.black;
        }

        _FadeController.FadeOut();
        spriteRenderer.sprite = cenas[idDialogo];
    }

    IEnumerator WaitFadeIn()
    {
        yield return new WaitForSeconds(timeToFadeOut);
        SceneManager.LoadScene(1);
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
