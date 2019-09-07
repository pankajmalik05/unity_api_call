using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UIManager : MonoBehaviour
{

    public ScrollRect _scrollRect;
    public InputField _searchBox;
    public Text _details;
    public Client _client;

    private int _offset = 0;
    private int _limit = 10;
    private string _searchQuery = "";

    public GameObject _item;
    private bool _fetchingMetaData=false;

    private List<GameObject> _listItems;

    // getting list of programs at the beginning 
    void Start()
    {

        _listItems = new List<GameObject>();

        GetPrograms( listener => {
            HandleAllContentResponse(listener);
        });

    }

    // clearing the list of programs and getting new programs with the search querry  [works on hitting enter at the end]
    public void OnSearchEnd()
    {
        for (int i = 0; i < _listItems.Count; i++)
        {
            Destroy(_listItems[i]);
        }

        _listItems.Clear();
        _listItems = new List<GameObject>();
        
        _offset = 0;
        _searchQuery = _searchBox.text;
        _fetchingMetaData = true;

        GetPrograms(listener => {
            HandleAllContentResponse(listener);
        });
    }

    // getting next list of programs on end of scroll
    public void OnGetNextPrograms()
    {
        if (_scrollRect.verticalNormalizedPosition <= 0.2f && !_fetchingMetaData)
        {
            _fetchingMetaData = true;
            _offset += _limit;

            GetPrograms(listener =>
            {
                HandleAllContentResponse(listener);
            });
        }

    }

    // sending request for getting programs
    public  void GetPrograms(Action<ResponseMessage<Data[]>> contentListener)
    {
        RequestMessage getMetadataRequest = new RequestMessage()
        {
            requestType = RequestMessage.RequestType.GET,
            requestPath = _client.url,
            headers = RequestMessage.defaultHeaders,
            requestParameters = new Dictionary<string, string>(){
                    {  "app_id", _client.app_id },
                    {  "app_key", _client.app_key },
                    {  "offset", _offset.ToString() },
                    {  "limit", _limit.ToString() },
                    {  "q", _searchQuery }

                }
        };

        _client.DispatchRequest<Data[]>(getMetadataRequest, contentListener);
    }

    // handling response from the request
    private void HandleAllContentResponse(ResponseMessage<Data[]> metadataResponse)
    {
        if (metadataResponse.data.Length > 0)
        {
            for (int i = 0; i < metadataResponse.data.Length; i++)
            {
                GameObject temp = Instantiate(_item, _scrollRect.content.transform);
                temp.GetComponent<Item>().data = metadataResponse.data[i];
                temp.GetComponent<Button>().onClick.AddListener(SetDetails);

                // setting title from dictionary using key "fi" and "und"

                if (metadataResponse.data[i].title.ContainsKey("fi"))
                {
                    temp.GetComponent<Item>().SetTitle(metadataResponse.data[i].title["fi"]);
                }
                else if (metadataResponse.data[i].title.ContainsKey("und"))
                {
                    temp.GetComponent<Item>().SetTitle(metadataResponse.data[i].title["und"]);
                }
                else
                {   
                    // setting title to first key of the dictionary if "fi" and "und" key is not available

                    foreach (KeyValuePair<string, string> title in metadataResponse.data[i].title)
                    {
                        if (title.Value != null)
                        {
                            temp.GetComponent<Item>().SetTitle(title.Value);
                            break;
                        }
                    }
                }

                _listItems.Add(temp);
            }

            _fetchingMetaData = false;
        }
    }

    // setting program details
    public void SetDetails()
    {
        Data data = EventSystem.current.currentSelectedGameObject.GetComponent<Item>().data;

        _details.text =
        string.Format(
        "Collection    : {0} \n " +
        "Type          : {1} \n " +
        "Duration      : {2} \n " +
        "ID            : {3} \n " +
        "Index Data Modified  : {4} \n " +
        "Type Creative    : {5} \n " +
        "Type Media       : {6} \n ",
        data.collection, data.type, data.duration, data.id, data.indexDataModified, data.typeCreative, data.typeMedia);

    }

}
