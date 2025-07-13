using UnityEngine;
using UnityEngine.InputSystem;

public class MoveSpotJS : MonoBehaviour
{
    public Transform player; // XR Rig 또는 카메라 리그
    public LayerMask markerLayer;
    // public float rayLength = 10f;
    public GameObject markerPrefab;
    private GameObject markerInstance;

    public GameObject video_000start;
    public GameObject video_001move;


    void Start()
    {
        // if (markerPrefab != null)
        //     markerInstance = Instantiate(markerPrefab);
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("interaction test");
            Vector2 mousePos = Mouse.current.position.ReadValue();

            // 카메라에서 마우스 위치로 레이 생성
            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            // 레이캐스트 실행, 최대거리 무한대, markerLayer 레이어만 충돌 체크
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, markerLayer))
            {
                if (hit.collider.CompareTag("movePoint"))
                {
                    // player 위치를 클릭한 지점으로 이동 (y축 유지)
                    // player.position = new Vector3(hit.point.x, player.position.y, hit.point.z);
                    hit.collider.gameObject.SetActive(false);

                    Debug.Log("move success");

                    if (video_000start != null) video_000start.SetActive(false);
                    if (video_001move != null) video_001move.SetActive(true);

                    Invoke("ActivateVideoStart", 10f);
                }
            }
        }

        // 마커 위치 표시
        // if (markerInstance != null)
        // {
        //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //     if (Physics.Raycast(ray, out RaycastHit hit, markerLayer))
        //     {
        //         markerInstance.transform.position = hit.point + Vector3.up * 0.01f;
        //     }
        // }
    }
    
    void ActivateVideoStart()
    {
        if (video_001move != null) video_001move.SetActive(false);
        if (video_000start != null) video_000start.SetActive(true);
    }
}
