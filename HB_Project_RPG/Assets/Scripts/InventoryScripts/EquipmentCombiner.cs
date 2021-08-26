using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MonoBehaviour 에서 사용
public class EquipmentCombiner
{
    // 기본 뼈대에 대한 전체 리스트가 필요
    // 외부에서 수정할 수 없도록 한다.
    private readonly Dictionary<int, Transform> rootBoneDictionary = new Dictionary<int, Transform>();

    // 게임 오브젝트에 대한 root
    private readonly Transform transform;

    public EquipmentCombiner(GameObject rootGo)
    {
        transform = rootGo.transform;
        TraverseHierachy(transform);
    }

    public Transform AddLimb(GameObject itemGo, List<string> boneNames)
    {
        // boneNames == ItemObject 의 boneName 을 의미

        // 아이템에 SkinnedMeshRenderer 가 포함되어 있는 게임 오브젝트가 들어오면
        // SkinnedMeshRenderer 에 영향을 주는 bone 을 가지고 새로운 SkinnedMeshRenderer 를 생성
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

    // SkinnedMeshRenderer 가 아닌 오브젝트
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
        // rootBone 의 자식들을 얻어온다.
        foreach(Transform child in root)
        {
            // GetHashCode => string ---Hash 화---> int
            // 문자열에 대한 연산 속도보다 정수형 연산 속도가 빨라
            // 정수형을 사용
            rootBoneDictionary.Add(child.name.GetHashCode(), child);

            // 재귀함수
            TraverseHierachy(child);
        }
    }
}
