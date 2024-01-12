using UnityEngine;

public abstract class ProcessItem : MonoBehaviour
{
    [SerializeField]
    private float _weight = 0.0f;

    public float Weight
    {
        get { return _weight; }
        protected set { _weight = value; }
    }
}
