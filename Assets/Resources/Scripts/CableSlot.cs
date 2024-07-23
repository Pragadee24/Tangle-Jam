using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CableSlot : MonoBehaviour
{
    private Vector3[] slotPos = new Vector3[7];
    public Dictionary<int, int> slotIndices = new Dictionary<int, int>();
    private int slotCount = 7, occupiedSlot = 0;
    public Vector3 targetSlotPos = Vector3.zero;
    public static CableSlot Instance;
    private int _score = 0, CableLeftCnt = 3;
    [SerializeField] public TextMesh ScoreLabel,CableLeft;
    [SerializeField] public GameObject GameNext,GameRetry,Title,Level,CableLeftGo;

    // Start is called before the first frame update
    void Start()
    {
        LoadSlotPos();
        RestSlotIndices();
        Instance = this;
    }

    public void UpdateSlotIndices(int colourType)
    {
        int sameColourcnt = 0;

        for (int i = 0; i < slotCount; i++)
        {
            if (slotIndices[i] != -1)
            {
                if (sameColourcnt == 2)
                {
                    
                    for (int temp = occupiedSlot; temp > i; temp--)
                        slotIndices[temp] = slotIndices[temp - 1];

                    occupiedSlot++;
                    slotIndices[i] = colourType;
                    targetSlotPos = slotPos[i];
                    occupiedSlot -= 3;
                    slotIndices[i - 2] = -1;
                    slotIndices[i - 1] = -1;
                    slotIndices[i] = -1;
                    RearrangeSlotIndices();
                    break;
                }
                else
                {
                    if (slotIndices[i] == colourType)
                    {
                        sameColourcnt++;
                        continue;
                    }

                    if (sameColourcnt == 1)
                    {
                        for (int temp = occupiedSlot; temp > i; temp--)
                            slotIndices[temp] = slotIndices[temp - 1];

                        occupiedSlot++;
                        slotIndices[i] = colourType;
                        targetSlotPos = slotPos[i];
                        if (occupiedSlot == 7)
                            StartCoroutine(RetryLevel());
                        break;
                    }
                }
            }
            else
            {
                if (sameColourcnt == 2)
                {
                    occupiedSlot++;
                    slotIndices[i] = colourType;
                    targetSlotPos = slotPos[i];

                    if (occupiedSlot == 3)
                    {
                        occupiedSlot = 0;
                        StartCoroutine(ScoreUpdate());
                        RestSlotIndices();
                    }
                    else
                    {
                        occupiedSlot -= 3;
                        slotIndices[i - 2] = -1;
                        slotIndices[i - 1] = -1;
                        slotIndices[i] = -1;
                        RearrangeSlotIndices();
                    }
                    break;
                }
                else
                {
                    occupiedSlot++;
                    slotIndices[i] = colourType;
                    targetSlotPos = slotPos[i];
                    if (occupiedSlot == 7)
                        StartCoroutine(RetryLevel());
                    break;
                }

            }
        }
    }
    void LoadSlotPos()
    {
        slotPos[0] = new Vector3(-0.778f, -1.708f, 3.515f);
        slotPos[1] = new Vector3(-0.512f, -1.708f, 3.515f);
        slotPos[2] = new Vector3(-0.259f, -1.708f, 3.515f);
        slotPos[3] = new Vector3(0f, -1.708f, 3.515f);
        slotPos[4] = new Vector3(0.255f, -1.708f, 3.515f);
        slotPos[5] = new Vector3(0.512f, -1.708f, 3.515f);
        slotPos[6] = new Vector3(0.77f, -1.708f, 3.515f);
    }

    void RestSlotIndices()
    {
        for (int i = 0; i < slotCount; i++)
            slotIndices[i] = -1;
    }

    void RearrangeSlotIndices()
    {

        int[] updatedSlotIndices = { -1, -1, -1, -1, -1, -1, -1 };
        int valueCnt = 0;


        for (int i = 0; i < slotCount; i++)
        {
            if (slotIndices[i] != -1)
                updatedSlotIndices[valueCnt++] = slotIndices[i];
        }

        for (int i = 0; i < slotCount; i++)
            slotIndices[i] = updatedSlotIndices[i];

        StartCoroutine(ScoreUpdate());
    }
    public void CableLeftDisplay()
    {
        StartCoroutine(CableLeftCntUpdate());
    }

    private IEnumerator CableLeftCntUpdate()
    {
        yield return new WaitForSeconds(0.5f);
        if(CableLeftCnt>0)
            CableLeftCnt--;
        CableLeft.text = ""+CableLeftCnt;
    }
    private IEnumerator ScoreUpdate()
    {
        yield return new WaitForSeconds(0.3f);
        _score++;
        ScoreLabel.text = "Score: " + _score;
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (_score == 5)
                StartCoroutine(ContinueLevel());
        }
        else
        {
            if (_score == 6)
                StartCoroutine(ContinueLevel());
        }
    }
    private IEnumerator RetryLevel()
    {
        yield return new WaitForSeconds(1.25f);
        if (CableLeftGo != null)
            CableLeftGo.SetActive(false);
        Title.SetActive(false);
        Level.SetActive(false);
        RestSlotIndices();
        CableHolder.Instance.cable_HolderClrUpdate();
        GameRetry.SetActive(true);
    }

    private IEnumerator ContinueLevel()
    {
        yield return new WaitForSeconds(1f);
        if (CableLeftGo != null)
            CableLeftGo.SetActive(false);
        Title.SetActive(false);
        GameNext.SetActive(true);
    }
    
}