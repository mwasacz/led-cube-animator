// This file is part of LED Cube Animator
// Copyright (C) 2019-2021, 2024 Mikolaj Wasacz
// SPDX-License-Identifier: GPL-3.0-only WITH GPL-3.0-linking-source-exception

using System;
using System.Linq;

namespace LedCubeAnimator.Model.Undo
{
    public class ArrayChangeAction<T> : ObjectAction
    {
        public ArrayChangeAction(object obj, string property, Array array, T newValue, params int[] indices) : base(obj, property)
        {
            Array = array;
            NewValue = newValue;
            Indices = indices;

            OldValue = (T)Array.GetValue(indices);
        }

        public Array Array { get; }
        public T OldValue { get; }
        public T NewValue { get; private set; }
        public int[] Indices { get; }

        public override bool IsEmpty => Equals(OldValue, NewValue);

        public override void Do()
        {
            Array.SetValue(NewValue, Indices);
        }

        public override void Undo()
        {
            Array.SetValue(OldValue, Indices);
        }

        public override bool TryMerge(IAction action)
        {
            if (action is ArrayChangeAction<T> next
                && next.Object == Object
                && next.Property == Property
                && next.Array == Array
                && next.Indices.SequenceEqual(Indices)
                && Equals(next.OldValue, NewValue))
            {
                NewValue = next.NewValue;
                return true;
            }
            return false;
        }
    }
}
