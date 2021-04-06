using System;

namespace AwesomeSnippets {

    public class ObservableVariable<T> {
        private T data;

        public ObservableVariable() {
            this.data = default;
        }

        /// <summary>
        /// invoked when Data is set
        /// </summary>
        public Action<T> OnChange { get; set; }

        public T Data {
            get => this.data;
            set {
                this.data = value;
                this.OnChange?.Invoke(this.data);
            }
        }
    }
}