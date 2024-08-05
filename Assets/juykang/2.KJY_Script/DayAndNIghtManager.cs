using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using static ConnectionKJY;

public class DayAndNIghtManager : MonoBehaviour
{
    public static DayAndNIghtManager instance;

    public List<GameObject> npcList;
    public List<GameObject> dieNpcList;

    public bool click;
    // ĳ���� ������ �ϸ�? ĳ���� �����ϱ� ��ư
    Ray ray;
    RaycastHit hit;
    Camera cam;

    GameObject obj;
    
    CameraTopDown cameratopdown;

    // ĳ���� ������ ���� ������? ��ŵ ��ư
    [SerializeField] GameObject skipBtn;
    [SerializeField] Image image;
    // ���Ǿư� ���ϱ�� �����
    [SerializeField] GameObject selectUI;
    Quaternion rotation;

    [SerializeField] GameObject selectBtn;
    
    [SerializeField] GameObject checkUI;

    [SerializeField] GameObject DayText;

    //ȸ�� ���ǵ� ��
    public float speed = 10;

    //ī�޶� ���� �ܾƿ� ��
    float maxZoom = 30;
    float curr = 40;
    bool isZoom;

    [SerializeField] Transform camPlace;
    [SerializeField] GameObject effect;
    [SerializeField] Transform sun;
    Quaternion firstSun;
    
    PostProcessProfile profile;
    ColorGrading cg;

    int number;

    [SerializeField] GameObject notifyDead;
    [SerializeField] Transform intellRoom;
    [SerializeField] Transform intellRoomPlayer;

