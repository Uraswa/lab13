using System.Diagnostics.CodeAnalysis;
using l10;

namespace Lab13;

/// <summary>
/// Журнал, в котором хранятся изменения коллекции.
/// </summary>
public class Journal
{
    /// <summary>
    /// Список изменений
    /// </summary>
    public readonly List<JournalEntry> History = new List<JournalEntry>();

    /// <summary>
    /// Добавление события в историю
    /// </summary>
    /// <param name="source">Объект источник, например коллекция</param>
    /// <param name="args">Аргументы, должен быть тип события, состояние объекта на момент изменения в виде строки</param>
    public void AddHistoryItem(object source, CollectionHandlerEventArgs args)
    {
        if (source == null) throw new ArgumentException("Источник не может быть null!");
        if (source is not MyObservableCollection<Game> srcObj) throw new ArgumentException("Источник должен быть типа MyObservableCollection<Game>");
        if (args == null) throw new ArgumentException("Аргументы не могут быть null");
        if (args.Object == null) throw new ArgumentException("Объект не может быть null!");
        History.Add(new JournalEntry(
            srcObj.Name,
            args.ChangeType,
            args.Object.ToString()
            ));
    }
    
    /// <summary>
    /// Вывод списка изменений в консоль
    /// </summary>
    [ExcludeFromCodeCoverage]
    public void PrintChanges()
    {
        if (History.Count == 0) Console.WriteLine("Журнал изменений пуст");
        int counter = 1;
        foreach (var journalEntry in History)
        {
            Console.WriteLine("----Событие #" + counter + "----");
            Console.WriteLine(journalEntry.ToString());
            counter++;
        }
    }
}