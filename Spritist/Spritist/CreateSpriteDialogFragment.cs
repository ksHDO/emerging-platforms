using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Spritist
{
    public class CreateSpriteDialogFragment : DialogFragment
    {
        private EditText editTextDimensionX;
        private EditText editTextDimensionY;

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
            builder.SetTitle("Create New Sprite");
            
            builder.SetView(Resource.Layout.dialog_create_sprite)
                .SetPositiveButton("Create!",
                    OnCreateButtonClicked)
                .SetNegativeButton("Cancel",
                    OnCancelButtonClicked);
            AlertDialog alertDialog = builder.Create();
            
            editTextDimensionX = alertDialog.FindViewById<EditText>(Resource.Id.dialog_create_sprite_x);
            editTextDimensionY = alertDialog.FindViewById<EditText>(Resource.Id.dialog_create_sprite_y);
            
            return alertDialog;
        }

        private void OnCancelButtonClicked(object sender, DialogClickEventArgs e)
        {
            Dismiss();
        }

        private void OnCreateButtonClicked(object sender, DialogClickEventArgs e)
        {
            Intent intent = new Intent();
            intent.SetClass(Activity, typeof(MakeSpriteActivity));
            Bundle bundle = new Bundle();
            
            //bundle.PutIntArray(GetString(Resource.String.bundle_sprite_dimensions), new[]
            //{
            //    int.Parse(editTextDimensionX.Text),
            //    int.Parse(editTextDimensionY.Text)
            //});

            StartActivity(intent, bundle);
        }
    }
}