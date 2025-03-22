using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFX : MonoBehaviour
{
    Camera cameraComponenet;
    CinemachineFollow cinemachineFollow;
    CinemachineHardLookAt cinemachineLook;
    //[SerializeField] bool fixCamInCenter = true;
    [SerializeField] GameObject posTarget;
    //float 

    [Header("FOV FX")]
    [SerializeField] float fOVNormal = 34f;
    [SerializeField] float fOVZoomout = 49f;
    [SerializeField] int fovMode = 0;
    private float _fovCur;

    [Header("Offset FX")]
    [SerializeField] float offsetMagnitudeLand = 0.05f;
    [SerializeField] float offsetMagnitudeDamage = 0.2f;
    [SerializeField] int offsetFXMode = 0;
    Vector3 _cinemachineOffset;
    Vector3 _cinemachineOffsetLook;
    Vector3 _cinemachineOffsetFX;
    private readonly float lerpDelay = 10f;

    void Start()
    {
        cameraComponenet = GetComponent<Camera>();
        cinemachineFollow = GetComponent<CinemachineFollow>();
        cinemachineLook = GetComponent<CinemachineHardLookAt>();

        _fovCur = fOVNormal;
        _cinemachineOffset = cinemachineFollow.FollowOffset;
        _cinemachineOffsetLook = cinemachineLook.LookAtOffset;
        cameraComponenet.fieldOfView = _fovCur;
    }

    // Update is called once per frame
    public void UpdateCamera()
    {
        SetFOV();
        SetOffsetFX();
    }

    void SetFOV()
    {
        switch(fovMode){
            case 0:
            {
                _fovCur = Mathf.Lerp(_fovCur, fOVNormal, Time.deltaTime * lerpDelay);
                break;
            }
            case 1:
            {
                _fovCur = Mathf.Lerp(_fovCur, fOVZoomout, Time.deltaTime * lerpDelay);
                break;
            }
        }

        cameraComponenet.fieldOfView = _fovCur;
    }

    public void SetFOVZoomOut()
    {
        fovMode = 1;
    }
    public void ResetFOV()
    {
        fovMode = 0;
    }

    public IEnumerator ApplyOffsetFXLand()
    {
        offsetFXMode = 1;

        yield return new WaitForSeconds(0.1f);
        
        offsetFXMode = 0;
    }

    public IEnumerator ApplyOffsetFXDamage2()
    {
        Debug.Log("Collision Detected");

        offsetFXMode = 2;

        yield return new WaitForSeconds(0.2f);
        
        offsetFXMode = 0;
    }
    
    void SetOffsetFX()
    {
        switch(offsetFXMode){
            case 0:
            {
                _cinemachineOffsetFX = Vector3.zero;
                break;
            }
            case 1:
            {
                _cinemachineOffsetFX = MakeOffset(offsetMagnitudeLand);
                break;
            }
            case 2:
            {
                _cinemachineOffsetFX = MakeOffset(offsetMagnitudeDamage);
                break;
            }
            default:
            {
                Debug.LogError("Offset FX code is incorrect");
                offsetFXMode = 0;
                break;
            }
        }

        cinemachineFollow.FollowOffset = _cinemachineOffset + _cinemachineOffsetFX;
        cinemachineLook.LookAtOffset = _cinemachineOffsetLook + _cinemachineOffsetFX;
    }
    Vector3 MakeOffset(float magnitude)
    {
        return new(
            Random.Range(-magnitude, magnitude),
            Random.Range(-magnitude, magnitude),
            0
        );
    }
}
