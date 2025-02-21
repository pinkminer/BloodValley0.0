using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;

public class PetalPuzzle : MonoBehaviour
{
    //public string tag = "petals";
    public Camera camera;

    public GameObject flower;
    private int petalCount;
    public List<GameObject> petals;
    public List<Rigidbody2D> petalRbs;

    private List<EventTrigger> clickTriggers;

    private List<Vector3> initPositions;

    private FallAndFloatIE FallController;
    private List<Coroutine> fallCoroutines;

    private List<Animator> petalAnimators;
    private Animator flowerAnimator;
    private ScreenShake screenShake;
    //private ZoomManagerLinear zoomManager;

    private List<int> records;
    private bool arePetalsGrowing = false;
    public int witherStatus = 0; //0��δ��ή��1����ʼ��ή��2���ѿ�ή

    // Start is called before the first frame update
    private void Awake()
    {
        records = new List<int>();
        initPositions = new List<Vector3>();
        FallController = FallAndFloatIE.getInstance();
        if (FallController == null)
        {
            Debug.Log("fallcontroller is null");
        }
        clickTriggers = new List<EventTrigger>();
        fallCoroutines = new List<Coroutine>();
        petalAnimators = new List<Animator>();
        flowerAnimator = flower.GetComponent<Animator>();
        screenShake = GetComponent<ScreenShake>();
        //zoomManager = GetComponent<ZoomManagerLinear>();

        //collider2d���ʹ����֮�以����ײ
        List<Collider2D> petalCols = new List<Collider2D>();

        foreach (GameObject petal in petals)
        {
            //GameObject petal = petals[i];

            //ȡ��������ʹ���꾲ֹ
            Rigidbody2D rb = petal.GetComponent<Rigidbody2D>();
            petalRbs.Add(rb);
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;

            //��¼λ��
            initPositions.Add(petal.transform.position);

            //��ӵ���¼�������
            EventTrigger trigger = petal.AddComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerClick
            };

            entry.callback.AddListener((data) => {
                //Debug.Log("Petal clicked: " + petal.name);
                OnPetalClick(petal);
            });
            trigger.triggers.Add(entry);

            clickTriggers.Add(trigger);

            //����animators���
            petalAnimators.Add(petal.GetComponent<Animator>());

            //����collider2d���
            petalCols.Add(petal.GetComponent<Collider2D>());
        }

        petalCount = petals.Count;
        //Debug.Log(petalCount);

        for (int i = 0; i < petalCount; i++)
        {
            for (int j = i + 1; j < petalCount; j++)
            {
                Physics2D.IgnoreCollision(petalCols[i], petalCols[j]);
            }
        }
        petalCols.Clear();
    }

    private void Start()
    {
        SetTriggers(false);
        //this.enabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (records.Count == petalCount && AreAllPetalsCleared()) //������ӣ����ܻ�Ҫ�޸ġ��������һ���ж������Ƿ������߼�����ȡ������Ʈ�仹�Ǵ�ֱ����
        {
            if (IsSolved(records))
            {
                Debug.Log("solved");
                witherStatus = 1;
                flowerAnimator.SetBool("isWithering", true);
                SetTriggers(false);
            }
            else
            {
                Debug.Log("failed");

                ResetAllThePetals();
                foreach (Animator animator in petalAnimators)
                {
                    animator.SetBool("isGrowing", true);
                }
                arePetalsGrowing = true;
                //���ڶ���/ʧ����ʾ
                StartCoroutine(screenShake.ShakeCo(camera));
            }
            records.Clear();
        }

        if (arePetalsGrowing)
        {
            SetTriggers(false); //�����ڳ�������ʱ���ܵ��
            ResetPetalAnimation();
        }
        else
        {
            SetTriggers(true);
        }

        if (witherStatus == 1)
        {
            MessageWitherEnd();
        }
    }


    private void OnPetalClick(GameObject petal)
    {
        //Debug.Log("Petal clicked: " + petal.name);
        if (petals.Contains(petal))
        {
            int petalID = petals.IndexOf(petal);
            //Debug.Log("petal clicked: "+petalID);
            bool hasClicked = false;  //���petal���Ƿ����и�index,��ֹ�ظ����
            foreach (int r in records)
            {
                if (r == petalID)
                {
                    hasClicked = true;
                    break;
                }
            }
            if (!hasClicked)
            {
                records.Add(petalID);
                //Debug.Log("record:"+petalID);
                fallCoroutines.Add(StartCoroutine(FallController.FallAndFloat(petal, petalRbs[petalID])));
            }

            //rb.gravityScale = 1.0f; //�������Ʈ�䣬���Դ���Ʈ��ĺ���
        }
    }

    private bool IsSolved(List<int> rec)
    {
        for (int i = 0; i < rec.Count; i++)
        {
            if (rec[i] != i)
            {
                return false;
            }
        }
        return true;
    }

    private bool AreAllPetalsCleared()
    {   //�Ƿ����еĻ��궼�ѱ������Ļ
        foreach (GameObject petal in petals)
        {
            if (IsOnePetalVisible(petal))
            {
                return false;
            }
        }
        return true;
    }

    private bool IsOnePetalVisible(GameObject petal)
    {
        // �����������λ��ת��Ϊ�ӿ�����
        Vector3 viewportPoint = camera.WorldToViewportPoint(petal.transform.position);

        // ����Ƿ����ӿڷ�Χ��
        return viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
               viewportPoint.y >= 0 && viewportPoint.y <= 1 &&
               viewportPoint.z > 0; // ȷ�������������ǰ��
    }

    private void ResetAllThePetals()
    {
        if (fallCoroutines.Count > 0)
        {
            //Debug.Log("co list not empty");
            int i = 1;
            foreach (Coroutine co in fallCoroutines)
            {
                StopCoroutine(co);
                //Debug.Log(i+"th co stoped");
                i++;
            }
        }
        fallCoroutines.Clear();

        for (int i = 0; i < petalCount; i++)
        {
            petalRbs[i].Sleep();
            petals[i].transform.rotation = Quaternion.identity;
            petals[i].transform.position = initPositions[i];
            petalRbs[i].WakeUp();
        }
        //Debug.Log("reset petal func excuted");
    }

    private void ResetPetalAnimation()
    {
        foreach (Animator animator in petalAnimators)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Grow") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
            {
                //������������һ���Ҫ�ص�Ĭ��״̬��
                animator.SetBool("isGrowing", false);
                Debug.Log("anim trigger set false");
                arePetalsGrowing = false;
            }
        }
    }

    private void MessageWitherEnd()
    {
        //Debug.Log("checking");
        //��ή����������
        if (flowerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Wither") &&
            flowerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            Debug.Log("wither anim end");
            flowerAnimator.SetBool("isWithering", false);
            this.witherStatus = 2;
        }
    }


    private void SetTriggers(bool isTriggersEnabled)
    {
        int i = 1;
        foreach (EventTrigger trigger in clickTriggers)
        {
            trigger.enabled = isTriggersEnabled;
            //Debug.Log(i+"th trigger is "+ trigger.enabled);
            i++;
        }
        //Debug.Log("triggers set " + isTriggersEnabled);
    }

    private void ClearRecords()
    {
        records.Clear();
    }

    //���ⲿ������ͣ������Ϸ�Ľӿ�
    public void PuzzleOn()
    {
        SetTriggers(true);
    }

    public void PuzzleOff()
    {
        ResetAllThePetals();
        ClearRecords();
        SetTriggers(false);
    }
}
