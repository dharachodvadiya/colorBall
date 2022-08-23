using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public enum enumTag
    {
        Obstacle,
        PathStart,
        PathEnd,
        Path,
        ColorBall
    }
    public AnimationCurve ycurve;
    private Transform ball;
    private Vector3 startMousePos, startBallPos;
    private bool moveTheBall;
    [Range (0,1)] public float maxSpeed;
    [Range (0,1)] public float camSpeed;
    [Range (0,50)] public float pathSpeed;
    private float velocity, camVelocity_x, camVelocity_y;

    private Camera mainCamera;
    public Transform path;
    private Rigidbody rb;
    private Collider _colider;
    private Renderer meshRenderer;

    bool isJump = false;
    float startYpos = 0;
    float timeElepsed = 0;

    Vector3 jumpStartPoint, jumpEndPoint;
    float offset_y = 5f;

    public Spawner effectSpawner;
    public ParticleSystem airParticle;
    public ParticleSystem dustParticle;

    void Start()
    {
        ball = transform;
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        _colider = GetComponent<Collider>();
        meshRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            moveTheBall = true;

            Plane newplane = new Plane(Vector3.up, 0f);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (newplane.Raycast(ray, out var distance))
            {
                startMousePos = ray.GetPoint(distance);
                startBallPos = ball.position;
            }
        } else if (Input.GetMouseButtonUp(0))
        {
            moveTheBall = false;
        }

        if(moveTheBall && MenuManager.instance.gameState)
        {
            Plane newplane = new Plane(Vector3.up, 0f);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            

            if (newplane.Raycast(ray, out var distance))
            {
                //Debug.DrawRay(ray.origin, ray.direction * distance, Color.yellow);

                Vector3 mouseNewPos = ray.GetPoint(distance);
                Vector3 diff = mouseNewPos - startMousePos;
                Vector3 DesireBallPos = startBallPos + diff;

                DesireBallPos.x = Mathf.Clamp(DesireBallPos.x, -1.5f, 1.5f);
                ball.position = new Vector3(Mathf.SmoothDamp(ball.position.x, DesireBallPos.x, ref velocity, maxSpeed)
                    , ball.position.y, ball.position.z);
            }
            else
            {

                //Debug.DrawRay(ray.origin, ray.direction * 10000, Color.red);
            }
        }

        if (MenuManager.instance.gameState)
        {
            var pathNewPos = path.position;
            path.position = new Vector3(pathNewPos.x, pathNewPos.y, Mathf.MoveTowards(pathNewPos.z, pathNewPos.z - pathSpeed, pathSpeed * Time.deltaTime));
        }

        if(isJump)
        {
            float diffZ = jumpEndPoint.z - jumpStartPoint.z;
            timeElepsed += (pathSpeed * Time.deltaTime / diffZ);
            Vector3 pos = ball.position;
            pos.y = startYpos + (ycurve.Evaluate(timeElepsed) * diffZ * 0.1f);

           

            if(timeElepsed >1)
            {
                offset_y = 5;
                isJump = false;
                dustParticle.Play();
                pos.y = startYpos;

                copyRender(meshRenderer, dustParticle.gameObject.GetComponent<Renderer>()) ;
            }

            ball.position = pos;
        }
    }

    public void gameStart()
    {
        airParticle.Play();
    }

    public void gameOver()
    {
        airParticle.Stop();
    }


    private void LateUpdate()
    {
        var currpos = mainCamera.transform.position;
        mainCamera.transform.position = new Vector3(Mathf.SmoothDamp(currpos.x, ball.transform.position.x, ref camVelocity_x, camSpeed)
                    , Mathf.SmoothDamp(currpos.y, ball.transform.position.y + offset_y, ref camVelocity_y, camSpeed)
                    , currpos.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(enumTag.Obstacle.ToString()))
        {
            gameObject.SetActive(false);
            MenuManager.instance.gameState = false;

            gameOver();
            MenuManager.instance.enableGameOverMenu();
        }
        else if (other.CompareTag(enumTag.PathEnd.ToString()))
        {
            Path currPath = other.GetComponentInParent<Path>();
            jumpStartPoint = currPath.pathEndPoint.transform.position;
            jumpEndPoint = currPath.nextPath.pathStartPoint.transform.position;

            isJump = true;
            offset_y = 7;
            timeElepsed = 0;
            startYpos = ball.transform.position.y;

            pathSpeed *= 1.1f;

            pathSpeed = Mathf.Clamp(pathSpeed, 0, 50);

            var airEffectMain = airParticle.main;
            airEffectMain.simulationSpeed += 0.1f;

        }
        else if(other.CompareTag(enumTag.ColorBall.ToString()))
        {

            copyRender(other.GetComponent<Renderer>(),meshRenderer);
            other.gameObject.SetActive(false);

            copyRender(other.GetComponent<Renderer>(), effectSpawner.spawnObject(other.transform.position).GetComponent<Renderer>());
        }
    }


    public void copyRender(Renderer sourceRender, Renderer renderer)
    {
        Color color = sourceRender.material.color;
        renderer.material.SetColor("_BaseColor", color);
        renderer.material.SetColor("_EmissionColor", color * Mathf.LinearToGammaSpace(2f));
    }

}
