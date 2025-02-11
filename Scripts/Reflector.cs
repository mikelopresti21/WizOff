using UnityEngine;

public class Reflector : MonoBehaviour
{
    public float reflectDuration = 2f;
    public bool usingReflector;
    private bool flipped;

    public void FlipHorizontally(){
        Vector3 scale = transform.localScale;
        scale.x = -scale.x;
        transform.localScale = scale;
        flipped = !flipped;
    }

    public void DestroyReflector(){
        Destroy(gameObject);
    }

    public bool IsFlipped(){
        return flipped;
    }
}
