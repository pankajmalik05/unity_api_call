using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class to map response object
	public class Response  
	{
        private string _apiVersion;
        private object _meta;
        private object _data;

        private RequestMessage _request;
        private Dictionary<string, object> _headers = new Dictionary<string, object>();

        public string apiVersion
        {
            get => _apiVersion;
            set => _apiVersion = value;
        }
        public object meta
        {
            get => _meta;
            set => _meta = value;
        }
        public object data
        {
            get => _data;
            set => _data = value;
        }

        public RequestMessage request
        {
            get => _request;
            set => _request = value;
        }
        public Dictionary<string,object> headers
        {
            get => _headers;
            set => _headers = value;
        }
        public override string ToString ()
		{
			return string.Format ("[ResponseMessage] apiVersion = {0}, meta = {1}, data = {2}", apiVersion, meta, data);
		}
	}
