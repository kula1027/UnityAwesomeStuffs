namespace AwesomeSnippets {

    public class SomePreloadedSingleton : SingletonPreloaded<SomePreloadedSingleton> {

        protected override void Awake() {
            base.Awake();
        }
    }
}