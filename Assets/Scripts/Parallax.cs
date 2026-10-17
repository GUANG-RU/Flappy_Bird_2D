using UnityEngine;

public class Parallax : MonoBehaviour
{
    // 控制背景移動速度，使視差效果更自然
    public float animationSpeed = 1f;

    // 用來操控背景材質的 MeshRenderer
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        // 取得 MeshRenderer，負責渲染背景材質
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        // 移動背景的材質偏移，製造視差滾動效果
        meshRenderer.material.mainTextureOffset += new Vector2(animationSpeed * Time.deltaTime, 0);
    }

}
