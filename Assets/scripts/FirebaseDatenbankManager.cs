using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using Firebase.Auth;

// Diese Klasse verwaltet die Verbindung zur Firebase-Datenbank und die Speicherung und das Laden von Benutzerdaten und Heizpumpenmodellen.
public class FirebaseDatenbankManager : MonoBehaviour
{
    // Referenz zur Datenbankwurzel in Firebase, über die wir auf alle Datenbankinhalte zugreifen können.
    private DatabaseReference reference;
    private FirebaseAuth auth;

    // Eine Liste, um die Heizpumpenmodelle zu speichern, die aus der Datenbank geladen werden
    public List<HeatingPumpModel> heatingPumpModels = new List<HeatingPumpModel>();

    // Die Startmethode wird beim Start des Spiels aufgerufen und initialisiert die Verbindung zur Firebase-Datenbank.
    void Start()
    {
        InitializeFirebase();
    }


    //Initialisiert Firebase und stellt die Verbindung zur Datenbank her.
    void InitializeFirebase()
    {
        // Prüft Firebase-Abhängigkeiten und initialisiert die App.
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            reference = FirebaseDatabase.DefaultInstance.RootReference; // Legt die Root-Referenz für alle Firebase-Daten fest.
            Debug.Log("Firebase Initialized"); // Bestätigung, dass Firebase initialisiert wurde.

            // Speichert einige vorab festgelegte (Mockup-)Benutzer- und Modelldaten in der Datenbank.
            //SaveMockupUserData();
            //SaveMockupModelData();

            // Lade die Heizpumpenmodelle nach der Initialisierung
            //LoadAllHeatingPumpModels();
        });
    }
    // Anmelden eines Benutzers
public void LoginUser(string email, string password, System.Action<FirebaseUser> onSuccess, System.Action<string> onError)
{
    auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => 
    {
        if (task.IsCanceled)
        {
            onError("Login fehlgeschlagen.");
            return;
        }
        if (task.IsFaulted)
        {
            onError("Hier ist leider ein Fehler unterlaufen: " + task.Exception?.InnerExceptions[0]?.Message);
            return;
        }

        AuthResult authResult = task.Result; // AuthResult anstelle von FirebaseUser
        FirebaseUser user = authResult.User; // Zugriff auf den FirebaseUser
        onSuccess(user);
    });
}


    // Registrieren eines Benutzers
   public void SignUpUser(string email, string password, string userName, System.Action<FirebaseUser> onSuccess, System.Action<string> onError)
{
    auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => 
    {
        if (task.IsCanceled)
        {
            onError("Registierung fehlgeschlagen");
            return;
        }
        if (task.IsFaulted)
        {
            onError("Hier ist leider ein Fehler unterlaufen: " + task.Exception?.InnerExceptions[0]?.Message);
            return;
        }

        AuthResult authResult = task.Result; // AuthResult anstelle von FirebaseUser
        FirebaseUser newUser = authResult.User; // Zugriff auf den FirebaseUser
        Debug.Log("Benutzer erfolgreich registriert: " + newUser.Email);

        // Benutzerinformationen in der Datenbank speichern
        SaveUserToDatabase(newUser.UserId, userName, email);
        onSuccess(newUser);
    });
}

    private void SaveUserToDatabase(string userId, string userName, string email)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        User user = new User {name = userName, email = email };
        string json = JsonUtility.ToJson(user);
        reference.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }
}


    

//     // Methode zum Speichern von vorab festgelegten Benutzerdaten als Mockup-Daten für Tests.
//     void SaveMockupUserData()
//     {
//         SaveUserData(new User { userId = "user1", name = "Amina El-Sayed", age = 29, email = "amina.elsayed@example.com" });
//         SaveUserData(new User { userId = "user2", name = "Lukas Müller", age = 33, email = "lukas.mueller@example.com" });
//         SaveUserData(new User { userId = "user3", name = "Mei Chen", age = 44, email = "mei.chen@example.com" });
//         SaveUserData(new User { userId = "user4", name = "Fiepko Kühn", age = 28, email = "fiepko.kuehn@example.com" });
//         SaveUserData(new User { userId = "user5", name = "Folkert Knoop", age = 60, email = "folkert.knoop@example.com" });
//         SaveUserData(new User { userId = "user6", name = "Claudia Kühn", age = 59, email = "claudia.kuehn@example.com" });
//     }

