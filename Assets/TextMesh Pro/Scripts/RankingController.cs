using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class RankingController : MonoBehaviour
{
    private StreamReader leitor;
    private int[] lista = null;
    private string path;
    private int aux;
    private int num;
    private string[] leitura;

    public RectTransform painelColocacoes;
    public TextMeshProUGUI textoSemResultados;
    public TextMeshProUGUI[] textPontuacao = new TextMeshProUGUI[5];
    public GameObject[] textColocacao = new GameObject[5];

    // Start is called before the first frame update
    void Start()
    {
        path = ".\\Assets\\Configure\\rank.txt";
        ControlePaineis(false);

        leitura = File.ReadAllLines(path);
        aux = leitura.Length - 50;
        num = (leitura.Length < 50) ? leitura.Length : leitura.Length - aux;

        if (num > 0)
        {
            lista = new int[num];
            LeituraRank(path);
            ControlePaineis(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(painelColocacoes.gameObject.active)
        {
            for(int i = 0; i < textPontuacao.Length; i++)
            {
                if (i < lista.Length)
                {
                    textPontuacao[i].SetText(lista[i].ToString());
                }
                else
                {
                    textColocacao[i].SetActive(false);
                }
            }
        }
    }

    public void LeituraRank(string path)
    {
        int[] numeros = new int[num];
        for (int i = 0; i < num; i++)
        {
            numeros[i] = int.Parse(leitura[i]);
            //Console.WriteLine(numeros[i]);
        }

        // Ordena a lista para decrescente
        int[] listaDecrescente = numeros.OrderByDescending(i => i).ToArray();

        lista = listaDecrescente;
    }

    private void ControlePaineis(bool input)
    {
        painelColocacoes.gameObject.SetActive(input);
        textoSemResultados.gameObject.SetActive(!input);
    }
}
