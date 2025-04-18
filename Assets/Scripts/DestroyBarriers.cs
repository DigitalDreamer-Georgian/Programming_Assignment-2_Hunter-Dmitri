using UnityEngine;
//code to make the barricades break on lvl 1 when all enemys are killed
public class DestroyBarriers : MonoBehaviour
{
    public GameObject[] barriers; // finds game object barriers

    void Update()
    {
        //finds objects with the game objects with the tags enemys and if the enemies are = to 0 destroy all the barriers
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
        {
            foreach (GameObject obstacle in barriers)
            {
                Destroy(obstacle);
            }

            enabled = false;
        }
    }
}
