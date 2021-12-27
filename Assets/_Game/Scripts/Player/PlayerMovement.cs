 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] private GameObject effects;
    [SerializeField] private Transform arrowHold;
    [SerializeField] private GameObject indicatorArrow;

    [Header("Line")]
    [SerializeField] private float distanceMax = 7.5f;
    [SerializeField] private Transform startPosLine;
    [SerializeField] private LineRenderer lineRenderer;

    [Header("Velocidade movimento")]
    [SerializeField] private float speed = 5.0f;
    
    [Header("Distancia")]
    [SerializeField] private float distanceMin = 0.5f;


    private Camera myCam;
    private bool isClicked = false;
    private bool onGround = true;
    private Vector2 destinationPositionPoint;
    private Animator anim;
    private bool foundPlatform = false;
    // Start is called before the first frame update
    void Start()
    {
        myCam = Camera.main;
        destinationPositionPoint = transform.position;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("onGround", onGround);

        if (Input.GetButtonDown("Fire1"))
        {
            effects.SetActive(true);
            isClicked = true;
        }

        if (isClicked)
        {
            Vector2 mousePosition = myCam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePosition - (Vector2)transform.position;
            arrowHold.up = direction;

            lineRenderer.SetPosition(0, startPosLine.position);

            RaycastHit2D hit = Physics2D.Raycast(startPosLine.position, startPosLine.up, distanceMax);

            if(hit.collider  != null)
            {
                Platform platform = hit.collider.GetComponent<Platform>();
                if (platform != null)
                {
                    lineRenderer.SetPosition(1, hit.point);
                    destinationPositionPoint = platform.Point(hit.point);
                    indicatorArrow.transform.position = platform.Point(hit.point) - ((Vector2)indicatorArrow.transform.up * 0.25f);
                    indicatorArrow.SetActive(true);
                    foundPlatform = true;
                }
                else
                {
                    lineRenderer.SetPosition(1, hit.point);
                    indicatorArrow.SetActive(false);
                    foundPlatform = false;
                }
                
            }
            else
            {
                lineRenderer.SetPosition(1, startPosLine.position + (startPosLine.up * distanceMax));
                indicatorArrow.SetActive(false);
                foundPlatform = false;
            }

            if (arrowHold.localEulerAngles.z <= 270 && arrowHold.localEulerAngles.z >= 90)
            {
                effects.SetActive(false);
                foundPlatform = false;
            }
            else
                effects.SetActive(true);
            

            if (Input.GetButtonUp("Fire1"))
            {
                if (foundPlatform)
                {
                    Flip(hit.transform.eulerAngles.z);
                    onGround = false;
                }
                foundPlatform = false;
                isClicked = false;
                effects.SetActive(false);
            }
        }
        if (!onGround)
        {
            transform.position = Vector2.Lerp(transform.position, destinationPositionPoint, speed * Time.deltaTime);
        }

        float distance = Vector2.Distance(transform.position, destinationPositionPoint);

        if(distance <= distanceMin && !onGround)
        {
            onGround = true;
            transform.position += transform.up * 0.5f;
        }

    }

    void Flip(float rot)
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, rot));
    }
}
