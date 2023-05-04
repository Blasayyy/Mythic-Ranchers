using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine.SceneManagement;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance { get; private set; }

    public const string KEY_PLAYER_NAME = "PlayerName";
    public const string KEY_PLAYER_CHARACTER = "Character";
    public const string KEY_KEY_LEVEL = "KeyLevel";
    public const string KEY_RELAY_START = "KeyRelayStart";
    public const int MAX_PLAYERS = 4;

    private Lobby hostLobby;
    private Lobby joinedLobby;
    private float heartBeatTimer;
    private float heartBeatTimerMax = 3;
    private float lobbyUpdateTimer;
    private float lobbyUpdateTimerMax = 1.1f;
    private string playerName;


    public event EventHandler <LobbyEventArgs> OnJoinedLobbyUpdate;
    public event EventHandler <LobbyEventArgs> OnKickFromLobby;
    public event EventHandler <LobbyEventArgs> OnJoinedLobby;
    public event EventHandler <OnLobbyListChangedEventArgs> OnLobbyListChanged;
    public event EventHandler <LobbyEventArgs> OnGameStarted;
    public event EventHandler OnLeaveLobby;


    public class LobbyEventArgs : EventArgs
    {
        public Lobby lobby;
    }

    public class OnLobbyListChangedEventArgs : EventArgs
    {
        public List<Lobby> lobbyList;
    }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        InitializeUnityAuthentication();
    }

    // Start is called before the first frame update
    private void Start()
    { 

        LobbyCreateUI.Instance.HideUI();

    }

    private void Update()
    {
        HandleLobbyHeartBeat();
        HandleLobbyPollForUpdates();
    }

    private async void InitializeUnityAuthentication()
    {
        if(UnityServices.State != ServicesInitializationState.Initialized)
        {
            await UnityServices.InitializeAsync();
            playerName = "Rancher" + UnityEngine.Random.Range(10, 100);
            Authenticate(playerName);
        }
        
    }

    public async void Authenticate(string playerName)
    {
        this.playerName = playerName;
        InitializationOptions initializationOptions = new InitializationOptions();
        initializationOptions.SetProfile(playerName);

        await UnityServices.InitializeAsync(initializationOptions);

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in player: " + AuthenticationService.Instance.PlayerId);


        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void HandleLobbyHeartBeat()
    {
        heartBeatTimer -= Time.deltaTime;
        if(heartBeatTimer < 0f)
        {
            heartBeatTimer = heartBeatTimerMax;

            ListLobbies();
            
        }
    }

    private async void HandleLobbyPollForUpdates()
    {
        if (joinedLobby != null)
        {
            lobbyUpdateTimer -= Time.deltaTime;
            if (lobbyUpdateTimer < 0f)
            {
                lobbyUpdateTimer = lobbyUpdateTimerMax;

                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                joinedLobby = lobby;

                OnJoinedLobbyUpdate?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });

                if (!PlayerInLobby())
                {
                    Debug.Log("Kicked from lobby!");

                    OnKickFromLobby?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });
                    joinedLobby = null;
                }

               
            }
        }
    }

    public Lobby GetJoinedLobby()
    {
        return joinedLobby;
    }

    public bool IsLobbyHost()
    {
        return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }

    private Player GetPlayer()
    {
        return new Player(AuthenticationService.Instance.PlayerId, null, new Dictionary<string, PlayerDataObject>
        {
            {KEY_PLAYER_NAME, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerName) },
            {KEY_PLAYER_CHARACTER, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, "placeholder") }
        });
    }

    private bool PlayerInLobby()
    {
        if(joinedLobby != null && joinedLobby.Players != null)
        {
            foreach(Player player in joinedLobby.Players)
            {
                if(player.Id == AuthenticationService.Instance.PlayerId)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public async void CreateLobby(string lobbyName, bool isPrivate, int keyLevel)
    {
        try
        {
            Player player = GetPlayer();

            CreateLobbyOptions options = new CreateLobbyOptions
            {
                Player = player,
                IsPrivate = isPrivate,
                Data = new Dictionary<string, DataObject>
                {
                    {KEY_KEY_LEVEL, new DataObject(DataObject.VisibilityOptions.Public, keyLevel.ToString()) },
                    {KEY_RELAY_START, new DataObject(DataObject.VisibilityOptions.Member, "0") }
                }
            };

            joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, MAX_PLAYERS, options);
            Debug.Log("Created Lobby " + joinedLobby.Name);

            OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });

            string joinCode = await Relay.Instance.CreateRelay();

            await LobbyService.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    { KEY_RELAY_START, new DataObject(DataObject.VisibilityOptions.Member, joinCode)}
                }
            });

            MythicGameManagerMultiplayer.Instance.StartHost();

            Debug.Log("started host");
            
        }
        catch(LobbyServiceException e )
        {
            Debug.Log(e);
        }
        
    }

    public async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Count = 10,
                Filters = new List<QueryFilter>
                {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
                },
                Order = new List<QueryOrder>
                {
                    new QueryOrder(false, QueryOrder.FieldOptions.Created)
                }
            };
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            OnLobbyListChanged?.Invoke(this, new OnLobbyListChangedEventArgs { lobbyList = queryResponse.Results });

            foreach(Lobby lobby in queryResponse.Results)
            {
                Debug.Log(lobby.Name);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }

    }

    public async void JoinLobby(Lobby lobby)
    {
        try
        {
            JoinLobbyByIdOptions joinLobbyByIdOptions = new JoinLobbyByIdOptions
            {
                Player = GetPlayer()
            };

            joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobby.Id, joinLobbyByIdOptions);

            OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = lobby });

            string relayJoinCode = joinedLobby.Data[KEY_RELAY_START].Value;

            await Relay.Instance.JoinRelay(relayJoinCode);

            MythicGameManagerMultiplayer.Instance.StartClient();

            Debug.Log("Joined Lobby: " + lobby.Id);
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        } 
    }

    public async void JoinLobbyByCode(string lobbyCode)
    {
        try
        {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
            {
                Player = GetPlayer()
            };
            joinedLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);
            
            OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });

            MythicGameManagerMultiplayer.Instance.StartClient();

            Debug.Log("Joined Lobby with code: " + lobbyCode);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
        
    }

    public async void LeaveLobby()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);

            joinedLobby = null;

            OnLeaveLobby?.Invoke(this, EventArgs.Empty);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void KickPlayer(string playerId)
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerId);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public void StartGame()
    {
        if (IsLobbyHost())
        {
            Loader.LoadNetwork(Loader.Scene.GameScene);
        }
    }

    public async void DeleteLobby()
    {
        if(joinedLobby != null)
        {
            try
            {
                await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
                joinedLobby = null;
            }
            catch(LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }
}
