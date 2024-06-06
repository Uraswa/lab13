using System.Diagnostics.CodeAnalysis;

namespace LAB12_3.ISD;

[ExcludeFromCodeCoverage]
class IsdNode<T> where T : class
{
    public T Value;
    public IsdNode<T> Left;
    public IsdNode<T> Right;
    
    public IsdNode(T value)
    {
        Value = value;
        Left = null;
        Right = null;
    }
}