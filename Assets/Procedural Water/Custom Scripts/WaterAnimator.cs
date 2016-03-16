using UnityEngine;
using System.Collections;

public class WaterAnimator : MonoBehaviour {

    MapGenerator water;

    public Vector2 Speed;

	// Use this for initialization
	void Start () {

        water = GetComponent<MapGenerator>();
	
	}
	
	// Update is called once per frame
	void Update () {
        water.offset += Speed;
        water.GenerateMap();
	}
}
