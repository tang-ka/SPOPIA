using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YS_Spuit : MonoBehaviour
{
    // ������Ʈ
    public Color spuit;
    bool b_spuit = false;

    public RawImage rawImg;

    public GameObject stadium;

    private void Update()
    {
        if(b_spuit == true)
        {
            StartCoroutine(Spuit());

            // ������Ʈ�� ���� �� �־��ֱ�
            if (Input.GetMouseButtonDown(0))
            {
                rawImg.color = spuit;

                b_spuit = false;

                // ���׸��� �ٲ��ֱ� (custom color)
                stadium.transform.Find("Custom").transform.Find("Wall_outside").transform.Find("Wall_outside").GetComponent<MeshRenderer>().material.color = rawImg.color;
            }
        }
    }

    public void SpuitOn()
    {
        b_spuit = true;
    }

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
