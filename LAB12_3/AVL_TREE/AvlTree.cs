using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace LAB12_3.AVL_TREE;

// тип бинарного дерева
// использует шаблон типа T, который должен имплементировать интерфейс IComparable, так как для
//бинарного дерева важно сравнение объекта типа > или <, а не только ==
[ExcludeFromCodeCoverage]
public class AvlTree<T> where T : IComparable, ICloneable
{
    public int Count { get; protected set; }
    protected AvlTreeNode<T> _root; //корень бинарного дерева

    /**
     * <summary>Getter высоты узла</summary>
     * @param node - узел
     * @return возвращает число типа int - высота. Высота одиночного узла = 1
     */
    int GetHeight(AvlTreeNode<T> node)
    {
        if (node == null) return 0;
        return node.Height;
    }

    /**
     * <summary>Фактор балансировки, нужен для того, чтобы определить какой из 4 способов вращения применить для
     * балансировки.
     * </summary>
     * @param node - узел
     * @return фактор балансировки (может быть как положительным так и отрицательным)
     */
    int BalanceFactor(AvlTreeNode<T> node)
    {
        //if (node == null) return 0;
        return GetHeight(node.Left) - GetHeight(node.Right);
    }

    /**
     * <summary>Расчет высоты текущего узла. 1 +, потому что высота листа в моей версии дерева = 1</summary>
     * @param node - узел
     */
    void UpdateHeight(AvlTreeNode<T> node)
    {
        node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
    }

    /**
     * <summary>Правое вращение в алгоритме AVL</summary>
     * @param y - узел
     * @return возвращает узел, который нужно поставить на место текущего узла
     */
    AvlTreeNode<T> RightRotation(AvlTreeNode<T> y)
    {
        AvlTreeNode<T> x = y.Left;
        AvlTreeNode<T> T2 = x.Right;

        x.Right = y;
        y.Left = T2;

        UpdateHeight(y);
        UpdateHeight(x);

        return x;
    }

    /**
 * <summary>Левое вращение в алгоритме AVL</summary>
 * @param x - узел
 * @return возвращает узел, который нужно поставить на место текущего узла
 */
    AvlTreeNode<T> LeftRotation(AvlTreeNode<T> x)
    {
        AvlTreeNode<T> y = x.Right;
        AvlTreeNode<T> T2 = y.Left;

        y.Left = x;
        x.Right = T2;

        UpdateHeight(x);
        UpdateHeight(y);

        return y;
    }

    /**
     * <summary>Найти по значению</summary>
     * @param node - текущий узел (изначально сюда передается корень)
     * @param val - значение для поиска
     * @return значение искомой ноды
     */
    T FindByValue(AvlTreeNode<T> node, T val)
    {
        if (node == null)
        {
            return default;
        }

        //сравниваем
        int comparisonResult = val.CompareTo(node.Value);

        //совпадение найдено
        if (comparisonResult == 0)
        {
            return node.Value;
        }
        else if (comparisonResult < 0)
        {
            //меньше влево
            return FindByValue(node.Left, val);
        }
        else
        {
            //больше вправо
            return FindByValue(node.Right, val);
        }
    }

