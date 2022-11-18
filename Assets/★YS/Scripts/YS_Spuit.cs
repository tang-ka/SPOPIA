using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YS_Spuit : MonoBehaviour
{
    // 스포이트
    public Color spuit;

    IEnumerator Spuit()
    {
        // ReadPixels함수는 현재 프레임 버퍼에서 픽셀들을 불러오기 때문에, 해당 프레임의 렌더링이 완전히 끝난 뒤 실행되어야 한다.
        // (즉, ReadPixels함수 보다 먼저 WaitForEndOfFrame이 실행되어야 한다.)
        yield return new WaitForEndOfFrame();

        // 스크린 샷 찍기
        Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, false);
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenShot.Apply();

        spuit = screenShot.GetPixel((int)Input.mousePosition.x, (int)Input.mousePosition.y);

        // 메모리 삭제(안해주면 과부하)
        Destroy(screenShot);
    }
}
