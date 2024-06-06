namespace Lab13;

/// <summary>
/// Объект истории
/// </summary>
public class JournalEntry
{
    /// <summary>
    /// Название коллекции, в которой было произведено изменение
    /// </summary>
    public string CollectionName { get; set; }

    /// <summary>
    /// Тип изменения(например удаление, добавление и т.д)
    /// </summary>
    public string ChangeType { get; set; }

    /// <summary>
    /// Состояние объекта, например, если произошло удаление из
    /// коллекции элемента, сюда можно записать состояние этого элемента на момент удаления
    /// </summary>
    public string ObjectData { get; set; }

    public JournalEntry(string collectionName, string changeType, string objectData)
    {
        CollectionName = collectionName;
        ChangeType = changeType;
        ObjectData = objectData;
    }

    public override string ToString()
    {
        return $"Название коллекции: {CollectionName} \nТип изменения: {ChangeType} \nДанные объекта: {ObjectData}";
    }
}