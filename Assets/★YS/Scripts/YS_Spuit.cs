using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YS_Spuit : MonoBehaviour
{
    // ������Ʈ
    public Color spuit;

    IEnumerator Spuit()
    {
        // ReadPixels�Լ��� ���� ������ ���ۿ��� �ȼ����� �ҷ����� ������, �ش� �������� �������� ������ ���� �� ����Ǿ�� �Ѵ�.
        // (��, ReadPixels�Լ� ���� ���� WaitForEndOfFrame�� ����Ǿ�� �Ѵ�.)
        yield return new WaitForEndOfFrame();

        // ��ũ�� �� ���
        Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, false);
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenShot.Apply();

        spuit = screenShot.GetPixel((int)Input.mousePosition.x, (int)Input.mousePosition.y);

        // �޸� ����(�����ָ� ������)
        Destroy(screenShot);
    }
}
