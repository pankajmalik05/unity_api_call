using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	public class Client : MonoBehaviour
	{
    
        private string _url = "https://external.api.yle.fi/v1/programs/items.json";  
        private string _app_id = "40c24978";
        private string _app_key = "7d1e2007baaab884849508255417f006";

		[SerializeField]
		private List<RequestMessage> _requests;

        public string url
        {
            get => _url;
        }
        public string app_id
        {
            get => _app_id;
        }
        public string app_key
        {
            get => _app_key;
        }
        void Start()
         { 
		    _requests = new List<RequestMessage> ();
		 }

    // generic request dispatcher
		public void DispatchRequest<T>(RequestMessage request, Action<ResponseMessage<T>> listener)
		{
			_requests.Add (request);

			GameObject requestMakerObject = new GameObject ("web request");
			requestMakerObject.transform.SetParent (transform);
			RequestDispatcher requestMaker = requestMakerObject.AddComponent<RequestDispatcher> ();

			requestMaker.Request (request, Response => {

				if (listener != null)
				{
					listener(GenericResponseFromResponse<T>(Response));
					RequestMessage req = _requests.Find(areq => areq == Response.request);
					if(req != null)
						_requests.Remove(req);
				}
			});
		}

    // generic response from the request 
		private static ResponseMessage<T> GenericResponseFromResponse<T>(Response response)
		{
			ResponseMessage<T> resp = new ResponseMessage<T> ();

            resp.apiVersion = response.apiVersion;
            resp.request = response.request;

           if (response.meta != null)
            {
                resp.meta = Serializer.Deserialize<Meta>(response.meta.ToString());
            }

            if (response.data != null)
            {
                resp.data = Serializer.Deserialize<T>(response.data.ToString());
            }

			return resp;
		}


	}

