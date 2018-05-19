using Android.App;
using Android.Content;
using Android.OS;

namespace Spritist
{
    public class CreateSpriteDialogFragment : DialogFragment
    {
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
            builder.SetTitle("Create New Sprite");
            builder.SetView(Resource.Layout.dialog_create_sprite)
                .SetPositiveButton("Create!",
                    OnCreateButtonClicked)
                .SetNegativeButton("Cancel",
                    OnCancelButtonClicked);
            return builder.Create();
        }

        private void OnCancelButtonClicked(object sender, DialogClickEventArgs e)
        {
            Dismiss();
        }

        private void OnCreateButtonClicked(object sender, DialogClickEventArgs e)
        {
            Intent intent = new Intent();
            intent.SetClass(Activity, typeof(MakeSpriteActivity));
            StartActivity(intent);
        }
    }
}