using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GrowTopia.UI
{
    public class ShortcutBarGrid : MonoBehaviour
    {
        [Tooltip("The item sprite used when the grid is empty.")]
        [SerializeField]
        private Sprite _emptySprite;

        [SerializeField]
        private Image _image;

        [SerializeField]
        private TextMeshProUGUI _countText;

        private Sprite _sprite;
        private int _count;

        public Sprite Sprite
        {
            get => _sprite;
            set
            {
                _sprite = value;
                _image.sprite = value;
            }
        }

        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                // if count = 1, we don't show the count text
                if (value == 1)
                    _countText.text = "";
                else
                    _countText.text = value.ToString();
            }
        }

        /// <summary>
        /// Set this grid to shown as empty grid.
        /// </summary>
        public void SetEmpty()
        {
            Count = 1;
            Sprite = _emptySprite;
        }

        void Start()
        {
            _countText.text = "";
        }
    }
}