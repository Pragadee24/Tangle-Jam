using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CableHolder : MonoBehaviour
{
    public MeshRenderer[] cableSlot;
    private int cableSlotCnt = 7;
    private Material[] cable_Materials;
    public static CableHolder Instance;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        cableSlot = new MeshRenderer[cableSlotCnt];
        for (int count = 0; count < cableSlotCnt; count++)
            cableSlot[count] = transform.GetChild(count).GetComponent<MeshRenderer>();

        LoadMaterials();
    }
    public void LoadMaterials()
    {
        cable_Materials = new Material[7];
        cable_Materials[0] = Resources.Load("Material/Normal/RopeMaterial_Green")as Material;
        cable_Materials[1] = Resources.Load("Material/Normal/RopeMaterial_SkyBlue")as Material;
        cable_Materials[2] = Resources.Load("Material/Normal/RopeMaterial_Yellow")as Material;
        cable_Materials[3] = Resources.Load("Material/Normal/RopeMaterial_Purple")as Material;
        cable_Materials[4] = Resources.Load("Material/Normal/RopeMaterial_Red")as Material;
        cable_Materials[5] = Resources.Load("Material/Normal/RopeMaterial_DarkBlue")as Material;
        cable_Materials[6] = Resources.Load("Material/BG/CableHoderMaterial")as Material;
    }

    public void cable_HolderClrUpdate()
    {
        for (int count = 0; count < cableSlotCnt; count++)
        {
            if (CableSlot.Instance.slotIndices[count] != -1)
                cableSlot[count].material = cable_Materials[CableSlot.Instance.slotIndices[count]];
            else
                cableSlot[count].material = cable_Materials[6];
        }
    }
}