//     // Speichert die Daten eines einzelnen Benutzers in der Datenbank unter dem Knoten "users".
//     public void SaveUserData(User user)
//     {
//         // Benutzerinformationen werden unter dem User-ID-Knoten gespeichert.
//         reference.Child("users").Child(user.userId).Child("name").SetValueAsync(user.name);
//         reference.Child("users").Child(user.userId).Child("age").SetValueAsync(user.age);
//         reference.Child("users").Child(user.userId).Child("email").SetValueAsync(user.email).ContinueWithOnMainThread(task =>
//         {
//             if (task.IsCompleted)
//             {
//                 Debug.Log($"User data for {user.name} saved successfully"); // Bestätigung, dass die Daten gespeichert wurden.
//             }
//         });
//     }

// //Registierung
//      //public void RegisterUser(string email, string password, string name, int age)
//     // {
//     //     auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
//     //     {
//     //         if (task.IsFaulted)
//     //         {
//     //             Debug.LogError("User Registration Failed: " + task.Exception);
//     //             return;
//     //         }

//     //         // Benutzer erfolgreich registriert
//     //         FirebaseUser newUser = task.Result;
//     //         User user = new User
//     //         {
//     //             userId = newUser.UserId,
//     //             name = name,
//     //             age = age,
//     //             email = email
//     //         };

//     //         // Speichert die Benutzerdaten in der Datenbank
//     //         SaveUserData(user);
//     //         Debug.Log("User registered successfully: " + newUser.UserId);
//     //     });
//     // }

//     // Anmeldung eines bestehenden Benutzers
//     // public void LoginUser(string email, string password)
//     // {
//     //     auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
//     //     {
//     //         if (task.IsFaulted)
//     //         {
//     //             Debug.LogError("User Login Failed: " + task.Exception);
//     //             return;
//     //         }

//     //         // Benutzer erfolgreich angemeldet
//     //         FirebaseUser user = task.Result;
//     //         Debug.Log("User logged in successfully: " + user.UserId);

//     //         // Hier kannst du die Benutzerdaten laden oder zur Profilseite navigieren
//     //         LoadUserData(user.UserId);
//     //     });
//     // }

//     // Methode zum Laden von Benutzerdaten aus der Datenbank
//     public void LoadUserData(string userId)
//     {
//         reference.Child("users").Child(userId)
//             .GetValueAsync().ContinueWithOnMainThread(task =>
//             {
//                 if (task.IsCompleted)
//                 {
//                     DataSnapshot snapshot = task.Result;
//                     if (snapshot.Exists)
//                     {
//                         User user = new User
//                         {
//                             userId = userId,
//                             name = snapshot.Child("name").Value.ToString(),
//                             age = int.Parse(snapshot.Child("age").Value.ToString()),
//                             email = snapshot.Child("email").Value.ToString()
//                         };
//                         Debug.Log($"User data - Name: {user.name}, Age: {user.age}, Email: {user.email}");
//                     }
//                     else
//                     {
//                         Debug.Log("User data does not exist.");
//                     }
//                 }
//             });
//     }


//     // Methode zum Speichern von vorab festgelegten Heizpumpenmodellen als Mockup-Daten.
//     void SaveMockupModelData()
//     {
//         SaveHeatingPumpModel("model1", new HeatingPumpModel
//         {
//             modelName = "Vaillant VWS 14/17 kW",
//             description = "High-efficiency geothermal heat pump for medium to large residential needs.",
//             priceRange = "8000-10000",
//             threeDModelUrl = "https://example.com/vws_model.glb",
//             size = new Size { depth = "600mm", height = "1200mm", width = "1500mm" },
//             capacities = "17 kW"
//         });

