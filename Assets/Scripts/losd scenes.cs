using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    public string nextlvl; // Set this in the Inspector for each checkpoint
    public BoxCollider2D Collider2D;
    private void OnTriggerEnter2D(Collider2D other)
    {
   
            SceneManager.LoadScene(nextlvl);
        
    }
}
