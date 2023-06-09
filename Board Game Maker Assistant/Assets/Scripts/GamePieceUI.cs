using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePieceUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField pieceName;
    [SerializeField] private TMP_Dropdown pieceType;
    [SerializeField] private GameObject dataSourceControl;
    [SerializeField] private TMP_Dropdown dataSource;
    [SerializeField] private GameObject tableControl;
    [SerializeField] private TMP_Dropdown table;
    [SerializeField] private Button filter;
    [SerializeField] private TMP_InputField width;
    [SerializeField] private TMP_InputField height;
    [SerializeField] private GameObject standardSizesControl;
    [SerializeField] private TMP_Dropdown standardSizes;
    [SerializeField] private GameObject isSameOnBothSidesControl;
    [SerializeField] private Toggle isSameOnBothSides;
    [SerializeField] private GameObject isDoubleSidedControl;
    [SerializeField] private Toggle isDoubleSided;

    [SerializeField] private ConditionsUI conditions;

    private bool _ignoreChanges = false;
    
    private void Awake()
    {
        pieceName.onValueChanged.AddListener(x =>
        {
            if (_ignoreChanges)
                return;
            Current.MutateAndSave(mutateProject: project => project.TrySetGamePieceName(Current.GamePiece, x));
        });
        pieceType.options = new List<TMP_Dropdown.OptionData>
        {
            new TMP_Dropdown.OptionData("Board"),
            new TMP_Dropdown.OptionData("Token"),
            new TMP_Dropdown.OptionData("Card"),
            new TMP_Dropdown.OptionData("Die")
        };
        pieceType.onValueChanged.AddListener(x =>
        {
            if (_ignoreChanges)
                return;
            Current.MutateAndSave(mutatePiece: piece => piece.Type = (GamePieceType) x);
            RefreshUI();
        });
        dataSource.onValueChanged.AddListener(x =>
        {
            if (_ignoreChanges)
                return;
            if (x != 0)
                Current.SelectDataSource(Current.Project.DataSources[x - 1]);
            Current.GamePiece.DataSourceId = x == 0 ? "" : Current.DataSource.Id;
            RefreshUI();
        });
        table.onValueChanged.AddListener(x =>
        {
            if (_ignoreChanges)
                return;
            Current.SelectTable(Current.DataSource.Tables[x]);
            Current.GamePiece.TableName = Current.Table.Name;
            RefreshUI();
        });
        filter.onClick.AddListener(() => conditions.Init(Current.GamePiece.Filter));
        width.onValueChanged.AddListener(x =>
        {
            if (_ignoreChanges)
                return;
            if (int.TryParse(x, out int width))
                Current.MutateAndSave(piece => piece.Width = width);
        });
        height.onValueChanged.AddListener(x =>
        {
            if (_ignoreChanges)
                return;
            if (int.TryParse(x, out int height))
                Current.MutateAndSave(piece => piece.Height = height);
        });
        standardSizes.options = new List<TMP_Dropdown.OptionData>
        {
            new TMP_Dropdown.OptionData("Custom"),
            new TMP_Dropdown.OptionData("US Game"),
            new TMP_Dropdown.OptionData("US Game R"),
            new TMP_Dropdown.OptionData("Poker"),
            new TMP_Dropdown.OptionData("Mini European"),
            new TMP_Dropdown.OptionData("Mini European R"),
            new TMP_Dropdown.OptionData("Mini US"),
            new TMP_Dropdown.OptionData("Mini US R"),
            new TMP_Dropdown.OptionData("Tarot")
        };
        standardSizes.onValueChanged.AddListener(x =>
        {
            if (_ignoreChanges)
                return;
            if (x > 0)
            {
                Current.MutateAndSave(mutatePiece: piece =>
                {
                    if (x == 1)
                    {
                        piece.Width = 660;
                        piece.Height = 1030;
                    }
                    else if (x == 2)
                    {
                        piece.Width = 1030;
                        piece.Height = 660;
                    }
                    else if (x == 3)
                    {
                        piece.Width = 750;
                        piece.Height = 1050;
                    }
                    else if (x == 4)
                    {
                        piece.Width = 520;
                        piece.Height = 792;
                    }
                    else if (x == 5)
                    {
                        piece.Width = 792;
                        piece.Height = 520;
                    }
                    else if (x == 6)
                    {
                        piece.Width = 484;
                        piece.Height = 744;
                    }
                    else if (x == 7)
                    {
                        piece.Width = 744;
                        piece.Height = 484;
                    }
                    else if (x == 8)
                    {
                        piece.Width = 825;
                        piece.Height = 1425;
                    }
                });
                width.text = Current.GamePiece.Width.ToString();
                height.text = Current.GamePiece.Height.ToString();
            }
        });
        isSameOnBothSides.onValueChanged.AddListener(x =>
        {
            if (_ignoreChanges)
                return;
            Current.MutateAndSave(mutatePiece: piece => piece.Token.SameOnBothSides = x);
        });
        isDoubleSided.onValueChanged.AddListener(x =>
        {
            if (_ignoreChanges)
                return;
            Current.MutateAndSave(mutatePiece: piece => piece.GameBoard.IsDoubleSided = x);
        });
    }

    private void OnEnable()
    {
        _ignoreChanges = true;
        pieceName.text = Current.GamePiece.Name;
        pieceType.value = (int)Current.GamePiece.Type;
        if (Current.Project.DataSources.Any())
        {
            dataSource.options = new [] { new TMP_Dropdown.OptionData("") }.Concat(Current.Project.DataSources.Select(x => new TMP_Dropdown.OptionData(x.Name))).ToList();
            if (string.IsNullOrWhiteSpace(Current.GamePiece.DataSourceId))
                dataSource.value = 0;
            else
            {
                dataSource.value = Current.Project.DataSources.FirstIndexOf(x => x.Id == Current.GamePiece.DataSourceId) + 1;
                Current.SelectDataSource(Current.Project.DataSources.First(x => x.Id == Current.GamePiece.DataSourceId));
            }
        }
        var pieceWidth = Current.GamePiece.Width;
        var pieceHeight = Current.GamePiece.Height;
        width.text = pieceWidth.ToString();
        height.text = pieceHeight.ToString();
        if (pieceWidth == 660 && pieceHeight == 1030)
            standardSizes.value = 1;
        else if (pieceWidth == 1030 && pieceHeight == 660)
            standardSizes.value = 2;
        else if (pieceWidth == 750 && pieceHeight == 1050)
            standardSizes.value = 3;
        else if (pieceWidth == 520 && pieceHeight == 792)
            standardSizes.value = 4;
        else if (pieceWidth == 792 && pieceHeight == 520)
            standardSizes.value = 5;
        else if (pieceWidth == 484 && pieceHeight == 744)
            standardSizes.value = 6;
        else if (pieceWidth == 744 && pieceHeight == 484)
            standardSizes.value = 7;
        else if (pieceWidth == 825 && pieceHeight == 1425)
            standardSizes.value = 8;
        else 
            standardSizes.value = 0;
        isSameOnBothSides.isOn = Current.GamePiece.Token.SameOnBothSides;
        isDoubleSided.isOn = Current.GamePiece.GameBoard.IsDoubleSided;
        RefreshUI();
        _ignoreChanges = false;
    }

    private void RefreshUI()
    {
        _ignoreChanges = true;
        Current.MutateAndSave(mutateProject: project => project.EnsureValid());
        dataSourceControl.SetActive(Current.Project.DataSources.Count > 0);
        if (!string.IsNullOrWhiteSpace(Current.GamePiece.DataSourceId) && Current.DataSource.Tables.Count > 1)
        {
            tableControl.SetActive(true);
            table.options = Current.DataSource.Tables.Select(x => new TMP_Dropdown.OptionData(x.Name)).ToList();
            table.value = Current.DataSource.Tables.FirstIndexOf(x => x.Name == Current.GamePiece.TableName);
            Current.SelectTable(Current.DataSource.Tables.First(x => x.Name == Current.GamePiece.TableName));
        }
        else
            tableControl.SetActive(false);
        filter.gameObject.SetActive(!string.IsNullOrWhiteSpace(Current.GamePiece.TableName));
        standardSizesControl.SetActive(Current.GamePiece.Type == GamePieceType.Card);
        isSameOnBothSidesControl.SetActive(Current.GamePiece.Type == GamePieceType.Token);
        isDoubleSidedControl.SetActive(Current.GamePiece.Type == GamePieceType.Board);
        _ignoreChanges = false;
    }
}