using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using static ConnectionKJY;

public class GameManager_KJY : MonoBehaviourPun
{
    public static GameManager_KJY instance;

    public List<GameObject> npcList;
    public List<GameObject> dieNpcList;

    public bool click;
    // 캐릭터 선택을 하면? 캐릭터 선택하기 버튼
    Ray ray;
    RaycastHit hit;
    Camera cam;

    GameObject obj;
    
    CameraTopDown cameratopdown;

    // 캐릭터 선택을 하지 않으면? 스킵 버튼
    [SerializeField] GameObject skipBtn;
    [SerializeField] Image image;
    // 마피아가 죽일까요 말까요
    [SerializeField] GameObject selectUI;
    Quaternion rotation;

    [SerializeField] GameObject selectBtn;

    [SerializeField] GameObject selectInBtn;
    
    [SerializeField] GameObject checkUI;

    [SerializeField] GameObject DayText;

    [Header("Camera")]
    //회전 스피드 값
    public float speed = 10;
    //카메라 줌인 줌아웃 값
    float maxZoom = 30;
    float curr = 40;
    bool isZoom;
    
    public GameObject detectiveCamera;
    public GameObject assistantCamera;

    [SerializeField] Transform camPlace;
    [SerializeField] GameObject effect;
    [SerializeField] Transform sun;
    Quaternion firstSun;
    
    PostProcessProfile profile;
    ColorGrading cg;

    int number;

    [SerializeField] GameObject notifyDead;
    
    [Header("IntellRoom")]
    [SerializeField] private Transform intellRoomNpc;
    [SerializeField] private Transform intellCameraSpot;
    
    [Header("GoIntellRoom")]
    [SerializeField] private Transform startSpot;
    [SerializeField] private Transform endSpot;
    [SerializeField] private Transform detectiveSpot;
    [SerializeField] private Transform assistantSpot;
    [SerializeField] private Transform arrestNpcSpot;
    [SerializeField] private Transform arrestCameraSpot;


    bool Winner;
    public int heartRate = 60;
    public bool isSeletNpc = false;

    private void Awake()
    {
        instance = this;
        DOTween.Init();
    }

    private void Start()
    {
        cam = Camera.main;
        click = false;
        npcList = KJY_CitizenManager.Instance.npcList;
        dieNpcList = KJY_CitizenManager.Instance.dieNpcList;
        selectUI.SetActive(false);
        cameratopdown = cam.GetComponent<CameraTopDown>();
        PhotonConnection.Instance.CameraOnOff(false);
        checkUI.SetActive(false);
        firstSun = sun.transform.rotation;
        effect.SetActive(false);
        profile = effect.GetComponent<PostProcessVolume>().profile;
        profile.TryGetSettings<ColorGrading>(out cg);
        cg.enabled.value = false;
        Winner = false;
        isZoom = false;
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (KJY_CitizenManager.Instance.call == true)
            {
                OnOffDetectiveCamera(false);
                PhotonConnection.Instance.CameraOnOff(true);
                ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (Input.GetMouseButtonDown(0) && click == false)
                    {
                        if (hit.collider.CompareTag("Npc") || hit.collider.CompareTag("NpcDummy"))
                        {
                            isSeletNpc = true;
                            PhotonView view =  hit.collider.GetComponent<PhotonView>();
                            int viewID = view.ViewID;
                            PhotonConnection.Instance.UpdateChooseNpc(viewID);
                        }
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F8))
        {
            interrogationBtn(true);
        }
    }

    [PunRPC]
    private void ChooseNpc(int viewID)
    {
        PhotonView view = PhotonView.Find(viewID);
        obj = view.gameObject;
        rotation = obj.transform.rotation;
        OnOffUI(false);
        skipBtn.SetActive(false);
        selectBtn.SetActive(true);
        selectUI.SetActive(true);
        FollowNpc(obj);
        isSeletNpc = false;
    }

    void FollowNpc(GameObject gameObject)
    {
        KJY_CitizenManager.Instance.OnOffCanvas(gameObject, false);
        cameratopdown.enabled = false;
        click = true;
        isZoom = true;
        Vector3 target = gameObject.transform.position - cam.transform.position;
        StartCoroutine(RotateVectorCoroutine(target, cam.transform, true));
        StartCoroutine(RotateVectorCoroutine(-target, gameObject.transform, false));
        StartCoroutine(ChangeZoom(maxZoom));
    }

