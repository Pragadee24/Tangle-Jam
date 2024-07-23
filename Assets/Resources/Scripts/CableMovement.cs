using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class CableMovement : MonoBehaviour
{
    public GameObject anotherEnd, Parent;
    private Vector3 tarPos, curPos, anotherEndCurPos,LerpCablePos;
    private bool canMove = false, canAnotherEndMove = false, canTranslate = false;
    private float speed = 2f, elapsedTime = 0, anotherEndelapsedTime = 0, lerpcableElapsedTime = 0;
    public GameObject[] behindCables;
    public GameObject LerpCable;
    public Material[] behindCable_Materials;
    public static CableMovement Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        curPos = transform.position;
        anotherEndCurPos = anotherEnd.transform.position;
        Parent = gameObject.transform.parent.gameObject;
        if(LerpCable!= null)
            LerpCablePos = LerpCable.transform.position;
        LoadMaterials();
    }

    // Update is called once per frame
    void Update()
    {
        ReachingTarget();
        translatecable();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider != null)
            StartCoroutine(DestroyGameObject());
    }

    public void OnMouseDown()
    {
        if (!canMove)
            canMove = true;

        for (int i = 0; i < behindCables.Length; i++)
        {
            if (behindCables[i] != null)
            {
                behindCables[i].transform.GetChild(0).GetComponent<SphereCollider>().enabled = true;
                behindCables[i].transform.GetChild(1).GetComponent<SphereCollider>().enabled = true;
                Material Material = behindCable_Materials[CableColorNum(behindCables[i].tag)];
                behindCables[i].transform.GetChild(0).GetComponent<MeshRenderer>().material = Material;
                behindCables[i].transform.GetChild(0).GetComponent<CableComponent>().cableMaterial = Material;
                behindCables[i].transform.GetChild(1).GetComponent<MeshRenderer>().material = Material;
            }
        }

        if (LerpCable != null)
        {
            canTranslate = true;
            CableSlot.Instance.CableLeftDisplay();
        }

        CableSlot.Instance.UpdateSlotIndices(CableColorNum(Parent.tag));
        tarPos = CableSlot.Instance.targetSlotPos;
        StartCoroutine(UpdateHolderClr());

    }
    public void ReachingTarget()
    {
        if (canMove)
        {
            if (canAnotherEndMove)
            {
                if (anotherEndelapsedTime > 1f)
                {
                    canMove = false;
                    canAnotherEndMove = false;
                }
                else
                    anotherEndelapsedTime += Time.deltaTime * speed;

                anotherEnd.transform.position = Vector3.Lerp(anotherEndCurPos, tarPos, anotherEndelapsedTime);
            }
            else
            {
                if (elapsedTime > 1f)
                    canAnotherEndMove = true;
                else
                    elapsedTime += Time.deltaTime * speed;

                transform.position = Vector3.Lerp(curPos, tarPos, elapsedTime);
            }

        }
    }

    public void translatecable()
    {
         if (canTranslate)
            {
                if (lerpcableElapsedTime > 1f)
                {
                    canTranslate = false;
                }
                else
                    lerpcableElapsedTime += Time.deltaTime * speed;

                LerpCable.transform.position = Vector3.Lerp(LerpCablePos,new Vector3(LerpCablePos.x,LerpCablePos.y+0.575f,LerpCablePos.z),lerpcableElapsedTime);
            }
    }
    private IEnumerator DestroyGameObject()
    {
        yield return new WaitForSeconds(1f);
        if (Parent != null)
            Destroy(Parent);
    }

    private IEnumerator UpdateHolderClr()
    {
        yield return new WaitForSeconds(0.5f);
        CableHolder.Instance.cable_HolderClrUpdate();
    }

    public int CableColorNum(String cableClrName)
    {
        switch (cableClrName)
        {
            case "Green":
                return (int)cableMaterial.Green;
            case "SkyBlue":
                return (int)cableMaterial.SkyBlue;
            case "Yellow":
                return (int)cableMaterial.Yellow;
            case "Purple":
                return (int)cableMaterial.Purple;
            case "Red":
                return (int)cableMaterial.Red;
            case "DarkBlue":
                return (int)cableMaterial.DarkBlue;
        }
        return -1;
    }

    public void LoadMaterials()
    {
        behindCable_Materials = new Material[6];
        behindCable_Materials[0] = Resources.Load("Material/Normal/RopeMaterial_Green")as Material;
        behindCable_Materials[1] = Resources.Load("Material/Normal/RopeMaterial_SkyBlue")as Material;
        behindCable_Materials[2] = Resources.Load("Material/Normal/RopeMaterial_Yellow")as Material;
        behindCable_Materials[3] = Resources.Load("Material/Normal/RopeMaterial_Purple")as Material;
        behindCable_Materials[4] = Resources.Load("Material/Normal/RopeMaterial_Red")as Material;
        behindCable_Materials[5] = Resources.Load("Material/Normal/RopeMaterial_DarkBlue")as Material;
    }

    public enum cableMaterial
    {
        Green,
        SkyBlue,
        Yellow,
        Purple,
        Red,
        DarkBlue
    }
}
