using System;
using UnityEngine;

public class Square : MonoBehaviour
{
    public event Action<int> CoordDelegate;
    private int coord;

	public void Init(int coord)
	{
		this.coord = coord;
	}

	private void OnMouseUpAsButton()
    {
		CoordDelegate?.Invoke(coord);
	}
}