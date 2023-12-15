using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.UI;

public class PanelBarRoleListExpedition : PanelBase
{
    public int NowPutIndex = -1;

    public List<int> ListNeedPutRoleIndex = new();
    public List<PanelCellRoleExpedition> ListCellRoleExpedition = new();

    public Transform RoleListExpeditionContent;

    protected override void Awake()
    {
        base.Awake();

        Hot.CenterEvent_.AddEventListener<KeyCode>(E_InputKeyEvent.KeyDown.ToString(),
        (key) =>
        {
            if (Hot.e_NowPlayerLocation == E_PlayerLocation.OnExpedition && key == Hot.MgrInput_.RoleList)
            {
                if (Hot.PoolNowPanel_.ContainPanel("PanelBarRoleListExpedition"))
                {
                    Hot.MgrUI_.HidePanel(false, gameObject, "PanelBarRoleListExpedition");
                }
                else
                {
                    Hot.MgrUI_.ShowPanel<PanelBarRoleListExpedition>(true, "PanelBarRoleListExpedition");
                }
            }
        });

        RoleListExpeditionContent = transform.FindSonSonSon("RoleListExpeditionContent");
    }

    public void Init()
    {
        ListNeedPutRoleIndex = new();
        for (int i = 0; i < Hot.DataNowCellGameArchive.ListExpeditionRoleIndex.Count; i++)
        {
            ListNeedPutRoleIndex.Add(Hot.DataNowCellGameArchive.ListExpeditionRoleIndex[i]);
        }

        for (int i = 0; i < ListNeedPutRoleIndex.Count; i++)
        {
            int tempi = i;

            Hot.MgrUI_.CreatePanel<PanelCellRoleExpedition>(false, "/PanelCellRoleExpedition",
            (panel) =>
            {
                panel.Init(tempi, ListNeedPutRoleIndex[tempi], RoleListExpeditionContent);
                ListCellRoleExpedition.Add(panel);
            });
        }
    }    

    public PanelCellRoleExpedition GetCellRoleExpedition(int p_Index)
    {
        return RoleListExpeditionContent.GetChild(p_Index).GetComponent<PanelCellRoleExpedition>();
    }

    public void ClearNoData()
    {
        foreach (PanelCellRoleExpedition item in ListCellRoleExpedition)
        {
            Destroy(item.gameObject);
        }

        NowPutIndex = -1;
        ListNeedPutRoleIndex.Clear();
        ListCellRoleExpedition.Clear();
    }

    public void ClearAndData()
    {
        foreach (PanelCellRoleExpedition item in ListCellRoleExpedition)
        {
            (item.CellExpeditionMiniMap.RootGrid as PanelGridExpeditionRoom).Data.IndexListRole = -1;
            Destroy(item.gameObject);
        }

        NowPutIndex = -1;
        ListNeedPutRoleIndex.Clear();
        ListCellRoleExpedition.Clear();
    }

    public void Sort()
    {
        List<int> ListIndex = Hot.DataNowCellGameArchive.ListExpeditionRoleIndex;
        int count = RoleListExpeditionContent.childCount;

        for (int i = 0; i < ListCellRoleExpedition.Count; i++)
        {
            ListCellRoleExpedition[i].transform.SetParent(Hot.MgrUI_.UIBaseCanvas, false);
        }

        for (int i = 0; count != RoleListExpeditionContent.childCount; i++)
        {
            if (i == count)
            {
                i = 0;
            }

            if (ListIndex[0] == ListCellRoleExpedition[i].IndexRoleList)
            {
                ListCellRoleExpedition[i].transform.SetParent(RoleListExpeditionContent, false);
                ListCellRoleExpedition[i].transform.localPosition = Vector3.zero;
                ListIndex.RemoveAt(0);
            }
        }
    }
}
