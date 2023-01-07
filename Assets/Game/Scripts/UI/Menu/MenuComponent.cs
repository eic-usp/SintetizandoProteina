namespace UI.Menu
{
    public class MenuComponent : Partition
    {
        public void OnPointerEnter()
        {
            selection.PositioningBoltByIndex(index);
            Audio.AudioManager.Instance.Play(Audio.SoundEffectTrack.ButtonClick);
        }
        public void DoOperation()
        {
            base.Resume();
        }

        public void Quit()
        {
            selection.Quit(); 
        }
    }
}