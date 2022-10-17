namespace Kitchen
{
    public class TrashView : ButtonHandler
    {
        protected override void OnClick()
        {
            PotionView potionView = SelectionManager.SelectedGameObject as PotionView;
            IngredientView ingredientView = SelectionManager.SelectedGameObject as IngredientView;
            SelectionManager.SelectedGameObject = null;

            if (potionView != null || (ingredientView != null && ingredientView.State != IngredientState.None)) 
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Cocina/Trash");

            if (potionView != null)
                potionView.Clear();
            else if (ingredientView != null && ingredientView.State != IngredientState.None)
                ingredientView.Release();
        }
    }
}
