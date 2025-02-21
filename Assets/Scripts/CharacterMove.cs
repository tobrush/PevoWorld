using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float speed;      // ĳ���� ������ ���ǵ�.
    public float jumpSpeedF; // ĳ���� ���� ��.
    public float gravity;    // ĳ���Ϳ��� �ۿ��ϴ� �߷�.

    private CharacterController controller; // ���� ĳ���Ͱ� �������ִ� ĳ���� ��Ʈ�ѷ� �ݶ��̴�.
    private Vector3 MoveDir;                // ĳ������ �����̴� ����.

    void Start()
    {
        speed = 6.0f;
        jumpSpeedF = 8.0f;
        gravity = 20.0f;

        MoveDir = Vector3.zero;
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // ���� ĳ���Ͱ� ���� �ִ°�?
        if (controller.isGrounded)
        {
            // ��, �Ʒ� ������ ����. 
            MoveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            // ���͸� ���� ��ǥ�� ���ؿ��� ���� ��ǥ�� �������� ��ȯ�Ѵ�.
            MoveDir = transform.TransformDirection(MoveDir);

            // ���ǵ� ����.
            MoveDir *= speed;

            // ĳ���� ����
            if (Input.GetButton("Jump"))
                MoveDir.y = jumpSpeedF;

        }

        // ĳ���Ϳ� �߷� ����.
        MoveDir.y -= gravity * Time.deltaTime;

        // ĳ���� ������.
        controller.Move(MoveDir * Time.deltaTime);
    }
}
