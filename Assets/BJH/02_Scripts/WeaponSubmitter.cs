using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSubmitter : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform putdownTr; // 어디 내려놓을지 위치 잡아서 아래 코드로 assign
    [SerializeField] private List<GameObject> weaponPrefabs; // PhotonNetwork.Instantiate로 무기를 찾으니 프리팹들 말고 미리 지정된 이름을 삽입하는건 어떨지 제시 -변지환- 리팩토링 요소
    private void Start()
    {
        WeaponManager.Instance.weaponSubmitter = this; // 자기 자신을 코드로 assign

        weaponPrefabs = new List<GameObject>(); // 초기화

        putdownTr = transform.Find("SubmitWeaponPosition").transform; // 성능 문제때문에 놓을 위치를 전체에서 찾지 않고 자식 리그에 담아서 자식으로 찾음
    }

    // 내려 놓는 메서드
    // 버튼을 누르면 매니저에서 해당 메서드를 호출 할 것
    // 지정된 위치에 선택된 gameobject를 내려놓음
    // 이건 동기화 되어야 하는가.. yes.. 그럼 단순한 전달 해야됨
    // 여기서 weapon prefab을 모두 담고 있다가 string이랑 일치하는 무기를 내려놓기 해야되고
    // rpc로 호출 해야됨
    // 마침 플레이어 한테 달려있으니 그 photonview 사용하면 될 듯
    // 여기서 pun 달게 아니라 얘를 호출하는 초기 메서드에 pun을 달아야
    
    public void PutDownWeaponOnTr(string weaponName)
    {
        Debug.Log("weaponsubmitter입니다. 무기 내려놓겠삼. 받은 무기는" + weaponName);
        foreach(GameObject weapon in weaponPrefabs)
        {
            if(weapon.name == weaponName)
            {
                PhotonNetwork.Instantiate(weaponName, putdownTr.position, Quaternion.identity);
            }
        }

        WeaponManager.Instance.ChangeActice_InventoryImage();

        PhotonView pv = ChatManager.instance.GetComponent<PhotonView>();
        pv.RPC("SetWeapon", RpcTarget.All, weaponName);
        //pv.RPC("StartTalkinterrogation", RpcTarget.MasterClient);
        ChatManager.instance.StartTalkinterrogation();
    }
}
