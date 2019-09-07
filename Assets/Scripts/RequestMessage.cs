using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


    [Serializable]
    public class RequestMessage
    {

        private string _requestPath;
        private string _body;
        private RequestType _requestType;
        private Dictionary<string, string> _requestParameters = new Dictionary<string, string>();
        private Dictionary<string, string> _headers;

        private const string KEY_PARAMETER = "parameters";
        private const string KEY_HEADER_CONTENT_TYPE = "Content-Type";
        private const string KEY_HEADER_CONTENT_JSON = "application/json";

        public string requestPath
        {
            get => _requestPath;
            set => _requestPath = value;
        }
        public string body
        {
            get => _body;
            set => _body = value;
        }
        public RequestType requestType
        {
            get => _requestType;
            set => _requestType = value;
        }
        public Dictionary<string, string> requestParameters
        {
            get => _requestParameters;
            set => _requestParameters = value;
        }
        public Dictionary<string, string> headers
        {
            get => _headers;
            set => _headers = value;
        }

        public RequestMessage()
        {
            if (_defaultHeaders == null)
            {
                _defaultHeaders = new Dictionary<string, string>();
             }
        }

        private static Dictionary<string, string> _defaultHeaders;
        public static Dictionary<string, string> defaultHeaders
        {
            get
            {
                if (_defaultHeaders == null)
                {
                    _defaultHeaders = new Dictionary<string, string>();
                }

                return new Dictionary<string, string>(_defaultHeaders);
            }
        }

    // binary enum for request type
        public enum RequestType : int
        {
            PUT = 0x0001 << 0,
            GET = 0x0001 << 1,
            POST = 0x0001 << 2,
            DELETE = 0x0001 << 3,
            PATCH = 0x0001 << 4,
        }
        public override string ToString()
        {
            return string.Format("[RequestMessage] _requestPath = {0}", _requestPath);
        }
    }


