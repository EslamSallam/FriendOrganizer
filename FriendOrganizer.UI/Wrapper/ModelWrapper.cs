using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace FriendOrganizer.UI.Wrapper
{
	public class ModelWrapper<T> : NotifyDataErrorBaseClass
	{
		public ModelWrapper(T model)
		{
			Model = model;
		}

		public T Model { get; }
		protected virtual void SetValue<TValue>(TValue? value, [CallerMemberName] string? PropertyName = null)
		{
			typeof(T).GetProperty(PropertyName).SetValue(Model, value);
			OnPropertyChanged(PropertyName);
			validatePropertyInternal(PropertyName);
		}

		protected virtual TValue GetValue<TValue>([CallerMemberName] string? PropertyName = null)
		{
			return (TValue)typeof(T).GetProperty(PropertyName).GetValue(Model);
		}

		private void validatePropertyInternal(string propertyName)
		{
			ClearErrors(propertyName);

			//1. TODO:: Validate Data Annotations



			//2. Validate Custom errors

			var errors = validateProperty(propertyName);
			if (errors != null)
			{
				foreach (var error in errors)
				{
					AddError(propertyName, error);
				}
			}
		}

		protected virtual IEnumerable<string> validateProperty(string propertyName)
		{
			return null;
		}
	}
}