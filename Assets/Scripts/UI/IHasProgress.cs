using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasProgress
{
    public event EventHandler<OnCuttingProgressChangedEventArgs> OnCuttingProgressChanged;
    public class OnCuttingProgressChangedEventArgs : EventArgs
    {
        public float ProgressNormalized;
    }
}
