using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Internal;

public class PhotoManager : MonoBehaviour
{
    public GetData getData;

    public static PhotoManager instance = null;

    public string entityId;
    public string entityType;

    public int GlobalFileLock = 0;
    private readonly Dictionary<string, string> _entityFileJson = new Dictionary<string, string>();
    public string ActiveUploadFileName;

    public Image SignUp_Dog1;
    public Button NextBtn;

   
    public OtherUserManager otherUserManager;

    public OtherUserDataView OUDV;

    public OtherUserData OUD;

    public string otherUserEntityID;
    public int photoIndex;

    void Awake()
    {
       
        if (instance == null) //instance?? null. ??, ?????????? ???????? ???? ??????
        {
            instance = this; //???????? instance?? ??????????.
            DontDestroyOnLoad(this.gameObject); //OnLoad(???? ???? ????????) ?????? ???????? ???? ????
        }

        else
        {
            if (instance != this) //instance?? ???? ???????? ???? instance?? ???? ???????? ?????? ????
                Destroy(this.gameObject); //?? ???? ???????? ?????? ???????? ???? AWake?? ?????? ????
        }
    }


    public void PlayerPhotoSelect()
    {
        if (NativeGallery.IsMediaPickerBusy())
        {
            return;
        }
        PickImage(512, 0); // ???? 
    }

    public void Dog1PhotoSelect()
    {
        if (NativeGallery.IsMediaPickerBusy())
        {
            return;
        }
        PickImage(512, 1); // ???? 
    }

    public void Dog2PhotoSelect()
    {
        if (NativeGallery.IsMediaPickerBusy())
        {
            return;
        }
        PickImage(512, 2); // ???? 
    }

    public void Dog3PhotoSelect()
    {
        if (NativeGallery.IsMediaPickerBusy())
        {
            return;
        }
        PickImage(512, 3); // ???? 
    }

  

    public void OnLogin(PlayFab.ClientModels.LoginResult result)
    {
        entityId = result.EntityToken.Entity.Id;
        entityType = result.EntityToken.Entity.Type;

        // LoadAllFiles();
    }


    private void PickImage(int maxSize, int PhotoNumber)
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                // Create Texture from selected image
             
                
                Texture2D texture = CropTextureToSquare( NativeGallery.LoadImageAtPath(path, maxSize));
                // 1
                //_profilePhoto.texture = texture; // ?????????? ?????? _profilePhoto ??????
                //
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                

                if (PhotoNumber == 1)
                {
                    //System.IO.File.Copy(path, Application.persistentDataPath + "/SavedImage1.png", true);

                    //texture.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
                    //texture.Apply();
                    byte[] bytes = texture.EncodeToPNG();
                    File.WriteAllBytes(Application.persistentDataPath + "/SavedImage1.png", bytes);
                    //DestroyImmediate(texture);

                    if (SignUp_Dog1 != null)
                    {
                        SignUp_Dog1.gameObject.SetActive(true);
                        SignUp_Dog1.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

                        NextBtn.interactable = true;
                    }


                    if(getData !=null)
                    {
                        getData.dog1_image.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                        getData.dog1_icon.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                        getData.petCard1.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                    }


                }
                else if(PhotoNumber == 2)
                {
                    //System.IO.File.Copy(path, Application.persistentDataPath + "/SavedImage2.png", true);
                    //texture.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
                    //texture.Apply();
                    byte[] bytes = texture.EncodeToPNG();
                    File.WriteAllBytes(Application.persistentDataPath + "/SavedImage2.png", bytes);
                    //DestroyImmediate(texture);
                    if (getData != null)
                    {
                        getData.dog2_image.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                        getData.dog2_icon.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                        getData.petCard2.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                    }

                }
                else if (PhotoNumber == 3)
                {
                    //System.IO.File.Copy(path, Application.persistentDataPath + "/SavedImage3.png", true);
                    //texture.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
                    //texture.Apply();
                    byte[] bytes = texture.EncodeToPNG();
                    File.WriteAllBytes(Application.persistentDataPath + "/SavedImage3.png", bytes);
                    //DestroyImmediate(texture);
                    if (getData != null)
                    {
                        getData.dog3_image.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                        getData.dog3_icon.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                        getData.petCard3.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                    }

                }
                else
                {
                    //System.IO.File.Copy(path, Application.persistentDataPath + "/SavedImage0.png", true);
                    //texture.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
                    //texture.Apply();
                    byte[] bytes = texture.EncodeToPNG();
                    File.WriteAllBytes(Application.persistentDataPath + "/SavedImage1.png", bytes);
                    //DestroyImmediate(texture);
                    if (getData.player_image != null)
                    {
                        getData.player_image.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                    }

                }


