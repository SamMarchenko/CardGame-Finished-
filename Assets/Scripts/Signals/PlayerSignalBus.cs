namespace Signals
{
    public class PlayerSignalBus
    {
        private ChangeStageSignalHandler _changeStageSignalHandler;
        private ChangeCurrentPlayerSignalHandler _changeCurrentPlayerSignalHandler;
        private PlayerClickSignalHandler _playerClickSignalHandler;

        public void Init(ChangeStageSignalHandler changeStageSignalHandler,
            ChangeCurrentPlayerSignalHandler changeCurrentPlayerSignalHandler,
            PlayerClickSignalHandler playerClickSignalHandler)
        {
            _changeStageSignalHandler = changeStageSignalHandler;
            _changeCurrentPlayerSignalHandler = changeCurrentPlayerSignalHandler;
            _playerClickSignalHandler = playerClickSignalHandler;
        }

        public void StageChangeFire(StageChangeSignal signal)
        {
            _changeStageSignalHandler.Fire(signal);
        }

        public void CurrentPlayerChangeFire(CurrentPlayerChangeSignal signal)
        {
            _changeCurrentPlayerSignalHandler.Fire(signal);
        }

        public void PlayerClickFire(PlayerClickSignal signal)
        {
            _playerClickSignalHandler.Fire(signal);
        }
    }
}