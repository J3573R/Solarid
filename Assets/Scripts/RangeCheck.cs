using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeCheck : MonoBehaviour {

    private MeshRenderer _renderer;
    private Player _player;
    public float PositionCheckInterval;

	// Use this for initialization
	void Start () {
        _renderer = GetComponent<MeshRenderer>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _renderer.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 tmp = _player.transform.position;
        tmp.y = 0f;
        transform.position = tmp;
	}

    /// <summary>
    /// Changes the indicator size to match max range and enables or disables the indicator
    /// </summary>
    /// <param name="range">Max range of the skill</param>
    /// <param name="draw">enable if true, disable if false</param>
    public void DrawRange(float range, bool draw)
    {
        Vector3 tmp = new Vector3(range * 2, 0.1f, range * 2);
        transform.localScale = tmp;

        if (draw)
        {
            _renderer.enabled = true;
        } else
        {
            _renderer.enabled = false;
        }
    }

    /// <summary>
    /// Gets the distance between player and cast point
    /// </summary>
    /// <returns></returns>
    public float GetDistance()
    {
        Vector3 tmpVec = transform.position;
        Vector3 tmpMouse = _player.Input.GetMouseGroundPosition();
        //Debug.Log(tmpMouse);
        tmpVec.y = tmpMouse.y;

        return Vector3.Distance(tmpMouse, transform.position);
    }

    public Vector3 GetMaxRangePosition(float maxRange)
    {
        var layerMask = 1 << 8;
        layerMask = ~layerMask;
        Vector3 start = transform.position;
        Vector3 target = _player.Input.GetMousePosition();       
        start.y = target.y;
        
        Vector3 direction = (target - start).normalized;
        RaycastHit hit;
        Vector3 tmpPos = start + (direction * maxRange);
        Ray ray = new Ray(tmpPos, Vector3.down);
        bool areaClear = true;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.transform.tag.Equals("Ground"))
            {
                Collider[] list = Physics.OverlapSphere(transform.position, 1);

                foreach(Collider col in list)
                {
                    if (col.transform.tag.Equals("Prop"))
                    {
                        areaClear = false;
                        Debug.Log("Löyty proppi");
                    }
                }
                if (areaClear)
                    return hit.point;
            }
            else
                return Vector3.zero;
        }
        return Vector3.zero;
        
    }

    /// <summary>
    /// This beast of a horrible code looks for a suitable position for abilities. It checks regular intervals and looks for 
    /// props on certain radius. If changed, careful not to create performance issues
    /// </summary>
    /// <param name="maxRange">Max range of the ability</param>
    /// <returns>Suitable position</returns>
    public Vector3 GetNextSuitablePosition(float maxRange)
    {
        Vector3 target = _player.Input.GetMousePosition();
        Vector3 start = transform.position;
        var layerMask = 1 << 8;
        layerMask = ~layerMask;
        start.y = target.y;
        float distance = Vector3.Distance(start, target);
        bool areaClear = true;

        if (distance > maxRange)
        {
            Debug.Log("Too Long");
            distance = maxRange;
        } 
        
        Vector3 direction = (target - start).normalized;
        RaycastHit hit;

        while (distance > 0)
        {
            distance -= PositionCheckInterval;
            Vector3 tmpPos = start + (direction * distance);
            Ray ray = new Ray(tmpPos, Vector3.down);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
                if (hit.transform.tag.Equals("Ground")) {
                    Collider[] list = Physics.OverlapSphere(hit.point, 0.7f);
                    areaClear = true;
                    foreach (Collider col in list)
                    {
                        if (col.transform.tag.Equals("Prop"))
                        {
                            areaClear = false;                            
                            break;
                        }
                    }

                    if (areaClear)
                        return hit.point;
                }
            }            
        }

        return Vector3.zero;
    }

}
