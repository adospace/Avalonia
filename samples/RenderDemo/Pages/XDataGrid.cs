using System.Reactive.Linq;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using RenderDemo.ViewModels;
using System;
using System.Linq;
using Avalonia.Controls.Presenters;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Controls.Templates;
using System.Linq.Expressions;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Collections;
using Avalonia.Styling;

namespace RenderDemo.Pages
{
    public class XDataGrid : TemplatedControl, IStyleable
    {
        Type IStyleable.StyleKey => typeof(XDataGrid);

        internal static readonly DirectProperty<XDataGrid, XDataGridHeaderDescriptors> HeaderDescriptorsProperty =
            AvaloniaProperty.RegisterDirect<XDataGrid, XDataGridHeaderDescriptors>(
                nameof(HeaderDescriptors),
                o => o.HeaderDescriptors,
                (o, v) => o.HeaderDescriptors = v);

        private XDataGridHeaderDescriptors _headerDescriptors;

        internal XDataGridHeaderDescriptors HeaderDescriptors
        {
            get => _headerDescriptors;
            set => SetAndRaise(HeaderDescriptorsProperty, ref _headerDescriptors, value);
        }





        public static readonly DirectProperty<XDataGrid, IEnumerable> ItemsProperty =
            AvaloniaProperty.RegisterDirect<XDataGrid, IEnumerable>(
                nameof(Items),
                o => o.Items,
                (o, v) => o.Items = v);

        private IEnumerable _items;

        public IEnumerable Items
        {
            get { return _items; }
            set
            {
                if (value is null) return;
                DoAutoGeneratedHeaders(value.GetItemType());
                SetAndRaise(ItemsProperty, ref _items, value);
            }
        }

        private void DoAutoGeneratedHeaders(Type DataType)
        {
            int i = 0;
            var xdghList = new XDataGridHeaderDescriptors();

            foreach (var property in DataType.GetProperties())
            {
                var dispNameAttrib = (DisplayNameAttribute)property
                                        .GetCustomAttributes(typeof(DisplayNameAttribute), false)
                                        .SingleOrDefault();

                // var colWidthAttrib = (ColumnWidthAttribute)property
                //                         .GetCustomAttributes(typeof(ColumnWidthAttribute), false)
                //                         .SingleOrDefault();

                if (dispNameAttrib is null)
                    continue;

                var dName = dispNameAttrib.DisplayName;

                var xdgh = new XDataGridHeaderDescriptor()
                {
                    ColumnDefinitionIndex = i,
                    HeaderText = dName,
                    PropertyName = property.Name,
                    // ColumnWidth = colWidthAttrib.Width
                };

                i++;

                xdghList.Add(xdgh);
            }

            HeaderDescriptors = xdghList;
            this.DataType = DataType;
        }



        public static readonly DirectProperty<XDataGrid, Type> DataTypeProperty =
            AvaloniaProperty.RegisterDirect<XDataGrid, Type>(
                nameof(DataType),
                o => o.DataType,
                (o, v) => o.DataType = v);

        private Type _dataType;

        public Type DataType
        {
            get { return _dataType; }
            set
            {
                if (value is null) return;

                SetAndRaise(DataTypeProperty, ref _dataType, value);
            }
        }

        public XDataGrid()
        {

        }
    }
}
