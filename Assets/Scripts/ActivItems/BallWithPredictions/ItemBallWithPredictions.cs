using UnityEngine;

[CreateAssetMenu(fileName = "BallWithPreditcions", menuName = "BallPredict")]
public class ItemBallWithPredictions : ScriptableObject
{
    [SerializeField] private string[] _texts;
    public string[] Text => _texts;
}
