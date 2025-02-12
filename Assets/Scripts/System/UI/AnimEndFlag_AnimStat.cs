using UnityEngine;
public class AnimEndFlag_AnimStat : MonoBehaviour
{
    public bool panelCloseFlag;
    public void  AnimStateEnd()
    {
        panelCloseFlag = true;
    }

    public void GameObjectActivait()
    {
        gameObject.SetActive(false);
    }
}
