using System;
using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Messaging;

public class FirebaseMessagingManager : MonoBehaviour
{
    public static FirebaseMessagingManager Instance { get; private set; }

    private bool _isFirebaseInitialized = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeFirebaseMessaging();
    }

    private void InitializeFirebaseMessaging()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Register Cloud Messaging Handlers
                FirebaseMessaging.TokenReceived += OnTokenReceived;
                FirebaseMessaging.MessageReceived += OnMessageReceived;

                _isFirebaseInitialized = true;
                Debug.Log("[Firebase Messaging] Initialized successfully.");
            }
            else
            {
                Debug.LogError($"[Firebase Messaging] Could not resolve dependencies: {dependencyStatus}");
            }
        });
    }

    private void OnDestroy()
    {
        if (_isFirebaseInitialized)
        {
            FirebaseMessaging.TokenReceived -= OnTokenReceived;
            FirebaseMessaging.MessageReceived -= OnMessageReceived;
        }
    }

    /// <summary>
    /// Triggered when a new FCM registration token is generated for the device.
    /// Send this token to your backend if you target specific players with push notifications.
    /// </summary>
    private void OnTokenReceived(object sender, TokenReceivedEventArgs token)
    {
        Debug.Log($"[Firebase Messaging] FCM Device Token: {token.Token}");
        // Optional: Save or upload token to PlayFab/server
    }

    /// <summary>
    /// Triggered when a notification is received while the app is in the foreground.
    /// </summary>
    private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        Debug.Log($"[Firebase Messaging] Received message from: {e.Message.From}");

        if (e.Message.Notification != null)
        {
            Debug.Log($"[Firebase Messaging] Title: {e.Message.Notification.Title}");
            Debug.Log($"[Firebase Messaging] Body: {e.Message.Notification.Body}");
        }

        // Check for custom key-value payload data sent with the push notification
        if (e.Message.Data.Count > 0)
        {
            foreach (var keyValuePair in e.Message.Data)
            {
                Debug.Log($"[Firebase Messaging] Payload Data -> Key: {keyValuePair.Key}, Value: {keyValuePair.Value}");
            }
        }
    }
}