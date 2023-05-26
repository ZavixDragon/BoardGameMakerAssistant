using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TextElement
{
    public bool UseProperty;
    public string Property;
    public string Content;
    public string Font;
    public bool Bold;
    public bool Italic;
    public bool Underline;
    public bool Strikethrough;
    public bool Lowercase;
    public bool Uppercase;
    public decimal FontSize;
    public bool AutoSize;
    public decimal AutoSizeMin;
    public decimal AutoSizeMax;
    public ConditionedSwitch<Color> Color;
    public float CharacterSpacing;
    public float WordSpacing;
    public float LineSpacing;
    public float ParagraphSpacing;
    public HorizontalTextAlignment HorizontalAlignment;
    public VerticalTextAlignment VerticalAlignment;
    public bool Wrapping;
    public TextOverflow Overflow;
}