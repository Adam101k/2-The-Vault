using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFlash : MonoBehaviour
{
    [Tooltip("Material to switch to during the flash.")]
    [SerializeField] private Material flashMaterial;

    [Tooltip("Duration of the flash.")]
    [SerializeField] private float duration;

    public SpriteRenderer bodyRenderer;
    public SpriteRenderer headRenderer;

    private Material originalBodyMaterial;
    private Material originalHeadMaterial;

    private Coroutine flashRoutine;

    private void Awake()
    {
        originalBodyMaterial = bodyRenderer.material;
        originalHeadMaterial = headRenderer.material;
    }

    private IEnumerator FlashRountine()
    {
        bodyRenderer.material = flashMaterial;
        headRenderer.material = flashMaterial;

        yield return new WaitForSeconds(duration);

        bodyRenderer.material = originalBodyMaterial;
        headRenderer.material = originalHeadMaterial;

        flashRoutine = null;
    }
    public void Flash()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(FlashRountine());
    }
}
