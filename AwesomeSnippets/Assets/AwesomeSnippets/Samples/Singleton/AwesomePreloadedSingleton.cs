namespace AwesomeSnippets {
    public class AwesomePreloadedSingleton : SingletonPreloaded<AwesomePreloadedSingleton> {
        protected override void Awake() {
            base.Awake();
        }
    }
}