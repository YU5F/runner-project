using System.Collections;
using System.Linq;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    [SerializeField] private GameObject groundPrefab;
    [SerializeField][Range(5, 20)] private int maxGroundAmount = 5;
    private float groundSizeZ;
    private float groundZPosition = 20f;
    private int activeGroundAmount;

    void Start(){
        groundSizeZ = groundPrefab.GetComponent<MeshRenderer>().bounds.size.z;
        StartCoroutine(SpawnGround());
    }

    void Update(){
        activeGroundAmount = GameObject.FindGameObjectsWithTag("Ground").Count();
    }

    IEnumerator SpawnGround(){
        while(true){
            if(activeGroundAmount < maxGroundAmount){
                CreateGround();
                yield return new WaitForSeconds(0f);
            }
            else{
                yield return null;
            }
        }
    }

    void CreateGround(){
        GameObject ground = Instantiate(groundPrefab);
        ground.transform.position = new Vector3(0, 0, groundZPosition);
        groundZPosition += groundSizeZ;
    }
}
