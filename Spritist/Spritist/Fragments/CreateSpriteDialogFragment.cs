using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using Spritist.Filters;
using Spritist.Utilities;

namespace Spritist.Fragments
{
    public class CreateSpriteDialogFragment : DialogFragment
    {
        private EditText spriteNameText;
        private TextView xView;
        private TextView yView;

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
            builder.SetTitle("Create New Sprite");

            LayoutInflater inflator = (LayoutInflater) Activity.GetSystemService(Context.LayoutInflaterService);
            View view = inflator.Inflate(Resource.Layout.dialog_create_sprite, null);

            builder.SetView(view)
                .SetPositiveButton("Create!",
                    OnCreateButtonClicked)
                .SetNegativeButton("Cancel",
                    OnCancelButtonClicked);

            
            xView = view.FindViewById<TextView>(Resource.Id.dialog_create_sprite_x);
            yView = view.FindViewById<TextView>(Resource.Id.dialog_create_sprite_y);
            xView.SetFilters(new IInputFilter[]{new InputFilterClamp(1, 64) });
            yView.SetFilters(new IInputFilter[]{new InputFilterClamp(1, 64) });

            return builder.Create();
        }

        private void OnCancelButtonClicked(object sender, DialogClickEventArgs e)
        {
            Dismiss();
        }

        private void OnCreateButtonClicked(object sender, DialogClickEventArgs e)
        {
            Dialog dialog = (Dialog) sender;
            spriteNameText = dialog.FindViewById<EditText>(Resource.Id.dialog_create_sprite_sprite_name);
            xView = dialog.FindViewById<EditText>(Resource.Id.dialog_create_sprite_x);
            yView = dialog.FindViewById<EditText>(Resource.Id.dialog_create_sprite_y);

            Intent intent = new Intent();
            intent.SetClass(Activity, typeof(MakeSpriteActivity));
            Bundle bundle = new Bundle();

            string spriteName = "Sprite";
            if (!string.IsNullOrWhiteSpace(spriteNameText.Text))
            {
                spriteName = spriteNameText.Text;
            }
            int xDimension = 16, yDimension = 16;
            if (!string.IsNullOrWhiteSpace(xView.Text))
            {
                int x = int.Parse(xView.Text);
                xDimension = x;
            }

            if (!string.IsNullOrWhiteSpace(yView.Text))
            {
                int y = int.Parse((yView.Text));
                yDimension = y;
            }

            bundle.PutString(GetString(Resource.String.bundle_sprite_name), spriteName);

            bundle.PutIntArray(GetString(Resource.String.bundle_sprite_dimensions), new[]
            {
                xDimension,
                yDimension
            });

            intent.PutExtras(bundle);

            StartActivity(intent);
        }
    }
}