﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel;

namespace Pihrtsoft.CodeAnalysis.CSharp.Refactorings.Tests
{
    internal class NotifyPropertyChangedRefactoring
    {
        private class Entity : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged(string propertyName)
            {
                throw new NotImplementedException();
            }

            private string _value;

            public string Value
            {
                get { return _value; }
                set { _value = value; }
            }
        }
    }
}
