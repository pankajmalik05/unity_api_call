using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using UnityEngine.Networking;


    public class RequestDispatcher : MonoBehaviour
    {
        
        private Response _response;
        private Action<Response> _responseListener;
        [SerializeField]
        private RequestMessage _request;

        public void Request(RequestMessage request, Action<Response> callback)
        {
            StartCoroutine(RequestAsync(request, callback));
        }

    // creating and sending the request 
        public IEnumerator RequestAsync(RequestMessage request, Action<Response> callback)
        {
            PrepareUrlWithParameters(request);

            UnityWebRequest www = null;

            _request = request;
            _responseListener = callback;

            www = new UnityWebRequest(request.requestPath);
            www.downloadHandler = new DownloadHandlerBuffer();

            request.headers.ToList().ForEach(pair => { www.SetRequestHeader(pair.Key, pair.Value); });

            www.method = request.requestType.ToString();

            if ((request.requestType & (RequestMessage.RequestType.POST | RequestMessage.RequestType.PUT)) > 0)
            {
                if (!string.IsNullOrEmpty(request.body))
                {
                    byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(request.body);
                    UploadHandler uploader = new UploadHandlerRaw(byteArray);
                    uploader.contentType = "application/json";

                    www.uploadHandler = uploader;
                }
            }

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                // TODO : handle exception
            }
            else
            {
                string strResponse = www.downloadHandler.text;

                _response = Serializer.Deserialize<Response>(strResponse);

                if (www.GetResponseHeaders() != null)
                {
                    www.GetResponseHeaders().Keys.ToList().ForEach(aHeader =>
                    {
                        _response.headers.Add(aHeader, www.GetResponseHeader(aHeader));
                    });
                }
            }


            if (_response != null)
            {
                _response.request = request;

                if (_responseListener != null)
                    _responseListener(_response);
            }

            www.Dispose();
            www = null;

            Destroy(gameObject);
        }

    // adding parameters to the request url 
        private void PrepareUrlWithParameters(RequestMessage request)
        {
            string finalUrl = request.requestPath;
            List<KeyValuePair<string, string>> parameters = request.requestParameters.ToList();
            for (int i = 0; i < parameters.Count; i++)
            {
                finalUrl += (i == 0 ? "?" : "&") + parameters[i].Key + "=" + parameters[i].Value;
            }
            request.requestPath = finalUrl;
        }
    }
