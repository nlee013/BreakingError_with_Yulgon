using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public GameObject[] weapons;
    public bool[] hasWeapon;

    public Camera followCamera; // ī�޶� �̵�

    public int coin;
    public int health;

    public int maxCoin;
    public int maxHealth;


    // ���� ����
    float hAxis;
    float vAxis;


    bool wDown; // �ȱ� (Shift)
    bool jDown; // ���� (Space)
    bool iDown; // ������ �ݱ� (E)
    bool fDown; // ����

    // ��� �ٲٱ� ����
    bool sDown1;
    bool sDown2;

    // ���� ���ϱ� ����
    bool isJump;
    bool isDodge;

    bool isSwap; // ��ü �ð���
    bool isFireReady = true; // �غ�� ���� Ȯ��

    Vector3 moveVec; // ������ ����
    Vector3 dodgeVec; // ȸ�� ���� ������ȯ�� ���� �ʵ��� ȸ�ǹ���

    Rigidbody rigid;
    Animator anim; // �ִϸ����� ����

    GameObject nearObject; // ������ �Ա�
    Weapon equipWeapon; // �������� ����
    int equipWeaponIndex = -1; // �������� ���� �ε���
    float fireDelay; // ���� ������

    

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        followCamera = FindObjectOfType<Camera>();
    }


    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Attack();
        Dodge();
        Swap();
        Interation();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
        fDown = Input.GetButtonDown("Fire1");
        iDown = Input.GetButtonDown("Interation");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized; // ������ ����

        if (isDodge) // ȸ�����̸� ���غ��͸� �־���
            moveVec = dodgeVec;

        if (isSwap || !isFireReady) // ���� �������̰ų� �̵��߿� �̵��Ұ�
            moveVec = Vector3.zero;

        // �ȱ� 0.3f, �޸��� 1f �ӵ� ����
        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;

        anim.SetBool("IsRun", moveVec != Vector3.zero);
        anim.SetBool("IsWalk", wDown);
    }

    void Turn()
    {
        // ���ư��� ���� �ٶ󺸱�
        transform.LookAt(transform.position + moveVec);

        // ���콺�� ���� ȸ��
        if(fDown)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position; // ����� ��ġ
                nextVec.y = 0; // ���ǰ� ������ ���� ������
                transform.LookAt(transform.position + nextVec); // ���� ����
            }
        }       
    }

    void Jump()
    {
        if(jDown && moveVec == Vector3.zero && !isJump && !isDodge && !isSwap)
        {
            rigid.AddForce(Vector3.up * 20, ForceMode.Impulse);
            anim.SetBool("IsJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }        
    }

    void Attack()
    {
        if (equipWeapon == null)
            return;

        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if(fDown && isFireReady && !isDodge && !isSwap)
        {
            equipWeapon.Use();
            anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
            fireDelay = 0;
        }
    }
    void Dodge() // ���� �浹 �ƴ�
    {
        if (jDown && moveVec != Vector3.zero && !isJump && !isDodge && !isSwap)
        {
            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;

            Invoke("DodgeOut", 0.5f); // �ð���
        }
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;        
    }

    void Swap()
    {
        if (sDown1 && (!hasWeapon[0] || equipWeaponIndex == 0))
            return;
        if (sDown2 && (!hasWeapon[1] || equipWeaponIndex == 1))
            return;

        int weaponIndex = -1;
        if(sDown1) weaponIndex = 0;
        if(sDown2) weaponIndex = 1;

        if ((sDown1 || sDown2) && !isJump && !isDodge)
        {
            if(equipWeapon != null)
                equipWeapon.gameObject.SetActive(false);

            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);

            anim.SetTrigger("doSwap");

            isSwap = true;

            Invoke("SwapOut", 0.4f); // �ð���
        }
    }

    void SwapOut()
    {
        isSwap = false;
    }

    void Interation()
    {
        if(iDown && nearObject != null && !isJump && !isDodge)
        {
            if(nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapon[weaponIndex] = true;

                Destroy(nearObject);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            anim.SetBool("IsJump", false);
            isJump = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon")
            nearObject = other.gameObject;

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
            nearObject = null;
    }


}
