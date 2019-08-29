using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UIHealthBar : MonoBehaviour
    {
        [SerializeField] private Image UIHealthIndicatorBar;

        private float maxWidth;
        private RectTransform healthIndicatorBarRectTransform;

        // Start is called before the first frame update
        void Start()
        {
            this.healthIndicatorBarRectTransform = UIHealthIndicatorBar.GetComponent<RectTransform>();
            this.maxWidth = this.healthIndicatorBarRectTransform.sizeDelta.x;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void UpdateHealthIndicator(float ratio)
        {
            ratio = Mathf.Clamp(ratio, 0.0f, 1.0f);
            float newWidth = ratio * maxWidth;
            this.healthIndicatorBarRectTransform.sizeDelta = new Vector2(newWidth, this.healthIndicatorBarRectTransform.sizeDelta.y);
        }
    }
}