//         SaveHeatingPumpModel("model2", new HeatingPumpModel
//         {
//             modelName = "Vaillant flexoTHERM",
//             description = "Versatile heat pump for different heat sources, providing eco-friendly heating.",
//             priceRange = "10000-14000",
//             threeDModelUrl = "https://example.com/flexotherm_model.glb",
//             size = new Size { depth = "900mm", height = "1400mm", width = "1100mm" },
//             capacities = "15 and 19 kW"
//         });

//         SaveHeatingPumpModel("model3", new HeatingPumpModel
//         {
//             modelName = "LG Therma V Monobloc",
//             description = "Energy-efficient heat pump for heating and cooling with smart controls.",
//             priceRange = "7000-10000",
//             threeDModelUrl = "URL_to_LG_ThermaV_3D_Model",
//             size = new Size { depth = "330mm", height = "1380mm", width = "950mm" },
//             capacities = "16 kW"
//         });
//     }

//     // Speichert die Daten eines Heizpumpenmodells in der Datenbank unter dem Knoten "heatingPumpModels".
//     public void SaveHeatingPumpModel(string modelId, HeatingPumpModel model)
//     {
//         var modelRef = reference.Child("heatingPumpModels").Child(modelId);
//         modelRef.Child("modelName").SetValueAsync(model.modelName);
//         modelRef.Child("description").SetValueAsync(model.description);
//         modelRef.Child("priceRange").SetValueAsync(model.priceRange);
//         modelRef.Child("threeDModelUrl").SetValueAsync(model.threeDModelUrl);
//         modelRef.Child("size").Child("depth").SetValueAsync(model.size.depth);
//         modelRef.Child("size").Child("height").SetValueAsync(model.size.height);
//         modelRef.Child("size").Child("width").SetValueAsync(model.size.width);
//         modelRef.Child("capacities").SetValueAsync(model.capacities).ContinueWithOnMainThread(task =>
//         {
//             if (task.IsCompleted)
//             {
//                 Debug.Log($"Heating pump model data for {model.modelName} saved successfully");
//             }
//         });
//     }

//     // Lädt alle Heizpumpenmodelle aus der Datenbank und speichert sie in der Liste heatingPumpModels.
//     public void LoadAllHeatingPumpModels()
//     {
//         reference.Child("heatingPumpModels")
//             .GetValueAsync().ContinueWithOnMainThread(task =>
//             {
//                 if (task.IsCompleted)
//                 {
//                     DataSnapshot snapshot = task.Result;
//                     if (snapshot.Exists)
//                     {
//                         heatingPumpModels.Clear(); // Leere die Liste vor dem Laden neuer Modelle
//                         foreach (var childSnapshot in snapshot.Children)
//                         {
//                             HeatingPumpModel model = new HeatingPumpModel
//                             {
//                                 modelName = childSnapshot.Child("modelName").Value.ToString(),
//                                 description = childSnapshot.Child("description").Value.ToString(),
//                                 priceRange = childSnapshot.Child("priceRange").Value.ToString(),
//                                 threeDModelUrl = childSnapshot.Child("threeDModelUrl").Value.ToString(),
//                                 capacities = childSnapshot.Child("capacities").Value.ToString(),
//                                 size = new Size
//                                 {
//                                     depth = childSnapshot.Child("size").Child("depth").Value.ToString(),
//                                     height = childSnapshot.Child("size").Child("height").Value.ToString(),
//                                     width = childSnapshot.Child("size").Child("width").Value.ToString()
//                                 }
//                             };
//                             heatingPumpModels.Add(model); // Füge das Modell der Liste hinzu
//                         }
//                         Debug.Log($"Loaded {heatingPumpModels.Count} heating pump models from the database.");
//                     }
//                     else
//                     {
//                         Debug.Log("No heating pump models found in the database.");
//                     }
//                 }
//             });
//     }

// }    // Lädt die Daten eines Heizpumpenmodells aus der Datenbank mith
