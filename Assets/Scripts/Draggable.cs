using UnityEngine;
using static UnityEditorInternal.ReorderableList;

namespace Game.Code.DragAndDrop
{
    public class Draggable : MonoBehaviour
    {
        private Container previousContainer = null;
        private Transform defaultParent;
        private Vector3 defaultPosition;
        private const int ignoreRaycastLayer = 2;
        private int defaultLayer;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            defaultLayer = gameObject.layer;
            defaultParent = transform.parent;
            defaultPosition = transform.localPosition;
        }

        public void Grab()
        {
            gameObject.layer = ignoreRaycastLayer;
        }

        public void Drop(bool canDropAnywhere, bool inContainer)
        {
            if (!canDropAnywhere && !inContainer)
                ReturnToContainer();
            else if (canDropAnywhere)
            {
                gameObject.layer = defaultLayer;
                previousContainer = null;
            }
        }

        public void ReturnToContainer()
        {
            if (previousContainer != null)
                previousContainer.AddItem(this);
            else
                ReturnToStartPosition();
        }

        public void ReturnToStartPosition()
        {
            transform.SetParent(defaultParent);
            transform.localPosition = defaultPosition;
            gameObject.layer = defaultLayer;
        }

        public void SetPreviousContainer(Container container)
        {
            previousContainer = container;
        }
    }
}