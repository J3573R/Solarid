using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour {

    public int Damage = 30;
    public int DamageRadius = 1;
    public Vector3 targetPosition;
    public Vector3 startPosition;
    public float minDistance;
    public float maxDistance;

    private MeshRenderer _renderer;    
    private Vector3 offsetPos;    
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
        gameObject.SetActive(false);
	}	

    /// <summary>
    /// Resets the grenade position
    /// </summary>
    /// <param name="playerTransform"></param>
    public void ResetPosition(Transform playerTransform)
    {
        transform.position = playerTransform.position;
    }

    /// <summary>
    /// Runs the necessary calculations and throws the grenade
    /// </summary>
    public void Throw()
    {
        gameObject.SetActive(true);
        _renderer.enabled = true;
        _rigidBody.useGravity = true;
        startPosition = transform.position;
        CalculatePositions();        
        _rigidBody.velocity = BallisticVelocity(targetPosition, angle);
    }

    /// <summary>
    /// Normalizes the tarrgetposition to be between min and max distance
    /// </summary>
    private void CalculatePositions()
    {
        float originalY = targetPosition.y;
        targetPosition.y = 1;        
        Vector3 heading = targetPosition - startPosition;        
        float distance = Vector3.Distance(startPosition, targetPosition);
        Vector3 direction = heading.normalized;

        targetPosition = startPosition + (direction * distance);
        
        if (distance < minDistance)
        {
            distance = minDistance;
            targetPosition = startPosition + (direction * distance);
        } else if (distance > maxDistance)
        {
            distance = maxDistance;
            float tmpY = targetPosition.y;

            targetPosition = startPosition + (direction * distance);
        }

        targetPosition.y = originalY;                 
    }

    /// <summary>
    /// Calculates the velocity for the grenade
    /// </summary>
    /// <param name="target">Target position for the grenade</param>
    /// <param name="angle">Angle, in which the grenade will be thrown</param>
    /// <returns>Correct velocity</returns>
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

    /// <summary>
    /// Does stuff if the grenade hits ground
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.transform.tag.Equals("Ground") || other.transform.tag.Equals("Enemy"))
        {
            _explosionParticle.transform.position = transform.position;
            _explosionParticle.Play();
            _renderer.enabled = false;
            _rigidBody.useGravity = false;
            _rigidBody.velocity = Vector3.zero;
            OnExplode();
        }
    }

    private void OnExplode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, DamageRadius);
        foreach(Collider c in colliders)
        {
            if(c.tag.Equals("Enemy"))
            {
                EnemyBase e = c.gameObject.GetComponent<EnemyBase>();
                e.TakeDamage(Damage);
            }
            
        }
    }
}
