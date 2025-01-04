using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ModelLoader : MonoBehaviour
{
    public ARTapToPlace taptoPlace;
    public Button model1Button;
    public Button model2Button;
    public Button model3Button;
    public GameObject model1Prefab;
    public GameObject model2Prefab;
    public GameObject model3Prefab;
    public AudioSource audioSource; 

private GameObject currentObject;
void OnEnable(){
    model1Button.onClick.AddListener(()=> LoadSelectedModel(model1Prefab));
    model2Button.onClick.AddListener(()=> LoadSelectedModel(model2Prefab));
    model3Button.onClick.AddListener(()=> LoadSelectedModel(model3Prefab));
    
    //Informationaudio Wärmepumpen
    model1Button.onClick.AddListener(() => {LoadSelectedModel(model1Prefab);audioSource.Play(); // Audio abspielen
    model2Button.onClick.AddListener(() => {LoadSelectedModel(model2Prefab);audioSource.Play(); 
    model3Button.onClick.AddListener(() => {LoadSelectedModel(model3Prefab);audioSource.Play(); 
    });
    });
    });


}

    void LoadSelectedModel(GameObject selectedModel)
    {
        if (selectedModel != null)
        {
            //Aktuelles Objekt ersetzen
            if(currentObject != null)
            Destroy(currentObject);

            currentObject = Instantiate(selectedModel, Vector3.zero, Quaternion.identity); // Instanzierung im Ursprungspunkt
            taptoPlace.SetModel(currentObject);
        }
        else
        {
            Debug.LogError("Kein Modell ausgewählt oder Modell-ID ungültig.");
        }


    }
}
