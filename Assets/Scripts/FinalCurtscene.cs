using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class FinalCurtscene : MonoBehaviour
{
    private AudioController _AudioController;
    private FadeController _FadeController;
    public float timeToFadeOut;
    public float timeStep;
    public float timeStepFury;
    private float timeStepPadrao;
    public GameObject transition;
    public GameObject btnSpace;
    public Dialogo dialogo;
    public List<string> falas;
    public Text txt;
    public bool isFinishWord;
    private string falaAtual;
    private int idFala = 0;
    private bool isStartStep;


    // Start is called before the first frame update
       private void Start() {
        _AudioController = FindObjectOfType(typeof(AudioController)) as AudioController;
        transition.SetActive(true);
        _FadeController = FindObjectOfType(typeof(FadeController)) as FadeController;
        timeStepPadrao = timeStep;
        txt.text = "";
        UpdateFalasList();
        falaAtual = falas[idFala];
    }

    // Update is called once per frame
    void Update()
    {
        
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
                _AudioController.PlayFX(_AudioController.uiClick, 1f);
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

        foreach(string fala in dialogo.falas)
        {
            falas.Add(fala);
        }
    }

    void NextFala()
    {
        idFala++;

        txt.text = "";
        if(idFala >= falas.Count)
        {
            StartCoroutine("WaitFadeIn");
        }
        else
        {
            falaAtual = falas[idFala];
            StartCoroutine("FalaStep");
        }
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
