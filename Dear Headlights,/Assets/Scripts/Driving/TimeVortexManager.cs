using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Den;
using DG.Tweening;

public class TimeVortexManager : MonoBehaviour {

    [SerializeField] Material skyboxMaterial;
    [SerializeField] Transform sceneParent;
    [SerializeField] float rotationNoiseSpeedMax = 0.002f;
    [SerializeField] float scaleNoiseSpeedMax = 0.005f;
    [SerializeField] float scaleRangeMax = 0.3f;
    [SerializeField] float scaleOffsetMax = 0.5f;
    [SerializeField] float cameraRotationSpeedMax = 1f;

    float currentRotationRange = 0f;
    float currentScaleRange = 0f;
    float currentScaleOffset = 0f;
    float currentCameraRotationSpeed = 0f;

    float rotationNoiseSpeed {
        get {
            return rotationNoiseX.speed.x;
        }

        set {
            rotationNoiseX.speed.x = value;
            rotationNoiseY.speed.x = value;
            rotationNoiseZ.speed.x = value;
        }
    }
    float scaleNoiseSpeed {
        get {
            return scaleNoiseX.speed.x;
        }

        set {
            scaleNoiseX.speed.x = value;
            scaleNoiseY.speed.x = value;
            scaleNoiseZ.speed.x = value;
        }
    }

    float rotationNoiseScale = 0f;
    float scaleNoiseScale = 0f;

    PerlinNoise rotationNoiseX;
    PerlinNoise rotationNoiseY;
    PerlinNoise rotationNoiseZ;

    PerlinNoise scaleNoiseX;
    PerlinNoise scaleNoiseY;
    PerlinNoise scaleNoiseZ;

    Coroutine coroutine;

    private void Awake() {
        rotationNoiseX = new PerlinNoise();
        rotationNoiseY = new PerlinNoise();
        rotationNoiseZ = new PerlinNoise();

        scaleNoiseX = new PerlinNoise();
        scaleNoiseY = new PerlinNoise();
        scaleNoiseZ = new PerlinNoise();
    }

    private void Update() {
        rotationNoiseX.Iterate();
        rotationNoiseY.Iterate();
        rotationNoiseZ.Iterate();

        scaleNoiseX.Iterate();
        scaleNoiseY.Iterate();
        scaleNoiseZ.Iterate();

        Vector3 newRotation = new Vector3(
            rotationNoiseX.MapValue(-currentRotationRange, currentRotationRange).x,
            rotationNoiseY.MapValue(-currentRotationRange, currentRotationRange).x,
            rotationNoiseZ.MapValue(-currentRotationRange, currentRotationRange).x
            );

        sceneParent.transform.rotation = Quaternion.Euler(newRotation);

        Vector3 newScale = new Vector3(
            scaleNoiseX.MapValue(1f - currentScaleOffset - currentScaleRange, 1f - currentScaleOffset + currentScaleRange).x,
            scaleNoiseY.MapValue(1f - currentScaleOffset - currentScaleRange, 1f - currentScaleOffset + currentScaleRange).x,
            scaleNoiseZ.MapValue(1f - currentScaleOffset - currentScaleRange, 1f - currentScaleOffset + currentScaleRange).x
            );

        Vector3 newCameraRotation = Camera.main.transform.localRotation.eulerAngles;
        newCameraRotation.z += currentCameraRotationSpeed * Time.deltaTime;
        Camera.main.transform.localRotation = Quaternion.Euler(newCameraRotation);

        sceneParent.transform.localScale = newScale;
    }

    public void GetReadyToStart() {
        Camera.main.clearFlags = CameraClearFlags.Skybox;
        Camera.main.orthographic = true;
        RenderSettings.skybox = skyboxMaterial;
    }

    public void BeginVortex() {
        if (coroutine != null) { StopCoroutine(coroutine); }
        coroutine = StartCoroutine(BeginVortexCoroutine());
    }

    IEnumerator BeginVortexCoroutine() {
        float tweenDuration = 4f;

        Ease tweenEase = Ease.InOutQuint;
        DOTween.To(() => currentRotationRange, x => currentRotationRange = x, 180f, tweenDuration).SetEase(tweenEase);
        DOTween.To(() => currentScaleRange, x => currentScaleRange = x, scaleRangeMax, tweenDuration).SetEase(tweenEase);
        DOTween.To(() => rotationNoiseSpeed, x => rotationNoiseSpeed = x, rotationNoiseSpeedMax, tweenDuration).SetEase(tweenEase);
        DOTween.To(() => scaleNoiseSpeed, x => scaleNoiseSpeed = x, scaleNoiseSpeedMax, tweenDuration).SetEase(tweenEase);
        DOTween.To(() => currentScaleOffset, x => currentScaleOffset = x, scaleOffsetMax, tweenDuration).SetEase(tweenEase);
        DOTween.To(() => currentCameraRotationSpeed, x => currentCameraRotationSpeed = x, cameraRotationSpeedMax, tweenDuration).SetEase(tweenEase);

        yield return new WaitForSeconds(tweenDuration);

        yield return null;
    }

    public void GetReadyToEnd() {
        Camera.main.clearFlags = CameraClearFlags.Color;
        Camera.main.orthographic = false;
        RenderSettings.skybox = null;
    }

    public void EndVortex() {
        if (coroutine != null) { StopCoroutine(coroutine); }
        coroutine = StartCoroutine(EndVortexCoroutine());
    }

    IEnumerator EndVortexCoroutine() {

        float tweenDuration = 1f;

        Ease tweenEase = Ease.InOutQuint;
        DOTween.To(() => currentRotationRange, x => currentRotationRange = x, 0f, tweenDuration).SetEase(tweenEase);
        DOTween.To(() => currentScaleRange, x => currentScaleRange = x, 0f, tweenDuration).SetEase(tweenEase);
        DOTween.To(() => rotationNoiseSpeed, x => rotationNoiseSpeed = x, 0f, tweenDuration).SetEase(tweenEase);
        DOTween.To(() => scaleNoiseSpeed, x => scaleNoiseSpeed = x, 0f, tweenDuration).SetEase(tweenEase);
        DOTween.To(() => currentScaleOffset, x => currentScaleOffset = x, 0f, tweenDuration).SetEase(tweenEase);
        DOTween.To(() => currentCameraRotationSpeed, x => currentCameraRotationSpeed = x, 0f, tweenDuration).SetEase(tweenEase);
        sceneParent.transform.DOScale(Vector3.one, tweenDuration);
        sceneParent.transform.DORotate(Vector3.zero, tweenDuration);
        Camera.main.transform.DORotate(Vector3.zero, tweenDuration);

        yield return new WaitForSeconds(tweenDuration);

        yield return null;
    }
}
