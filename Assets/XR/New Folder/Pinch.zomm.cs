using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchZoom : MonoBehaviour
{
    private float initialDistance;
    private Vector3 initialScale;

    // Modell-IDs zur Identifizierung der Modelle
    private string modelId;

    void Start()
    {
        // Modell-ID basierend auf dem Namen des GameObjects setzen
        modelId = gameObject.name; // Angenommen, das GameObject hat den Namen des Modells (model1, model2, model3)
    }

    void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                initialDistance = Vector2.Distance(touch1.position, touch2.position);
                initialScale = transform.localScale;
            }
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                float currentDistance = Vector2.Distance(touch1.position, touch2.position);
                if (Mathf.Approximately(initialDistance, 0)) return;

                float scaleFactor = currentDistance / initialDistance;

                // Verhindern, dass das Modell zu klein oder zu groß wird
                float newScaleX = initialScale.x * scaleFactor;
                float newScaleY = initialScale.y * scaleFactor;
                float newScaleZ = initialScale.z * scaleFactor;

                float minScale = 0.5f; // Minimale Größe
                float maxScale = 3f;   // Maximale Größe

                newScaleX = Mathf.Clamp(newScaleX, minScale, maxScale);
                newScaleY = Mathf.Clamp(newScaleY, minScale, maxScale);
                newScaleZ = Mathf.Clamp(newScaleZ, minScale, maxScale);

                transform.localScale = new Vector3(newScaleX, newScaleY, newScaleZ);

                // Modellspezifische Interaktionen
                switch (modelId)
                {
                    case "model1":
                        Debug.Log("Interaktion mit Vaillant VWS 14/17 kW");
                        // Füge hier spezifische Logik für model1 hinzu.
                        break;

                    case "model2":
                        Debug.Log("Interaktion mit Vaillant flexoTHERM");
                        // Füge hier spezifische Logik für model2 hinzu.
                        break;

                    case "model3":
                        Debug.Log("Interaktion mit LG Therma V Monobloc");
                        // Füge hier spezifische Logik für model3 hinzu.
                        break;

                    default:
                        Debug.Log("Unbekanntes Modell angeklickt.");
                        break;
                }
            }
        }
    }
}
