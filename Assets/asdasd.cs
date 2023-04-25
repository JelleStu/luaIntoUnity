using System.Collections;
using System.Collections.Generic;
using System.IO;
using LuaBridge.Core.Events;
using LuaBridge.Unity.Scripts.LuaBridgeHelpers.JSonSerializer;
using Services;
using UnityEngine;
using UnityEngine.UI;

public class asdasd : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
      LoadSprite();
    }

    private async void LoadSprite()
    { EventBus.Factory.Create();
        var fs = new FileService(new JsonSerializer());
        var tex = await fs.LoadTexture( $"{Path.Combine(Application.dataPath, "LuaBridge", "Unity", "Tests", "Assets", "Images", "test_image_1.png")}");
        var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f));
        GetComponent<Image>().sprite = sprite;
    }
    
}
