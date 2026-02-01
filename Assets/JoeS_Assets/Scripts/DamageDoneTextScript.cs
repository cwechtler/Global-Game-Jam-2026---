using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;


public class DamageDoneTextScript : MonoBehaviour
{
    private float OldHealth;
    private float Damage;
     private Transform mainCameraTransform;
    // Start is called before the first frame update
    
  
     public TMP_Text DamageDoneText;
     public Enemy EnemyObject; 
    private Camera mainCamera;
    void Start()
    {
        DamageDoneText.text = "0";
        EnemyObject = GetComponentInParent<Enemy>();
        mainCameraTransform = Camera.main.transform;
        mainCamera = Camera.main;
        if (mainCameraTransform == null)
        {
            UnityEngine.Debug.Log("Main Camera not found! Please tag a camera as 'MainCamera'.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (OldHealth > EnemyObject.Health)
        {
            Damage = OldHealth - EnemyObject.Health;
        }
        DamageDoneText.text = Damage.ToString();
        transform.LookAt(mainCamera.transform.position);
        if (DamageDoneText != null)
        {
            // Set the color to a new random opaque color
            //DamageDoneText.color = Random.ColorHSV();
        }
        else
        {
//            Debug.LogError("TextMeshPro component reference not set!");
        }
        //UnityEngine.Debug.Log(OldHealth + " " + EnemyObject.Health + " " + Damage);
        OldHealth = EnemyObject.Health;

    }
        
}

