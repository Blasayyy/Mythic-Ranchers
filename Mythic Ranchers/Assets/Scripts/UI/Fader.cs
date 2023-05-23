using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //IEnumerator FonduIn()
    //{
    //    Color colTemp = Color.black;
    //    fonduRectangle.color = colTemp;
    //    while (fonduRectangle.color.a > 0.0f)
    //    {
    //        colTemp.a -= taux;
    //        fonduRectangle.color = colTemp;
    //        yield return new WaitForEndOfFrame(); //a Chaque update
    //    }
    //    //Pour ne pas avoir de alpha negatif
    //    colTemp.a = 0.0f;
    //    fonduRectangle.color = colTemp;
    //}

    //IEnumerator FonduOut()
    //{
    //    Color colTemp = Color.black;
    //    colTemp.a = 0.0f;
    //    fonduRectangle.color = colTemp;
    //    while (fonduRectangle.color.a < 1f)
    //    {
    //        colTemp.a += taux;
    //        fonduRectangle.color = colTemp;
    //        yield return new WaitForEndOfFrame(); //a Chaque update
    //    }
    //    //Pour ne pas avoir de alpha plus que 1
    //    colTemp.a = 1f;
    //    fonduRectangle.color = colTemp;
    //}
}
