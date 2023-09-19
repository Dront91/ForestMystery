namespace MysteryForest
{
    public class SquirrelSoundController : BaseEnemySoundController
    {
        private Destructible _destructible;
        private AIShooter _aISquirrel;
        protected override void Start()
        {
            base.Start();
            _destructible = GetComponentInParent<Destructible>();
            _aISquirrel = GetComponentInParent<AIShooter>();
            _destructible.DamageTaken += OnDamageTaken;
            _aISquirrel.OnConeThrow += OnConeThrow;
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            _destructible.DamageTaken -= OnDamageTaken;
            _aISquirrel.OnConeThrow -= OnConeThrow;
        }

        private void OnConeThrow()
        {
            _audioClip = Sound.SquirrelThrowCone;
            PlaySound();
        }

        private void OnDamageTaken()
        {
            _audioClip = Sound.SquirrelDamageTaken;
            PlaySound();
        }
    }
}