                //Destroy( texture, 5f ); //5???? ??????

                UploadFile(entityId + PhotoNumber, PhotoNumber);
            }
        }, "Select a PNG image", "image/png");

        Debug.Log("Permission result: " + permission);
    }

    /// <summary>
    /// Native Gallary
    /// </summary>

    //crop
    public Texture2D CropTextureToSquare(Texture2D texture)
    {
        int minLength = Mathf.Min(texture.width, texture.height);
        Texture2D squareTexture = new Texture2D(minLength, minLength, TextureFormat.ARGB32, false);

        Color[] data2 = texture.GetPixels((int)((texture.width - minLength) * .5f),
                                          (int)((texture.height - minLength) * .5f),
                                          minLength, minLength);
        squareTexture.SetPixels(data2);
        squareTexture.Apply();

        int targetWidth = 150;
        int targetHeight = 150;

        Texture2D result = new Texture2D(targetWidth, targetHeight, squareTexture.format ,true);
        Color[] rpixels = result.GetPixels(0);
        float intX = (1.0f / (float)targetWidth);
        float intY = (1.0f / (float)targetHeight);
        for(int px = 0; px < rpixels.Length; px++)
        {
            rpixels[px] = squareTexture.GetPixelBilinear(intX * ((float)px % targetWidth), intY * ((float)Mathf.Floor(px / targetWidth)));
        }
        result.SetPixels(rpixels, 0);
        result.Apply();



        return result;
    }

    public void UploadFile(string fileName, int photoNumber)
    {
        photoIndex = photoNumber;

        if (GlobalFileLock != 0)
        { 
            throw new Exception("This example overly restricts file operations for safety. Careful consideration must be made when doing multiple file operations in parallel to avoid conflict.");
        }
        ActiveUploadFileName = fileName;

        GlobalFileLock += 1; // Start InitiateFileUploads
        var request = new PlayFab.DataModels.InitiateFileUploadsRequest
        {
            Entity = new PlayFab.DataModels.EntityKey { Id = entityId, Type = entityType },
            FileNames = new List<string> { ActiveUploadFileName },
        };
        
        if (photoNumber == 1)
        {
            PlayFabDataAPI.InitiateFileUploads(request, OnInitFileUpload1, OnInitFailed1);
       
        }
        else if (photoNumber == 2)
        {
            PlayFabDataAPI.InitiateFileUploads(request, OnInitFileUpload2, OnInitFailed2);
        }
        else if (photoNumber == 3)
        {
            PlayFabDataAPI.InitiateFileUploads(request, OnInitFileUpload3, OnInitFailed3);
        }
        else
        {
            PlayFabDataAPI.InitiateFileUploads(request, OnInitFileUpload0, OnInitFailed0);
        }

       
    }
   
        void OnInitFileUpload0(PlayFab.DataModels.InitiateFileUploadsResponse response)
    {
        // Texture2D tex = (Texture2D)_profilePhoto.texture;
        // tex.ReadPixels(new Rect(0, 0, _profilePhoto.texture.width, _profilePhoto.texture.height), 0, 0);
        // tex.Apply();
        // byte[] bytes = tex.EncodeToPNG();

        var payload = File.ReadAllBytes(Application.persistentDataPath + "/SavedImage0.png");

        GlobalFileLock += 1; // Start SimplePutCall
        PlayFabHttp.SimplePutCall(response.UploadDetails[0].UploadUrl,
            payload,   // 1???????????? bytes,
            FinalizeUpload,
            error => { Debug.Log(error); }
        );
        GlobalFileLock -= 1; // Finish InitiateFileUploads
    }
    void OnInitFileUpload1(PlayFab.DataModels.InitiateFileUploadsResponse response)
    {
        // Texture2D tex = (Texture2D)_profilePhoto.texture;
        // tex.ReadPixels(new Rect(0, 0, _profilePhoto.texture.width, _profilePhoto.texture.height), 0, 0);
        // tex.Apply();
        // byte[] bytes = tex.EncodeToPNG();

        var payload = File.ReadAllBytes(Application.persistentDataPath + "/SavedImage1.png");

        GlobalFileLock += 1; // Start SimplePutCall
        PlayFabHttp.SimplePutCall(response.UploadDetails[0].UploadUrl,
            payload,   // 1???????????? bytes,
            FinalizeUpload,
            error => { Debug.Log(error); }
        );
        GlobalFileLock -= 1; // Finish InitiateFileUploads
    }

    void OnInitFileUpload2(PlayFab.DataModels.InitiateFileUploadsResponse response)
    {
        // Texture2D tex = (Texture2D)_profilePhoto.texture;
        // tex.ReadPixels(new Rect(0, 0, _profilePhoto.texture.width, _profilePhoto.texture.height), 0, 0);
        // tex.Apply();
        // byte[] bytes = tex.EncodeToPNG();

        var payload = File.ReadAllBytes(Application.persistentDataPath + "/SavedImage2.png");

        GlobalFileLock += 1; // Start SimplePutCall
        PlayFabHttp.SimplePutCall(response.UploadDetails[0].UploadUrl,
            payload,   // 1???????????? bytes,
            FinalizeUpload,
            error => { Debug.Log(error); }
        );
        GlobalFileLock -= 1; // Finish InitiateFileUploads
    }

    void OnInitFileUpload3(PlayFab.DataModels.InitiateFileUploadsResponse response)
    {
        // Texture2D tex = (Texture2D)_profilePhoto.texture;
        // tex.ReadPixels(new Rect(0, 0, _profilePhoto.texture.width, _profilePhoto.texture.height), 0, 0);
        // tex.Apply();
        // byte[] bytes = tex.EncodeToPNG();

        var payload = File.ReadAllBytes(Application.persistentDataPath + "/SavedImage3.png");

        GlobalFileLock += 1; // Start SimplePutCall
        PlayFabHttp.SimplePutCall(response.UploadDetails[0].UploadUrl,
            payload,   // 1???????????? bytes,
            FinalizeUpload,
            error => { Debug.Log(error); }
        );
        GlobalFileLock -= 1; // Finish InitiateFileUploads
    }



    void FinalizeUpload(byte[] obj)
    {
        GlobalFileLock += 1; // Start FinalizeFileUploads
        var request = new PlayFab.DataModels.FinalizeFileUploadsRequest
        {
            Entity = new PlayFab.DataModels.EntityKey { Id = entityId, Type = entityType },
            FileNames = new List<string> { ActiveUploadFileName },
        };
        PlayFabDataAPI.FinalizeFileUploads(request, OnUploadSuccess, OnSharedFailure);
        GlobalFileLock -= 1; // Finish SimplePutCall
    }


    void OnUploadSuccess(PlayFab.DataModels.FinalizeFileUploadsResponse result)
    {
        Debug.Log("File upload success: " + ActiveUploadFileName);
        GlobalFileLock -= 1; // Finish FinalizeFileUploads
    }


    void OnInitFailed0(PlayFabError error)
    {
        if (error.Error == PlayFabErrorCode.EntityFileOperationPending)
        {
            // This is an error you should handle when calling InitiateFileUploads, but your resolution path may vary
            GlobalFileLock += 1; // Start AbortFileUploads
            var request = new PlayFab.DataModels.AbortFileUploadsRequest
            {
                Entity = new PlayFab.DataModels.EntityKey { Id = entityId, Type = entityType },
                FileNames = new List<string> { ActiveUploadFileName },
            };
            PlayFabDataAPI.AbortFileUploads(request, (result) => { GlobalFileLock -= 1; UploadFile(ActiveUploadFileName,0); }, OnSharedFailure); GlobalFileLock -= 1; // Finish AbortFileUploads
            GlobalFileLock -= 1; // Failed InitiateFileUploads
        }
        else
            OnSharedFailure(error);
    }
    void OnInitFailed1(PlayFabError error)
    {
        if (error.Error == PlayFabErrorCode.EntityFileOperationPending)
        {
            // This is an error you should handle when calling InitiateFileUploads, but your resolution path may vary
            GlobalFileLock += 1; // Start AbortFileUploads
            var request = new PlayFab.DataModels.AbortFileUploadsRequest
            {
                Entity = new PlayFab.DataModels.EntityKey { Id = entityId, Type = entityType },
                FileNames = new List<string> { ActiveUploadFileName },
            };
            PlayFabDataAPI.AbortFileUploads(request, (result) => { GlobalFileLock -= 1; UploadFile(ActiveUploadFileName,1); }, OnSharedFailure); GlobalFileLock -= 1; // Finish AbortFileUploads
            GlobalFileLock -= 1; // Failed InitiateFileUploads
        }
        else
            OnSharedFailure(error);
    }
    void OnInitFailed2(PlayFabError error)
    {
        if (error.Error == PlayFabErrorCode.EntityFileOperationPending)
        {
            // This is an error you should handle when calling InitiateFileUploads, but your resolution path may vary
            GlobalFileLock += 1; // Start AbortFileUploads
            var request = new PlayFab.DataModels.AbortFileUploadsRequest
            {
                Entity = new PlayFab.DataModels.EntityKey { Id = entityId, Type = entityType },
                FileNames = new List<string> { ActiveUploadFileName },
            };
            PlayFabDataAPI.AbortFileUploads(request, (result) => { GlobalFileLock -= 1; UploadFile(ActiveUploadFileName,2); }, OnSharedFailure); GlobalFileLock -= 1; // Finish AbortFileUploads
            GlobalFileLock -= 1; // Failed InitiateFileUploads
        }
        else
            OnSharedFailure(error);
    }
    void OnInitFailed3(PlayFabError error)
    {
        if (error.Error == PlayFabErrorCode.EntityFileOperationPending)
        {
            // This is an error you should handle when calling InitiateFileUploads, but your resolution path may vary
            GlobalFileLock += 1; // Start AbortFileUploads
            var request = new PlayFab.DataModels.AbortFileUploadsRequest
            {
                Entity = new PlayFab.DataModels.EntityKey { Id = entityId, Type = entityType },
                FileNames = new List<string> { ActiveUploadFileName },
            };
            PlayFabDataAPI.AbortFileUploads(request, (result) => { GlobalFileLock -= 1; UploadFile(ActiveUploadFileName,3); }, OnSharedFailure); GlobalFileLock -= 1; // Finish AbortFileUploads
            GlobalFileLock -= 1; // Failed InitiateFileUploads
        }
        else
            OnSharedFailure(error);
    }

    void OnSharedFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
        GlobalFileLock -= 1;
    }


    /// <summary>
    /// DownLoadPhoto
    /// </summary>

    /*
    void Start()
    {

        if (PlayFabClientAPI.IsClientLoggedIn() == true)
        {
            PlayFabAuthenticationAPI.GetEntityToken(new GetEntityTokenRequest(),
              (entityResult) =>
              {

                  entityId = entityResult.Entity.Id;
                  entityType = entityResult.Entity.Type;

                  Debug.Log(entityId + entityType);
                  LoadAllFiles();
              }, OnPlayFabError);


        }

    }
   

    public void OnPlayFabError(PlayFabError error)
    {
        //??
    }
     */

    public void OtherUserLoadAllFiles(string OtherUserEntityID)
    {
        otherUserEntityID = OtherUserEntityID;

        if (GlobalFileLock != 0)
            throw new Exception("?? ?????? ?????? ???? ???? ?????? ???????? ??????????. ?????? ?????? ???? ???? ???? ?????? ?????? ?????? ???? ???????? ???????? ??????.");

        GlobalFileLock += 1; // Start GetFiles
        var request = new PlayFab.DataModels.GetFilesRequest { Entity = new PlayFab.DataModels.EntityKey { Id = OtherUserEntityID, Type = entityType } };
        PlayFabDataAPI.GetFiles(request, OtherUserOnGetFileMeta, OnSharedFailure);

    }
    void OtherUserOnGetFileMeta(PlayFab.DataModels.GetFilesResponse result)
    {
        Debug.Log("Loading " + result.Metadata.Count + " files");

        _entityFileJson.Clear();
        foreach (var eachFilePair in result.Metadata)
        {
            _entityFileJson.Add(eachFilePair.Key, null);
            StartCoroutine(OtherUserGetActualFile(eachFilePair.Value));
        }
        GlobalFileLock -= 1; // Finish GetFiles
    }
    IEnumerator OtherUserGetActualFile(PlayFab.DataModels.GetFileMetadata fileData)
    {


        GlobalFileLock += 1; // Start Each SimpleGetCall
        PlayFabHttp.SimpleGetCall(fileData.DownloadUrl,
            result => { _entityFileJson[fileData.FileName] = Encoding.UTF8.GetString(result); GlobalFileLock -= 1; }, // Finish Each SimpleGetCall           
            error => { Debug.Log(error); }
        );
        /*
        PlayFabClientAPI.UpdateAvatarUrl(new UpdateAvatarUrlRequest()
        {
            ImageUrl = fileData.DownloadUrl
        },
OnSuccess => { },
OnFailed => { });
        */
        Debug.Log("File name : " + fileData.FileName);
        Debug.Log("File URL : " + fileData.DownloadUrl);
        Debug.Log("File Size : " + fileData.Size);

        string LastNumber = string.Empty;
        LastNumber = fileData.FileName.Substring(fileData.FileName.Length - 1, 1);

        if (LastNumber == "1")
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(fileData.DownloadUrl);
            yield return www.SendWebRequest();
            Texture2D myTexture = CropTextureToSquare(DownloadHandlerTexture.GetContent(www));


           
            if (OUDV != null)
            {
                OUDV.Dog1Photo.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
                OUD.OhterDog1Photo.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            }
            else
            {
                otherUserManager.Dog1Photo_Multi.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
                OUD.OhterDog1Photo.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            }

        }
        else if (LastNumber == "2")
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(fileData.DownloadUrl);
            yield return www.SendWebRequest();
            Texture2D myTexture = CropTextureToSquare(DownloadHandlerTexture.GetContent(www));
            if (OUDV != null)
            {
                OUDV.Dog2Photo.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
                OUD.OhterDog2Photo.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            }
            else
            {
                otherUserManager.Dog2Photo_Multi.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
                OUD.OhterDog2Photo.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            }
        }
        else if (LastNumber == "3")
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(fileData.DownloadUrl);
            yield return www.SendWebRequest();
            Texture2D myTexture = CropTextureToSquare(DownloadHandlerTexture.GetContent(www));
            if (OUDV != null)
            {
                OUDV.Dog3Photo.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
                OUD.OhterDog3Photo.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            }
            else
            {
                otherUserManager.Dog3Photo_Multi.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
                OUD.OhterDog3Photo.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            }
        }
        else
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(fileData.DownloadUrl);
            yield return www.SendWebRequest();
            Texture2D myTexture = CropTextureToSquare(DownloadHandlerTexture.GetContent(www));
            if (OUDV != null)
            {
                OUDV.PlayerPhoto.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
                OUD.OhterPlayerPhoto.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            }
            else
            {
                otherUserManager.PlayerPhoto_Multi.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
                OUD.OhterPlayerPhoto.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            }
        }




        // 1
        /*
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
            _profilePhoto.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        */


    }






    /// <summary>
    /// /////////////////
    /// </summary>




    public void LoadAllFiles()
    {

      
            if (GlobalFileLock != 0)
                throw new Exception("?? ?????? ?????? ???? ???? ?????? ???????? ??????????. ?????? ?????? ???? ???? ???? ?????? ?????? ?????? ???? ???????? ???????? ??????.");

            GlobalFileLock += 1; // Start GetFiles
            var request = new PlayFab.DataModels.GetFilesRequest { Entity = new PlayFab.DataModels.EntityKey { Id = entityId, Type = entityType } };
            PlayFabDataAPI.GetFiles(request, OnGetFileMeta, OnSharedFailure);
    
    }
    void OnGetFileMeta(PlayFab.DataModels.GetFilesResponse result)
    {
        Debug.Log("Loading " + result.Metadata.Count + " files");

        _entityFileJson.Clear();
        foreach (var eachFilePair in result.Metadata)
        {
            _entityFileJson.Add(eachFilePair.Key, null);
            StartCoroutine(GetActualFile(eachFilePair.Value));
        }
        GlobalFileLock -= 1; // Finish GetFiles
    }









    // ???? 4???? ?????????? ?????????? ???? ?????? ??????????????
    // 

    IEnumerator GetActualFile(PlayFab.DataModels.GetFileMetadata fileData)
    {
       

        GlobalFileLock += 1; // Start Each SimpleGetCall
        PlayFabHttp.SimpleGetCall(fileData.DownloadUrl,
            result => { _entityFileJson[fileData.FileName] = Encoding.UTF8.GetString(result); GlobalFileLock -= 1; }, // Finish Each SimpleGetCall           
            error => { Debug.Log(error); }
        );
        /*
        PlayFabClientAPI.UpdateAvatarUrl(new UpdateAvatarUrlRequest()
        {
            ImageUrl = fileData.DownloadUrl
        },
OnSuccess => { },
OnFailed => { });
        */
        Debug.Log("File name : " + fileData.FileName);
        Debug.Log("File URL : " + fileData.DownloadUrl);
        Debug.Log("File Size : " + fileData.Size);

        string LastNumber = string.Empty;
        LastNumber = fileData.FileName.Substring(fileData.FileName.Length - 1, 1);

        if(LastNumber == "1")
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(fileData.DownloadUrl);
            yield return www.SendWebRequest();
            Texture2D myTexture = CropTextureToSquare(DownloadHandlerTexture.GetContent(www));

            getData.dog1_icon.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            getData.dog1_image.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            getData.petCard1.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            getData.player_dog1.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            getData.card_Dog1pic.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
        else if (LastNumber == "2")
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(fileData.DownloadUrl);
            yield return www.SendWebRequest();
            Texture2D myTexture = CropTextureToSquare(DownloadHandlerTexture.GetContent(www));

            getData.dog2_icon.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            getData.dog2_image.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            getData.petCard2.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            getData.SignAdd.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            getData.player_dog2.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            getData.card_Dog2pic.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
        else if (LastNumber == "3")
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(fileData.DownloadUrl);
            yield return www.SendWebRequest();
            Texture2D myTexture = CropTextureToSquare(DownloadHandlerTexture.GetContent(www));

            getData.dog3_icon.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            getData.dog3_image.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            getData.petCard3.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            getData.SignAdd.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            getData.player_dog3.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            getData.card_Dog3pic.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);

        }
        else
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(fileData.DownloadUrl);
            yield return www.SendWebRequest();
            Texture2D myTexture = CropTextureToSquare(DownloadHandlerTexture.GetContent(www));

            getData.player_image.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
        }




        // 1
        /*
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
            _profilePhoto.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        */


    }

}
