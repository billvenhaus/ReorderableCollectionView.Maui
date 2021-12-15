using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;

namespace ReorderableCollectionView.Maui
{
	public class ReorderableCollectionView : CollectionView
	{
		INotifyCollectionChanged _notifyCollection;
		IItemsLayout _itemsLayout;

		public static readonly BindableProperty CanMixGroupsProperty = BindableProperty.Create("CanMixGroups", typeof(bool), typeof(ReorderableCollectionView), false);
		public bool CanMixGroups
		{
			get { return (bool)GetValue(CanMixGroupsProperty); }
			set { SetValue(CanMixGroupsProperty, value); }
		}

		public static readonly BindableProperty CanReorderItemsProperty = BindableProperty.Create("CanReorderItems", typeof(bool), typeof(ReorderableCollectionView), false);
		public bool CanReorderItems
		{
			get { return (bool)GetValue(CanReorderItemsProperty); }
			set { SetValue(CanReorderItemsProperty, value); }
		}

		public event EventHandler ReorderCompleted;

		void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (ItemsLayout is VariableSpanGridItemsLayout)
			{
				if (Device.IsInvokeRequired)
				{
					Device.BeginInvokeOnMainThread(() => InvalidateMeasureNonVirtual(InvalidationTrigger.MeasureChanged));
				}
			}
		}

		void HandleLayoutPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (ItemsLayout is VariableSpanGridItemsLayout)
			{
				InvalidateMeasureNonVirtual(InvalidationTrigger.MeasureChanged);
			}
		}

		protected virtual void OnItemsLayoutChanged()
		{
			if (_itemsLayout != null)
			{
				_itemsLayout.PropertyChanged -= HandleLayoutPropertyChanged;
			}
			_itemsLayout = ItemsLayout;
			if (_itemsLayout != null)
			{
				_itemsLayout.PropertyChanged += HandleLayoutPropertyChanged;
			}

			if (ItemsLayout is VariableSpanGridItemsLayout)
			{
				InvalidateMeasureNonVirtual(InvalidationTrigger.MeasureChanged);
			}
		}

		protected virtual void OnItemsSourceChanged()
		{
			if (_notifyCollection != null)
			{
				_notifyCollection.CollectionChanged -= HandleCollectionChanged;
			}
			_notifyCollection = ItemsSource as INotifyCollectionChanged;
			if (_notifyCollection != null)
			{
				_notifyCollection.CollectionChanged += HandleCollectionChanged;
			}

			if (ItemsLayout is VariableSpanGridItemsLayout)
			{
				InvalidateMeasureNonVirtual(InvalidationTrigger.MeasureChanged);
			}
		}

		protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
		{
			if (ItemsLayout is VariableSpanGridItemsLayout)
			{
				if (!IsPlatformEnabled)
					return new SizeRequest(new Size(-1, -1));

				return Device.PlatformServices.GetNativeSize(this, widthConstraint, heightConstraint);
			}
			else
			{
				return base.OnMeasure(widthConstraint, heightConstraint);
			}
		}

		protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (propertyName == ItemsLayoutProperty.PropertyName)
			{
				OnItemsLayoutChanged();
			}
			else if (propertyName == ItemsSourceProperty.PropertyName)
			{
				OnItemsSourceChanged();
			}

			base.OnPropertyChanged(propertyName);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public void SendReorderCompleted() => ReorderCompleted?.Invoke(this, EventArgs.Empty);
	}
}