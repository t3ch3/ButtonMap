using System;
using System.Collections.Generic;

using UIKit;

namespace JustButtons
{
    public class PickerModel : UIPickerViewModel
    {
        public IList<Object> values;

        public event EventHandler<PickerChangedEventArgs> PickerChanged;

        public PickerModel(IList<Object> values)
        {
            this.values = values;
        }

        public override nint GetComponentCount(UIPickerView picker)
        {
            return 1;
        }

        public override nint GetRowsInComponent(UIPickerView picker, nint component)
        {
            return values.Count;
        }

        public override string GetTitle(UIPickerView picker, nint row, nint component)
        {
            return values[(int)row].ToString();
        }

        public override nfloat GetRowHeight(UIPickerView picker, nint component)
        {
            return 60f;
        }
        public override UIView GetView(UIPickerView pickerView, nint row, nint component, UIView view)
        {
            var label = new UILabel();

            label.Text = values[(int)row].ToString();
            label.BackgroundColor = UIColor.Clear;
            label.TextAlignment = UITextAlignment.Center;
            label.Font = UIFont.FromName("Helvetica", 34);
            label.TextColor = UIColor.Black;


            return label;
        }


        public override void Selected(UIPickerView picker, nint row, nint component)
        {
            if (this.PickerChanged != null)
            {
                this.PickerChanged(this, new PickerChangedEventArgs { SelectedValue = values[(int)row] });
            }
        }
    }

    public class PickerChangedEventArgs : EventArgs
    {
        public object SelectedValue { get; set; }
    }
}
