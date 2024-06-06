using System.Diagnostics.CodeAnalysis;

namespace LAB12_3.AVL_TREE;

[ExcludeFromCodeCoverage]
public class AvlTreeNode<T>
{
    public T Value; //полезная нагрузка ноды
    public AvlTreeNode<T> Left; // ссылка на элемент который меньше текущего
    public AvlTreeNode<T> Right; // элемент который больше текущего
    public int Height; // высота текущего узла, нужна для балансировки бинарного дерева

    public AvlTreeNode(T value, AvlTreeNode<T> left, AvlTreeNode<T> right, int height)
    {
        this.Value = value;
        this.Left = left;
        this.Right = right;
        this.Height = height;
    }
}