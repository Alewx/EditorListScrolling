
namespace EditorListScrolling
{
	public class EditorListScrollingLockingManager
	{
		private EditorListScrollingLockingManager()
        {
        }

		private static EditorListScrollingLockingManager _instance = new EditorListScrollingLockingManager();
		private bool _updateLock = false;
		private bool _updateLocked = false;

		public static EditorListScrollingLockingManager instance
		{
			get { return _instance; }
		}

		public void startLockingUpdate()
		{
			_updateLock = false;
		}

		public void updateLocking()
		{
			_updateLock = true;
		}
		public void endlockingUpdate()
		{
			if (_updateLock && !_updateLocked)
			{
				InputLockManager.SetControlLock(ControlTypes.CAMERACONTROLS, Constants.lockKey);
				_updateLocked = true;
			}
			if (!_updateLock && _updateLocked)
			{
				InputLockManager.RemoveControlLock(Constants.lockKey);
				_updateLocked = false;
			}
		}

	}
}
