using UnityEngine;


abstract class Wrapper
{
    public GameObject gameObject;

    public Wrapper(GameObject wrapperGameObject)
    {
        gameObject = wrapperGameObject;
        RecalculateVariables();
    }

    public virtual void SwitchTo(GameObject newWrapperGameObject)
    {
        gameObject = newWrapperGameObject;
        RecalculateVariables();
    }

    public abstract void RecalculateVariables();
}


class RoomWrapper : Wrapper
{
    public float Width { get; private set; }
    public float StartX { get; private set; }
    public float EndX { get; private set; }

    public RoomWrapper(GameObject roomWrapperGameObject)
        : base(roomWrapperGameObject)
    { }

    public override void RecalculateVariables()
    {
        Transform transform = gameObject.transform;
        Width = transform.FindChild("Floor").localScale.x;
        StartX = transform.position.x - (Width * 0.5f);
        EndX = StartX + Width;
    }
}


class ObjectWrapper : Wrapper
{
    public float PositionX { get; private set; }

    public ObjectWrapper(GameObject objectWrapperGameObject)
        : base(objectWrapperGameObject)
    { }

    public override void RecalculateVariables()
    {
        PositionX = gameObject.transform.position.x;
    }
}
