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
        protected Vector3 fixedScale;
        protected Vector3 fixedPosition;
        protected Vector3 fixedRotation;

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
            }else if (gameObject.tag == "Player")
            {
                state = ItemState.OnHand;

                meshRenderer = GetComponent<MeshRenderer>();
                meshFilter = GetComponent<MeshFilter>();
                meshRenderer.material = material;
                meshFilter.mesh = mesh;
            }
        }

        public void Destroythis()
        {
            meshRenderer.material = null;
            meshFilter.mesh = null;
            Destroy(this);
        }
    }
}
