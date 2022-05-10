using IngameConsole;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LogTest : MonoBehaviour {
    [SerializeField] private Sprite sprite;

    [SerializeField] private ConsoleViewer consoleViewer;

    private string GenerateRandomString(int length) {
        const string chars = "           abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[Random.Range(0, s.Length)]).ToArray());
    }

    private void Awake() {
        //IgConsole.Log("ASD", sprite);
        //IgConsole.Log("NullSprite", null);
        //Debug.Log(null);
        //string asd = null;
        //IgConsole.Log(asd);
        //Debug.Log("QWEQ");
        ////IgConsole.Log(RandomString(Random.Range(10, 200)), sprite);
        //Debug.Assert(true);
        //Debug.Assert(false);
        //Debug.LogError("Error!");
        //Debug.LogWarning("Warning!");
    }

    private IEnumerator Start() {
        for (int loop = 0; loop < 100; loop++) {
            Debug.Log(loop + "___" + GenerateRandomString(Random.Range(100, 300)));
        }
        //Debug.Log(qwer);


        IgConsole.OnSubmit += (string str) => {
            if (str.Contains("hehe")) {
                IgConsole.Log("HEHE??");
                try {
                    LogTest a = null;
                    bool enabled1 = a.enabled;
                } catch (Exception e) {
                    Debug.LogException(e);
                }
            }
            if (str.Contains("hide")) {
                IgConsole.Hide();
            }
            string[] vs = str.Split(' ');

            if (str.Equals("clear")) {
                IgConsole.Clear();
            }
        };

        int aasd = 0;
        for (int loop = 0; loop < 1000; loop++) {
            //Debug.Log(aasd++ + "....." + "asnlsnflksdgznzsbdjkbzkjsgbkzjlsdbgkjzsdgbzksdjbzdskgjbzkldsjzljkdsgbjzlb");
            Debug.Log(aasd++ + "....." + GenerateRandomString(Random.Range(10, 20)));
            Debug.Log(aasd++ + "....." + GenerateRandomString(Random.Range(10, 20)));
            Debug.Log(aasd++ + "....." + GenerateRandomString(Random.Range(100, 200)));
        }

        yield return null;


        while (true) {
            //for (int loop = 0; loop < 3000; loop++) {
            //Debug.Log(aasd++ + "....." + "asnlsnflksdgznzsbdjkbzkjsgbkzjlsdbgkjzsdgbzksdjbzdskgjbzkldsjzljkdsgbjzlb");
            Debug.Log(aasd++ + "....." + GenerateRandomString(Random.Range(10, 20)));
            Debug.Log(aasd++ + "....." + GenerateRandomString(Random.Range(10, 20)));
            Debug.Log(aasd++ + "....." + GenerateRandomString(Random.Range(100, 200)));

            //yield return new WaitForSeconds(0.1f);

            yield return null;
        }


        yield break;
        //yield return new WaitForSeconds(10);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.L)) {
            IgConsole.Log("....." + GenerateRandomString(Random.Range(1000, 3000)));
        }
    }
}