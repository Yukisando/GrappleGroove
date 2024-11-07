#region

using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

#endregion

namespace HelloWorld
{
    public class HelloWorldManager : MonoBehaviour
    {
        Button clientButton;
        Button hostButton;
        VisualElement rootVisualElement;
        Button serverButton;
        Label statusLabel;

        void Update() {
            UpdateUI();
        }

        void OnEnable() {
            var uiDocument = GetComponent<UIDocument>();
            rootVisualElement = uiDocument.rootVisualElement;

            hostButton = CreateButton("HostButton", "Host");
            clientButton = CreateButton("ClientButton", "Client");
            serverButton = CreateButton("ServerButton", "Server");
            statusLabel = CreateLabel("StatusLabel", "Not Connected");

            rootVisualElement.Clear();
            rootVisualElement.Add(hostButton);
            rootVisualElement.Add(clientButton);
            rootVisualElement.Add(serverButton);
            rootVisualElement.Add(statusLabel);

            hostButton.clicked += OnHostButtonClicked;
            clientButton.clicked += OnClientButtonClicked;
            serverButton.clicked += OnServerButtonClicked;
        }

        void OnDisable() {
            hostButton.clicked -= OnHostButtonClicked;
            clientButton.clicked -= OnClientButtonClicked;
            serverButton.clicked -= OnServerButtonClicked;
        }

        void OnHostButtonClicked() {
            NetworkManager.Singleton.StartHost();
        }

        void OnClientButtonClicked() {
            NetworkManager.Singleton.StartClient();
        }

        void OnServerButtonClicked() {
            NetworkManager.Singleton.StartServer();
        }

        // Disclaimer: This is not the recommended way to create and stylize the UI elements, it is only utilized for the sake of simplicity.
        // The recommended way is to use UXML and USS. Please see this link for more information: https://docs.unity3d.com/Manual/UIE-USS.html
        Button CreateButton(string name, string text) {
            var button = new Button();
            button.name = name;
            button.text = text;
            button.style.width = 240;
            button.style.backgroundColor = Color.white;
            button.style.color = Color.black;
            button.style.unityFontStyleAndWeight = FontStyle.Bold;
            return button;
        }

        Label CreateLabel(string name, string content) {
            var label = new Label {
                name = name,
                text = content,
                style = {
                    color = Color.black,
                    fontSize = 18,
                },
            };
            return label;
        }

        void UpdateUI() {
            if (NetworkManager.Singleton == null) {
                SetStartButtons(false);
                SetStatusText("NetworkManager not found");
                return;
            }

            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer) {
                SetStartButtons(true);
                SetStatusText("Not connected");
            }
            else {
                SetStartButtons(false);
                UpdateStatusLabels();
            }
        }

        void SetStartButtons(bool state) {
            hostButton.style.display = state ? DisplayStyle.Flex : DisplayStyle.None;
            clientButton.style.display = state ? DisplayStyle.Flex : DisplayStyle.None;
            serverButton.style.display = state ? DisplayStyle.Flex : DisplayStyle.None;
        }

        void SetStatusText(string text) {
            statusLabel.text = text;
        }

        void UpdateStatusLabels() {
            string mode = NetworkManager.Singleton.IsHost ? "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";
            string transport = "Transport: " + NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name;
            string modeText = "Mode: " + mode;
            SetStatusText($"{transport}\n{modeText}");
        }
    }
}