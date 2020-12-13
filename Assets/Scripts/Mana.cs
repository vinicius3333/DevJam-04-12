using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Mana : MonoBehaviour {

    public int quantidadeTiro = 50;

    public GameObject textoQuantidadeNeve;

    public GameObject ManaUI;

    public bool mostrarManaUI;

    private TextMeshProUGUI textMeshProUGUI;

    private void Awake() {
        if (!mostrarManaUI) {
            ManaUI.SetActive(false);
        }

        textMeshProUGUI = textoQuantidadeNeve.GetComponent<TextMeshProUGUI>();
        textMeshProUGUI.text = quantidadeTiro.ToString();
    }

    public void atualizarQuantidade(int qtd) {
        quantidadeTiro = qtd;
        textMeshProUGUI.text = qtd.ToString();
    }
}