    //부드럽게 회전시켜주는 함수
    IEnumerator RotateVectorCoroutine(Vector3 target, Transform tr, bool isCam)
    {
        while (true)
        {
            tr.transform.rotation = Quaternion.Lerp(tr.transform.rotation, Quaternion.LookRotation(target), Time.deltaTime * speed);
            if (isCam == false)
            {
                Quaternion tmp = tr.transform.rotation;
                tmp.x = 0;
                tr.transform.rotation = tmp;
            }
            // 거리가 stopDistance 이하이면 코드 중단
            if (AreQuaternionsSimilar(tr.transform.rotation, Quaternion.LookRotation(target)) || cameratopdown.enabled == true)
            {
                yield break; // 코루틴 중단
            }

            yield return null;
        }
    }

    //빛 원래대로 돌련놓기
    IEnumerator RotateReset(Quaternion target, Transform tr)
    {
        while (true)
        {
            tr.rotation = Quaternion.Lerp(tr.rotation, target, Time.deltaTime * speed);

            if (AreQuaternionsSimilar(tr.transform.rotation, target))
            {
                yield break; // 코루틴 중단
            }

            yield return null;
        }
    }

    //해 회전시키기
    IEnumerator RotateSun(Quaternion target, Transform tr)
    {
        while (true)
        {
            tr.rotation = Quaternion.Lerp(tr.rotation, target, Time.deltaTime * speed * 0.04f);

            if (AreQuaternionsSimilar(tr.transform.rotation, target, 0.3f))
            {
                yield break; // 코루틴 중단
            }

            yield return null;
        }
    }


    //각도 계산 함수
    bool AreQuaternionsSimilar(Quaternion a, Quaternion b, float tolerance = 0.001f)
    {
        float angleDiff = Quaternion.Angle(a, b);

        return Mathf.Abs(angleDiff) < tolerance;
    }

    //각도 계산 함수
    bool AreQuaternionsSimilarInObj(Quaternion a, Quaternion b, float tolerance = 0.001f)
    {
        a.x = 0;
        b.x = 0;

        float angleDiff = Quaternion.Angle(a, b);

        return Mathf.Abs(angleDiff) < tolerance;
    }

