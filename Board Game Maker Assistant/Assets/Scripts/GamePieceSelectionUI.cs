using UnityEngine;

public class GamePieceSelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GamePieceButton prototype;
    
    private void OnEnable()
    {
        panel.DestroyAllChildren();
        foreach (var piece in Current.Project.Pieces)
            Instantiate(prototype, panel.transform).Init(piece);
        Instantiate(prototype, panel.transform);
    }
}