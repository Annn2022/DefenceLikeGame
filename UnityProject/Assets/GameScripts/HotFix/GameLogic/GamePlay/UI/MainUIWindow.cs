using GameBase;
using TMPro;
using UnityEngine.UI;

namespace GameLogic.GamePlay.UI
{
    public class MainUIWindow: SingletonBehaviour<MainUIWindow>
    {
        public Button ClearButton;
        public TMP_Text HpText;
        public void SetClearButton(bool isEnable)
        {
            ClearButton.gameObject.SetActive(isEnable);
        }
        
        public void SetHpText(int hp)
        {
            HpText.text = $"HP:{hp.ToString()}";
        }
    }
}