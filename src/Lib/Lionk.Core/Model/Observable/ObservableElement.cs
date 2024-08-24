// Copyright © 2024 Lionk Project

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Lionk.Core.Observable;

/// <summary>
///     An abstract class that implement <see cref="INotifyPropertyChanged" />.
/// </summary>
public abstract class ObservableElement : INotifyPropertyChanged
{
    #region delegate and events

    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion

    #region others methods

    /// <summary>
    ///     Raises the <see cref="PropertyChanged" /> event.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    /// <summary>
    ///     Sets the field and raises the <see cref="PropertyChanged" /> event if the value has changed.
    /// </summary>
    /// <typeparam name="T">The type of the property.</typeparam>
    /// <param name="field">A referencing to the field backing the property.</param>
    /// <param name="value">The new value for the property.</param>
    /// <param name="propertyName">
    ///     The name of the property that changed.
    ///     This is automatically supplied by the CallerMemberAttribute.
    /// </param>
    /// <returns>True if the value was changed and the event was raised. False otherwise.</returns>
    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    #endregion
}
