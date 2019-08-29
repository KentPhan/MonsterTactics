using UnityEngine;
using Assets.Scripts.Constants;

namespace Assets.Scripts.Inventory_System.Items
{
    public abstract class AbstractItem : MonoBehaviour
    {
        public enum ItemState {OnField, OnHand, OnInventory}
        ItemState state;

        protected MeshRenderer meshRenderer;
        protected MeshFilter meshFilter;

        protected Mesh mesh;
        protected Material material;

        // On filed, On hand, On inventory
        protected Vector3[] fixedScale = new Vector3[3];
        protected Vector3[] fixedPosition = new Vector3[3];
        protected Vector3[] fixedRotation = new Vector3[3];

        private void Start()
        {
            // If this script attached to ItemSlot game object
            if ( (1<<gameObject.layer) == LayerMask.GetMask(Layers.GRID))
            {
                state = ItemState.OnField;

                meshRenderer = GetComponent<MeshRenderer>();
                meshFilter = GetComponent<MeshFilter>();
                meshRenderer.material = material;
                meshFilter.mesh = mesh;
            }
            else if (gameObject.tag == "Player")
            {
                state = ItemState.OnHand;

                meshRenderer = GetComponent<MeshRenderer>();
                meshFilter = GetComponent<MeshFilter>();
                meshRenderer.material = material;
                meshFilter.mesh = mesh;
            }

            if (state == ItemState.OnField)
            {
                transform.localPosition = fixedPosition[0];
                transform.localEulerAngles = fixedRotation[0];
                transform.localScale = fixedScale[0];
            }
            else if (state == ItemState.OnHand)
            {
                transform.localPosition = fixedPosition[1];
                transform.localEulerAngles = fixedRotation[1];
                transform.localScale = fixedScale[1];
            }
            else if (state == ItemState.OnInventory)
            {
                transform.localPosition = fixedPosition[2];
                transform.localEulerAngles = fixedRotation[2];
                transform.localScale = fixedScale[2];
            }
        }

        private void Update()
        {
            if(state == ItemState.OnField)
                transform.Rotate(Vector3.up * Time.deltaTime * 100, Space.World);
        }

        public void Destroythis()
        {
            meshRenderer.material = null;
            meshFilter.mesh = null;
            Destroy(this);
        }
    }
}
