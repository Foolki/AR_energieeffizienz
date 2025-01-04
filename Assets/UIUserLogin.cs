using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using System;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class UIUserLogin : MonoBehaviour
{
    public GameObject loginPanel, profilePanel, signupPanel;
    public TMP_InputField loginEmail, loginPassword, signupEmail, signupPassword, signupCPassword, signupUserName;
    public Button loginButton;
    public Button signUpButton;
    public Text notif_Title_text, notif_Message_Text;

    private FirebaseAuth auth;
    private FirebaseUser user;
    private bool isSignIn = false;

    void Start()
    {
        InitializeFirebase(); // Initialisiert Firebase
    }

    public void OpenLoginPanel()
    {
        loginPanel.SetActive(true);
        signupPanel.SetActive(false);
        profilePanel.SetActive(false);
    }

    public void OpenSignUpPanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(true);
        profilePanel.SetActive(false);
    }

    // public void OpenProfilePanel()
    // {
    //     loginPanel.SetActive(false);
    //     signupPanel.SetActive(false);
    //     profilePanel.SetActive(true);
    // }

    public void showNotificationMessage(string title, string message)
    {
        // Aktualisiere die UI-Textfelder mit dem Titel und der Nachricht
        notif_Title_text.text = title;
        notif_Message_Text.text = message;
    }

    public void LoginUser()
    {
        if (string.IsNullOrEmpty(loginEmail.text) || string.IsNullOrEmpty(loginPassword.text))
        {
            showNotificationMessage("Fehler", "Felder sind leer! Bitte geben Sie Details in alle Felder ein");
            return;
        }

        // Do Login
        SignInUser(loginEmail.text, loginPassword.text);
    }

    public void SignUpUser()
    {
        if (string.IsNullOrEmpty(signupEmail.text) || string.IsNullOrEmpty(signupPassword.text) || string.IsNullOrEmpty(signupCPassword.text) || string.IsNullOrEmpty(signupUserName.text))
        {
            showNotificationMessage("Fehler", "Felder sind leer! Bitte geben Sie Details in alle Felder ein");
            return;
        }

        // Do SignUp
        CreateUser(signupEmail.text, signupPassword.text, signupUserName.text);
    }

    public void SignInUser(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }
            if (task.IsCompleted)
            {
                var authResult = task.Result;
                FirebaseUser newUser = authResult.User;
                Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
                //OpenProfilePanel(); // Wechsel zur Profilansicht nach erfolgreichem Login
            }
        });
    }

    public void CreateUser(string email, string password, string username)
    {
        if (password != signupCPassword.text)
        {
            Debug.LogError("Passwords do not match.");
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }
            if (task.IsCompleted)
            {
                var authResult = task.Result;
                FirebaseUser newUser = authResult.User;
                Debug.LogFormat("Firebase user created successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);

                // Benutzername aktualisieren
                UserProfile profile = new UserProfile { DisplayName = username };
                newUser.UpdateUserProfileAsync(profile).ContinueWith(updateTask => {
                    if (updateTask.IsCanceled)
                    {
                        Debug.LogError("UpdateUserProfileAsync was canceled.");
                        return;
                    }
                    if (updateTask.IsFaulted)
                    {
                        Debug.LogError("UpdateUserProfileAsync encountered an error: " + updateTask.Exception);
                        return;
                    }
                    if (updateTask.IsCompleted)
                    {
                        Debug.Log("User profile updated successfully.");
                        //OpenProfilePanel(); // Wechsel zur Profilansicht nach erfolgreichem Erstellen und Aktualisieren
                    }
                });
            }
        });
    }

    void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                auth = FirebaseAuth.DefaultInstance;
                auth.StateChanged += AuthStateChanged;
                AuthStateChanged(this, null);
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }

    void AuthStateChanged(object sender, EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                isSignIn = true;
            }
        }
    }
}
