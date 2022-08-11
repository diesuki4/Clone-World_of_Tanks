using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 카메라 시점 1 : 3인칭 기본 뷰
// 카메라는 탱크를 중심으로 마우스 상하좌우 및 마우스 휠스크롤로 이동 및 회전한다
// 기본 카메라의 위치는 탱크의 뒷편
// 조준선 UI가 스크린의 중앙에 위치(조준선 UI는 알파값이 낮아 시야에 크게 방해되지 않음)
// 카메라 위치는 6단계로 구성
// 저격 뷰 / barrel 뷰 / turret 뷰 / 근거리 3인칭 뷰(기본) / 중거리 3인칭 뷰 / 원거리 3인칭 뷰 
// 특징 : 마우스 휠로 시점 변환이 매우 빠름

// 카메라 시점 2 : 원거리 탑 뷰


public class LSJ_CamControl2 : MonoBehaviour
{
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
