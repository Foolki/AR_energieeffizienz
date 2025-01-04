using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CatalogManager : MonoBehaviour
{
    public GameObject catalogItemPrefab; // UI-Element (Prefab) für ein Katalogmodell
    public Transform catalogContent; // Container für die Katalogelemente

    // Aktualisiert den Katalog und zeigt die Modelle an
    public void UpdateCatalog(List<HeatingPumpModel> models)
    {
        foreach (Transform child in catalogContent)
        {
            Destroy(child.gameObject); // Entferne alte Katalogelemente
        }

        foreach (var model in models)
        {
            GameObject item = Instantiate(catalogItemPrefab, catalogContent);
            item.GetComponentInChildren<Text>().text = model.modelName; // Setze Namen im UI

            // Füge weitere Details wie Bilder oder Beschreibungen hinzu, falls erforderlich
            Button selectButton = item.GetComponentInChildren<Button>();
            selectButton.onClick.AddListener(() => OnModelSelected(model));
        }
    }

    public void OnModelSelected(HeatingPumpModel selectedModel)
    {
        Debug.Log($"Selected Model: {selectedModel.modelName}");
        // Implementiere hier die Logik zum Starten der AR-Szene oder zum Speichern des ausgewählten Modells
    }
}

    