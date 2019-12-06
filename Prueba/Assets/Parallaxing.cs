using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{   public Transform[] backgrounds;    //ARREGLO DE TODOS LAS PARTES TRASERAS Y DELANTERAS QUE VAN A PARALLEXEARSE
    private float[] parallaxScales;    //PROPORCIONES DE LA CAMARA QUE SE VAN A MOVER CON EL BACKGROUND
    public float smoothing = 1f;            //QUE TAN FLUIDO VA A SER EL PARALLAX (SETEAR SIEMPRE EN 0)

    private Transform cam;            //Referencia a la camara main transform
    private Vector3 previosCamPos;    //La posicion de la camara en el frame previo

    //Awake se llama antes del start (es para asignar variables como la cam) (referencias)
    void Awake ()
    {
        cam = Camera.main.transform;

    }
    // Start is called before the first frame update
    void Start()
    {
        previosCamPos = cam.position;

        //ASIGNAR LA ESCALA CORRESPONDIENTE
        parallaxScales = new float[backgrounds.Length];

        for (int i=0; i<backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z*-1;

        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            //PARALLAX OPUESTO A EL MOVIMIENTO DE LA CAMARA PORQUE EL ULTIMO FRAME ES MULTIPLICADO POR LA ESCALA
            float parallax = (previosCamPos.x - cam.position.x) * parallaxScales[i];


            //SETEAR LA POS X EN LA MISMA POSICION PERO SUMANDOLE EL PARALLAX
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            //AJUSTAR LOS VECTORES DE LA CAMARA CON LA POSICION DE X
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);


            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        //SETEAR LA CAMARA PREVIA AL FINAL DEL FRAME
        previosCamPos = cam.position;
    }
}
