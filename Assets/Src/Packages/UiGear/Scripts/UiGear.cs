using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

namespace YsoCorp {

    public class UiGear : MonoBehaviour {

        public class OnChange : UnityEvent<int> {}

        static public float SPEED = 0.1f;

        private Image _iLever;
        private Image _iKms;
        private Text _tKms;
        private Button _b0;
        private GameObject _gPositions;

        public OnChange onChange { get; set; } = new OnChange();

        void Awake() {
            //_gPositions = gameObject.GetChildGameObjectByName("GPositions");
            //_iLever = gameObject.GetChildGameObjectByName("ILever").GetComponent<Image>();
            //_iKms = gameObject.GetChildGameObjectByName("IKmh").GetComponent<Image>();
            //_iKms.gameObject.SetActive(false);
            //_tKms = _iKms.gameObject.GetChildGameObjectByName("TKmh").GetComponent<Text>();
            //_b0 = gameObject.GetChildGameObjectByNameRec("0").GetComponent<Button>();
            foreach (Button b in this.GetComponentsInChildren<Button>()) {
                b.onClick.AddListener(() => { this.OnClick(b); });
            }
        }

        public void SetLeverPosition(int i) {
            if (i > 0 && i <= 6) {
                this.OnClick(this._gPositions.GetComponentsInChildren<Button>()[i]);
            } 
        }

        void OnClick(Button b) {
            Sequence s = DOTween.Sequence();
            if (this._iLever.transform.position.x != b.transform.position.x) {
                s.Append(this._iLever.transform.DOMoveY(this._b0.transform.position.y, SPEED));
                s.Append(this._iLever.transform.DOMoveX(b.transform.position.x, SPEED));
            }
            s.Append(this._iLever.transform.DOMoveY(b.transform.position.y, SPEED));
            s.Play();
            this.onChange.Invoke(int.Parse(b.name));
        }

        public void Reset() {
            this._iLever.transform.position = this._b0.transform.position;
        }

        public void SetKmh(int kmh) {
            this._iKms.gameObject.SetActive(true);
            this._tKms.text = "" + kmh;
        }

        public GameObject GetChildGameObjectByName(GameObject parent, string name)
        {
            Transform[] children = parent.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in children)
            {
                if (child.name == name)
                {
                    return child.gameObject;
                }
            }
            return null;
        }

    }

}
