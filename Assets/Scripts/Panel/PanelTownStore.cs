using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanelTownStore : PanelBaseDynamicScrollView
{    
    public Transform RootPanelTownItem;    

    protected override void Awake()
    {
        base.Awake();

        Hot.CenterEvent_.AddEventListener<KeyCode>("KeyHold",
        (key) =>
        {
            if (Hot.NowPanelCanStoreItem != null && key == Hot.MgrInput_.AddMapSize && Hot.NowPanelCanStoreItem.AllContent.localScale.x < 2f)
            {
                Hot.NowPanelCanStoreItem.AllContent.localScale +=
                    new Vector3(Hot.ValueChangeMapSize * Time.deltaTime, Hot.ValueChangeMapSize * Time.deltaTime, 0);
            }
        });

        Hot.CenterEvent_.AddEventListener<KeyCode>("KeyHold",
        (key) =>
        {
            if (Hot.NowPanelCanStoreItem != null && key == Hot.MgrInput_.ReduceMapSize && Hot.NowPanelCanStoreItem.AllContent.localScale.x > 1f)
            {
                Hot.NowPanelCanStoreItem.AllContent.localScale -=
                    new Vector3(Hot.ValueChangeMapSize * Time.deltaTime, Hot.ValueChangeMapSize * Time.deltaTime, 0);                
            }
        });

        Hot.CenterEvent_.AddEventListener<KeyCode>("KeyDown", (key) =>
        {
            if (Hot.NowIndexCellGameArchive != -1 && key == Hot.MgrInput_.PanelTownStore)
            {
                if (Hot.PoolNowPanel_.ListNowPanel.Contains("PanelTownStore"))
                    Hot.MgrUI_.HidePanel(false, Hot.PanelTownStore_.gameObject, "PanelTownStore");
                else
                    Hot.MgrUI_.ShowPanel<PanelTownStore>(true, "PanelTownStore");
            }
        });

        RootPanelTownItem = transform.FindSonSonSon("RootPanelTownItem");
        Content = transform.FindSonSonSon("TownStoreContent");
    }

    public override void InitContent()
    {
        for (int i1 = 0; i1 < Hot.DataNowCellGameArchive.ListCellStore.Count; i1++)
        {
            int tempi = i1;

            Hot.MgrUI_.CreatePanel<PanelCellTownStore>
            (false, "/PanelCellTownStore", 
            (PanelCellTownStore_) =>
            {
                PanelCellTownStore_.Index = tempi;
                GameObject obj =
                    Hot.MgrRes_.Load<GameObject>("Prefabs/" + "DynamicContentStep" + PanelCellTownStore_.PrefabsDynamicContentStepSuffix);
                obj.name = tempi.ToString();
                obj.transform.SetParent(Content, false);
                obj.GetComponent<DynamicContentStep>().Init(tempi);
                PanelCellTownStore_.transform.SetParent(obj.GetComponent<DynamicContentStep>().DependentObjRoot, false);                
                ListDynamicContentStep.Add(obj.GetComponent<DynamicContentStep>());

                PanelCellTownStore_.Init();

                Hot.MgrUI_.CreatePanelAndPush<PanelTownItem>
                (true, "/PanelTownItem", true, false, "PanelTownItem" + tempi,
                (PanelTownItem_) =>
                {
                    PanelTownItem_.gameObject.name = "PanelTownItem" + tempi;
                    PanelTownItem_.transform.SetParent(RootPanelTownItem, false);
                    PanelTownItem_.gameObject.SetActive(false);

                    PanelTownItem_.PanelCellTownStore_ = PanelCellTownStore_;
                    PanelCellTownStore_.PanelCellItem_ = PanelTownItem_;

                    PanelTownItem_.InitContent();                    
                });
            });
        }        
    }

    public override void Clear()
    {
        PanelCellTownStore[] all = Content.GetComponentsInChildren<PanelCellTownStore>();
        for (int i = 0; i < all.Length; i++)
        {
            Hot.MgrUI_.DicPanel.Remove(all[i].PanelCellItem_.gameObject.name);
            Hot.PoolBuffer_.DicPool.Remove(all[i].PanelCellItem_.gameObject.name);            
            DestroyImmediate(all[i].PanelCellItem_.gameObject);
            DestroyImmediate(all[i].gameObject);
        }
    }
    
    public override void SortContent()
    {
        DynamicContentStep[] allDynamicContentStep = transform.GetComponentsInChildren<DynamicContentStep>();
        List<DynamicContentStep> tempList = new();
        for (int i = 0; i < allDynamicContentStep.Length; i++)
        {
            tempList.Add(allDynamicContentStep[i]);
            tempList[i].SetIndex(i);
            tempList[i].SetIndex(i);
            tempList[i].gameObject.name = i.ToString();
        }
        ListDynamicContentStep = tempList;

        PanelCellTownStore[] all = transform.GetComponentsInChildren<PanelCellTownStore>();
        List<DataContainer_CellTownStore> tempData = new();
        for (int i = 0; i < all.Length; i++)
        {
            tempData.Add(Hot.DataNowCellGameArchive.ListCellStore[all[i].Index]);
            all[i].Index = i;
        }
        Hot.DataNowCellGameArchive.ListCellStore = tempData;

        Hot.Data_.Save();
    }
}
