using UnityEngine;

namespace Game.Code.DragAndDrop
{
    public abstract class Container : MonoBehaviour
    {
        protected bool isOpen = true;

        public abstract Draggable AddItem(Draggable draggable);
        public abstract Draggable RemoveItem();
        public abstract void Clear();
        public abstract bool IsEmpty();
    }
}
