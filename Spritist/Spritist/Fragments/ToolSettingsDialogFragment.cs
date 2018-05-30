using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Spritist
{
    public class ToolSettingsDialogFragment : DialogFragment
    {
        public class ToolSettingResult
        {
            public int pixelSize;
        }

        private Action<ToolSettingResult> onOkAction;

        private SeekBar pixelSizeSeekbar;
        private TextView pixelSizeText;

        public ToolSettingsDialogFragment(Action<ToolSettingResult> onOkAction)
        {
            this.onOkAction = onOkAction;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            MakeSpriteActivity activity = (MakeSpriteActivity) Activity;
            string toolName = Arguments.GetString(GetString(Resource.String.bundle_draw_tool_name));
            
            AlertDialog.Builder builder = new AlertDialog.Builder(activity);

            LayoutInflater inflator =
                (LayoutInflater) Activity.GetSystemService(Context.LayoutInflaterService);
            View view = inflator.Inflate(Resource.Layout.dialog_tool_settings, null);

            builder.SetTitle($"Edit {toolName}");
            builder.SetView(view)
                .SetPositiveButton("OK", OnPositiveButtonClicked)
                .SetNegativeButton("Cancel", OnCancelButtonClicked);

            pixelSizeSeekbar = view.FindViewById<SeekBar>(Resource.Id.dialog_tool_settings_pixel_size);
            pixelSizeText =
                view.FindViewById<TextView>(Resource.Id
                    .dialog_tool_settings_pixel_size_text);
            pixelSizeSeekbar.ProgressChanged += OnPixelSizeSeekbarChanged;

            return builder.Create();
        }

        private void OnPixelSizeSeekbarChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            pixelSizeText.Text = pixelSizeSeekbar.Progress.ToString();
        }

        private void OnCancelButtonClicked(object sender, DialogClickEventArgs e)
        {
            Dismiss();
        }

        private void OnPositiveButtonClicked(object sender, DialogClickEventArgs e)
        {
            ToolSettingResult result = new ToolSettingResult
            {
                pixelSize = pixelSizeSeekbar.Progress
            };
            onOkAction(result);
        }
    }
}