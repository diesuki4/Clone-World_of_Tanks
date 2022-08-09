using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 플레이어를 기준으로 3인칭 시점 : 360도 회전
// 플레이어를 기준으로 1인칭 시점으로 전환 가능
// 조준 모드
// 시점은 총 6개 : 원거리(3인칭 쿼터뷰) - 근거리(주포 포탑뷰) - 조준(1인칭 조준점 뷰)
// Shift 또는 MouseWheel 사용

// 마우스 우클릭을 한 상태에서 움직이면 주포 회전 없이 360도 회전 가능
// 이 경우, 포탑 및 주포 이동 정지
// 마우스 휠로 기본거리 - 중거리 - 원거리 이동

// 카메라는 Canvas의 조준점 UI 중앙을 바라본다
// 카메라의 Pivot은 포신을 기준으로 한다
// 

// 터렛은 조준점 ui 위치(즉 screen space의 중앙을 바라보도록 이동) / 마우스 이동과는 상관 없음(주포의 y값 정도만 받아옴)

public class LSJ_CamControl2 : MonoBehaviour
{

    
}
