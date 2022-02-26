using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DungeonGenerator : MonoBehaviour
{
	public static DungeonGenerator instance;
	public RoomType up, down, left, right;
	public int numberOfRooms, currentRooms;
    public float difficultyMod;

	public List<GameObject> pieces = new List<GameObject>();
    public List<GameObject> startRooms = new List<GameObject>();

    private float counterTilGen;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    void Start() {
        UIController.instance.loadScreen.SetActive(true);

        Instantiate(startRooms[UnityEngine.Random.Range(0, startRooms.Count)], transform.position, Quaternion.identity);

        StartCoroutine(CheckForCompletion());

        difficultyMod = -PlayerController.instance.luck;
    }

    // Update is called once per frame
    void Update()
    {
        counterTilGen += Time.deltaTime;
    }

    public IEnumerator CheckForCompletion() {
        int pieceNo = pieces.Count;

        yield return new WaitForSeconds(2.5f);

        if(pieceNo != pieces.Count) {
            StartCoroutine(CheckForCompletion());
        } else {
            Debug.Log("Generation Finished,  " + counterTilGen.ToString());

            UIController.instance.loadScreen.SetActive(false);

            PlayerController.instance.isPaused = false;
        }
    }

    public GameObject GetNextPiece(DungeonPiece.Direction directionToGet) {
    	int rand;

    	switch(directionToGet) {
    		case DungeonPiece.Direction.down:
    			rand = UnityEngine.Random.Range(0,100);

    			if(rand < 2) {
    				return down.veryRare[UnityEngine.Random.Range(0,down.veryRare.Count)];
    			} else if(rand < 40) {
    				return down.rare[UnityEngine.Random.Range(0,down.rare.Count)];
    			} else {
    				return down.common[UnityEngine.Random.Range(0,down.common.Count)];
    			}
    			break;
    		case DungeonPiece.Direction.up:
    			rand = UnityEngine.Random.Range(0,100);

    			if(rand < 2) {
    				return up.veryRare[UnityEngine.Random.Range(0,up.veryRare.Count)];
    			} else if(rand < 40) {
    				return up.rare[UnityEngine.Random.Range(0,up.rare.Count)];
    			} else {
    				return up.common[UnityEngine.Random.Range(0,up.common.Count)];
    			}
    			break;
    		case DungeonPiece.Direction.right:
    			rand = UnityEngine.Random.Range(0,100);

    			if(rand < 2) {
    				return right.veryRare[UnityEngine.Random.Range(0,right.veryRare.Count)];
    			} else if(rand < 40) {
    				return right.rare[UnityEngine.Random.Range(0,right.rare.Count)];
    			} else {
    				return right.common[UnityEngine.Random.Range(0,right.common.Count)];
    			}
    			break;
    		case DungeonPiece.Direction.left:
    			rand = UnityEngine.Random.Range(0,100);

    			if(rand < 2) {
    				return left.veryRare[UnityEngine.Random.Range(0,left.veryRare.Count)];
    			} else if(rand < 40) {
    				return left.rare[UnityEngine.Random.Range(0,left.rare.Count)];
    			} else {
    				return left.common[UnityEngine.Random.Range(0,left.common.Count)];
    			}
    			break;
    	}

    	return null;
    }

    [System.Serializable]
    public class RoomType {
    	public List<GameObject> common, rare, veryRare;
    }
}
