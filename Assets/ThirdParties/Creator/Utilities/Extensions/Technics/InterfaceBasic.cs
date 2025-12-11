using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Giao diện cho các đối tượng có thể bật hoặc tắt.
/// Ví dụ: đèn, máy móc, thiết bị điện.
/// </summary>
public interface ISwitchable
{
    void TurnOn();
    void TurnOff();
}

/// <summary>
/// Giao diện cho các vật phẩm có thể được thu thập.
/// Ví dụ: coin, gem, power-up.
/// </summary>
public interface ICollectible
{
    void Collect();
}

/// <summary>
/// Giao diện cho các đối tượng mà người chơi có thể tương tác.
/// Ví dụ: NPC, bảng điều khiển, cửa.
/// </summary>
public interface IInteractable
{
    void Interact();
}

/// <summary>
/// Giao diện cho các đối tượng cần được khởi tạo với dữ liệu.
/// Ví dụ: cấu hình từ dữ liệu JSON hoặc ScriptableObject.
/// </summary>
public interface IInitializable
{
    void Initialize();
}

public interface IInitializableData<T>
{
    void Initialize(T data);
}


[System.Serializable]
public class ObjectSwitch<T> : ISwitchable
{
    public T on;

    public T off;

    public bool isOn;

    public void TurnOn() => Set(true);

    public void TurnOff() => Set(false);

    protected virtual void Set(bool isOn) { }
}

[System.Serializable]
public class ImageSwitch : ObjectSwitch<Sprite>
{
    public Image imageTarget;

    protected override void Set(bool isOn)
    {
        if (imageTarget)
        {
            this.isOn = isOn;
            if (this.isOn)
            {
                imageTarget.sprite = on;
            }
            else
            {
                imageTarget.sprite = off;
            }
        }
    }
}

[System.Serializable]
public class TextColorSwitch : ObjectSwitch<Color>
{
    public TextMeshProUGUI txtTarget;

    protected override void Set(bool isOn)
    {
        if (txtTarget)
        {
            this.isOn = isOn;
            if (this.isOn)
            {
                txtTarget.color = on;
            }
            else
            {
                txtTarget.color = off;
            }
        }
    }
}

[System.Serializable]
public class GameObjectSwitch : ObjectSwitch<GameObject>
{
    protected override void Set(bool isOn)
    {
        this.isOn = isOn;

        on.SetActive(false);
        off.SetActive(false);

        on.SetActive(this.isOn);
        off.SetActive(!this.isOn);
    }
}


