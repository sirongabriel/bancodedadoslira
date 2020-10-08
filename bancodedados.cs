using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teste : MonoBehaviour
{

    private int inventorySlots = 15;
    //primeira funcao. cria o "banco de dados"
    public void create_data(){
        //valores do banco
        Random numAleatorio = new Random();
        int valorInteiro = numAleatorio.Next(1000,9999);

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

    //funcao global que será utilizada nos códigos
    public void changeData(int slot, string campo, int value = 0, string name = ""){
        string where = "slot" + slot.ToString();
        string qnt = "qnt" + slot.ToString();
        if(campo == "lifes") addLife(value);
        else if(campo == "currency"){
            if(value > 0) addCurrency(value);
            else removeCurrency(value);
        } else if(campo == "xp"){
            if(PlayerPrefs.GetInt("xp") + value > PlayerPrefs.GetInt("level")){
              levelUp();
              // add a diferenca entre o valor atual e o valor maximo do level
              PlayerPrefs.SetInt("xp", PlayerPrefs.GetInt("xp") + value - PlayerPrefs.GetInt("level"));
            }
        }
    }
    //_____________________FUNCOES PARCIAIS_____________________________
    public void addCurrency(int value){
        PlayerPrefs.SetInt("currency", PlayerPrefs.GetInt("currency") + value);
        PlayerPrefs.Save();
    }

    public void removeCurrency(int value){
        int new_currency = PlayerPrefs.GetInt("currency") - value;

        if (new_currency > 0) PlayerPrefs.SetInt("currency", new_currency);
        else PlayerPrefs.SetInt("currency", 0);

        PlayerPrefs.Save();
    }

    public void addLife(int value){
        PlayerPrefs.SetInt("lifes", PlayerPrefs.GetInt("lifes") + value);
        PlayerPrefs.Save();
    }

    public void removeLife(int value){
        int new_life = PlayerPrefs.GetInt("lifes") - value;

        if (new_life> 0)  PlayerPrefs.SetInt("lifes", new_life);
        else PlayerPrefs.SetInt("lifes", 0);

        PlayerPrefs.Save();
    }

    public void levelUp(){
        int new_level = PlayerPrefs.GetInt("level") + 1;
        PlayerPrefs.SetInt("level", new_level);
        PlayerPrefs.Save();
    }
    // ______________________FUNCOES DE ALTERACAO DO BANCO_________________________

    public void addItem(int slot, int item, int value){
        string where = "slot" + slot.ToString();
        string qnt = "qnt" + slot.ToString();

        PlayerPrefs.SetInt(where, item);
        PlayerPrefs.SetInt(qnt, value);
        PlayerPrefs.Save();
    }

    public void removeItem(int slot){
        string where = "slot" + slot.ToString();
        string qnt = "qnt" + slot.ToString();

        PlayerPrefs.SetInt(where, 0);
        PlayerPrefs.SetInt(qnt, 0);
        PlayerPrefs.Save();
    }

    public void removeQnt(int slot, int value){
        string where = "slot" + slot.ToString();
        string qnt = "qnt" + slot.ToString();

        if (PlayerPrefs.GetInt(qnt) - value > 0) PlayerPrefs.SetInt(qnt, PlayerPrefs.GetInt(qnt) - value);
        else {
            PlayerPrefs.SetInt(where, 0);
            PlayerPrefs.SetInt(qnt, 0);
        }

        PlayerPrefs.Save();
    }

    public bool editName(string new_name){
        if (new_name.Length < 60)
        {
            PlayerPrefs.SetString("name",new_name);
            PlayerPrefs.Save();
            return true;
        }
        else return false;

    }
}
