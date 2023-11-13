using System;

public class HeapNode : IComparable<HeapNode> {

    public HeapNode(int priority, int value) {
        this.priority = priority;
        this.value = value;
    }

    public readonly int priority;
    public readonly int value;

    public int CompareTo(HeapNode other) {
        return priority - other.priority;
    }

	public override string ToString() {
		return String.Format("{0} {1}", priority, value);
	}
}