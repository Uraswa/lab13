namespace Lab13;

/// <summary>
/// Объект аргументов для событий
/// </summary>
public class CollectionHandlerEventArgs : System.EventArgs
{
    /// <summary>
    /// Тип события (например удаление, добавление и т.д)
    /// </summary>
    public string ChangeType { get; set; }
    
    /// <summary>
    /// Объект, который был вовлечен в событие.
    /// Например, если мы удалили элемент из коллекции, здесь можно хранить этот элемент
    /// </summary>
    public object Object { get; set; }

    public CollectionHandlerEventArgs(string changeType, object obj)
    {
        ChangeType = changeType;
        Object = obj;
    }

}