using System;
using System.Collections.Generic;
using UnityEngine;

namespace IngameConsole {

    public abstract class ConsoleViewInterface : MonoBehaviour {

        public abstract void Initialize(Func<List<ConsoleData>> getFilteredData);

        public abstract void Add(ConsoleData data);

        public abstract void ResetData();

        public abstract void Hide();

        public abstract void Show();

        public abstract bool IsVisible { get; }
    }
}