using UnityEngine;
public class PlayerMovement : MonoBehaviour {
    private const float SMOOTH_ROT = 1000f;
    private static readonly int SPEED_X = Animator.StringToHash("SpeedX");
    private static readonly int SPEED_Y = Animator.StringToHash("SpeedY");

    public float playerForwardSpeed;
    public float playerSideSpeed;
    //private Animator _animator;
    [SerializeField] private DynamicJoystick _joystick;
    private Vector3 _playerInputMovement;
    private Rigidbody _rbCharacter;
    [SerializeField] private GameManager game;
    [SerializeField] private Camera cam;
    [SerializeField] private int maxX = 3;
    [SerializeField] private int minX = -3;

    void Awake() {
        _rbCharacter = GetComponent<Rigidbody>();
        //_animator = GetComponentInChildren<Animator>();
        //game = GameManager.instance;
    }

    private void Update()
    {
        if (game.state != States.Playing)
        {
            return;
        }
        transform.position -= Vector3.back * playerForwardSpeed * Time.deltaTime;
    }

    protected void FixedUpdate() {
        if (game.state != States.Playing) {
            return;
        }
        this.Move();
    }

    private void Move() {
        if(game.state != States.Playing) 
            return;
        this._playerInputMovement = new Vector3(this._joystick.Horizontal, 0, 0);
        Vector3 movement = cam.transform.TransformDirection(this._playerInputMovement) * this.playerSideSpeed;
        if (this.game.state == States.Playing && movement.magnitude > 0.1f) {
            this._rbCharacter.velocity = new Vector3(movement.x, this._rbCharacter.velocity.y, movement.z);

            //Rotation
            Vector3 targetRot = movement;
            targetRot.y = 0;
        }
        else {
            this._rbCharacter.velocity = new Vector3(0, this._rbCharacter.velocity.y, 0);
        }
        float clampedX = Mathf.Clamp(gameObject.transform.position.x, minX, maxX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);


        //this._animator.SetFloat(SPEED_X, Vector3.Dot(movement, this.transform.right), 0.1f, Time.deltaTime);
        //this._animator.SetFloat(SPEED_Y, Vector3.Dot(movement, this.transform.forward), 0.1f, Time.deltaTime);
    }
}