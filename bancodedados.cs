using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teste : MonoBehaviour
{
    private int inventorySlots = 15;
    
    /*
    Essa variavel define o fator de multiplicacao para cada nivel.
    No primeiro nivel, sao 100 pontos de xp, no segundo, serao 200,
    pois o total é o nivel atual multiplicado por "nextlevel".
    */
    private int nextlevel = 100;
    
    /*
    Essa função cria o "banco de dados" por meio das preferencias do jogador
    */
    public void create_data(){
        //valores do banco
        Random numAleatorio = new Random();
        int valorInteiro = numAleatorio.Next(1000,9999);

        /*
        Essas sao as preferencias. Se quiser adicionar mais uma,
        siga a sintaxe
        */
        PlayerPrefs.SetString("name", "user" + valorInteiro.ToString());
        PlayerPrefs.SetInt("xp", 0);
        PlayerPrefs.SetInt("level", 1);
        PlayerPrefs.SetInt("currency", 0);
        PlayerPrefs.SetInt("lifes", 3);

        for (int i = 0; i < inventorySlots; i++)
        {
            string slot = "slot" + i.ToString();
            string qnt = "qnt" + i.ToString();

            PlayerPrefs.SetInt(slot, 0);
            PlayerPrefs.SetInt(qnt, 0);
        }
        PlayerPrefs.Save();
    }

    /*
    Essa sera a principal funcao. quando quiser alterar o xp, o life, etc 
    qualquer preferencia use essa funcao. Ex:

    1- Alterar level: changeData(slot, "vida", 100);
        Aqui voce adiciona vida. Se o valor passado for negativo, voce remove
    2- Alterar xp: changeData(slot, "xp", 150);
        Se o novo xp ultrapassar o limite do level atual, o jogador sobe de nivel
        automaticamente
    3- Alterar currency: changeData(slot, "currency", -15);
        Remove 15 unidades de currency e, se isso resultar num numero menor que 0, 
        entao currency recebe 0 
    */
    public void changeData(string campo, string value = 0, string name = "", int slot = 0){
        string where = "slot" + slot.ToString();
        string qnt = "qnt" + slot.ToString();

        if(campo == "lifes") changeLife(value);

        else if(campo == "currency") changeCurrency(value);

        else if(campo == "xp"){
            if(PlayerPrefs.GetInt("xp") + value >= nextlevel*PlayerPrefs.GetInt("level")){
              levelUp();
              // add a diferenca entre o valor (ganho e atual) e o valor maximo do level
              PlayerPrefs.SetInt("xp", PlayerPrefs.GetInt("xp") + value - nextlevel * PlayerPrefs.GetInt("level"));
            }
            else PlayerPrefs.SetInt("xp", PlayerPrefs.GetInt("xp") + value);}

        else if(campo == "name") editName(name);
        else if(campo == "item") {
            if (value > 0){
                addItem(slot, int.Parse(name), value);
            }else{
                removeItem(slot, value);
            }
        }

    }
    /*
    AQUI SE ENCONTRAM AS FUNCOES SECUNDARIAS (A FUNCAO PRINCIPAL CHAMA ESSAS FUNCOES). 
    O OBJETIVO É QUE ELAS NUNCA PRECISEM SER CHAMADAS DIRETAMENTE, APENAS POR MEIO DA
    FUNCAO PRINCIPAL
    */
    public bool editName(string new_name){
        if (new_name.Length < 60)
        {
            PlayerPrefs.SetString("name",new_name);
            PlayerPrefs.Save();
            return true;
        }
        else return false;

    }
    public void changeCurrency(int value){
        int new_currency = PlayerPrefs.GetInt("currency") + value;

        if (new_currency > 0) PlayerPrefs.SetInt("currency", new_currency);
        else PlayerPrefs.SetInt("currency", 0);

        PlayerPrefs.Save();
    }

    public void changeLife(int value){
        int new_life = PlayerPrefs.GetInt("lifes") + value;

        if (new_life > 0)  PlayerPrefs.SetInt("lifes", new_life);
        else PlayerPrefs.SetInt("lifes", 0);

        PlayerPrefs.Save();
    }

    public void levelUp(){
        int new_level = PlayerPrefs.GetInt("level") + 1;
        PlayerPrefs.SetInt("level", new_level);
        PlayerPrefs.Save();
    }
    /*
    FUNÇÕES QUE CUIDAM DE ADICIONAR OU REMOVER ITENS
    */

    public void addItem(int slot, int item, int value){
        string where = "slot" + slot.ToString();
        string qnt = "qnt" + slot.ToString();

        PlayerPrefs.SetInt(where, item);
        PlayerPrefs.SetInt(qnt, value);
        PlayerPrefs.Save();
    }

    //FUNÇÃO que remove um item do slot informado.  caso find seja true ele faz uma busca em todo o inventário pelo item_id que deve estar acusado no argumento slot
    public void removeItem(int slot, int value, bool find = false){
        string where = "slot" + slot.ToString();
        string qnt = "qnt" + slot.ToString();
        if (!find){
            if (value == 0)
            {
                PlayerPrefs.SetInt(where, 0);
                PlayerPrefs.SetInt(qnt, 0);
            } else {
                if (PlayerPrefs.GetInt(qnt) - value > 0) PlayerPrefs.SetInt(qnt, PlayerPrefs.GetInt(qnt) - value);
                else {
                    PlayerPrefs.SetInt(where, 0);
                    PlayerPrefs.SetInt(qnt, 0);
                }
            }
        } else {
            for (int i = 0; i < inventorySlots; i++){
                if (PlayerPrefs.GetInt("slot" + i.ToString()) == slot){
                    if (PlayerPrefs.GetInt("qnt" + i.ToString()) - value > 0) PlayerPrefs.GetInt("qnt" + i.ToString()) - value;
                        else PlayerPrefs.SetInt("qnt" + i.ToString(),0);
                    break;
                }
            }
        }

        PlayerPrefs.Save();
    }
}
