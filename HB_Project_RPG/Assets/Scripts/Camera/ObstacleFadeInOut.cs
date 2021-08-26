using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleFadeInOut : MonoBehaviour
{
    private Shader TransparentShader;
    private Shader StandardShader;
    private Color TransparentColor;
    private Color StandardColor;

    public Transform target;
    public LayerMask obstacleMask;

    private Dictionary<string, MeshRenderer> TransRendererList = new Dictionary<string, MeshRenderer>();

    private void Start()
    {
        TransparentShader = Shader.Find("Legacy Shaders/Transparent/Diffuse");
        StandardShader = Shader.Find("Standard");
        TransparentColor = new Color(1f, 1f, 1f, 0.2f);
        StandardColor = new Color(1f, 1f, 1f, 1f);
    }

    private void LateUpdate()
    {
        CheckObstacles();
    }

    public RaycastHit[] LaunchRay()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        float distacne = Vector3.Distance(transform.position, target.position);

        Ray ray = new Ray(transform.position, direction);
        RaycastHit[] hitInfos = Physics.RaycastAll(ray, distacne, obstacleMask);

        Debug.DrawRay(transform.position, direction * distacne, Color.cyan);

        return hitInfos;
    }

    public void CheckObstacles()
    {
        RaycastHit[] hitInfos = LaunchRay();

        foreach (RaycastHit hit in hitInfos)
        {
            MeshRenderer[] obstacleRenderers = hit.transform.GetComponents<MeshRenderer>();

            foreach (MeshRenderer mesh in obstacleRenderers)
            {
                if (!TransRendererList.ContainsKey(mesh.name))
                    TransRendererList.Add(mesh.name, mesh);
            }
        }

        foreach(MeshRenderer mesh in TransRendererList.Values)
        {
            Material allMaterial = mesh.material;
            allMaterial.shader = StandardShader;
            allMaterial.color = StandardColor;

            for(int i = 0; i < hitInfos.Length; i++)
            {
                if(mesh.name == hitInfos[i].transform.name)
                {
                    Material obstacleMaterial = mesh.material;
                    obstacleMaterial.shader = TransparentShader;
                    obstacleMaterial.color = TransparentColor;
                }
            }
        }

        if (hitInfos.Length == 0)
        {
            foreach(MeshRenderer mesh in TransRendererList.Values)
            {
                Material mat = mesh.material;
                mat.shader = StandardShader;
                mat.color = StandardColor;
            }

            TransRendererList.Clear();
        }
    }
}
