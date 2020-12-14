using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ReturnTo0 : MonoBehaviour
{
    private AudioController _AudioController;
    // Start is called before the first frame update
    void Start()
    {
        _AudioController = FindObjectOfType(typeof(AudioController)) as AudioController;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Escape))
        {
            _AudioController.ChangeMusic(_AudioController.intro);
            SceneManager.LoadScene(0);
        }
    }
}