    bool Winner;
    public int heartRate = 60;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        cam = Camera.main;
        click = false;
        npcList = KJY_CitizenManager.Instance.npcList;
        dieNpcList = KJY_CitizenManager.Instance.dieNpcList;
        selectUI.SetActive(false);
        cameratopdown = cam.GetComponent<CameraTopDown>();  
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
        ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0) && KJY_CitizenManager.Instance.call == true && click == false)
            {
                if (hit.collider.CompareTag("Npc") || hit.collider.CompareTag("NpcDummy"))
                {
                    obj = hit.collider.gameObject;
                    rotation = obj.transform.rotation;
                    OnOffUI(false);
                    skipBtn.SetActive(false);
                    selectBtn.SetActive(true);
                    selectUI.SetActive(true);
                    FollowNpc();
                }
            }
        }
    }

    void FollowNpc()
    {
        KJY_CitizenManager.Instance.OnOffCanvas(obj, false);
        cameratopdown.enabled = false;
        click = true;
        isZoom = true;
        Vector3 target = obj.transform.position - cam.transform.position;
        StartCoroutine(RotateVectorCoroutine(target, cam.transform, true));
        StartCoroutine(RotateVectorCoroutine(-target, obj.transform, false));
        StartCoroutine(ChangeZoom(maxZoom));
    }

    //�ε巴�� ȸ�������ִ� �Լ�
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
            // �Ÿ��� stopDistance �����̸� �ڵ� �ߴ�
            if (AreQuaternionsSimilar(tr.transform.rotation, Quaternion.LookRotation(target)) || cameratopdown.enabled == true)
            {
                yield break; // �ڷ�ƾ �ߴ�
            }

            yield return null;
        }
    }

    //�� ������� ���ó���
    IEnumerator RotateReset(Quaternion target, Transform tr)
    {
        while (true)
        {
            tr.rotation = Quaternion.Lerp(tr.rotation, target, Time.deltaTime * speed);

            if (AreQuaternionsSimilar(tr.transform.rotation, target))
            {
                yield break; // �ڷ�ƾ �ߴ�
            }

            yield return null;
        }
    }

    //�� ȸ����Ű��
    IEnumerator RotateSun(Quaternion target, Transform tr)
    {
        while (true)
        {
            tr.rotation = Quaternion.Lerp(tr.rotation, target, Time.deltaTime * speed * 0.04f);

            if (AreQuaternionsSimilar(tr.transform.rotation, target, 0.3f))
            {
                print("break");
                yield break; // �ڷ�ƾ �ߴ�
            }

            yield return null;
        }
    }


    //���� ��� �Լ�
    bool AreQuaternionsSimilar(Quaternion a, Quaternion b, float tolerance = 0.001f)
    {
        float angleDiff = Quaternion.Angle(a, b);

        return Mathf.Abs(angleDiff) < tolerance;
    }

    //���� ��� �Լ�
    bool AreQuaternionsSimilarInObj(Quaternion a, Quaternion b, float tolerance = 0.001f)
    {
        a.x = 0;
        b.x = 0;

        float angleDiff = Quaternion.Angle(a, b);

        return Mathf.Abs(angleDiff) < tolerance;
    }

    //�� �ٲٱ�
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

    //���밪�������� �� ��������
    bool AreValuSimilar(float a, float b, float tolerance = 0.005f)
    {
        float valueDiff = a - b;

        return Mathf.Abs(valueDiff) < tolerance;
    }

    //Ȯ�� ��ư
    public void SelectNpc()
    {
        StartCoroutine(SelectEffect());
    }

    //��� ��ư
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

    //�ٽ��ʱ�ȭ
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


    //��ŵ�� �� �����ϴ� ��
    public void AfterSkip()
    {
        StartCoroutine(EffectNight());
    }

    //��ŵ�� �� �����ϴ� ��
    IEnumerator EffectNight()
    {
        image.DOFade(1, 1f);
        InfoManagerKJY.instance.voteNightNumber = 0;
        InfoManagerKJY.instance.voteNpcName = null;
        InfoManagerKJY.instance.voteNpcName = null;
        UI.instance.DeathInfo();

        RequestSave requst = new RequestSave();
        requst.Request();

        yield return new WaitForSeconds(2f);

        TurnNpcList(false, 2);
        StartCoroutine(UIReset());
    }

    public void CheckMafiaUI()
    {
        TextMeshProUGUI text = checkUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        SetDummyDataAndNpcDataEquel(null,true);

        InfoManagerKJY.instance.voteNightNumber = UI.instance.dayInt;
        InfoManagerKJY.instance.voteNpcName = obj.GetComponent<NpcData>().npcName;
        if (obj.GetComponent<NpcData>().isMafia == true)
        {
            text.text = obj.GetComponent<NpcData>().npcName + "��(��) ���Ǿƿ����ϴ�.";
            InfoManagerKJY.instance.voteResult = "FOUND";
            InfoManagerKJY.instance.voteNpcObjectName = obj.name;
            Winner = true;
        }
        else
        {
            text.text = obj.GetComponent<NpcData>().npcName + "��(��) �ù��̿����ϴ�.";
            InfoManagerKJY.instance.voteResult = "NOTFOUND";
        }
        checkUI.SetActive(true);
    }

    //�������� �ֹ����̱�
    public void DieResidentSet()
    {
        StartCoroutine(VictimSet());
    }

    IEnumerator VictimSet()
    {
        yield return new WaitForSeconds(2f);
        //SetDummyDataAndNpcDataEquel(InfoManagerBJH.instance.victim, false);
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
        CheckMafiaUI();
        RequestSave requst = new RequestSave();
        requst.Request();

        yield return new WaitForSeconds(2f);
        TurnNpcList(false, 2);
        if (obj != null)
        {
            obj.SetActive(false);
        }
        if (Winner == false)
        {
            if (npcList.Count <= 2)
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

    //�Ϸ� �Ѿ�� UI reset��Ű�� �� 
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
        //���� ��Ű�� ��
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

    //�㿡�� �� ��
    public void NightToMorining()
    {
        Vector3 rotationInEulerAngles = sun.rotation.eulerAngles;
        rotationInEulerAngles.x = 190;
        Quaternion newRotation = Quaternion.Euler(rotationInEulerAngles);
        sun.rotation = newRotation;
        DayText.SetActive(false);

        //���Ǵ� ����
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
        KJY_CitizenManager.Instance.player.SetActive(value);
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

    //��ü �߰�
    private void foundCorpse()
    {
        KJY_CitizenManager.Instance.DieNpcSpotSet();
        TurnNpcList(true, 2);
        dieNpcList[dieNpcList.Count - 1].GetComponent<NPC>().Die();
        Vector3 cropsePosition = dieNpcList[dieNpcList.Count - 1].transform.position;
        Vector3 target = cropsePosition - cam.transform.position;
        StartCoroutine(RotateVectorCoroutine(target, cam.transform, true));
        StartCoroutine(MoveCamera(cropsePosition));
    }

    //��ü�� ī�޶� �̵�
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
        text.text = dieNpcList[dieNpcList.Count - 1].GetComponent<NpcData>().npcName + "�� ��� �׾����ϴ�.";
        TurnNpcList(true, 1);
        KJY_CitizenManager.Instance.ActiveNpc();
        yield return new WaitForSeconds (4f);
        notifyDead.SetActive(false);
        cameratopdown.enabled = true;
        cam.transform.rotation = cameratopdown.first;
        KJY_CitizenManager.Instance.call = false;
        OnOffUI(true);
    }

    void SetDummyDataAndNpcDataEquel(string selectNpc, bool isNpcSelectMafia)
    {

        if (isNpcSelectMafia == true)
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

    void OnOffUI(bool value)
    {
        ChatManager.instance.interactiveBtn.SetActive(value);
        UI.instance.noteObject.SetActive(value);
        UI.instance.lifeObject.SetActive(value);
        UI.instance.dayObject.SetActive(value);
    }

    public void GointerrogationRoom()
    {
        image.DOFade(1, 0.5f);
        ChatManager.instance.nowNpc = obj;
        ChatManager.instance.npcdata = obj.GetComponent<NpcData>();
        selectUI.SetActive(false);
        obj.transform.position = intellRoom.transform.position;
        obj.transform.rotation = intellRoom.transform.rotation;
        GameObject.FindWithTag("Player").transform.position = intellRoomPlayer.transform.position;
        UI.instance.talkBt.onClick.AddListener(ChatManager.instance.Startinterrogation);
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        ChatManager.instance.Startinterrogation();
        image.DOFade(0, 1f);
    }
}
