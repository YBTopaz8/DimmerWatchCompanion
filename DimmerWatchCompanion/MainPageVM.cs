using DimmerWatchCompanion.Utils;
using FlowHub_MAUI.Utilities.OtherUtils;
using Parse;
using Parse.LiveQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DimmerWatchCompanion;
public partial class MainPageVM : ObservableObject
{
    [ObservableProperty]
    ObservableCollection<SongModelView> displayedSongs=new();
    [ObservableProperty]
    SongModelView? currentSong=new();
    [ObservableProperty]
    bool isPlaying;
    [ObservableProperty]
    bool isPaused;
    [ObservableProperty]
    bool isStopped;
    [ObservableProperty]
    bool isMuted;
    [ObservableProperty]
    double volume;
    [ObservableProperty]
    double currentPosition;
    [ObservableProperty]
    bool isShuffle;
    [ObservableProperty]
    bool isRepeat;
    [ObservableProperty]
    bool isRepeatOne;
    [ObservableProperty]
    bool isRepeatAll;


    public MainPageVM()
    {
     
    }

    public async Task LoginUser()
    {
        ParseUser signUpUser = new ParseUser();
        signUpUser.Email = APIKeys.Email;
        signUpUser.Username = APIKeys.UserName;
        signUpUser.Password = APIKeys.Password;
        var usr = await ParseClient.Instance.LogInAsync(signUpUser.Email, signUpUser.Password!);
        if (usr is not null)
        {            
            await Shell.Current.DisplayAlert("Login", "Login OK", "OK");
            var e = await ParseClient.Instance.CurrentUserController.GetCurrentSessionTokenAsync(ParseClient.Instance.Services);
            
        }
    }
    public ParseLiveQueryClient LiveQueryClient { get; set; } = new();

    public async void SetupLiveQueries()
    {
        try
        {
            LiveQueryClient = new();
            var SongQuery = ParseClient.Instance.GetQuery("SongModelView");
            var subscription = LiveQueryClient.Subscribe(SongQuery);
            
            LiveQueryClient.OnConnected.
                Subscribe(_ =>
                {
                    Debug.WriteLine("Connected to LiveQuery Server");
                });

            LiveQueryClient.OnError.
                Subscribe(ex =>
                {
                    Debug.WriteLine($"Error: {ex.Message}");
                });

            LiveQueryClient.OnSubscribed.
                Subscribe(sub =>
                {
                    Debug.WriteLine($"Subscribed to {sub.requestId}");
                });

            LiveQueryClient.OnObjectEvent
                .Where( e => e.evt == Subscription.Event.Update)
                .Subscribe(e =>
                {
                    SongModelView song = new();
                    var objData = (e.objectDictionnary as Dictionary<string, object>);

                    song = ObjectMapper.MapFromDictionary<SongModelView>(objData);

                    //ideally, I would do something here, like update the UI and whatnot, not I will.


                    //CurrentSong.IsCurrentPlayingHighlight = !song.IsCurrentPlayingHighlight;
                    //CurrentSong = song;

                });


            var SongsQuery = ParseClient.Instance.GetQuery("SongModelView").Limit(3000);
            var allSongs = await SongsQuery.FindAsync();
            DisplayedSongs.Clear();
            foreach (var item in allSongs)
            {
                var obj = new SongModelView();
                item.TryGetValue("Title", out string Title);
                obj.Title = Title;
                item.TryGetValue("FilePath", out string FilePath);
                obj.FilePath = FilePath;
                item.TryGetValue("ArtistName", out string ArtistName);
                obj.ArtistName = ArtistName;
                item.TryGetValue("LatestUpdateBy", out string LatestUpdateBy);
                obj.LatestUpdateBy = Title;
                //item.TryGetValue("IsCurrentPlayingHighlight", out bool IsCurrentPlayingHighlight);
                obj.IsCurrentPlayingHighlight = false;
                item.TryGetValue("LocalDeviceId", out string LocalDeviceId);
                obj.LocalDeviceId = LocalDeviceId;
                DisplayedSongs.Add(obj);
            }
        }
        catch (Exception ex )
        {

            throw new Exception(ex.Message); 
        }
    }


    public async Task PlaySong(SongModelView song)
    {
        try
        {
            if (CurrentSong!.IsCurrentPlayingHighlight)
            {
                CurrentSong.IsCurrentPlayingHighlight = false;
            }
            song.IsCurrentPlayingHighlight = true;
            CurrentSong = song;

            var query = ParseClient.Instance.GetQuery("SongModelView")
                .WhereEqualTo(nameof(song.LocalDeviceId), song.LocalDeviceId);
            var existingSong = await query.FirstAsync();

            existingSong["IsCurrentPlayingHighlight"] = true;

            await existingSong.SaveAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to play song: {ex.Message}");
        }
    }
}

public static class ObjectMapper
{
    /// <summary>
    /// Maps values from a dictionary to an instance of type T.
    /// Logs any keys that don't match properties in T.
    ///     
    /// Helper to Map from Parse Dictionnary Response to Model
    /// Example usage TestChat chat = ObjectMapper.MapFromDictionary<TestChat>(objData);    
    /// </summary>
    public static T MapFromDictionary<T>(IDictionary<string, object> source) where T : new()
    {
        // Create an instance of T
        T target = new T();

        // Get all writable properties of T
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanWrite)
            .ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);

        // Track unmatched keys
        List<string> unmatchedKeys = new();

        foreach (var kvp in source)
        {
            if (properties.TryGetValue(kvp.Key, out var property))
            {
                try
                {
                    // Convert and assign the value to the property
                    if (kvp.Value != null && property.PropertyType.IsAssignableFrom(kvp.Value.GetType()))
                    {
                        property.SetValue(target, kvp.Value);
                    }
                    else if (kvp.Value != null)
                    {
                        // Attempt conversion for non-directly assignable types
                        var convertedValue = Convert.ChangeType(kvp.Value, property.PropertyType);
                        property.SetValue(target, convertedValue);
                    }
                }
                catch (Exception ex)
                {
                    
                }
            }
            else
            {
                // Log unmatched keys
                unmatchedKeys.Add(kvp.Key);
            }
        }

        //// Log keys that don't match
        //if (unmatchedKeys.Count > 0)
        //{
        //    Debug.WriteLine("Unmatched Keys:");
        //    foreach (var key in unmatchedKeys)
        //    {
        //        Debug.WriteLine($"- {key}");
        //    }
        //}

        return target;
    }
}
