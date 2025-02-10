using UnityEngine;

namespace Game.Code.DragAndDrop
{
    public class SingleContainer : Container
    {
        [SerializeField] private bool canReplace = true;
        [SerializeField] private Draggable contained = null;    

        public override Draggable AddItem(Draggable draggable)
        {
            if (draggable == contained) return null;
            if (!isOpen) return draggable;
            if (!canReplace && contained != null) return draggable;

            Draggable oldContained = contained;

            draggable.transform.SetParent(transform);
            draggable.transform.localPosition = Vector3.zero;
            contained = draggable;

            return oldContained;
        }

        public override void Clear()
        {
            if (contained != null) Destroy(contained);
            contained = null;
        }

        public override bool IsEmpty()
        {
            return contained == null;
        }

        public override Draggable RemoveItem()
        {
            if (contained == null) return null;

            Draggable toReturn = contained;
            contained.SetPreviousContainer(this);

            contained = null;
            return toReturn;
        }
    }
}
