using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour
{
    public int offsetX = 2; //PARA NO TENER ERRORES

    //PARA CHEQUEAR LOS BORDES SI NECESITAMOS
    public bool hasARightBuddy = false;
    public bool hasALeftBuddy = false;

    public bool reverseScale = false; //PARA SABER SI EL OBJETO NO ESTA TILABLE

    private float spriteWidth = 0f;  //PARA SABER LA ANCHURA DEL ELEMENTO
    private Camera cam;
    private Transform myTransform;

    void Awake ()
    {
        cam = Camera.main;
        myTransform = transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        //Si todavía necesita agrandarse, sino no hace nada
        if (hasALeftBuddy == false || hasARightBuddy == false)
        {
            //calcular la extensión desde el centro de la cámara hacia el marco exterior
            float camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;

            //calcular la camara que avanza y la que se retrasa (visualmente hablando)
            float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth / 2) - camHorizontalExtend;
            float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth / 2 + camHorizontalExtend);

            //si el margen de la cámara alcanza suelo creamos una porción de suelo
            if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && hasARightBuddy == false)
            {
                MakeNewBuddy(1);
                hasARightBuddy = true;
            }
            else if (cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasALeftBuddy == false)
            {
                MakeNewBuddy(-1);
                hasALeftBuddy = true;
            }
        }
    }
    //PARA CALCULAR LA NUEVA PORCION DE SUELO
    void MakeNewBuddy(int rightOrLeft)
    {
        Vector3 newPosition = new Vector3(myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
        //instanciar nueva porcion en una variable
        Transform newBuddy = Instantiate(myTransform, newPosition,myTransform.rotation) as Transform;
        if (reverseScale == true)
        {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x*-1,newBuddy.localScale.y,newBuddy.localScale.z);
        }
        newBuddy.parent = myTransform.parent;
        if (rightOrLeft > 0)
        {
            newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
        }
        else
        {
            newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
        }
    }
}
