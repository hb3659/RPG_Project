using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MonoBehaviour ���� ���
public class EquipmentCombiner
{
    // �⺻ ���뿡 ���� ��ü ����Ʈ�� �ʿ�
    // �ܺο��� ������ �� ������ �Ѵ�.
    private readonly Dictionary<int, Transform> rootBoneDictionary = new Dictionary<int, Transform>();

    // ���� ������Ʈ�� ���� root
    private readonly Transform transform;

    public EquipmentCombiner(GameObject rootGo)
    {
        transform = rootGo.transform;
        TraverseHierachy(transform);
    }

    public Transform AddLimb(GameObject itemGo, List<string> boneNames)
    {
        // boneNames == ItemObject �� boneName �� �ǹ�

        // �����ۿ� SkinnedMeshRenderer �� ���ԵǾ� �ִ� ���� ������Ʈ�� ������
        // SkinnedMeshRenderer �� ������ �ִ� bone �� ������ ���ο� SkinnedMeshRenderer �� ����
        Transform limb = ProcessBoneObject(itemGo.GetComponent<SkinnedMeshRenderer>(), boneNames);
        limb.SetParent(transform);

        return limb;
    }

    private Transform ProcessBoneObject(SkinnedMeshRenderer renderer, List<string> boneNames)
    {
        Transform itemTransform = new GameObject().transform;

        SkinnedMeshRenderer meshRenderer = itemTransform.gameObject.AddComponent<SkinnedMeshRenderer>();

        Transform[] boneTransforms = new Transform[boneNames.Count];

        for(int i =0; i< boneNames.Count; i++)
        {
            boneTransforms[i] = rootBoneDictionary[boneNames[i].GetHashCode()];
         }

        meshRenderer.bones = boneTransforms;
        meshRenderer.sharedMesh = renderer.sharedMesh;
        meshRenderer.materials = renderer.sharedMaterials;

        return itemTransform;
    }

    public Transform[] AddMesh(GameObject itemGO)
    {
        Transform[] itemTransforms = ProcessMeshObject(itemGO.GetComponentsInChildren<MeshRenderer>());

        return itemTransforms;
    }

    // SkinnedMeshRenderer �� �ƴ� ������Ʈ
    private Transform[] ProcessMeshObject(MeshRenderer[] meshRenderer)
    {
        List<Transform> itemTransforms = new List<Transform>();

        foreach(MeshRenderer renderer in meshRenderer)
        {
            if(renderer.transform.parent != null)
            {
                Transform parent = rootBoneDictionary[renderer.transform.parent.name.GetHashCode()];

                GameObject itemGO = GameObject.Instantiate(renderer.gameObject, parent);

                itemTransforms.Add(itemGO.transform);
            }
        }

        return itemTransforms.ToArray();
    }

    private void TraverseHierachy(Transform root)
    {
        // rootBone �� �ڽĵ��� ���´�.
        foreach(Transform child in root)
        {
            // GetHashCode => string ---Hash ȭ---> int
            // ���ڿ��� ���� ���� �ӵ����� ������ ���� �ӵ��� ����
            // �������� ���
            rootBoneDictionary.Add(child.name.GetHashCode(), child);

            // ����Լ�
            TraverseHierachy(child);
        }
    }
}
