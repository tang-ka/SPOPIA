using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YS_Capture : MonoBehaviour
{
    // ΩÃ±€≈Ê
    public static YS_Capture instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Camera cam;
    public Image img;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shot()
    {
        StartCoroutine(nameof(Capture));
    }

    IEnumerator Capture()
    {
        yield return new WaitForEndOfFrame();

        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        cam.targetTexture = rt;
        cam.Render();

        Texture2D t = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
        RenderTexture.active = rt;

        t.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        t.Apply();

        cam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        Sprite captureImg = Sprite.Create(t, new Rect(0, 0, Screen.width, Screen.height), Vector2.one * 0.5f);

        img.sprite = captureImg;
    }
}
