using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Inventory_System.Items;

namespace Assets.Scripts
{
    public class Square : MonoBehaviour
    {
        public enum OccupationState
        {
            NONE,
            BOSS
        }

        [SerializeField]
        protected Color none = Color.clear,
            peek = Color.cyan,
            traversible = Color.yellow,
            select = Color.green,
            traversing = Color.blue,
            intraversable = new Color(0.8f, 0, 0),
            intraversableHighlight = Color.red;

        public List<Square> neighbors = new List<Square>();

        [SerializeField] protected MeshRenderer meshRenderer;
        [SerializeField] protected GameObject loot, danger;
        protected LineRenderer lineRenderer;
        protected int steps = int.MaxValue;

        private OccupationState occupationState;

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
            meshRenderer.material.color = Color.clear;
            this.occupationState = OccupationState.NONE;
        }

        private void Start()
        {
            if (GridSystem.Instance.theMap.ContainsKey(transform.localPosition + Vector3.forward))
                neighbors.Add(GridSystem.Instance.theMap[transform.localPosition + Vector3.forward]);
            if (GridSystem.Instance.theMap.ContainsKey(transform.localPosition + Vector3.back))
                neighbors.Add(GridSystem.Instance.theMap[transform.localPosition + Vector3.back]);
            if (GridSystem.Instance.theMap.ContainsKey(transform.localPosition + Vector3.left))
                neighbors.Add(GridSystem.Instance.theMap[transform.localPosition + Vector3.left]);
            if (GridSystem.Instance.theMap.ContainsKey(transform.localPosition + Vector3.right))
                neighbors.Add(GridSystem.Instance.theMap[transform.localPosition + Vector3.right]);
        }

        private void OnMouseEnter()
        {
            if (meshRenderer.material.color == none)
            {
                GridSystem.Instance.hoveringSquare = this;
                meshRenderer.material.color = peek;
            }
            else if (meshRenderer.material.color == traversible)
            {
                GridSystem.Instance.hoveringSquare = this;
                meshRenderer.material.color = select;
            }
            else if (meshRenderer.material.color == intraversable)
            {
                GridSystem.Instance.hoveringSquare = this;
                meshRenderer.material.color = intraversableHighlight;
            }
        }

        private void OnMouseOver()
        {
            if (Input.GetKeyDown("d"))
            {
                Instantiate(danger, transform.position, Quaternion.identity);
            }
            else if (Input.GetKeyDown("l"))
            {
                Instantiate(loot, transform.position, Quaternion.identity);
            }
        }

        private void OnMouseExit()
        {
            if (meshRenderer.material.color == peek)
                meshRenderer.material.color = none;
            else if (meshRenderer.material.color == select)
                meshRenderer.material.color = traversible;
            else if (meshRenderer.material.color == intraversableHighlight)
                meshRenderer.material.color = intraversable;
        }

        public void Range(int range, int isteps = 0)
        {
            if (meshRenderer.material.color == intraversable || meshRenderer.material.color == intraversableHighlight)
                return;
            steps = isteps;
            meshRenderer.material.color = traversible;
            if (range > 0)
            {
                foreach (Square square in neighbors)
                {
                    if ((steps + 1) < (square.steps))
                        square.Range(range - 1, steps + 1);
                }
            }
        }

        public bool IsInRange()
        {
            return meshRenderer.material.color == select;
        }

        public int ActionPointCost()
        {
            return steps;
        }

        public void Clear()
        {
            if (meshRenderer.material.color == intraversable || meshRenderer.material.color == intraversableHighlight)
                return;

            steps = int.MaxValue;
            meshRenderer.material.color = none;
            foreach (Square square in neighbors)
                if (square.meshRenderer.material.color == traversible || square.meshRenderer.material.color == select || square.meshRenderer.material.color == intraversable)
                    square.Clear();
        }

        public void SetNotTraversable()
        {
            meshRenderer.material.color = intraversable;
        }

        public void SetTraversable()
        {
            meshRenderer.material.color = none;
        }

        public bool IsTraversable()
        {
            return (meshRenderer.material.color != intraversable &&
                    meshRenderer.material.color != intraversableHighlight);
        }

        public bool hasItemOnThis()
        {
            Transform itemslotforgrid = gameObject.transform.GetChild(1);
            if (itemslotforgrid.GetComponent<AbstractItem>() != null)
            {
                return true;
            }
            return false;
        }
    }


}
