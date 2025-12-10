using UniRx;
using UnityUtilities;

public class SettingModel
{
    // Các key lưu trữ
    private const string KeySound = "sound";
    private const string KeyVibration = "vibration";
    private const string KeyMusic = "music";

    // Giá trị mặc định
    private const bool DefaultSound = true;
    private const bool DefaultVibration = true;
    private const bool DefaultMusic = false;

    // Reactive Properties
    private ReactiveProperty<bool> _sound;
    private ReactiveProperty<bool> _vibration;
    private ReactiveProperty<bool> _music;

    public IReadOnlyReactiveProperty<bool> Sound => _sound;
    public IReadOnlyReactiveProperty<bool> Vibration => _vibration;
    public IReadOnlyReactiveProperty<bool> Music => _music;

    // Constructor
    public SettingModel()
    {
        // Load từ storage hoặc gán default
        _sound = new ReactiveProperty<bool>(SaveExtensions.GetSetting(KeySound, DefaultSound));
        _vibration = new ReactiveProperty<bool>(SaveExtensions.GetSetting(KeyVibration, DefaultVibration));
        _music = new ReactiveProperty<bool>(SaveExtensions.GetSetting(KeyMusic, DefaultMusic));

        // Tự động lưu lại mỗi khi thay đổi
        _sound.Subscribe(value => SaveExtensions.PutSetting(KeySound, value));
        _vibration.Subscribe(value => SaveExtensions.PutSetting(KeyVibration, value));
        _music.Subscribe(value => SaveExtensions.PutSetting(KeyMusic, value));
    }

    /// <summary>
    /// Gán giá trị mới cho sound
    /// </summary>
    public void SetSound(bool value) => _sound.Value = value;

    /// <summary>
    /// Gán giá trị mới cho vibration
    /// </summary>
    public void SetVibration(bool value) => _vibration.Value = value;

    /// <summary>
    /// Gán giá trị mới cho music
    /// </summary>
    public void SetMusic(bool value) => _music.Value = value;

    /// <summary>
    /// Reset tất cả về mặc định
    /// </summary>
    public void ResetToDefault()
    {
        _sound.Value = DefaultSound;
        _vibration.Value = DefaultVibration;
        _music.Value = DefaultMusic;
    }
}
