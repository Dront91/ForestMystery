using UnityEngine;
using UnityEngine.UI;

namespace MysteryForest
{
  public class PassiveItemView : MonoBehaviour
  {
    [SerializeField] private Image _image;
    
    public void Init(Sprite sprite)
    {
      _image.sprite = sprite;
    }
  }
}