using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static int health = 3;
    public static bool gameOver = false;
    public static void DecreaseHealth(){
        if(health > 1){
            health--;
        }
        else{
            gameOver = true;
        }
    }

    public static void Restart(){
        gameOver = false;
        health = 3;
    }
}
