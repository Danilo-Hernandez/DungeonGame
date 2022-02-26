using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DungeonPiece : MonoBehaviour
{
	public enum Direction {up, down, left, right};

	public List<SpawnPoint> points;

	public int width, length;

	public bool generated, initialized, isRoom, startRoom, isWall, hasEnemies;

    public GameObject mapPiece, pickupSpawner;

    public List<GameObject> doors, enemies;

    // Start is called before the first frame update
    void Start()
    {
        if(startRoom) {
        	DungeonGenerator.instance.pieces.Add(gameObject);
        }

        DetectCollisions();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isWall) {
            StartCoroutine(Initialize());
        }

        for(int i = 0; i < enemies.Count; i++) {
            if(enemies[i] == null) {
                enemies.RemoveAt(i);

                i--;
            }
        }

        if(hasEnemies && enemies.Count == 0) {
            for(int i = 0; i < doors.Count; i++) {
                doors[i].SetActive(false);
            }

            hasEnemies = false;

            PlayerController.instance.inBattle = false;

            if(UnityEngine.Random.Range(0,100) < 10) {
                SpawnItem();
            }
        }
    }

    public IEnumerator Initialize() {
        if(generated && !initialized) {
            initialized = true;
            for(int i = 0; i < points.Count; i++) {
                if(!points[i].isConnected) {
                    yield return null;

                    GenerateConnectedPiece(points[i].pos, points[i].direction);
                }
            }
        }
    }

    public void GenerateConnectedPiece(GameObject objectToAttachTo, Direction directionToGenerate) {
        GameObject nextPiece;
        Vector3 posToGen;
        DungeonPiece newPiece;

        float pointPos = 0;

    	switch(directionToGenerate) {
    		case Direction.up:
	    		nextPiece = DungeonGenerator.instance.GetNextPiece(Direction.down);

                while(nextPiece.GetComponent<DungeonPiece>().isRoom && DungeonGenerator.instance.currentRooms >= DungeonGenerator.instance.numberOfRooms) {
                    nextPiece = DungeonGenerator.instance.GetNextPiece(Direction.down);
                }

                for(int i = 0; i < nextPiece.GetComponent<DungeonPiece>().points.Count; i++) {
                    if(nextPiece.GetComponent<DungeonPiece>().points[i].direction == Direction.down) {
                        pointPos = nextPiece.GetComponent<DungeonPiece>().points[i].pos.transform.localPosition.x;
                    }
                }

                if(!isWall) {
                    posToGen = new Vector3(objectToAttachTo.transform.position.x - pointPos, objectToAttachTo.transform.position.y + nextPiece.GetComponent<DungeonPiece>().length/2, 0);
                } else {
                    posToGen = new Vector3(objectToAttachTo.transform.position.x, objectToAttachTo.transform.position.y + nextPiece.GetComponent<DungeonPiece>().length/2, 0);
                }
	    		newPiece = Instantiate(nextPiece, posToGen, Quaternion.identity).GetComponent<DungeonPiece>();

	    		StartCoroutine(Generation(objectToAttachTo, directionToGenerate, Direction.down, newPiece));
	    			
    			break;
    		case Direction.down:
    			nextPiece = DungeonGenerator.instance.GetNextPiece(Direction.up);

                while(nextPiece.GetComponent<DungeonPiece>().isRoom && DungeonGenerator.instance.currentRooms >= DungeonGenerator.instance.numberOfRooms) {
                    nextPiece = DungeonGenerator.instance.GetNextPiece(Direction.up);
                }

                for(int i = 0; i < nextPiece.GetComponent<DungeonPiece>().points.Count; i++) {
                    if(nextPiece.GetComponent<DungeonPiece>().points[i].direction == Direction.up) {
                        pointPos = nextPiece.GetComponent<DungeonPiece>().points[i].pos.transform.localPosition.x;
                    }
                }

                if(!isWall) {
                    posToGen = new Vector3(objectToAttachTo.transform.position.x - pointPos, objectToAttachTo.transform.position.y - nextPiece.GetComponent<DungeonPiece>().length/2, 0);
                } else {
                    posToGen = new Vector3(objectToAttachTo.transform.position.x, objectToAttachTo.transform.position.y - nextPiece.GetComponent<DungeonPiece>().length/2, 0);
                }
    			
    			newPiece = Instantiate(nextPiece, posToGen, Quaternion.identity).GetComponent<DungeonPiece>();

    			StartCoroutine(Generation(objectToAttachTo, directionToGenerate, Direction.up, newPiece));

    			break;
    		case Direction.left:
    			nextPiece = DungeonGenerator.instance.GetNextPiece(Direction.right);

                while(nextPiece.GetComponent<DungeonPiece>().isRoom && DungeonGenerator.instance.currentRooms >= DungeonGenerator.instance.numberOfRooms) {
                    nextPiece = DungeonGenerator.instance.GetNextPiece(Direction.right);
                }

                for(int i = 0; i < nextPiece.GetComponent<DungeonPiece>().points.Count; i++) {
                    if(nextPiece.GetComponent<DungeonPiece>().points[i].direction == Direction.right) {
                        pointPos = nextPiece.GetComponent<DungeonPiece>().points[i].pos.transform.localPosition.y;
                    }
                }

    			if(!isWall) {
                    posToGen = new Vector3(objectToAttachTo.transform.position.x - nextPiece.GetComponent<DungeonPiece>().width/2, objectToAttachTo.transform.position.y - pointPos, 0);
                } else {
                    posToGen = new Vector3(objectToAttachTo.transform.position.x - nextPiece.GetComponent<DungeonPiece>().width/2, objectToAttachTo.transform.position.y, 0);
                }
    			newPiece = Instantiate(nextPiece, posToGen, Quaternion.identity).GetComponent<DungeonPiece>();

    			StartCoroutine(Generation(objectToAttachTo, directionToGenerate, Direction.right, newPiece));

    			break;
    		case Direction.right:
    			nextPiece = DungeonGenerator.instance.GetNextPiece(Direction.left);

                while(nextPiece.GetComponent<DungeonPiece>().isRoom && DungeonGenerator.instance.currentRooms >= DungeonGenerator.instance.numberOfRooms) {
                    nextPiece = DungeonGenerator.instance.GetNextPiece(Direction.left);
                }

                for(int i = 0; i < nextPiece.GetComponent<DungeonPiece>().points.Count; i++) {
                    if(nextPiece.GetComponent<DungeonPiece>().points[i].direction == Direction.left) {
                        pointPos = nextPiece.GetComponent<DungeonPiece>().points[i].pos.transform.localPosition.y;
                    }
                }

    			if(!isWall) {
                    posToGen = new Vector3(objectToAttachTo.transform.position.x + nextPiece.GetComponent<DungeonPiece>().width/2, objectToAttachTo.transform.position.y - pointPos, 0);
                } else {
                    posToGen = new Vector3(objectToAttachTo.transform.position.x + nextPiece.GetComponent<DungeonPiece>().width/2, objectToAttachTo.transform.position.y, 0);
                }
    			newPiece = Instantiate(nextPiece, posToGen, Quaternion.identity).GetComponent<DungeonPiece>(); 

    			StartCoroutine(Generation(objectToAttachTo, directionToGenerate, Direction.left, newPiece));

    			break;
    	}
    }

    public IEnumerator Generation(GameObject objToAttach, Direction dirToGen, Direction dir, DungeonPiece piece) {
            GameObject nextPiece;
            Vector3 posToGen;
            DungeonPiece newPiece = piece;

            float pointPos = 0;

            int iterations = 0;

    	while(iterations < 5000) {
            iterations++;

            yield return null;

    		if(newPiece == null) {
		    	switch(dirToGen) {
			    	case Direction.up:
						nextPiece = DungeonGenerator.instance.GetNextPiece(Direction.down);

                        while(nextPiece.GetComponent<DungeonPiece>().isRoom && DungeonGenerator.instance.currentRooms >= DungeonGenerator.instance.numberOfRooms) {
                            nextPiece = DungeonGenerator.instance.GetNextPiece(Direction.down);
                        }

                        for(int i = 0; i < nextPiece.GetComponent<DungeonPiece>().points.Count; i++) {
                            if(nextPiece.GetComponent<DungeonPiece>().points[i].direction == Direction.down) {
                                pointPos = nextPiece.GetComponent<DungeonPiece>().points[i].pos.transform.localPosition.x;
                            }
                        }

						posToGen = new Vector3(objToAttach.transform.position.x - pointPos, objToAttach.transform.position.y + nextPiece.GetComponent<DungeonPiece>().length/2, 0);
						newPiece = Instantiate(nextPiece, posToGen, Quaternion.identity).GetComponent<DungeonPiece>();
			    		break;
			    	case Direction.down:
			    		nextPiece = DungeonGenerator.instance.GetNextPiece(Direction.up);

                        while(nextPiece.GetComponent<DungeonPiece>().isRoom && DungeonGenerator.instance.currentRooms >= DungeonGenerator.instance.numberOfRooms) {
                            nextPiece = DungeonGenerator.instance.GetNextPiece(Direction.up);
                        }

                        for(int i = 0; i < nextPiece.GetComponent<DungeonPiece>().points.Count; i++) {
                            if(nextPiece.GetComponent<DungeonPiece>().points[i].direction == Direction.up) {
                                pointPos = nextPiece.GetComponent<DungeonPiece>().points[i].pos.transform.localPosition.x;
                            }
                        }

			    		posToGen = new Vector3(objToAttach.transform.position.x - pointPos, objToAttach.transform.position.y - nextPiece.GetComponent<DungeonPiece>().length/2, 0);
			    		newPiece = Instantiate(nextPiece, posToGen, Quaternion.identity).GetComponent<DungeonPiece>();
			    		break;
			    	case Direction.left:
			    		nextPiece = DungeonGenerator.instance.GetNextPiece(Direction.right);

                        while(nextPiece.GetComponent<DungeonPiece>().isRoom && DungeonGenerator.instance.currentRooms >= DungeonGenerator.instance.numberOfRooms) {
                            nextPiece = DungeonGenerator.instance.GetNextPiece(Direction.right);
                        }

                        for(int i = 0; i < nextPiece.GetComponent<DungeonPiece>().points.Count; i++) {
                            if(nextPiece.GetComponent<DungeonPiece>().points[i].direction == Direction.right) {
                                pointPos = nextPiece.GetComponent<DungeonPiece>().points[i].pos.transform.localPosition.y;
                            }
                        }

			    		posToGen = new Vector3(objToAttach.transform.position.x - nextPiece.GetComponent<DungeonPiece>().width/2, objToAttach.transform.position.y - pointPos, 0);
			    		newPiece = Instantiate(nextPiece, posToGen, Quaternion.identity).GetComponent<DungeonPiece>();
			    		break;
			    	case Direction.right:
			    		nextPiece = DungeonGenerator.instance.GetNextPiece(Direction.left);

                        while(nextPiece.GetComponent<DungeonPiece>().isRoom && DungeonGenerator.instance.currentRooms >= DungeonGenerator.instance.numberOfRooms) {
                            nextPiece = DungeonGenerator.instance.GetNextPiece(Direction.left);
                        }

                        for(int i = 0; i < nextPiece.GetComponent<DungeonPiece>().points.Count; i++) {
                            if(nextPiece.GetComponent<DungeonPiece>().points[i].direction == Direction.left) {
                                pointPos = nextPiece.GetComponent<DungeonPiece>().points[i].pos.transform.localPosition.y;
                            }
                        }

			    		posToGen = new Vector3(objToAttach.transform.position.x + nextPiece.GetComponent<DungeonPiece>().width/2, objToAttach.transform.position.y - pointPos, 0);
			    		newPiece = Instantiate(nextPiece, posToGen, Quaternion.identity).GetComponent<DungeonPiece>();
			    		break;
			    }    
		    } else {
                newPiece.GetComponent<DungeonPiece>().generated = true;

                if(newPiece.GetComponent<DungeonPiece>().isRoom) {
                    DungeonGenerator.instance.currentRooms++;
                }

                DungeonGenerator.instance.pieces.Add(newPiece.gameObject);

                for(int i = 0; i < newPiece.points.Count; i++) {
                    if(newPiece.points[i].direction == dir) {
                        newPiece.points[i].isConnected = true;
                    }
                }

                for(int i = 0; i < points.Count; i++) {
                    if(points[i].direction == dirToGen) {
                        points[i].isConnected = true;
                    }
                }

                iterations = 5000;
            }
	    }

	    if(!newPiece.GetComponent<DungeonPiece>().generated) {
	    	Destroy(newPiece.gameObject);
	    }
    }

    public void DetectCollisions() {
        RaycastHit2D[] results = new RaycastHit2D[10];

        if(gameObject.GetComponent<BoxCollider2D>().Cast(new Vector2(0,0), results, 0, true) > 0) {
            for(int i = 0; i < results.Length; i++) {
                if(results[i].collider != null) {
                    if(results[i].collider.isTrigger) {
                        Destroy(gameObject);
                        return;
                    }
                }
            }
        }
    }

    public void SpawnItem() {
        Instantiate(pickupSpawner, transform.position, Quaternion.identity);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            mapPiece.SetActive(true);

            if(hasEnemies) {
                PlayerController.instance.inBattle = true;

                for(int i = 0; i < enemies.Count; i++) {
                    enemies[i].SetActive(true);
                }

                StartCoroutine(CloseDoors());
            }
        }
    }

    public IEnumerator CloseDoors() {
        yield return new WaitForSeconds(0.5f);

        for(int i = 0; i < doors.Count; i++) {
            doors[i].SetActive(true);
        }
    }

    [System.Serializable]
    public class SpawnPoint {
    	public GameObject pos;
    	public Direction direction;
    	public bool isConnected;
    }
}
