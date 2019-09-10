using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Assets.Scripts.Inventory_System.Items;
using Cinemachine;

namespace Assets.Scripts
{
    public enum SquareStates
    {
        NONE,
        PEEK,
        TRAVERSABLE,
        INTRAVERSABLE,
        SELECTED,
        INTRAVERSABLE_HIGHLIGHT
    }

    public enum SquareColorStates
    {
        NONE,
        PEEK,
        TRAVERSABLE,
        INTRAVERSABLE,
        SELECTED,
        INTRAVERSABLE_HIGHLIGHT,
        BOSS_ATTACK_ZONE
    }

    [Serializable]
    public struct ColorConfig
    {
        public SquareStates SquareState;
        public SquareColorStates ColorState;
        public Color Color;
    }

    public class Square : MonoBehaviour
    {
        [SerializeField]
        private List<ColorConfig> colorConfigs = new List<ColorConfig>()
        {
            new ColorConfig(){SquareState = SquareStates.NONE, ColorState = SquareColorStates.NONE, Color = Color.clear},
            new ColorConfig(){SquareState = SquareStates.PEEK, ColorState = SquareColorStates.PEEK, Color = Color.cyan},
            new ColorConfig(){SquareState = SquareStates.TRAVERSABLE, ColorState = SquareColorStates.TRAVERSABLE, Color = Color.yellow},
            new ColorConfig(){SquareState = SquareStates.SELECTED, ColorState = SquareColorStates.SELECTED, Color = Color.green},
            new ColorConfig(){SquareState = SquareStates.INTRAVERSABLE, ColorState = SquareColorStates.INTRAVERSABLE, Color = new Color(0.8f, 0, 0)},
            new ColorConfig(){SquareState = SquareStates.INTRAVERSABLE_HIGHLIGHT, ColorState = SquareColorStates.INTRAVERSABLE_HIGHLIGHT, Color = Color.red},
            new ColorConfig(){SquareState = SquareStates.NONE, ColorState = SquareColorStates.BOSS_ATTACK_ZONE, Color = new Color(0.5f, 0.0f, 0.0f)},
        };
        private Dictionary<SquareColorStates, ColorConfig> squareMap;
        private SquareStates m_currentState;
        public SquareStates State => m_currentState;






        public List<Square> neighbors = new List<Square>();

        [SerializeField] protected MeshRenderer meshRenderer;
        [SerializeField] protected GameObject loot, danger;
        protected LineRenderer lineRenderer;
        protected int steps = int.MaxValue;
        private void Awake()
        {
            // Map dictionary
            squareMap = new Dictionary<SquareColorStates, ColorConfig>();
            foreach (var config in colorConfigs)
            {
                squareMap[config.ColorState] = config;
            }


            lineRenderer = GetComponent<LineRenderer>();
            meshRenderer.material.color = Color.clear;

            this.m_currentState = SquareStates.NONE;
        }

        private void Start()
        {
            if (GridSystem.Instance.theMap.ContainsKey(transform.localPosition + (Vector3.forward * 10f)))
                neighbors.Add(GridSystem.Instance.theMap[transform.localPosition + (Vector3.forward * 10f)]);
            if (GridSystem.Instance.theMap.ContainsKey(transform.localPosition + (Vector3.back * 10f)))
                neighbors.Add(GridSystem.Instance.theMap[transform.localPosition + (Vector3.back * 10f)]);
            if (GridSystem.Instance.theMap.ContainsKey(transform.localPosition + (Vector3.left * 10f)))
                neighbors.Add(GridSystem.Instance.theMap[transform.localPosition + (Vector3.left * 10f)]);
            if (GridSystem.Instance.theMap.ContainsKey(transform.localPosition + (Vector3.right * 10f)))
                neighbors.Add(GridSystem.Instance.theMap[transform.localPosition + (Vector3.right * 10f)]);
        }