    //줌 바꾸기
    IEnumerator ChangeZoom(float target)
    {
        while (true)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, target, Time.deltaTime * speed);
            if (AreValuSimilar(cam.fieldOfView, target))
            {
                yield break;
            }
            if ((target == maxZoom && isZoom == false) || (target == curr && isZoom == true))
            {
                yield break;
            }
            yield return null;
        }
    }

    //절대값으로했을 때 유사한지
    bool AreValuSimilar(float a, float b, float tolerance = 0.005f)
    {
        float valueDiff = a - b;

        return Mathf.Abs(valueDiff) < tolerance;
    }

    //확인 버튼
    [PunRPC]
    public void SelectNpc()
    {
        InfoManagerKJY.instance.voteNightNumber = UI.instance.dayInt;
        InfoManagerKJY.instance.voteNpcName = obj.GetComponent<NpcData>().npcName;
        interrogationBtn(false);
        StartCoroutine(SelectEffect());
        PhotonConnection.Instance.ResetDummyNpc();
    }

    //취소 버튼
    [PunRPC]
    public void CancleSeletNpc()
    {
        KJY_CitizenManager.Instance.OnOffCanvas(obj, true);
        skipBtn.SetActive(true);
        selectBtn.SetActive(false);
        selectUI.SetActive(false);
        OnOffUI(true);
        click = false;
        cameratopdown.enabled = true;
        isZoom = false;
        StartCoroutine(RotateReset(rotation, obj.transform));
        StartCoroutine(RotateReset(cameratopdown.first, cam.transform));
        StartCoroutine(ChangeZoom(curr));
        obj = null;
    }

    //다시초기화
    void ResetSelect()
    {
        skipBtn.SetActive(false);
        selectBtn.SetActive(false);
        selectUI.SetActive(false);
        click = false;
        cameratopdown.enabled = true;
        isZoom = false;
        StartCoroutine(RotateReset(cameratopdown.first, cam.transform));
        StartCoroutine(ChangeZoom(curr));
    }


    //스킵할 때 결정하는 것
    [PunRPC]
    public void AfterSkip()
    {
        InfoManagerKJY.instance.voteNightNumber = UI.instance.dayInt;
        InfoManagerKJY.instance.voteNpcName = null;
        if (obj != null)
        {
            PhotonConnection.Instance.ResetDummyNpc();
        }
        isSeletNpc = false;
        interrogationBtn(false);
        StartCoroutine(EffectNight());
        
    }

    //스킵할 때 결정하는 것
    IEnumerator EffectNight()
    {
        image.DOFade(1, 1f);
        InfoManagerKJY.instance.voteNightNumber = 0;
        InfoManagerKJY.instance.voteNpcName = null;
        UI.instance.DeathInfo();

        RequestSave requst = new RequestSave();
        requst.Request(false);

        yield return new WaitForSeconds(2f);

        TurnNpcList(false, 2);
        StartCoroutine(UIReset());
    }

    public void CheckMafiaUI(string answer)
    {
        TextMeshProUGUI text = checkUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        SetDummyDataAndNpcDataEquel(obj.GetComponent<NpcData>().npcName, true);
        if (answer == "FOUND")
        {
            //text.text = obj.GetComponent<NpcData>().npcName + "은(는) 마피아였습니다.";
            InfoManagerKJY.instance.voteResult = "FOUND";
            InfoManagerKJY.instance.voteNpcObjectName = obj.name;
            InfoManagerKJY.instance.npcListInfo = npcList;
            InfoManagerKJY.instance.dieNpcListInfo = dieNpcList;
            PhotonConnection.Instance.VictoryGo();
            Winner = true;
        }
        else
        {
            //text.text = obj.GetComponent<NpcData>().npcName + "은(는) 시민이였습니다.";
            InfoManagerKJY.instance.voteResult = "NOTFOUND";
        }
        checkUI.SetActive(true);
    }

    //랜덤으로 주민죽이기
    public void DieResidentSet()
    {
        StartCoroutine(VictimSet());
    }

    IEnumerator VictimSet()
    {
        yield return new WaitForSeconds(2f);
        SetDummyDataAndNpcDataEquel(InfoManagerKJY.instance.victim, false);
        TurnNpcList(false, 3);
        KJY_CitizenManager.Instance.SetnpcSpot(false);
        yield return new WaitForSeconds(1f);
        obj = null;
        NightToMorining();
    }

    IEnumerator SelectEffect()
    {
        image.DOFade(1, 1f);
        //cameratopdown.enabled = true;
        interrogationBtn(false);
        RequestSave requst = new RequestSave();
        requst.Request(true);
        yield return new WaitForSeconds(2f);
        TurnNpcList(false, 2);
        if (obj != null)
        {
            obj.SetActive(false);
        }
        if (Winner == false)
        {
            if (UI.instance.dayInt == 5)
            {
                ConnectionKJY.instance.RequestGameEnd();
            }
            else
            {
                StartCoroutine(UIReset());
            }
        }
        else
        {
            //KJYKJYKJY
            //KJY_SceneManager.instance.ChangeWinScene();
        }
    }

    //하루 넘어가서 UI reset시키는 것 
    public IEnumerator UIReset()
    {
        yield return new WaitForSeconds(2);
        OnOffUI(false);
        checkUI.SetActive(false);
        ResetSelect();
        //UI.instance.DayAndNight(true);
        UI.instance.InitLife();
        UI.instance.UpdateDay();
        UI.instance.deathInfoImg.SetActive(false);
        yield return new WaitForSeconds(1);
        //리셋 시키는 것
        image.DOFade(0, 1f);
        KJY_CitizenManager.Instance.npcSpots.SetActive(false);
        ToNight();
    }

    public void ToNight()
    {
        KJY_SenarioConnection.instance.ReqeustScenario();
        cameratopdown.enabled = false;
        effect.SetActive(true);
        cam.transform.position = camPlace.transform.position;
        cam.transform.rotation = camPlace.transform.rotation;
        cg.enabled.value = true;
        cg.postExposure.value = -6;
        DayText.SetActive(true);
    }

    //밤에서 낮 됨
    public void NightToMorining()
    {
        Vector3 rotationInEulerAngles = sun.rotation.eulerAngles;
        rotationInEulerAngles.x = 190;
        Quaternion newRotation = Quaternion.Euler(rotationInEulerAngles);
        sun.rotation = newRotation;
        DayText.SetActive(false);

        //낮되는 연출
        StartCoroutine(postChange(-6, 0));
        StartCoroutine(RotateSun(firstSun, sun.transform));
    }

    IEnumerator postChange(int start, int end)
    {
        cg.enabled.value = true;
        cg.postExposure.value = start;
        while (true)
        {
            cg.postExposure.value = Mathf.Lerp(cg.postExposure.value, end, Time.deltaTime * speed * 0.05f);
            if (AreValuSimilar(cg.postExposure.value, end, 0.3f))
            {
                foundCorpse();
                break;
            }
            yield return null;
        }
    }

    public void TurnNpcList(bool value, int who)
    {
        GameObject lastDieNpc = dieNpcList[dieNpcList.Count - 1];
        if (InfoManagerKJY.instance.role == "Detective")
        {
            photonView.RPC("OnOffDetectiveShape", RpcTarget.All, value);
        }
        else
        {
            photonView.RPC("OnOffAssistantShape", RpcTarget.All, value);
        }

        if (who == 3)
        {
            for (int i = 0; i < npcList.Count; i++)
            {
                npcList[i].GetComponent<KJY_NPCHighlight>().SetActiveMaterial(value);
            }
            lastDieNpc.SetActive(value);
            lastDieNpc.GetComponent<KJY_NPCHighlight>().SetActiveMaterial(value);
        }
        else if (who == 1)
        {
            for (int i = 0; i < npcList.Count; i++)
            {
                npcList[i].GetComponent<KJY_NPCHighlight>().SetActiveMaterial(value);
            }
        }
        else
        {
            lastDieNpc.SetActive(value);
            lastDieNpc.GetComponent<KJY_NPCHighlight>().SetActiveMaterial(value);
            dieNpcList[dieNpcList.Count - 1].GetComponent<NPC>().Die();
        }
    }

    //시체 발견
    private void foundCorpse()
    {
        KJY_CitizenManager.Instance.DieNpcSpotSet();
        TurnNpcList(true, 2);
        dieNpcList[dieNpcList.Count - 1].GetComponent<NPC>().Die();
        Vector3 cropsePosition = dieNpcList[dieNpcList.Count - 1].transform.position;
        print(cropsePosition);
        Vector3 target = cropsePosition - cam.transform.position;
        StartCoroutine(RotateVectorCoroutine(target, cam.transform, true));
        StartCoroutine(MoveCamera(cropsePosition));
    }

    //시체로 카메라 이동
    IEnumerator MoveCamera(Vector3 target)
    {
        yield return new WaitForSeconds(1);
        while (true)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, target, Time.deltaTime * speed);
            if (Vector3.Distance(cam.transform.position, target) < 8f)
            {
                StartCoroutine(ResetAndPlay());
                break;
            }
            yield return null;
        }
    }

    IEnumerator ResetAndPlay()
    {
        notifyDead.SetActive(true);
        TMP_Text text = notifyDead.transform.GetChild(0).GetComponent<TMP_Text>();
        text.text = dieNpcList[dieNpcList.Count - 1].GetComponent<NpcData>().npcName + "가 밤새 죽었습니다.";
        TurnNpcList(true, 1);
        KJY_CitizenManager.Instance.ActiveNpc();
        yield return new WaitForSeconds (4f);
        notifyDead.SetActive(false);
        cameratopdown.enabled = true;
        cam.transform.rotation = cameratopdown.first;
        KJY_CitizenManager.Instance.call = false;
        PhotonConnection.Instance.CameraOnOff(false);
        OnOffUI(true);
    }

    void SetDummyDataAndNpcDataEquel(string selectNpc, bool isNpcSelectMafia)
    {

        if (isNpcSelectMafia == true)
        {
            if (obj != null)
            {
                obj.GetComponent<NpcData>().status = "DEAD";
                obj.GetComponent<NpcData>().npcDeathNightNumer = UI.instance.dayInt;
                for (int i = 0; i < npcList.Count; i++)
                {
                    if (obj.GetComponent<NpcData>().npcName == npcList[i].GetComponent<NpcData>().npcName)
                    {
                        npcList[i].GetComponent<NpcData>().status = "DEAD";
                        npcList[i].GetComponent<NpcData>().npcDeathNightNumer = UI.instance.dayInt;
                        dieNpcList.Add(npcList[i]);
                        npcList.RemoveAt(i);
                    }
                }
            }

        }
        else
        {
            for (int i = 0; i < npcList.Count; i++)
            {
                if (selectNpc == npcList[i].GetComponent<NpcData>().npcName)
                {
                    npcList[i].GetComponent<NpcData>().status = "DEAD";
                    npcList[i].GetComponent<NpcData>().npcDeathNightNumer = UI.instance.dayInt;
                    dieNpcList.Add(npcList[i]);
                    npcList.RemoveAt(i);
                }
            }

            for (int j = 0; j < KJY_CitizenManager.Instance.dummyNpcList.Count; j++)
            {
                if (selectNpc == KJY_CitizenManager.Instance.dummyNpcList[j].GetComponent<NpcData>().npcName)
                {
                    KJY_CitizenManager.Instance.dummyNpcList[j].GetComponent<NpcData>().status = "DEAD";
                    KJY_CitizenManager.Instance.dummyNpcList[j].GetComponent<NpcData>().npcDeathNightNumer = UI.instance.dayInt;
                }
            }
        }
    }

    private void OnOffUI(bool value)
    {
        if (InfoManagerKJY.instance.role == "Detective")
        {
            ChatManager.instance.interactiveBtn.SetActive(value);
            UI.instance.inventoryObject.SetActive(value);
            OnOffDetectiveCamera(true);
        }
        UI.instance.lifeObject.SetActive(value);
        UI.instance.dayObject.SetActive(value);
    }

    [PunRPC]
    public void GointerrogationRoom()
    {
        selectUI.SetActive(false);
        StopAllCoroutines();
        cameratopdown.enabled = false;
        ChatManager.instance.npcdata = obj.GetComponent<NpcData>();
        Camera.main.transform.position = arrestCameraSpot.transform.position;
        Camera.main.transform.rotation = arrestCameraSpot.transform.rotation;
        if (InfoManagerKJY.instance.role == "Detective")
        {
            GameObject detective = KJY_CitizenManager.Instance.player;
            detective.transform.position = detectiveSpot.position;
            detective.transform.rotation = detectiveSpot.rotation;
        }
        else
        {
            GameObject assistant = KJY_CitizenManager.Instance.dog;
            assistant.transform.position = assistantSpot.position;
            assistant.transform.rotation = assistantSpot.rotation;
        }
        obj.transform.position = arrestNpcSpot.position;
        obj.transform.rotation = arrestNpcSpot.rotation;
        StartCoroutine(HanginterrogationRoom());
    }

    private void SettingInterrogationRoom()
    {
        ChatManager.instance.nowNpc = obj;
        ChatManager.instance.npcdata = obj.GetComponent<NpcData>();
        obj.transform.position = intellRoomNpc.transform.position;
        obj.transform.rotation = intellRoomNpc.transform.rotation;
        Camera.main.transform.position = intellCameraSpot.transform.position;
        Camera.main.transform.rotation = intellCameraSpot.transform.rotation;
        KJY_CitizenManager.Instance.OnOffCanvas(obj, true);
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        KJY_CitizenManager.Instance.ResetPlayersSpot(false);
        ChatManager.instance.Startinterrogation();
        image.DOFade(0, 2f);
    }

    private IEnumerator HanginterrogationRoom()
    {
        image.DOFade(1, 1f);
        yield return new WaitForSeconds(3f);
        SettingInterrogationRoom();
    }

    public void interrogationBtn(bool value)
    {
        selectInBtn.SetActive(value);
    }

    [PunRPC]
    private void UpdateMainCamera(bool value)
    {
        if (value == false)
            cam.transform.rotation = cameratopdown.first;
        cam.enabled = value;
    }

    [PunRPC]
    private void npcSpotResetPun()
    {
        obj.transform.localPosition = new Vector3(0, 0, 0);
        obj.transform.localRotation = Quaternion.identity;
    }

    private void OnOffDetectiveCamera(bool value)
    {
        if (InfoManagerKJY.instance.role == "Detective")
        {
            detectiveCamera.SetActive(value);
        }
    }

    [PunRPC]
    private void OnOffDetectiveShape(bool value)
    {
        KJY_CitizenManager.Instance.player.SetActive(value);
    }

    [PunRPC]
    private void OnOffAssistantShape(bool value)
    {
        KJY_CitizenManager.Instance.dog.SetActive(value);
    }
}
