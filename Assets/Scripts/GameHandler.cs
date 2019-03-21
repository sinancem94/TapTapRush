
// GameState sahip On ana ekran da, Start oyun oynarken, Over da bitiş ekranı
public class GameHandler {

    public GameState state;

    public enum GameState
    {
        BeginingPage,
        GameRunning,
        LevelPassed,
        GameOver
    };

    public GameHandler(GameState currState)
    {
        state = currState;
    }

    public GameState GetGameState() 
    {
        return state;
    }

    public void GameOver()
    {
        state = GameState.GameOver;
    }
    public void StartGame()
    {
        state = GameState.GameRunning;
    }
    public void LevelPassed()
    {
        state = GameState.LevelPassed;
    }

}