    /**
     * <summary>
     * Метод добавления узла в бинарное дерево и последующей балансировкой
     * Это обеспечивает, что глубина дерева остается логарифмической относительно количества узлов,
     * что позволяет операциям поиска, вставки и удаления выполняться за O(log n)
     * Вычисление балансировочного фактора:
     * Балансировочный фактор каждого узла вычисляется как разность высот левого и правого поддеревьев.
     * Если этот фактор больше 1 или меньше -1, дерево требует балансировки.
        Применение ротаций для балансировки:

        Left Left (LL): Происходит, когда добавление узла произошло в левое поддерево левого дочернего узла.
        Производится правый поворот.
        Right Right (RR): Происходит, когда добавление узла произошло в правое поддерево правого дочернего узла.
        Производится левый поворот.
        Left Right (LR): Происходит, когда добавление узла произошло в правое поддерево левого дочернего узла.
        Сначала делается левый поворот левого дочернего, а затем правый поворот текущего узла.
        Right Left (RL): Происходит, когда добавление узла произошло в левое поддерево правого дочернего узла.
        Сначала делается правый поворот правого дочер
     * </summary>
     * @param node - текущий узел (изначально сюда передается корень дерева)
     * @param val - значение для добавления
     * @return возвращает ноду для замены.
     */
    AvlTreeNode<T> AddNode(AvlTreeNode<T> node, T val, ref bool added)
    {
        if (node == null)
        {
            Count++;
            added = true;
            return new AvlTreeNode<T>(val, null, null, 1);
        }

        int comparisonResult = val.CompareTo(node.Value);

        if (comparisonResult < 0)
            node.Left = AddNode(node.Left, val, ref added); // нужно добавить влево
        else if (comparisonResult > 0)
            node.Right = AddNode(node.Right, val, ref added); //нужно добавить вправо
        else
        {
            added = false;
            return node; //дубликаты запрещены
        }

        UpdateHeight(node); //перерасчет высоты текущей ветви

        int balance = BalanceFactor(node); //расчет балансирующего фактора


        //непосредственно сами вращения
        // Right right
        if (balance > 1 && val.CompareTo(node.Left.Value) < 0)
            return RightRotation(node);

        // Left left
        if (balance < -1 && val.CompareTo(node.Right.Value) > 0)
            return LeftRotation(node);

        // Left Right
        if (balance > 1 && val.CompareTo(node.Left.Value) > 0)
        {
            node.Left = LeftRotation(node.Left);
            return RightRotation(node);
        }

        // Right Left
        if (balance < -1 && val.CompareTo(node.Right.Value) < 0)
        {
            node.Right = RightRotation(node.Right);
            return LeftRotation(node);
        }

        return node;
    }


    /**
     * <summary>Получить минимальный элемент в поддереве. В бинарном дереве поиска это делается очень удобно
     * Нужно лишь постоянно выбирать левый дочерний элемент
     * </summary>
     * @param node - нода начала поддерева для поиска
     * @return минимальный узел
     */
    AvlTreeNode<T> GetMin(AvlTreeNode<T> node)
    {
        AvlTreeNode<T> current = node;
        while (current.Left != null)
            current = current.Left;
        return current;
    }

    /**
     * <summary>Удаление узла из бинарного дерева
     * Балансировочный фактор каждого узла вычисляется как разность высот левого и правого поддеревьев.
     * Если этот фактор больше 1 или меньше -1, дерево требует балансировки.
        Применение ротаций для балансировки:

        Left Left (LL): Происходит, когда добавление узла произошло в левое поддерево левого дочернего узла.
        Производится правый поворот.
        Right Right (RR): Происходит, когда добавление узла произошло в правое поддерево правого дочернего узла.
        Производится левый поворот.
        Left Right (LR): Происходит, когда добавление узла произошло в правое поддерево левого дочернего узла.
        Сначала делается левый поворот левого дочернего, а затем правый поворот текущего узла.
        Right Left (RL): Происходит, когда добавление узла произошло в левое поддерево правого дочернего узла.
        Сначала делается правый поворот правого дочер
     * </summary>
     * @param node - нода начала поддерева для поиска
     * @param removeFlag - true, если элемент был найден и удален
     * @return минимальный узел
     */
    AvlTreeNode<T> RemoveNode(AvlTreeNode<T> node, T val, ref bool removeFlag)
    {
        if (node == null) return node;

        //сравниваем узел и значение
        int comparisonResult = val.CompareTo(node.Value);

        //если узел меньше, то рекурсивно вызываем метод remove для левого узла
        if (comparisonResult < 0)
            node.Left = RemoveNode(node.Left, val, ref removeFlag);
        else if (comparisonResult > 0)
            //если узел больше, то рекурсивно вызываем метод remove для правого узла
            node.Right = RemoveNode(node.Right, val, ref removeFlag);
        else
        {
            removeFlag = true;
            //если у искомого узла один потомок, то нужно текущий узел заменить этим потомком
            if (node.Left == null || node.Right == null)
            {
                if (node.Left == null && node.Right != null)
                {
                    node = node.Right;
                } else if (node.Left != null && node.Right == null)
                {
                    node = node.Left;
                }
                else
                {
                    node = null;
                }
            }
            else
            {
                //если же у искомого узла два потомка-поддерева, тогда заменяем искомый узел на минимальный элемент в
                //правом поддереве, а старый минимальный правый элемент удаляем
                AvlTreeNode<T> minRightBranchNode = GetMin(node.Right);
                node.Value = minRightBranchNode.Value;
                node.Right = RemoveNode(node.Right, minRightBranchNode.Value, ref removeFlag);
            }
        }

        if (node == null) return node;

        //обновляем высоту
        UpdateHeight(node);

        int balance = BalanceFactor(node);

        //балансировка бинарного дерева
        // right rotation
        if (balance > 1 && BalanceFactor(node.Left) >= 0)
            return RightRotation(node);

        // Left Right вращение
        if (balance > 1 && BalanceFactor(node.Left) < 0)
        {
            node.Left = LeftRotation(node.Left);
            return RightRotation(node);
        }

        // Left rotation
        if (balance < -1 && BalanceFactor(node.Right) <= 0)
            return LeftRotation(node);

        // Right Left вращение
        if (balance < -1 && BalanceFactor(node.Right) > 0)
        {
            node.Right = RightRotation(node.Right);
            return LeftRotation(node);
        }

        return node;
    }

