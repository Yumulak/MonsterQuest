using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    //credits: https://www.youtube.com/watch?v=tMXgLBwtsvI

    public Camera cam;
    public Transform followTarget;

    //starting position for the parallax game object
    Vector2 startingPosition;

    //starting z value of the parallax game object 
    float startingZ;

    //distance that the camera has moved from the starting position of the parallax object
    Vector2 camMoveSinceStart => (Vector2)cam.transform.position - startingPosition;
    
    float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z;

    //if object is in front of target, use near clip plane. if behind target, use far clip plane
    float clippingPlane => (cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));

    //the further the object from the player, the faster the parallax effect object will move. drag its z value closer to the target to make it move slower
    float parallaxFactor => Mathf.Abs(zDistanceFromTarget / clippingPlane);

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        //when the target moves, move the parallax object the same distance times a multiplier
        Vector2 newPosition = startingPosition + camMoveSinceStart * parallaxFactor;

        //the x/y position changes based on target travel speed times the parallax factor, but z stays consistent
        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);


    }
}
