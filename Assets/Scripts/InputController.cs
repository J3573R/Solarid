using UnityEngine;

public class InputController : MonoBehaviour
{

    private Rigidbody _rigidbody;
    private bool _targeting;
    private PlayerDash _dash;
    private Camera _camera;


    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _camera = FindObjectOfType<Camera>();
        _dash = GetComponent<PlayerDash>();
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
        _rigidbody.velocity = new Vector3(horizontalDirection * 10f, 0f, verticalDirection * 10f);

        if (Input.GetButtonDown("Ability1"))
            _dash.Targeting();
         
        if (Input.GetButtonDown("Fire1")) {
            _dash.Dash();

            


            _targeting = false;
        }



        
        

        







    }
   
}



