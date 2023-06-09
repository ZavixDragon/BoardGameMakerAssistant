using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TextElement
{
    public DataDependent<string> Content = new DataDependent<string> { Concrete = "" };
    public string Font;
    public bool Bold = false;
    public bool Italic = false;
    public bool Underline = false;
    public bool Strikethrough = false;
    public bool Lowercase = false;
    public bool Uppercase = false;
    public decimal FontSize = 16;
    public bool AutoSize = false;
    public decimal AutoSizeMin = 8;
    public decimal AutoSizeMax = 32;
    public DataDependent<Color> Color = new DataDependent<Color> { Concrete = UnityEngine.Color.white };
    public float CharacterSpacing = 0;
    public float WordSpacing = 0;
    public float LineSpacing = 0;
    public float ParagraphSpacing = 0;
    public HorizontalTextAlignment HorizontalAlignment = HorizontalTextAlignment.Center;
    public VerticalTextAlignment VerticalAlignment = VerticalTextAlignment.Center;
    public bool Wrapping = true;
    public TextOverflow Overflow = TextOverflow.Overflow;
}