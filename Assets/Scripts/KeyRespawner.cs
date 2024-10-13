//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class KeyRespawner : MonoBehaviour
//{
//    public GameObject keyObject;  // Referencja do obiektu klucza

//    private bool keyDeactivated = false;

//    void Update()
//    {
//        // Sprawdzamy, czy klucz zosta³ zebrany (czyli dezaktywowany)
//        if (!keyObject.activeSelf)
//        {
//            keyDeactivated = true;
//        }

//        // Sprawdzamy, czy gracz zgin¹³ i klucz by³ wczeœniej dezaktywowany
//        if (SpikeTrigger.isPlayerDead && keyDeactivated)
//        {
//            // Ponowna aktywacja klucza
//            keyObject.SetActive(true);
//            keyDeactivated = false;  // Resetowanie flagi, poniewa¿ klucz znowu jest aktywny
//        }
//    }
//}
