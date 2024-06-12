using l10;
using LAB12_3.AVL_TREE;
using LAB12_4.AvlTreeNET;

namespace Lab13;

/// <summary>
/// Делегат, для событий изменения коллекции MyObserveCollection
/// </summary>
public delegate void CollectionHandler(object source, CollectionHandlerEventArgs args);

/// <summary>
/// Колекция, которую нужно создать по заданию
/// </summary>
/// <typeparam name="T"></typeparam>
public class MyObservableCollection<T> : AvlTreeNet<T> where T : IComparable, ICloneable, IInit, new()
{
    /// <summary>
    /// Название коллекции
    /// </summary>
    public string Name { get; private set; }
    public int Length => Count; // количество элементов в дереве

    public event CollectionHandler CollectionCountChanged; // событие изменения количества объектов в дереве
    public event CollectionHandler CollectionReferenceChanged; // событие изменения ссылки, исп. в this set
    
    /// <summary>
    /// Конструктор, инициализирующий коллекцию с именем и size количеством элементов ДСЧ
    /// </summary>
    /// <param name="name"></param>
    /// <param name="size"></param>
    public MyObservableCollection(string name, int size) : base(size)
    {
        Name = name;
    }

    /// <summary>
    /// Индексатор
    /// </summary>
    /// <param name="key">Объект в качестве индекса</param>
    public T this[T key]
    {
        get
        {
            var val = this.FindByValue(key);
            if (val == null) throw new ArgumentException("Элемент не был найден");
            return val;
        }
        set
        {
            if (value == null) throw new ArgumentException("Добавляемое значение не может быть null");

            if (this.FindByValue(key) == null)
            {
                throw new ArgumentException("Элемент с данным ключом не найден");
            }
            
            var val = this.FindByValue(value);

            if (val != null)
            {
                throw new ArgumentException("Дубликаты запрещены");
            }

            Remove(key);
            Add(value);
            //вызов ивента изменения ссылки
            OnCollectionReferenceChanged(this, new CollectionHandlerEventArgs("Изменение ссылки", key.ToString() + "\nизменены на:\n" + value.ToString()));
        } 
    }

    /// <summary>
    /// Добавление элемента в дерево
    /// </summary>
    /// <param name="item">Элемент для добавления</param>
    public override void Add(T item)
    {
        if (base.Insert(item))
            OnCollectionCountChanged(this, new CollectionHandlerEventArgs("Добавление", item));
    }

    /// <summary>
    /// Удаление элемента из дерева
    /// </summary>
    /// <param name="value">Удаляемый элемент</param>
    /// <returns>true, если элемент найден и удален</returns>
    public override bool Remove(T value)
    {
         bool didRemove = base.Remove(value);
         if (didRemove)
         {
             //Поск. элемент был удален, вызываем событие уменьшения количества элементов в дереве
             OnCollectionCountChanged(this, new CollectionHandlerEventArgs("Удаление", value));
         }

         return didRemove;
    }

    /// <summary>
    /// Очистка дерева
    /// </summary>
    public override void Clear()
    {
        DestroyNode(_root);
        base.Clear();
    }

    /// <summary>
    /// Метод, уничтожающий поддерево.
    /// Так как можно приравнять уничтожение и удаление, то на каждом рекурсивном вызове
    /// дополнительно вызываем ивент изменения  количества элементов в дереве
    /// </summary>
    /// <param name="node"></param>
    private void DestroyNode(AvlTreeNode<T> node)
    {
        if (node == null) return;
        
        //уничтожение левого поддерева
        DestroyNode(node.Left);
        //уничтожение правого поддерева
        DestroyNode(node.Right);

        node.Left = null;
        node.Right = null;

        Count -= 1;
        // Так как можно приравнять уничтожение и удаление, то на каждом рекурсивном вызове
        // дополнительно вызываем ивент изменения  количества элементов в дереве
        OnCollectionCountChanged(this, new CollectionHandlerEventArgs("Удаление", node.Value));

        //занулление текущего значения
        node.Value = default(T);
    }

    /// <summary>
    /// Вызыватор для ивента изменения поля Count
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected virtual void OnCollectionCountChanged(object source, CollectionHandlerEventArgs args)
    {
        CollectionCountChanged?.Invoke(source, args);
    }

    /// <summary>
    /// Вызыватор для ивента изменения ссылки по индексу
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected virtual void OnCollectionReferenceChanged(object source, CollectionHandlerEventArgs args)
    {
        CollectionReferenceChanged?.Invoke(source, args);
    }
}