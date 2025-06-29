using UnityEngine;
using System.Collections;

public class KrakenAttack : MonoBehaviour
{
    [Header("Referências")]
    public Animation krakenAnimation;
    public AnimationClip[] attackClips; // 0 - Tentacle, 1 - Spin
    public Transform krakenCenter;

    [Header("Avisos Visuais")]
    public GameObject circularWarningPrefab;
    public GameObject lineWarningPrefab;
    public Transform[] tentaclePoints;
    public float lineLength = 6f;

    [Header("Parâmetros de Ataque")]
    public float spinDamageRadius = 8f;
    public float tentacleDamageRadius = 2f;
    public int damage = 25;
    public float timeBetweenAttacks = 5f;
    public LayerMask playerLayer;

    private bool useSpinNext = true;

    void Start()
    {
        for (int i = 0; i < attackClips.Length; i++)
        {
            if (attackClips[i] != null)
            {
                string clipName = $"Attack{i}";
                if (krakenAnimation.GetClip(clipName) == null)
                {
                    krakenAnimation.AddClip(attackClips[i], clipName);
                }
            }
        }

        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenAttacks);

            if (useSpinNext)
                yield return StartCoroutine(PerformSpinAttack());
            else
                yield return StartCoroutine(PerformTentacleAttack());

            useSpinNext = !useSpinNext;
        }
    }

    IEnumerator PerformSpinAttack()
    {
        Vector3 centerPos = krakenCenter != null ? krakenCenter.position : transform.position;
        GameObject circle = Instantiate(circularWarningPrefab, centerPos + Vector3.up * 0.1f, Quaternion.identity);
        DrawCircle(circle.GetComponent<LineRenderer>(), centerPos, spinDamageRadius);
        Destroy(circle, 2f);

        yield return new WaitForSeconds(1f);

        PlayAnimationByIndex(1); // Spin
        yield return new WaitForSeconds(0.6f);

        DealSpinDamage(centerPos);
    }

    IEnumerator PerformTentacleAttack()
    {
        foreach (Transform point in tentaclePoints)
        {
            GameObject line = Instantiate(lineWarningPrefab, point.position, Quaternion.identity);
            LineRenderer lr = line.GetComponent<LineRenderer>();

            Vector3 start = point.position + Vector3.up * 0.05f;
            Vector3 end = krakenCenter.position + Vector3.up * 0.05f;

            DrawLine(lr, start, end - start);

            Destroy(line, 2f);
        }

        yield return new WaitForSeconds(1f);

        PlayAnimationByIndex(0); // Tentacle
        yield return new WaitForSeconds(2.4f);

        DealTentacleDamage();
    }

    void PlayAnimationByIndex(int index)
    {
        if (attackClips == null || index < 0 || index >= attackClips.Length || attackClips[index] == null)
        {
            Debug.LogError("Índice de animação inválido ou clipe não atribuído!");
            return;
        }

        string clipName = $"Attack{index}";
        krakenAnimation.Play(clipName);
    }

    void DealSpinDamage(Vector3 center)
    {
        Collider[] hits = Physics.OverlapSphere(center, spinDamageRadius, playerLayer);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
                hit.GetComponent<PlayerStats>()?.TakeDamage(damage);
        }
    }

    void DealTentacleDamage()
    {
        Vector3 center = krakenCenter != null ? krakenCenter.position : transform.position;

        foreach (Transform point in tentaclePoints)
        {
            Vector3 dir = (center - point.position).normalized;
            Vector3 start = point.position + Vector3.up * 0.1f; // ligeiramente acima do solo
            Vector3 end = start + dir * lineLength;

            // Faz o SphereCast no caminho do tentáculo
            RaycastHit[] hits = Physics.SphereCastAll(start, tentacleDamageRadius, dir, lineLength, playerLayer);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    PlayerStats stats = hit.collider.GetComponentInParent<PlayerStats>();
                    if (stats != null)
                    {
                        stats.TakeDamage(damage);
                        Debug.Log("Dano do tentáculo aplicado!");
                    }
                }
            }

            // Debug visual (opcional)
            Debug.DrawLine(start, end, Color.magenta, 1f);
        }
    }

    void DrawCircle(LineRenderer lr, Vector3 center, float radius, int segments = 50)
    {
        lr.positionCount = segments + 1;
        for (int i = 0; i <= segments; i++)
        {
            float angle = i * 2 * Mathf.PI / segments;
            Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            lr.SetPosition(i, center + pos + Vector3.up * 0.05f);
        }
    }

    void DrawLine(LineRenderer lr, Vector3 start, Vector3 offset)
    {
        lr.positionCount = 2;
        lr.SetPosition(0, start + Vector3.up * 0.05f);
        lr.SetPosition(1, start + offset + Vector3.up * 0.05f);
    }
}