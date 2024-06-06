using System.Collections;
using System.Diagnostics.CodeAnalysis;
using l10;

namespace LAB12_3.ISD;

/// <summary>
/// Идеально сбалансированное бинарное дерево
/// </summary>
/// <typeparam name="T"></typeparam>
[ExcludeFromCodeCoverage]
public class IsdTree<T> where T : class, ICloneable
{
    /// <summary>
    /// Корень дерева
    /// </summary>
    private IsdNode<T> _root;
    
    /// <summary>
    /// Количество элементов в дереве
    /// </summary>
    public int Count { get; private set; } 

    /// <summary>
    /// Конструктор дерева на основе массива.
    /// Элементы клонируются.
    /// </summary>
    /// <param name="array"></param>
    public IsdTree(T[] array) 
    {
        //создание пустого дерева
        if (array == null || array.Length == 0)
        {
            _root = null;
        }
        else
        {
            _root = BuildTreeFromArray(array, 0, array.Length);
        }
    }

    /// <summary>
    /// Обёртка над рекурсивным выводом дерева
    /// </summary>
    public void PrintTree()
    {
        if (_root == null)
        {
            Console.WriteLine("Дерево пустое!");
            return;
        }

        PrintTree(_root); //Иначе метод печати
    }

    /// <summary>
    /// Рекурсивный вывод дерева
    /// </summary>
    /// <param name="node">Текущий нод</param>
    /// <param name="level">Уровень в глубину</param>
    void PrintTree(IsdNode<T> node, int spaces = 5)
    {
        if (node != null) //Если точка дерева равна ноль, то ничего не делаем, иначе печатаем
        {
            PrintTree(node.Right, spaces + 5); //Запускаем печать правой ветки
            for (int i = 0; i < spaces; i++)
            {
                Console.Write(" "); //Выводим нужное количество пробелов, чтобы дерево вывелось в нужном порядке
            }

            string nodeName = node.Value == null ? "" : node.Value.ToString();
            Console.WriteLine(nodeName.Length == 0 ? "Пустой элемент" : nodeName); //Вывод саму информацию в точке
            PrintTree(node.Left, spaces + 5); //Запускаем печать левой ветки
        }
    }

    /// <summary>
    /// Очистка дерева
    /// </summary>
    public void Clear() 
    {
        _root = null; 
        Count = 0; 
    }
    
    /// <summary>
    /// Создание идеально сбалансированного дерева, массив должен быть отсортирован!
    /// </summary>
    /// <param name="arr">изначальный массив из элементов, из которых формируется дерево</param>
    /// <param name="start">стартовый индекс</param>
    /// <param name="end">конечный индекс</param>
    /// <returns>поддерево</returns>
    private IsdNode<T> BuildTreeFromArray(T[] arr, int start, int end)
    {
        if (start > end)
            return null;

        int mid = (start + end) / 2;
        
        //обязательно клонируем элемент, можем это сделать, так как в описании типа T стоит условие,
        // что он обязательно должен быть ICloneable
        var cloned = arr[mid] == null ? null : (T) arr[mid].Clone();
        IsdNode<T> node = new IsdNode<T>(cloned);
        Count++;

        node.Left = BuildTreeFromArray(arr, start, mid - 1);
        if (mid + 1 < arr.Length) node.Right = BuildTreeFromArray(arr, mid + 1, end);

        return node;
    }

    /// <summary>
    /// Получить все элементы дерева
    /// </summary>
    /// <returns>Все элементы дерева</returns>
    public IEnumerable<T> GetAll()
    {
        return GetAll(_root);
    }

    /// <summary>
    /// Обертка над методом поиска элементов по предикату
    /// </summary>
    /// <param name="item">Объект для поиска</param>
    /// <returns>Найденные элементы</returns>
    public IEnumerable<T> FindElements(T item)
    {
        return FindElements(_root, item);
    }

    /// <summary>
    /// Поиск элементов по предикату
    /// </summary>
    /// <param name="node">Текущий узел</param>
    /// <param name="item">Объект для поиска</param>
    /// <returns>Найденные элементы</returns>
    private IEnumerable<T> FindElements(IsdNode<T> node, T item)
    {
        if (node == null) yield break;
        
        //проверяем текущий элемент, если подходит, возвращаем
        if (node.Value.Equals(item))
        {
            yield return node.Value; //используем функцию генератор,
                                     //так как нам нужна возможность возвращать множество значений из метода
        }

        //идем по левому поддереву
        if (node.Left != null)
        {
            foreach (var val in FindElements(node.Left, item))
            {
                yield return val;
            }
        }
        
        //идем по правому поддереву.
        if (node.Right != null)
        {
            foreach (var val in FindElements(node.Right, item))
            {
                yield return val;
            }
        }
    }

    /// <summary>
    /// Получить все элементы дерева
    /// </summary>
    /// <param name="node">Текущая нода</param>
    /// <returns>Все элементы дерева</returns>
    private IEnumerable<T> GetAll(IsdNode<T> node)
    {
        if (node == null) yield break;
        
        yield return node.Value; //используем функцию генератор,
        //так как нам нужна возможность возвращать множество значений из метода

        //идем по левому поддереву
        if (node.Left != null)
        {
            foreach (var val in GetAll(node.Left))
            {
                yield return val;
            }
        }
        
        //идем по правому поддереву.
        if (node.Right != null)
        {
            foreach (var val in GetAll(node.Right))
            {
                yield return val;
            }
        }
    }
}