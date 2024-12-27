using UnityEngine;
using UnityEngine.UI;

public class EquipmentController : MonoBehaviour
{
    [SerializeField] GameObject equipment_ScrollView;
    [SerializeField] Button back_button;
    BaseStatus robotbase;
    UpdateTime_Class updateTime = new UpdateTime_Class();
    UNLOCK_EQUIPMENT_SLOT selectslot_Nomber;
    [SerializeField] EquipmentSO equipmentSO;
    EquipmentSO.EQUIPMENT_VALUE[] e_values;
    [SerializeField] Transform equipment_parent;
    EquipmentSelectSlot[] e_select_slots;

    
    [Header("装備")]
    [SerializeField] EquipmentSlot battery_slot;
    [SerializeField] Transform equipmentSlot_parent;
    [SerializeField] EquipmentSlot[] equipmentSlots;

    private void Start()
    {
        back_button.onClick.AddListener(OnClick_BackButton);
        e_select_slots = equipment_parent.GetComponentsInChildren<EquipmentSelectSlot>();
        equipmentSlots = equipmentSlot_parent.GetComponentsInChildren<EquipmentSlot>();
        e_values = equipmentSO.equipment_values;

        // 装備 
        battery_slot.icon.sprite = null;
        battery_slot.icon.enabled = false;
        foreach(var _slot in equipmentSlots)
        {
            _slot.SetEquipmentController(this);
            _slot.icon.sprite = null;
            _slot.icon.enabled = false;
        }

        SetText_EquipmentInfo();
        SetActive_Equipment_ScrollView(false);
    }

    private void Update() {
        if(updateTime.UpdateTime() == true)
        {
            SetText_EquipmentInfo();    // テキストの更新
        }
    }

    /// <summary>
    /// 装備のInfoパネルにあるテキストなどの設定
    /// </summary>
    /// <param name="_name"></param>
    void SetText_EquipmentInfo()
    {
        for(int ii = 0; ii < e_values.Length; ii++)
        {
            e_select_slots[ii].SetText_EquipmentInfo(e_values[ii], this);
        }
    }

    /// <summary>
    /// 装備を選んだあと装備スロットに設定する処理
    /// また、"RobotBaseStatus"の個々の"Equipment_Value"を上書き
    /// </summary>
    /// <param name="selectslot_Nomber">選択された装備欄のスロット</param>
    /// <param name="_value"></param>
    public void SetEquipmentSlot(EquipmentSO.EQUIPMENT_VALUE _value)
    {
        switch(selectslot_Nomber)
        {
            case UNLOCK_EQUIPMENT_SLOT.ONE:
                robotbase.equipment_value[0] = _value;
                equipmentSlots[0].SetText_EquipmentValue(_value);
                break;
            case UNLOCK_EQUIPMENT_SLOT.TWO:
                robotbase.equipment_value[1] = _value;
                equipmentSlots[1].SetText_EquipmentValue(_value);
                break;
            case UNLOCK_EQUIPMENT_SLOT.THREE:
                robotbase.equipment_value[2] = _value;
                equipmentSlots[2].SetText_EquipmentValue(_value);
                break;
        }
    }

    /// <summary>
    /// ロボットを押された時に初めに処理される
    /// </summary>
    /// <param name="_robotBase"></param>
    public void Check_UnlockEquipmentSlot(BaseStatus _robotBase)
    {
        robotbase = _robotBase;

        switch(_robotBase.unlock_equipment_slot)
        {
            case UNLOCK_EQUIPMENT_SLOT.NONE:
                SetButtonInteractable(0);
                break;
            case UNLOCK_EQUIPMENT_SLOT.ONE:
                equipmentSlots[0].SetText_EquipmentValue(_robotBase.equipment_value[0]);
                SetButtonInteractable(1);
                break;
            case UNLOCK_EQUIPMENT_SLOT.TWO:
                equipmentSlots[0].SetText_EquipmentValue(_robotBase.equipment_value[0]);
                equipmentSlots[1].SetText_EquipmentValue(_robotBase.equipment_value[1]);
                SetButtonInteractable(2);
                break;
            case UNLOCK_EQUIPMENT_SLOT.THREE:
                equipmentSlots[0].SetText_EquipmentValue(_robotBase.equipment_value[0]);
                equipmentSlots[1].SetText_EquipmentValue(_robotBase.equipment_value[1]);
                equipmentSlots[2].SetText_EquipmentValue(_robotBase.equipment_value[2]);
                SetButtonInteractable(3);
                break;
        }
    }

    /// <summary>
    /// 装備スロットのボタンの"Interactable"の設定と"Icon"の表示・非表示
    /// </summary>
    /// <param name="trueNum"></param>
    void SetButtonInteractable(int trueNum)
    {
        for(int ii = 0; ii < equipmentSlots.Length; ii++)
        {
            if(ii < trueNum)
            {
                equipmentSlots[ii].button.interactable = true;
                if(equipmentSlots[ii].icon.sprite == null)
                {
                    equipmentSlots[ii].icon.enabled = false;
                }
            }
            else
            {
                equipmentSlots[ii].button.interactable = false;
                equipmentSlots[ii].icon.sprite = equipmentSO.stop_sprite;
                equipmentSlots[ii].icon.enabled = true;
            }
            
        }
    }

    public void SetSelectNomber(UNLOCK_EQUIPMENT_SLOT nomber) { selectslot_Nomber = nomber; }
    public void SetActive_Equipment_ScrollView(bool flag) { equipment_ScrollView.SetActive(flag); }
    public void OnClick_BackButton() { SetActive_Equipment_ScrollView(false); }
}