        private void OnMouseEnter()
        {
            if (this.m_currentState == SquareStates.NONE)
            {
                GridSystem.Instance.hoveringSquare = this;
                meshRenderer.material.color = squareMap[SquareColorStates.PEEK].Color;
                this.m_currentState = SquareStates.PEEK;
            }
            else if (this.m_currentState == SquareStates.TRAVERSABLE)
            {
                GridSystem.Instance.hoveringSquare = this;
                meshRenderer.material.color = squareMap[SquareColorStates.SELECTED].Color;
                this.m_currentState = SquareStates.SELECTED;
            }
            else if (this.m_currentState == SquareStates.INTRAVERSABLE)
            {
                GridSystem.Instance.hoveringSquare = this;
                meshRenderer.material.color = squareMap[SquareColorStates.INTRAVERSABLE_HIGHLIGHT].Color;
                this.m_currentState = SquareStates.INTRAVERSABLE_HIGHLIGHT;
            }
        }

        private void OnMouseOver()
        {
            if (Input.GetKeyDown("f"))
            {
                Instantiate(danger, transform.position, Quaternion.identity);
            }
            else if (Input.GetKeyDown("g"))
            {
                Instantiate(loot, transform.position, Quaternion.identity);
            }
        }

        private void OnMouseExit()
        {

            if (this.m_currentState == SquareStates.PEEK)
            {
                meshRenderer.material.color = squareMap[SquareColorStates.NONE].Color;
                this.m_currentState = SquareStates.NONE;
            }
            else if (this.m_currentState == SquareStates.SELECTED)
            {
                meshRenderer.material.color = squareMap[SquareColorStates.TRAVERSABLE].Color;
                this.m_currentState = SquareStates.TRAVERSABLE;
            }
            else if (this.m_currentState == SquareStates.INTRAVERSABLE_HIGHLIGHT)
            {
                meshRenderer.material.color = squareMap[SquareColorStates.INTRAVERSABLE].Color;
                this.m_currentState = SquareStates.INTRAVERSABLE;
            }
        }

        public void Range(int range, int isteps = 0)
        {
            if (this.m_currentState == SquareStates.INTRAVERSABLE || this.m_currentState == SquareStates.INTRAVERSABLE_HIGHLIGHT)
                return;
            steps = isteps;
            meshRenderer.material.color = this.squareMap[SquareColorStates.TRAVERSABLE].Color;
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
            return this.m_currentState == SquareStates.SELECTED;
        }

        public int ActionPointCost()
        {
            return steps;
        }

        public void Clear()
        {
            if (this.m_currentState == SquareStates.INTRAVERSABLE || this.m_currentState == SquareStates.INTRAVERSABLE_HIGHLIGHT)
                return;

            steps = int.MaxValue;

            this.m_currentState = SquareStates.NONE;
            meshRenderer.material.color = this.squareMap[SquareColorStates.NONE].Color;

            foreach (Square square in neighbors)
                if (square.State == SquareStates.TRAVERSABLE || square.State == SquareStates.SELECTED || square.State == SquareStates.INTRAVERSABLE)
                    square.Clear();
        }

        public void SetNotTraversable()
        {
            this.m_currentState = SquareStates.INTRAVERSABLE;
            meshRenderer.material.color = squareMap[SquareColorStates.INTRAVERSABLE].Color;
        }

        public void SetTraversable()
        {
            this.m_currentState = SquareStates.NONE;
            meshRenderer.material.color = squareMap[SquareColorStates.NONE].Color;
        }

        public bool IsTraversable()
        {
            return (this.m_currentState != SquareStates.INTRAVERSABLE &&
                    this.m_currentState != SquareStates.INTRAVERSABLE_HIGHLIGHT);
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

        public Square getNearestAttackableZone()
        {
            foreach (Square neighbor in neighbors)
            {
                if (!neighbor.IsTraversable())
                    return neighbor;
            }
            return null;
        }


        public void SetBossAttackZone()
        {
            meshRenderer.material.color =  squareMap[SquareColorStates.BOSS_ATTACK_ZONE].Color;
        }
        public  void ResetColorToState()
        {
            foreach (KeyValuePair<SquareColorStates, ColorConfig> pair in squareMap)
            {
                if (this.m_currentState == pair.Value.SquareState)
                {
                    meshRenderer.material.color = squareMap[pair.Value.ColorState].Color;
                    return;
                }
            }
        }
    }


}
