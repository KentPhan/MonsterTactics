using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Assets.Scripts.Inventory_System.Items;
using Cinemachine;

namespace Assets.Scripts
{
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
        public SquareColorStates ColorState;
        public Color Color;
    }

    public class Square : MonoBehaviour
    {
        [SerializeField]
        private List<ColorConfig> colorConfigs = new List<ColorConfig>()
        {
            new ColorConfig(){ColorState = SquareColorStates.NONE, Color = Color.clear},
            new ColorConfig(){ColorState = SquareColorStates.PEEK, Color = Color.cyan},
            new ColorConfig(){ColorState = SquareColorStates.TRAVERSABLE, Color = Color.yellow},
            new ColorConfig(){ColorState = SquareColorStates.SELECTED, Color = Color.green},
            new ColorConfig(){ColorState = SquareColorStates.INTRAVERSABLE, Color = new Color(0.8f, 0, 0)},
            new ColorConfig(){ColorState = SquareColorStates.INTRAVERSABLE_HIGHLIGHT, Color = Color.red},
            new ColorConfig(){ColorState = SquareColorStates.BOSS_ATTACK_ZONE, Color = new Color(0.5f, 0.0f, 0.0f)},
        };
        private Dictionary<SquareColorStates, ColorConfig> squareMap;

        public bool isShowingRange;
        public bool isShowingBossAttack;
        private bool isIntraversable;




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

            this.isShowingRange = false;
            this.isShowingBossAttack = false;
            this.isIntraversable = false;
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
            if (isIntraversable)
            {
                GridSystem.Instance.hoveringSquare = this;
                meshRenderer.material.color = squareMap[SquareColorStates.INTRAVERSABLE_HIGHLIGHT].Color;
            }
            else if (isShowingRange || isShowingBossAttack)
            {
                GridSystem.Instance.hoveringSquare = this;
                meshRenderer.material.color = squareMap[SquareColorStates.SELECTED].Color;
            }
            else
            {
                GridSystem.Instance.hoveringSquare = this;
                meshRenderer.material.color = squareMap[SquareColorStates.PEEK].Color;
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
            if (isIntraversable)
            {
                meshRenderer.material.color = squareMap[SquareColorStates.INTRAVERSABLE].Color;
            }
            else if (isShowingBossAttack)
            {
                meshRenderer.material.color = squareMap[SquareColorStates.BOSS_ATTACK_ZONE].Color;
            }
            else if( isShowingRange)
            {
                meshRenderer.material.color = squareMap[SquareColorStates.TRAVERSABLE].Color;
            }
            else
            {
                meshRenderer.material.color = squareMap[SquareColorStates.NONE].Color;
            }
        }

        public void Range(int range, int isteps = 0)
        {
            if (this.isIntraversable)
                return;

            steps = isteps;
            meshRenderer.material.color = this.squareMap[SquareColorStates.TRAVERSABLE].Color;
            this.isShowingRange = true;
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
            return this.isShowingRange;
        }

        public int ActionPointCost()
        {
            return steps;
        }

        public void Clear()
        {
            if (this.isIntraversable)
                return;

            steps = int.MaxValue;

            this.isShowingRange = false;
            ResetColor();

            foreach (Square square in neighbors)
                if (square.isShowingRange)
                    square.Clear();
        }

        public void SetNotTraversable()
        {
            this.isIntraversable = true;
            ResetColor();
        }

        public void SetTraversable()
        {
            this.isIntraversable = false;
            ResetColor();
        }

        public bool IsTraversable()
        {
            return !this.isIntraversable;
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
            this.isShowingBossAttack = true;
            meshRenderer.material.color =  squareMap[SquareColorStates.BOSS_ATTACK_ZONE].Color;
        }

        public void UnsetBossAttackZone()
        {
            this.isShowingBossAttack = false;
            ResetColor();
        }


        public  void ResetColor()
        {
            if (isIntraversable)
            {
                meshRenderer.material.color = squareMap[SquareColorStates.INTRAVERSABLE].Color;
            }
            else if (isShowingBossAttack)
            {
                meshRenderer.material.color = squareMap[SquareColorStates.BOSS_ATTACK_ZONE].Color;
            }
            else if (isShowingRange)
            {
                meshRenderer.material.color = squareMap[SquareColorStates.TRAVERSABLE].Color;
            }
            else
            {
                meshRenderer.material.color = squareMap[SquareColorStates.NONE].Color;
            }
        }
    }


}
