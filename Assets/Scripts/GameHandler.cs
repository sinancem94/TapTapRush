
// GameState sahip On ana ekran da, Start oyun oynarken, Over da bitiş ekranı
public class GameHandler {

    public GameState state;

    public enum GameState
    {
        BeginingPage,
        GameRunning,
        GameOver
    };

    public GameHandler(GameState currState)
    {
        state = currState;
    }

    public void GameOver()
    {
        state = GameState.GameOver;
        Platform.instance.runner.GetComponent<Runner>().enabled = false;
    }
    public void StartGame()
    {
        state = GameState.GameRunning;
    }

}
