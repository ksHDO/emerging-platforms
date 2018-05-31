using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Spritist
{
    public class ToolSettingsDialogFragment : DialogFragment
    {
        public class ToolSettingResult
        {
            public int PixelSize;
            public Color Color;
        }

        private Action<ToolSettingResult> onOkAction;

        private class ViewItems
        {
            public int SeekBarProgress
            {
                get { return Seekbar.Progress; }
                set { Seekbar.Progress = value; }
            }

            public string Text
            {
                get { return Textview.Text; }
                set { Textview.Text = value; }
            }

            public SeekBar Seekbar;
            public TextView Textview;

            public ViewItems()
            {
            }

            public ViewItems(SeekBar s, TextView v)
            {
                Seekbar = s;
                Textview = v;
            }
        }

        private int size;
        private Color color;

        private ViewItems pixelSizeView;
        private ViewItems colorRView;
        private ViewItems colorGView;
        private ViewItems colorBView;
        private ViewItems colorAView;

        public ToolSettingsDialogFragment(int pixelSize, Color drawColor, Action<ToolSettingResult> onOkAction)
        {
            size = pixelSize;
            color = drawColor;

            this.onOkAction = onOkAction;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            MakeSpriteActivity activity = (MakeSpriteActivity) Activity;

            AlertDialog.Builder builder = new AlertDialog.Builder(activity);

            LayoutInflater inflator =
                (LayoutInflater) Activity.GetSystemService(Context.LayoutInflaterService);
            View view = inflator.Inflate(Resource.Layout.dialog_tool_settings, null);

            builder.SetTitle($"Edit Drawing Settings");
            builder.SetView(view)
                .SetPositiveButton("OK", OnPositiveButtonClicked)
                .SetNegativeButton("Cancel", OnCancelButtonClicked);

            SetupViewsItems(view);

            return builder.Create();
        }

        private void SetupViewsItems(View view)
        {
            pixelSizeView = SetupViewItem(view, size, Resource.Id.dialog_tool_settings_pixel_size,
                Resource.Id.dialog_tool_settings_pixel_size_text);
            colorRView = SetupViewItem(view, color.R, Resource.Id.dialog_tool_settings_color_r,
                Resource.Id.dialog_tool_settings_color_r_text);
            colorGView = SetupViewItem(view, color.G, Resource.Id.dialog_tool_settings_color_g,
                Resource.Id.dialog_tool_settings_color_g_text);
            colorBView = SetupViewItem(view, color.B, Resource.Id.dialog_tool_settings_color_b,
                Resource.Id.dialog_tool_settings_color_b_text);
            colorAView = SetupViewItem(view, color.A, Resource.Id.dialog_tool_settings_color_a,
                Resource.Id.dialog_tool_settings_color_a_text);
        }

        private ViewItems SetupViewItem(View view, int value, int seekbarId, int textId)
        {
            ViewItems item = new ViewItems()
            {
                Seekbar  = view.FindViewById<SeekBar>(seekbarId),
                Textview = view.FindViewById<TextView>(textId)
            };
            item.SeekBarProgress = value;
            item.Text = value.ToString();
            item.Seekbar.ProgressChanged += ProgressBarChanged(item);
            return item;
        }

        private EventHandler<SeekBar.ProgressChangedEventArgs> ProgressBarChanged(
            ViewItems viewItem)
        {
            void BarChanged(object sender, SeekBar.ProgressChangedEventArgs e)
            {
                viewItem.Text = viewItem.SeekBarProgress.ToString();
            }
            return BarChanged;
        }

        private void OnCancelButtonClicked(object sender, DialogClickEventArgs e)
        {
            Dismiss();
        }

        private void OnPositiveButtonClicked(object sender, DialogClickEventArgs e)
        {
            ToolSettingResult result = new ToolSettingResult
            {
                PixelSize = pixelSizeView.SeekBarProgress,
                Color = new Color(
                    (byte) colorRView.SeekBarProgress,
                    (byte) colorGView.SeekBarProgress,
                    (byte) colorBView.SeekBarProgress,
                    (byte) colorAView.SeekBarProgress
                    )
            };
            onOkAction(result);
        }
    }
}