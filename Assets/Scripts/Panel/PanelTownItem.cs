using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelTownItem : PanelBase
{    
    public int NowIndex;
    public PanelCellTownStore FatherPanelCellTownStore;
    public Transform Content;
    
    protected override void Awake()
    {
        base.Awake();

        Content = transform.FindSonSonSon("Content");        

        gameObject.SetActive(false);
    }

    public void Show(PanelCellTownStore NowPanelCellTownStore)
    {
        if (GlobalHot.PanelTownStore_.NowPanelCellTownStore == NowPanelCellTownStore)
        {
            PoolEsc.GetInstance().RemoveListNoInMgrUI(gameObject);
            GlobalHot.PanelTownStore_.NowPanelCellTownStore = null;            
            gameObject.SetActive(false);
            return;
        }

        if (GlobalHot.PanelTownStore_.NowPanelCellTownStore != null)
        {
            PoolEsc.GetInstance().RemoveListNoInMgrUI(gameObject);
            GlobalHot.PanelTownStore_.NowPanelCellTownStore.PanelCellItem_.gameObject.SetActive(false);
        }
        GlobalHot.PanelTownStore_.NowPanelCellTownStore = NowPanelCellTownStore;
        PoolEsc.GetInstance().AddListNoInMgrUI(gameObject, 
        () => 
        {
            GlobalHot.PanelTownStore_.NowPanelCellTownStore = null;            
        });
        gameObject.SetActive(true);
    }     

    public void UpdateContent()
    {
        NowIndex = 0;

        for (int i = 0; i < GlobalHot.NowCellGameArchive.DataListCellStore[FatherPanelCellTownStore.Index].DataListCellStoreItem.Count; i++)
        {
            int tempi = i;

            MgrUI.GetInstance().CreatePanelAndPush<PanelCellTownItem>
                             (false, "/PanelCellTownItem", callback :
            (panel) =>
            {
                panel.transform.SetParent(Content, false);
                panel.e_Location = E_Location.TownStore;
                panel.Index = NowIndex;                
                NowIndex++;
            });
        }
    }

    public void Add()
    {

    }

    public void Subtraction()
    {

    }
}
