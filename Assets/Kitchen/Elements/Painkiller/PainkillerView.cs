namespace Kitchen
{
    public class PainkillerView : ButtonHandler
    {
        protected override void OnClick() =>
            SelectionManager.SelectedGameObject = this;
    }
}
