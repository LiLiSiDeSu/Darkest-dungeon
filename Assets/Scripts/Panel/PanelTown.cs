using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelTown : PanelBase
{
    protected override void Start()
    {
        base.Start();

        transform.FindSonSonSon("ImgRecruit").GetComponent<Image>().alphaHitTestMinimumThreshold = 0.2f;
        transform.FindSonSonSon("ImgWineHouse").GetComponent<Image>().alphaHitTestMinimumThreshold = 0.2f;
        transform.FindSonSonSon("ImgGraveyard").GetComponent<Image>().alphaHitTestMinimumThreshold = 0.2f;
        transform.FindSonSonSon("ImgShop").GetComponent<Image>().alphaHitTestMinimumThreshold = 0.2f;
        transform.FindSonSonSon("ImgSmithy").GetComponent<Image>().alphaHitTestMinimumThreshold = 0.2f;
        transform.FindSonSonSon("ImgMercenaryaAssociation").GetComponent<Image>().alphaHitTestMinimumThreshold = 0.2f;
        transform.FindSonSonSon("ImgTownStore").GetComponent<Image>().alphaHitTestMinimumThreshold = 0.2f;
    }  

    protected override void Button_OnClick(string controlname)
    {
        base.Button_OnClick(controlname);

        switch (controlname)
        {
            case "BtnRecruit":
                MgrUI.GetInstance().GetPanel<PanelRooms>("PanelRooms").StartByTown("PanelRoomRecruit");               
                break;

            case "BtnWineHouse":
                MgrUI.GetInstance().GetPanel<PanelRooms>("PanelRooms").StartByTown("PanelRoomWineHouse");              
                break;

            case "BtnGraveyard":
                MgrUI.GetInstance().GetPanel<PanelRooms>("PanelRooms").StartByTown("PanelRoomGraveyard");              
                break;

            case "BtnShop":
                MgrUI.GetInstance().GetPanel<PanelRooms>("PanelRooms").StartByTown("PanelRoomShop");
                break;

            case "BtnSmithy":
                MgrUI.GetInstance().GetPanel<PanelRooms>("PanelRooms").StartByTown("PanelRoomSmithy");              
                break;

            case "BtnMercenaryaAssociation":
                MgrUI.GetInstance().GetPanel<PanelRooms>("PanelRooms").StartByTown("PanelRoomMercenaryaAssociation");           
                break;

            case "BtnTownStore":
                if (PoolNowPanel.GetInstance().ListNowPanel.Contains("PanelTownStore"))
                {
                    MgrUI.GetInstance().HidePanel
                    (false, MgrUI.GetInstance().DicPanel["PanelTownStore"].gameObject, "PanelTownStore");
                    GlobalHot.PanelTownStore_.NowPanelCellTownStore = null;
                    break;
                }
                MgrUI.GetInstance().ShowPanel<PanelTownStore>(true, "PanelTownStore");
                break;

            case "BtnAncestralProperty":
                if (PoolNowPanel.GetInstance().ListNowPanel.Contains("PanelStoreAncestralProperty"))
                {
                    MgrUI.GetInstance().HidePanel
                    (false, MgrUI.GetInstance().DicPanel["PanelStoreAncestralProperty"].gameObject, "PanelStoreAncestralProperty");
                    break;
                }
                MgrUI.GetInstance().ShowPanel<PanelStoreAncestralProperty>(true, "PanelStoreAncestralProperty");
                break;

            case "BtnCoin":
                if (PoolNowPanel.GetInstance().ListNowPanel.Contains("PanelStoreCoin"))
                {
                    MgrUI.GetInstance().HidePanel
                    (false, MgrUI.GetInstance().DicPanel["PanelStoreCoin"].gameObject, "PanelStoreCoin");
                    break;
                }
                MgrUI.GetInstance().ShowPanel<PanelStoreCoin>(true, "PanelStoreCoin");
                break;

            case "BtnTransformRes":
                break;
        }
    }   
}
