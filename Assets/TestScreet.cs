using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Text.RegularExpressions;

public class TestScreet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string a = "«—±€¿Œµ• ≈◊Ω∫∆Æ¿‘¥œ¥Ÿ¿Ã∞≈ øµπÆ≥≠ºˆ∑Œ∫Ø»Ø«œ∞Ì ∆ØºˆπÆ¡¶¡¶∞≈";

        string b = EncryptStringData(a);

        string c = Regex.Replace(b, @"[^a-zA-Z0-9∞°-∆R_]", "", RegexOptions.Singleline);

        Debug.Log(c);
    }
    string EncryptStringData(string channelName) // password ¿Ã¬ ¿∏∑Œ ª©º≠ ¿⁄¡÷ ∫Ø∞Ê
    {
        string Password = "9430582907839242";
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(Password);
        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(channelName);

        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.Mode = CipherMode.ECB;
        rDel.Padding = PaddingMode.PKCS7;
        ICryptoTransform cTransform = rDel.CreateEncryptor();

        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        Debug.Log("EncryptStringData : " + Convert.ToBase64String(resultArray, 0, resultArray.Length));
        return Convert.ToBase64String(resultArray, 0, resultArray.Length);

    }
}
