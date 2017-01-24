using UnityEngine;

public class InputController : MonoBehaviour
{

    private Rigidbody _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 playerPosOnScreen = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mousePos = Input.mousePosition;

        mousePos.x -= Screen.width / 2;
        mousePos.y -= Screen.height / 2;

        mousePos += transform.position;

        float angle = Vector3.Angle(-mousePos, Vector3.up);
        //float angle = Vector3.Angle(playerPosOnScreen, -mousePos );

        if (mousePos.x > 0)
            angle = 360 - angle;

        transform.rotation = Quaternion.Euler(0, angle + 180, 0);

  
        var horizontalDirection = Input.GetAxis("Horizontal");
        var verticalDirection = Input.GetAxis("Vertical");
        //transform.Translate(horizontalDirection * 10f *Time.deltaTime, 0f, verticalDirection * 10f * Time.deltaTime);
        _rigidbody.velocity = new Vector3(horizontalDirection * 5f, 0f, verticalDirection * 5f);
 
    }
   
}



