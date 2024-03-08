using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class AuthenticationManager : MonoBehaviour
{
    private string url = "https://sid-restapi.onrender.com";

    public string Token { get; set; }
    public string Username { get; set; }

    public GameObject PanelAuth;
    public GameObject PanelMenu;
    void Start()
    {
        Token = PlayerPrefs.GetString("token");
        if (string.IsNullOrEmpty(Token))
        {
            Debug.Log("No hay token");
            PanelAuth.SetActive(true);
        }
        else
        {
            Username = PlayerPrefs.GetString("username");
            StartCoroutine("GetProfile");
        }
    }

    public void enviarRegistro()
    {
        AuthHandler.AuthData data = new AuthHandler.AuthData();
        data.username = GameObject.Find("InputFieldUsername").GetComponent<TMP_InputField>().text;
        data.password = GameObject.Find("InputFieldPassword").GetComponent<TMP_InputField>().text;

        StartCoroutine("Registro", JsonUtility.ToJson(data));
    }
    public void enviarLogin()
    {
        AuthHandler.AuthData data = new AuthHandler.AuthData();
        data.username = GameObject.Find("InputFieldUsername").GetComponent<TMP_InputField>().text;
        data.password = GameObject.Find("InputFieldPassword").GetComponent<TMP_InputField>().text;

        StartCoroutine("Login", JsonUtility.ToJson(data));
    }
    IEnumerator Registro(string json)
    {
        UnityWebRequest request = UnityWebRequest.Put(url + "/api/usuarios", json);
        request.method = UnityWebRequest.kHttpVerbPOST;
        request.SetRequestHeader("Content-Type", "application/json");
        Debug.Log("Send Request Registro");
        yield return request.SendWebRequest();


        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.GetRequestHeader("Content-Type"));
            Debug.Log(request.downloadHandler.text);

            if (request.responseCode == 200)
            {
                Debug.Log("Registro exitoso");
                StartCoroutine("Login", json);
            }
            else
            {
                Debug.Log(request.responseCode + "|" + request.error);
            }
        }
    }
    IEnumerator Login(string json)
    {
        UnityWebRequest request = UnityWebRequest.Put(url + "/api/auth/login", json);
        request.method = UnityWebRequest.kHttpVerbPOST;
        request.SetRequestHeader("Content-Type", "application/json");
        Debug.Log("Send Request Login");
        yield return request.SendWebRequest();


        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.GetRequestHeader("Content-Type"));
            Debug.Log(request.downloadHandler.text);

            if (request.responseCode == 200)
            {
                Debug.Log("Login exitoso");

                AuthHandler.AuthData data = JsonUtility.FromJson<AuthHandler.AuthData>(request.downloadHandler.text);
                Username = data.usuario.username;
                Token = data.token;

                PlayerPrefs.SetString("token", Token);
                PlayerPrefs.SetString("username", Username);

                PanelAuth.SetActive(false);
                PanelMenu.SetActive(true);
            }
            else
            {
                Debug.Log(request.responseCode + "|" + request.error);
                PanelAuth.SetActive(true);
            }
        }
    }
    IEnumerator GetProfile()
    {
        UnityWebRequest request = UnityWebRequest.Get(url + "/api/usuarios" + Username);
        request.SetRequestHeader("x-token", Token);
        Debug.Log("Send Request GetProfile");
        yield return request.SendWebRequest();


        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {

            if (request.responseCode == 200)
            {
                Debug.Log("Get Profile exitoso");

                AuthHandler.AuthData data = JsonUtility.FromJson<AuthHandler.AuthData>(request.downloadHandler.text);

                Debug.Log("El Usuario " + data.usuario.username + " se encuentra autenticado y su puntaje es: " + data.usuario.data.score);

                PanelMenu.SetActive(true);
            }
            else
            {
                PanelAuth.SetActive(true);
                Debug.Log(request.responseCode + "|" + request.error);
            }
        }
    }
}
