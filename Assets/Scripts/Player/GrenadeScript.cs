using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour {

    private MeshRenderer _renderer;
    public Vector3 startPosition;
    public float minDistance;
    public float maxDistance;
    private Vector3 offsetPos;
    public  Vector3 targetPosition;
    private Rigidbody _rigidBody;

    [SerializeField] private ParticleSystem _explosionParticle;

    public  float angle;

	// Use this for initialization
	void Start () {
        _renderer = GetComponent<MeshRenderer>();
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.useGravity = false;
        _rigidBody.velocity = Vector3.zero;
        _renderer.enabled = false;
        _explosionParticle = Instantiate(_explosionParticle, transform.position, Quaternion.identity);
        _explosionParticle.Stop();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ResetPosition(Transform playerTransform)
    {

        transform.position = playerTransform.position;
    }

    public void Throw()
    {

        //_renderer.enabled = true;
        //_rigidBody.useGravity = true;
        startPosition = transform.position;
        CalculatePositions();

        //_rigidBody.velocity = BallisticVelocity(targetPosition, angle);
    }

    private void Move()
    {
        
    }

    private void CalculatePositions()
    {
        
        
        Vector3 heading = targetPosition - startPosition;
        float distance = Vector3.Distance(startPosition, targetPosition);

        Debug.Log("DISTANCE = " + distance);
        Debug.Log("MINDISTANCE = " + minDistance);
        Vector3 direction = heading.normalized;
        

        if (distance < minDistance)
        {
            Debug.Log("small");
            distance = minDistance;
            targetPosition = startPosition + (direction * distance);
        } else if (distance > maxDistance)
        {
            Debug.Log("big");
            distance = maxDistance;
            targetPosition = startPosition + (direction * distance);
        }

        Debug.Log("NEWDISTANCE = " + distance);

        _explosionParticle.transform.position = targetPosition;
        _explosionParticle.Play();
    }

    Vector3 BallisticVelocity(Vector3 target, float angle)
    {
        Vector3 dir = target - transform.position; // get Target Direction
        float height = dir.y; // get height difference
        dir.y = 0; // retain only the horizontal difference
        float dist = dir.magnitude + 0.2f; // get horizontal direction
        float a = angle * Mathf.Deg2Rad; // Convert angle to radians

        dir.y = dist * Mathf.Tan(a); // set dir to the elevation angle.
        dist += height / Mathf.Tan(a); // Correction for small height differences

        

        float tmp = dist * Physics.gravity.magnitude / Mathf.Sin(2 * a);

        // Calculate the velocity magnitude
        float velocity = Mathf.Sqrt(tmp);      
        

        return velocity * dir.normalized; // Return a normalized vector.
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.transform.tag.Equals("Ground"))
        {
            _explosionParticle.transform.position = transform.position;
            _explosionParticle.Play();
            _renderer.enabled = false;
            _rigidBody.useGravity = false;
            _rigidBody.velocity = Vector3.zero;
        }
    }
}
