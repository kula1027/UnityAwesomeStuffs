using System;

namespace AwesomeSnippets {
    public class ObservableVariable<T> {
        private T data;

        public ObservableVariable() {
            data = default;
        }

        /// <summary>
        ///     invoked when Data is set
        /// </summary>
        public Action<T> OnChange { get; set; }

        public T Data {
            get => data;
            set {
                data = value;
                OnChange?.Invoke(data);
            }
        }
    }
}