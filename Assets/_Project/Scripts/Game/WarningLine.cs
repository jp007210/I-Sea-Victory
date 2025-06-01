using UnityEngine;

public class WarningLine : MonoBehaviour
{
    public float length = 40f;      // Distância visual do aviso
    public float width = 0.2f;      // Largura do rastro visual

    void Start()
    {
        LineRenderer lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;

        lr.startWidth = width;
        lr.endWidth = width;

        Vector3 start = transform.position;
        Vector3 end = transform.position + transform.forward * length;

        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        // Garante visibilidade com material Unlit
        if (lr.material == null)
        {
            Material lineMat = new Material(Shader.Find("Unlit/Color"));
            lineMat.color = Color.red;
            lr.material = lineMat;
        }
    }
}
