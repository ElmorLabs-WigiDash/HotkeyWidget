using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HotkeyWidget
{
    /// <summary>
    /// Interaction logic for ActionRow.xaml
    /// </summary>
    public partial class ActionRow : UserControl
    {

        private Action DeleteAction;
        private Action EditAction;
        private Action<bool> MoveAction;

        public ActionRow()
        {
            InitializeComponent();
        }

        public ActionRow(string actionName, Action deleteAction, Action editAction, Action<bool> moveAction)
        {
            DeleteAction = deleteAction;
            EditAction = editAction;
            MoveAction = moveAction;

            InitializeComponent();

            ActionNameText.Content = actionName;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteAction.Invoke();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            EditAction.Invoke();
            InvalidateVisual();
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            MoveAction.Invoke(true);
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            MoveAction.Invoke(false);
        }
    }
}
