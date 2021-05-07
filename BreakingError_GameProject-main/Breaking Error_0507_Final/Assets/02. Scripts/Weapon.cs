using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type
    {
        Melee, // ���� ����
        Range // ���Ÿ� ����
    }
    public Type type;
    public int damage;
    public float rate; // ���� �ӵ�
    public BoxCollider meleeArea; // �����Ÿ�
    public TrailRenderer trailEffect; // ������� ����Ʈ
        
    public Transform bulletPos; // �Ѿ� ��ġ
    public Transform bulletCasePos; // ź�� ���� ��ġ
    public GameObject bullet; // �Ѿ� ������
    public GameObject bulletCase; // ź�� ������

    public void Use()
    {
        if(type == Type.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
        else if (type == Type.Range)
        {
            StartCoroutine("Shot");
        }
    }

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(1f);
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        yield return new WaitForSeconds(1f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(1f);
        trailEffect.enabled = false;
    }

    IEnumerator Shot()
    {
        // #1. �߻�
        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 100;

        yield return null;
        // #2. ź�� ����
        GameObject intantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody caseRigid  = intantCase.GetComponent<Rigidbody>();
        
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        caseRigid.AddForce(caseVec, ForceMode.Impulse);
        caseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse); // ź�� ȸ�� (Impulse : ���)
    }
  

}
