using System;

// @author Egor Grishechko https://egorikas.com/max-and-min-heap-implementation-with-csharp/
public class MaxHeap {
    private readonly HeapNode[] _elements;
    private int _size;

    public MaxHeap(int size) {
        _elements = new HeapNode[size];
    }

    private int GetLeftChildIndex(int elementIndex) => 2 * elementIndex + 1;
    private int GetRightChildIndex(int elementIndex) => 2 * elementIndex + 2;
    private int GetParentIndex(int elementIndex) => (elementIndex - 1) / 2;

    private bool HasLeftChild(int elementIndex) => GetLeftChildIndex(elementIndex) < _size;
    private bool HasRightChild(int elementIndex) => GetRightChildIndex(elementIndex) < _size;
    private bool IsRoot(int elementIndex) => elementIndex == 0;

    private HeapNode GetLeftChild(int elementIndex) => _elements[GetLeftChildIndex(elementIndex)];
    private HeapNode GetRightChild(int elementIndex) => _elements[GetRightChildIndex(elementIndex)];
    private HeapNode GetParent(int elementIndex) => _elements[GetParentIndex(elementIndex)];

    private void Swap(int firstIndex, int secondIndex) {
        var temp = _elements[firstIndex];
        _elements[firstIndex] = _elements[secondIndex];
        _elements[secondIndex] = temp;
    }

    public bool IsEmpty() {
        return _size == 0;
    }

    public int Size() {
        return _size;
    }

    public HeapNode Peek() {
        if (_size == 0)
            throw new IndexOutOfRangeException();

        return _elements[0];
    }

    public HeapNode Pop() {
        
        if (_size <= 0) {

            throw new IndexOutOfRangeException("HERE");
		}

        HeapNode result = _elements[0];

        _elements[0] = _elements[_size - 1];
        _size--;

        ReCalculateDown();
        
        return result;
    }

    public void Add(HeapNode element) {
        if (_size == _elements.Length)
            throw new IndexOutOfRangeException();

        _elements[_size] = element;
        _size++;
        ReCalculateUp();
    }

    private void ReCalculateDown() {
        int index = 0;
        while (HasLeftChild(index)) {
            var biggerIndex = GetLeftChildIndex(index);
            if (HasRightChild(index) && GetRightChild(index).priority > GetLeftChild(index).priority) {
                biggerIndex = GetRightChildIndex(index);
            }

            if (_elements[biggerIndex] != null && _elements[index] != null && _elements[biggerIndex].priority < _elements[index].priority) {
                break;
            }

            Swap(biggerIndex, index);
            index = biggerIndex;
        }
    }

    private void ReCalculateUp() {
        var index = _size - 1;
        while (!IsRoot(index) && _elements[index].priority > GetParent(index).priority) {
            int parentIndex = GetParentIndex(index);
            Swap(parentIndex, index);
            index = parentIndex;
        }
    }
}