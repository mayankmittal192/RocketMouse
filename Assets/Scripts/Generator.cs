using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class Generator : MonoBehaviour
{
    
    private RoomWrapper nearestRoom;
    private RoomWrapper farthestRoom;
    private ObjectWrapper nearestObject;
    private ObjectWrapper farthestObject;
    private float screenWidthInPoints;

    public GameObject[] availableRooms;
    public List<GameObject> currentRooms;
    public GameObject[] availableObjects;
    public List<GameObject> currentObjects;
    public float objectsMinDistance = 5.0f;
    public float objectsMaxDistance = 10.0f;


    void Start()
    {
        float height = 2.0f * Camera.main.orthographicSize;
        screenWidthInPoints = height * Camera.main.aspect;

        if (currentRooms == null)
        {
            currentRooms = new List<GameObject>();
            AddRoom();
        }

        if (currentObjects == null)
        {
            currentObjects = new List<GameObject>();
            AddObject();
        }

        currentRooms = currentRooms.OrderBy(room => room.transform.position.x).ToList();
        currentObjects = currentObjects.OrderBy(room => room.transform.position.x).ToList();

        nearestRoom = new RoomWrapper(currentRooms[0]);
        farthestRoom = new RoomWrapper(currentRooms[currentRooms.Count - 1]);

        nearestObject = new ObjectWrapper(currentObjects[0]);
        farthestObject = new ObjectWrapper(currentObjects[currentObjects.Count - 1]);
    }


    void FixedUpdate()
    {
        float playerX = transform.position.x;
        float removeX = playerX - screenWidthInPoints;
        float addX = playerX + screenWidthInPoints;

        HandleRoomGeneration(removeX, addX);
        HandleObjectGeneration(removeX, addX);
    }


    void HandleRoomGeneration(float removeX, float addX)
    {
        if (nearestRoom.EndX < removeX)
        {
            RemoveRoom();
            nearestRoom.SwitchTo(currentRooms[0]);
        }

        if (farthestRoom.StartX < addX)
        {
            AddRoom();
            farthestRoom.SwitchTo(currentRooms[currentRooms.Count - 1]);
        }
    }


    void HandleObjectGeneration(float removeX, float addX)
    {
        if (nearestObject.PositionX < removeX)
        {
            RemoveObject();
            nearestObject.SwitchTo(currentObjects[0]);
        }

        if (farthestObject.PositionX < addX)
        {
            AddObject();
            farthestObject.SwitchTo(currentObjects[currentObjects.Count - 1]);
        }
    }


    void RemoveRoom()
    {
        GameObject roomGameObject = nearestRoom.gameObject;
        currentRooms.Remove(roomGameObject);
        Destroy(roomGameObject);
    }


    void AddRoom()
    {
        int randomRoomIndex = Random.Range(0, availableRooms.Length);
        GameObject roomGameObject = Instantiate(availableRooms[randomRoomIndex]);
        float roomCenterX = 0;

        if (farthestRoom != null)
        {
            float roomWidth = roomGameObject.transform.FindChild("Floor").localScale.x;
            roomCenterX = farthestRoom.EndX + (roomWidth * 0.5f);
        }

        roomGameObject.transform.position = new Vector3(roomCenterX, 0, 0);
        roomGameObject.transform.rotation = Quaternion.identity;
        currentRooms.Add(roomGameObject);
    }


    void RemoveObject()
    {
        GameObject objGameObject = nearestObject.gameObject;
        currentObjects.Remove(objGameObject);
        Destroy(objGameObject);
    }


    void AddObject()
    {
        int randomIndex = Random.Range(0, availableObjects.Length);
        GameObject obj = Instantiate(availableObjects[randomIndex]);
        float objectPositionX = 0;

        if (farthestObject != null)
        {
            objectPositionX = farthestObject.PositionX + Random.Range(objectsMinDistance, objectsMaxDistance);
        }
        
        obj.transform.position = new Vector3(objectPositionX, 0, 0);
        obj.transform.rotation = Quaternion.identity;
        currentObjects.Add(obj);
    }

}
