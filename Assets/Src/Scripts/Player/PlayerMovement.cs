using UnityEngine;
public class PlayerMovement : MonoBehaviour {
    private const float SMOOTH_ROT = 1000f;
    private static readonly int SPEED_X = Animator.StringToHash("SpeedX");
    private static readonly int SPEED_Y = Animator.StringToHash("SpeedY");
    public float playerSpeed;
    private Animator _animator;
    private VariableJoystick _joystick;
    private Vector3 _playerInputMovement;
    private Rigidbody _rbCharacter;
    private GameManager game;
    private Camera cam;

    void Awake() {
        _rbCharacter = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        VariableJoystick j = game.gameObject.GetComponentInChildren<VariableJoystick>();
        if (j != null) {
            this._joystick = j.GetComponent<VariableJoystick>();
        }
        else {
            Debug.LogError("No Joystick child of Game");
        }
    }

    protected void FixedUpdate() {
        if (game.state != States.Playing) {
            return;
        }
        this.Move();
    }

    private void Move() {
        this._playerInputMovement = new Vector3(this._joystick.Horizontal, 0, this._joystick.Vertical);
        Vector3 movement = cam.transform.TransformDirection(this._playerInputMovement) * this.playerSpeed;
        if (this.game.state == States.Playing && movement.magnitude > 0.1f) {
            this._rbCharacter.velocity = new Vector3(movement.x, this._rbCharacter.velocity.y, movement.z);

            //Rotation
            Vector3 targetRot = movement;
            targetRot.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(targetRot);
            this._rbCharacter.MoveRotation(targetRotation);
        }
        else {
            this._rbCharacter.velocity = new Vector3(0, this._rbCharacter.velocity.y, 0);
        }
        this._animator.SetFloat(SPEED_X, Vector3.Dot(movement, this.transform.right), 0.1f, Time.deltaTime);
        this._animator.SetFloat(SPEED_Y, Vector3.Dot(movement, this.transform.forward), 0.1f, Time.deltaTime);
    }
}