using UnityEngine;

namespace UnityEssentials
{
    public class SetDisplayResolutions : MonoBehaviour
    {
        private const string DataReference = "test";

        private UIMenuOptionsData _optionsData;

        public void Awake()
        {
            if (transform.parent.GetComponent<UIMenu>().Data
                .GetDataByReference(DataReference, out _optionsData))
            {
                _optionsData.IsDynamic = true;
                _optionsData.Options = new string[] { "Test" };
            }
        }
    }
}
