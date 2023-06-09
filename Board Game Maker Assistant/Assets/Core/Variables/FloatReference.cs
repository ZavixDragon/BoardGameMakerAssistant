﻿using System;

[Serializable]
public class FloatReference
{
    public bool UseConstant = true;
    public float ConstantValue;
    public FloatVariable FloatVariable;

    public float Value => UseConstant || FloatVariable == null ? ConstantValue : FloatVariable.Value;

    public FloatReference() { }

    public FloatReference(float value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public override string ToString() => Value.ToString();
    public static implicit operator float(FloatReference reference) => reference.Value;
}
