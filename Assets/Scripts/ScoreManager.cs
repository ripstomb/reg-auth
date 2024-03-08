using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreManager : MonoBehaviour
{
    private string url = "https://sid-restapi.onrender.com";
    private string Token;

    private AuthHandler.Usuario Usuario;
    void Start()
    {
        Usuario = new AuthHandler.Usuario();
        Usuario.username = PlayerPrefs.GetString("username");

        Token = PlayerPrefs.GetString("token");
    }
    public void ActualizarScore(int score)
    {
        Usuario.data.score = score;

        StartCoroutine("SetScore", JsonUtility.ToJson(Usuario));
    }

    IEnumerator SetScore(string json)
    {
        UnityWebRequest request = UnityWebRequest.Put(url + "/api/usuarios/score", json);
        request.method = "PATCH";
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("x-token", Token);

        Debug.Log("Send Request SetScore");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            if (request.responseCode == 200)
            {
                Debug.Log("Setscore exitoso");
            }
            else
            {
                Debug.Log(request.responseCode + "|" + request.error);
            }
        }
    }
}
