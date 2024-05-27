using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FinGoods.Infrastructure
{
    internal class DataGridMultiSelect : DataGrid
    {
        public IList SelectedItemsList
        {
            get => (IList)GetValue(SelectedItemsListProperty);
            set => throw new Exception("This property is read-only. To bind to it you must use 'Mode=OneWayToSource'.");
        }


        public static readonly DependencyProperty SelectedItemsListProperty =
            DependencyProperty.Register("SelectedItemsList", typeof(IList), typeof(DataGridMultiSelect),
                    new PropertyMetadata(default(IList)));

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            SetValue(SelectedItemsListProperty, base.SelectedItems);
        }


    }

}
