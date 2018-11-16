using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simple2DPlayerController : MonoBehaviour {
    public float speed;

	void Update () {
		if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position = (Vector2)transform.position + Vector2.left * speed * Time.deltaTime; 
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position = (Vector2)transform.position + Vector2.right * speed * Time.deltaTime;
        }

    }
}
