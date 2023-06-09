using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePieceButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gamePieceName;
    [SerializeField] private Button button;
    [SerializeField] private Button delete;
    
    private GamePiece _gamePiece;

    public void Init(GamePiece gamePiece) => _gamePiece = gamePiece;
    
    private void Start()
    {
        if (_gamePiece == null)
        {
            gamePieceName.text = "Add Game Piece";
            button.onClick.AddListener(() =>
            {
                Current.MutateAndSave(project =>
                {
                    var gamePiece = project.AddGamePiece();
                    Current.SelectGamePiece(gamePiece);
                });
                Message.Publish(new NavigateTo(Location.GamePiece));
            });
            delete.gameObject.SetActive(false);
        }
        else
        {
            gamePieceName.text = _gamePiece.Name;
            button.onClick.AddListener(() =>
            {
                Current.SelectGamePiece(_gamePiece);
                Message.Publish(new NavigateTo(Location.GamePiece));
            });
            delete.onClick.AddListener(() =>
            {
                Current.MutateAndSave(project => project.Pieces.Remove(_gamePiece));
                gameObject.SetActive(false);
            });
        }
    }
}