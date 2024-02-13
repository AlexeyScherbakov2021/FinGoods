using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FinGoods.Infrastructure
{
    public class ScrollIntoViewBehavior : Behavior<DataGrid>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged +=
                new SelectionChangedEventHandler(AssociatedObjectSelectionChanged);
        }
        protected override void OnDetaching()
        {
            AssociatedObject.SelectionChanged -=
                new SelectionChangedEventHandler(AssociatedObjectSelectionChanged);
            base.OnDetaching();
        }

        private async void AssociatedObjectSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid)
            {
                DataGrid grid = sender as DataGrid;
                object item = grid?.SelectedItem;

                if (item != null)
                {
                    Action action = () =>
                    {
                        if (grid != null && item != null)
                        {
                            grid.UpdateLayout();
                            grid.ScrollIntoView(item, null);
                        }
                    };

                    if (grid.Dispatcher.CheckAccess())
                    {
                        action();
                    }
                    else
                    {
                        await grid.Dispatcher.BeginInvoke(action);
                    }
                }
            }
        }
    }
}
