using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// generic response message
	public class ResponseMessage<T>
	{
		private RequestMessage _request;
        private string _apiVersion;
        private T _data;
        private Meta _meta;

        public RequestMessage request
        {
            get => _request;
            set => _request = value;
        }

        public string apiVersion
        {
            get => _apiVersion;
            set => _apiVersion = value;
        }
        public T data
        {
            get => _data;
            set => _data = value;
        }
        public Meta meta
        {
            get => _meta;
            set => _meta = value;
        }


    public override string ToString ()
		{
			return string.Format ("[ResponseMessage] _apiVersion = {0}, _data = {1}", _apiVersion, _data);
		}

		public static ResponseMessage<T> GenericResponseFromResponse<T>(Response response)
		{
			 ResponseMessage<T> resp = new ResponseMessage<T> ();
             resp._apiVersion = response.apiVersion;
             resp._meta = Serializer.Deserialize<Meta>(response.meta.ToString());
             resp._data = Serializer.Deserialize<T>(response.data.ToString());
             resp._request = response.request;
			 return resp;
		}
	}
		