    /**
     * <summary>Вывести узлы</summary>
     * @param node - корень поддерева
     */
    void PrintTree(AvlTreeNode<T> node, int spaces = 5)
    {
        if (node == null) return;
        PrintTree(node.Left, spaces + 5); //Запускаем печать правой ветки
        for (int i = 0; i < spaces; i++)
        {
            Console.Write(" "); //Выводим нужное количество пробелов, чтобы дерево вывелось в нужном порядке
        }
        Console.WriteLine(node.Value == null ? "" : node.Value.ToString()); //Вывод саму информацию в точке
        PrintTree(node.Right, spaces + 5); //Запускаем печать левой ветки
    }

    /**
     * <summary>Вставить узел в дерево</summary>
     * @param val значение
     */
    public bool Insert(T val)
    {
        bool added = false;
        _root = AddNode(_root, (T)val.Clone(), ref added);
        return added;
    }

    /**
     * <summary>Удалить узел по значению</summary>
     * @param value - значение
     */
    public virtual bool Remove(T value)
    {
        bool removed = false;
        _root = RemoveNode(_root, value, ref removed);
        if (removed) Count--;
        return removed;
    }

    /**
     * <summary>Вывести узлы на экран</summary>
     */
    public void PrintTree()
    {
        if (_root == null)
        {
            Console.WriteLine("Дерево пустое");
            return;
        }

        PrintTree(_root);
    }
    

    /// <summary>
    /// Очистить дерево
    /// </summary>
    public virtual void Clear()
    {
        _root = null;
        Count = 0;
    }

    /**
     * <summary>Найти по значению</summary>
     * @param value -  значение
     */
    public T FindByValue(T value)
    {
        return FindByValue(_root, value);
    }

    /// <summary>
    /// Получить значения в дереве, сначала итерация всегда идет по левым веткам
    /// </summary>
    /// <returns></returns>
    public IEnumerable<T> GetValues()
    {
        return GetValues(_root);
    }

    private IEnumerable<T> GetValues(AvlTreeNode<T> node)
    {
        if (node == null) yield break;
        yield return node.Value;

        if (node.Left != null)
        {
            foreach (var leftBranch in GetValues(node.Left))
            {
                yield return leftBranch;
            }
        }
        
        if (node.Right != null)
        {
            foreach (var rightBranch in GetValues(node.Right))
            {
                yield return rightBranch;
            }
        }
    }
    
};