using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static int health = 3;
    public static void DecreaseHealth(){
        if(health > 1){
            health--;
        }
        else{
            Debug.Log("game over");
            Time.timeScale = 0;
        }
    }
}
