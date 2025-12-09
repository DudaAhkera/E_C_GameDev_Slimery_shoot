using UnityEngine;
using TMPro;

public class SlimeTextDrip : MonoBehaviour
{
    [Header("Text & Fall")]
    public TMP_Text textMesh;
    public float fallSpeed = 600f;       
    public float bounceHeight = 10f;     
    public float settleSpeed = 6f;       
    public float spawnHeight = 700f;     

    [Header("Wobble After Landing")]
    public float wobbleAmount = 1.5f;    
    public float wobbleSpeed = 4f;       

    [Header("Impact Shake (Whole Text)")]
    public float impactShake = 0f;       
    public float shakeDecay = 3f;        
    public float shakeSpeed = 12f;

    [Header("Slime Puddle")]
    public SlimeShake slimePuddle;       // poça de slime
    public float puddleHitStrength = 0.25f;

    // Internos
    private TMP_TextInfo textInfo;
    private Vector3[][] baseVertices;
    private float[] currentY;
    private bool[] hasLanded;
    private bool initialized = false;

    // Posicao original do texto para shake global
    private Vector3 originalLocalPos;

    void Start()
    {
        if (textMesh == null)
        {
            Debug.LogError("SlimeTextDrip: textMesh não atribuído!");
            enabled = false;
            return;
        }

        originalLocalPos = transform.localPosition;
        InitText();
    }

    void InitText()
    {
        textMesh.ForceMeshUpdate();
        textInfo = textMesh.textInfo;

        baseVertices = new Vector3[textInfo.meshInfo.Length][];
        currentY = new float[Mathf.Max(1, textInfo.characterCount)];
        hasLanded = new bool[textInfo.characterCount];

        for (int m = 0; m < textInfo.meshInfo.Length; m++)
            baseVertices[m] = (Vector3[])textInfo.meshInfo[m].vertices.Clone();

        // força todas as letras a nascerem no topo
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible) continue;
            currentY[i] = spawnHeight;
        }

        initialized = true;
    }

    void Update()
    {
        if (!initialized) return;

        textMesh.ForceMeshUpdate();
        textInfo = textMesh.textInfo;

        AnimateLetters();
        ApplyMeshChanges();
        ApplyGlobalShake();
    }

    void AnimateLetters()
    {
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible) continue;

            int meshIdx = textInfo.characterInfo[i].materialReferenceIndex;
            int vertIdx = textInfo.characterInfo[i].vertexIndex;
            Vector3[] vertices = textInfo.meshInfo[meshIdx].vertices;

            float baseY = baseVertices[meshIdx][vertIdx].y;
            float targetY = baseY;

            // queda
            if (currentY[i] > targetY)
            {
                currentY[i] = Mathf.MoveTowards(currentY[i], targetY, fallSpeed * Time.deltaTime);

                // bounce / acomodação
                if (currentY[i] <= targetY + bounceHeight)
                    currentY[i] = Mathf.Lerp(currentY[i], targetY, Time.deltaTime * settleSpeed);
            }
            else
            {
                // wobble leve após pousar
                float wob = Mathf.Sin(Time.time * wobbleSpeed + i) * wobbleAmount;
                currentY[i] = targetY + wob;
            }

            // DETECTA IMPACTO NO CHÃO
            if (!hasLanded[i] && currentY[i] <= targetY + 1f)
            {
                hasLanded[i] = true;

                if (slimePuddle != null)
                {
                    slimePuddle.Grow();
                }
            }
            // aplica offset nos vertices
            float offset = currentY[i] - baseY;
            for (int v = 0; v < 4; v++)
                vertices[vertIdx + v] = baseVertices[meshIdx][vertIdx + v] + new Vector3(0f, offset, 0f);
        }
    }

    void ApplyMeshChanges()
    {
        for (int m = 0; m < textInfo.meshInfo.Length; m++)
        {
            textInfo.meshInfo[m].mesh.vertices = textInfo.meshInfo[m].vertices;
            textMesh.UpdateGeometry(textInfo.meshInfo[m].mesh, m);
        }
    }

    void ApplyGlobalShake()
    {
        if (impactShake > 0f)
        {
            float shake = Mathf.Sin(Time.time * shakeSpeed) * impactShake;
            transform.localPosition = new Vector3(
                originalLocalPos.x + shake,
                originalLocalPos.y,
                originalLocalPos.z
            );
            impactShake = Mathf.MoveTowards(impactShake, 0f, shakeDecay * Time.deltaTime);
        }
        else
        {
            if (transform.localPosition != originalLocalPos)
                transform.localPosition = Vector3.Lerp(transform.localPosition, originalLocalPos, Time.deltaTime * 10f);
        }
    }

    public void Hit(float strength)
    {
        impactShake = Mathf.Clamp(impactShake + strength, 0f, 2f);
    }
}








