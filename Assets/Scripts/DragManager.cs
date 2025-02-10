using Unity.VisualScripting;
using UnityEngine;

namespace Game.Code.DragAndDrop
{
    public class DragManager : MonoBehaviour
    {
        [Tooltip("Settings")]
        [SerializeField] private MouseButton dragButton = MouseButton.Left;
        [SerializeField] private bool canDropAnywhere = false;

        // private
        private const float distanceFromCamera = 5f;
        private Camera cam;
        private Draggable draggable = null;
        RaycastHit hit;

        // static 
        private static DragManager instance;
        public static DragManager Instance => instance;

        private Vector3 GetMousePosition
        {
            get
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = distanceFromCamera;
                return mousePos;
            }
        }

        private void Start()
        {
            Init();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown((int)dragButton))
                StartDrag();
            if (Input.GetMouseButtonUp((int)dragButton) && draggable != null)
                StopDrag();
            if (draggable != null)
                Drag();

        }

        private void StartDrag()
        {
            Debug.Log("Start dragging");

            if (Physics.Raycast(cam.ScreenPointToRay(GetMousePosition), out RaycastHit hit))
            {
                draggable = hit.collider.gameObject.GetComponent<Draggable>();
                if (HasDrag()) return;

                Container container = hit.collider.GetComponentInChildren<Container>();
                draggable = container?.RemoveItem();
                HasDrag();
            }
        }

        private void Drag()
        {
            if (Physics.Raycast(cam.ScreenPointToRay(GetMousePosition), out RaycastHit hit))
                draggable.transform.position = hit.point;
            else
                draggable.transform.position = cam.ScreenToWorldPoint(GetMousePosition);
        }

        private void StopDrag()
        {
            Debug.Log("Stop dragging");

            Container container = null;

            if (Physics.Raycast(cam.ScreenPointToRay(GetMousePosition), out RaycastHit hit))
            {
                container = hit.collider.gameObject.GetComponentInChildren<Container>();

                if (container != null)
                {
                    Draggable removedDraggable = container.AddItem(draggable);
                    removedDraggable?.ReturnToStartPosition();
                }
            }

            draggable.Drop(canDropAnywhere, container != null);
            draggable = null;
        }

        private bool HasDrag()
        {
            if (draggable != null)
            {
                draggable?.Grab();
                return true;
            }
            return false;
        }

        private void Init()
        {
            cam = Camera.main;

            if (instance == null) instance = this;
            else Destroy(this);
        }
    }
}