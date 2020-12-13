using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Mana : MonoBehaviour {

    public int quantidadeTiro = 50;

    public GameObject textoQuantidadeNeve;

    public GameObject ManaUI;

    public bool mostrarManaUI;

    private void Awake() {
        if (!mostrarManaUI) {
            ManaUI.SetActive(false);
        }
    }

    public void atualizarQuantidade(int qtd) {
        quantidadeTiro = qtd;
        TextMeshProUGUI textMeshProUGUI = textoQuantidadeNeve.GetComponent<TextMeshProUGUI>();
        textMeshProUGUI.text = qtd.ToString();
    }
}
