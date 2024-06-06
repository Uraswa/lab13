using System.Collections;
using System.Diagnostics.CodeAnalysis;
using l10;
using LAB12_3.AVL_TREE;

namespace LAB12_4.AvlTreeNET;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
[ExcludeFromCodeCoverage]
public class AvlTreeNet<T> : AvlTree<T>, ICollection<T> where T : IComparable, ICloneable, IInit, new()
{
    public bool IsReadOnly => false;

    /// <summary>
    /// пустой конструктор
    /// </summary>
    public AvlTreeNet(){}
    
    /// <summary>
    /// Конструктор, инициализирующий дерево с заданным количеством случайных элементов
    /// </summary>
    /// <param name="size">Количество элементов</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public AvlTreeNet(int size)
    {
        if (size < 0)
        {
            throw new ArgumentOutOfRangeException("Размер коллекции не может быть отрицательным");
            return;
        }

        for (int i = 0; i < size; i++)
        {
            T newItem = new T();
            newItem.RandomInit();
            Add(newItem);
        }
    }

    /// <summary>
    /// Конструктор, который копирует уже существующее дерево
    /// </summary>
    /// <param name="collection"></param>
    public AvlTreeNet(AvlTreeNet<T> collection)
    {
        _root = CloneNode(collection._root);
    }

    /// <summary>
    ///  Клонирование поддерева
    /// </summary>
    /// <param name="node">Нода для клонирования</param>
    /// <returns>отклонированное поддерево</returns>
    private AvlTreeNode<T> CloneNode(AvlTreeNode<T> node)
    {
        if (node == null) return null;
        
        //клонирование текущей ноды
        var parent = new AvlTreeNode<T>((T)node.Value.Clone(), null, null, node.Height);
        Count++;
        
        //клонирование левого поддерева
        if (node.Left != null)
        {
            parent.Left = CloneNode(node.Left);
        }

        //клонирование правого поддерева
        if (node.Right != null)
        {
            parent.Right = CloneNode(node.Right);
        }

        return parent;
    }
    
    /// <summary>
    ///  Итератор по дереву
    /// </summary>
    /// <returns></returns>
    public IEnumerator<T> GetEnumerator()
    {
        return GetValues().GetEnumerator();
    }

    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    ///  Добавление элемента в дерево
    /// </summary>
    /// <param name="item"></param>
    public virtual void Add(T item)
    {
        Insert(item);
    }

    /// <summary>
    /// Содержит ли дерево элемент
    /// </summary>
    /// <param name="item">Элемент содержание которого нужно проверить</param>
    /// <returns>true - если содержит</returns>
    public bool Contains(T item)
    {
        var result = FindByValue(item);
        return result != null;
    }

    /// <summary>
    ///  Копирование дерева в массив
    /// </summary>
    /// <param name="array">Массив, куда копируем</param>
    /// <param name="arrayIndex">Индекс в массиве, откуда начать вставлять последовательность</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public void CopyTo(T[] array, int arrayIndex)
    {
        if (array == null) throw new ArgumentNullException("Массив не ициализирован."); 
        if (arrayIndex < 0) throw new ArgumentOutOfRangeException("Недопустимый индекс массива."); 
        if (array.Length - arrayIndex < Count) throw new ArgumentException("Недостаточно места в массиве.");
        CopyTo(_root, array, ref arrayIndex);
    }
    
    /// <summary>
    /// Копирование дерева в массив
    /// </summary>
    /// <param name="node">Узел для копирования элемента</param>
    /// <param name="array">Массив, куда копируем</param>
    /// <param name="index">Индекс текущей свободной позиции в массиве для вставки</param>
    private void CopyTo(AvlTreeNode<T>? node, T[] array, ref int index)
    {
        if (node == null) return;
        
        array[index++] = (T)node.Value.Clone(); // текущий элемент
        CopyTo(node.Left, array, ref index); // Рекурсивное копирование в левое поддерево
        CopyTo(node.Right, array, ref index); // Рекурсивное копирование в правое поддерево
    }

    
}