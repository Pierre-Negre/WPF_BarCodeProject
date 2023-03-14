namespace WPF_Front.Custom_Components
{
    public class DataGridNumericColumn : DataGridTextColumn
    {
        /// <summary>
        /// Link events for custom components
        /// </summary>
        /// <param name="editingElement"></param>
        /// <param name="editingEventArgs"></param>
        /// <returns></returns>
        protected override object PrepareCellForEdit(System.Windows.FrameworkElement editingElement, System.Windows.RoutedEventArgs editingEventArgs)
        {
            TextBox edit = editingElement as TextBox;
            edit.PreviewTextInput += OnPreviewTextInput;
            edit.TextChanged += OnTextChanged;

            return base.PrepareCellForEdit(editingElement, editingEventArgs);
        }

        /// <summary>
        /// Fires on text changed, prevents out of bounds values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var send = sender as TextBox;
                var txt = send.Text;
                if (string.IsNullOrEmpty(txt)) (sender as TextBox).Text = "0";
                var test = Convert.ToInt32(txt);

                if (test > 100)
                {
                    (sender as TextBox).Text = "100";
                    e.Handled = true;
                }

                if (test < 0)
                {
                    (sender as TextBox).Text = "0";
                    e.Handled = true;
                }
            }
            catch
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Fires before inputs, prevents non numerical values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            try
            {
                Convert.ToInt32(e.Text);
            }
            catch
            {
                e.Handled = true;
            }
        }
    }
}
