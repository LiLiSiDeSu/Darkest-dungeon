using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelOtherEditorRoleConfig : PanelBaseVector2<PanelCellRoleConfig, PanelGridRoleConfig>
{
    public E_Skill e_ChoseSkill;
    public E_RoleName e_ChoseRoleName;

    public Image ImgCurrentRole;
    public Image ImgCurrentSkill;

    public InputField IptChangeY;
    public InputField IptChangeX;

    public Toggle TogIsReversal;

    public Transform RoleContent;
    public Transform SkillContent;
    public Transform RoleSkillContent;

    public Dictionary<E_Skill ,PanelCellRoleConfigChooseSkill> DicSkills = new();

    protected override void Awake()
    {
        base.Awake();

        ImgCurrentRole = transform.FindSonSonSon("ImgCurrentRole").GetComponent<Image>();
        ImgCurrentSkill = transform.FindSonSonSon("ImgCurrentSkill").GetComponent<Image>();

        IptChangeY = transform.FindSonSonSon("IptChangeY").GetComponent<InputField>();
        IptChangeX = transform.FindSonSonSon("IptChangeX").GetComponent<InputField>();

        TogIsReversal = transform.FindSonSonSon("TogIsReversal").GetComponent<Toggle>();

        RoleContent = transform.FindSonSonSon("RoleContent");
        SkillContent = transform.FindSonSonSon("SkillContent");
        RoleSkillContent = transform.FindSonSonSon("RoleSkillContent");

        Hot.TriggerEvent_.AddEventListener<KeyCode>
        (E_KeyEvent.KeyDown.ToString(), 
        (key) =>
        {
            if (Hot.PoolNowPanel_.ContainPanel(E_PanelName.PanelOtherEditorRoleConfig) && key == KeyCode.Mouse1)
            {
                if (Hot.ChoseCellRoleConfig != null)
                {
                    return;
                }
                if (e_ChoseSkill != E_Skill.None)
                {
                    UpdateImgCurrentSkill(E_Skill.None);

                    Hot.ChoseCellRoleConfig = null;
                    Hot.NowEnterCellRoleConfig = null;
                    Hot.NowEnterGridRoleConfig = null;

                    //Clear skill area

                    return;
                }
                if (e_ChoseRoleName != E_RoleName.None)
                {
                    Clear();

                    return;
                }
            }
        });

        Hot.TriggerEvent_.AddEventListener<KeyCode>
        (E_KeyEvent.KeyDown.ToString(),
        (key) =>
        {
            if (Hot.PoolNowPanel_.ContainPanel(E_PanelName.PanelOtherEditorRoleConfig) && key == KeyCode.Mouse1)
            {
                ClearImgStatus();

                //Unselect the CellMapEditor that was selected
                if (Hot.ChoseCellRoleConfig != null)
                {
                    Hot.ChoseCellRoleConfig.ImgStatus.sprite = Hot.LoadSprite(E_Res.ImgEmpty);
                    Hot.ChoseCellRoleConfig.ImgItem.raycastTarget = true;
                    Hot.ChoseCellRoleConfig = null;

                    return;
                }
            }
        });

        LimitAdd = 5f;
        LimitReduce = 0.3f;

        InitChooseContent();
    }

    protected override void Button_OnClick(string controlname)
    {
        base.Button_OnClick(controlname);

        switch (controlname)
        {
            case "BtnClearMap":
                Clear();
                break;
            case "BtnSave":
                Debug.Log("BtnSave");
                break;
            case "BtnChangeMapSize":
                if (Grids.Count > 0)
                {
                    ChangeMapSize();
                }
                break;
        }
    }
    
    public void InitChooseContent()
    {
        foreach (E_RoleName item in Enum.GetValues(typeof(E_RoleName)))
        {
            if (item == E_RoleName.None)
            {
                continue;
            }

            Hot.MgrUI_.CreatePanel<PanelCellRoleConfigChooseRole>
            (false, E_PanelName.PanelCellRoleConfigChooseRole,
            (panel) =>
            {
                panel.Init(item, RoleContent);
            });
        }

        foreach (E_Skill item in Enum.GetValues(typeof(E_Skill)))
        {
            if (item == E_Skill.None)
            {
                continue;
            }

            Hot.MgrUI_.CreatePanel<PanelCellRoleConfigChooseSkill>
            (false, E_PanelName.PanelCellRoleConfigChooseSkill,
            (panel) =>
            {
                panel.Init(item, SkillContent);
                DicSkills.Add(item, panel);
            });
        }
    }
    
    public void Clear()
    {
        ClearAll();
        ClearRoleSkillContent();
        UpdateImgCurrentRole(E_RoleName.None);
        UpdateImgCurrentSkill(E_Skill.None);
    }
    public void ClearRoleSkillContent()
    {
        if (e_ChoseRoleName == E_RoleName.None)
        {
            return;
        }

        foreach (E_Skill item in Hot.DicRoleConfig[e_ChoseRoleName].DicSkill.Keys)
        {
            DicSkills[item].UpdateImgIsRoleSkill(false);
        }

        int count = RoleSkillContent.childCount;
        for (int i = 0; i < count; i++)
        {
            DestroyImmediate(RoleSkillContent.GetChild(0).gameObject);
        }
    }
    public void Save()
    {
        Hot.MgrJson_.Save(Hot.DicRoleConfig, "", "/Config");
    }

    public override void InitGrids(int Y, int X)
    {
        ClearAll();

        base.InitGrids(Y, X);
    }
    public void GenerateByData(E_RoleName p_e_Rolename)
    {
        (AllContent as RectTransform).sizeDelta = 
            new(Hot.DicRoleConfig[p_e_Rolename].SizeBody.X * Hot.BodySizeCellMinimap.X, Hot.DicRoleConfig[p_e_Rolename].SizeBody.Y * Hot.BodySizeCellMinimap.Y);

        InitGrids(Hot.DicRoleConfig[p_e_Rolename].SizeBody.Y, Hot.DicRoleConfig[p_e_Rolename].SizeBody.X);

        Hot.MgrUI_.CreatePanel<PanelCellRoleConfig>
        (false, E_PanelName.PanelCellRoleConfig, 
        (panel) =>
        {
            panel.Init(p_e_Rolename, Grids[0][0] as PanelGridRoleConfig);

            for (int iy = 0; iy < Hot.DicRoleConfig[p_e_Rolename].SizeBody.Y; iy++)
            {
                for (int ix = 0; ix < Hot.DicRoleConfig[p_e_Rolename].SizeBody.X; ix++)
                {
                    Grids[panel.RootGrid.Y + iy][panel.RootGrid.X + ix].Item = panel;
                }
            }
        });
    }

    public void UpdateImgCurrentSkill(E_Skill p_e_Skill)
    {
        Hot.PanelOtherEditorRoleConfig_.e_ChoseSkill = p_e_Skill;

        if (p_e_Skill == E_Skill.None)
        {
            Hot.PanelOtherEditorRoleConfig_.ImgCurrentSkill.sprite = Hot.MgrRes_.LoadSprite("Portrait" + p_e_Skill);
        }
        else
        {
            Hot.PanelOtherEditorRoleConfig_.ImgCurrentSkill.sprite = Hot.MgrRes_.LoadSprite(p_e_Skill.ToString());   
        }
    }
    public void UpdateImgCurrentRole(E_RoleName p_e_RoleName)
    {
        Hot.PanelOtherEditorRoleConfig_.e_ChoseRoleName = p_e_RoleName;
        Hot.PanelOtherEditorRoleConfig_.ImgCurrentRole.sprite = Hot.MgrRes_.LoadSprite("Portrait" + p_e_RoleName);
    }

    public void UpdateRoleSkillContent(E_RoleName p_e_RoleName)
    {
        ClearRoleSkillContent();

        foreach (E_Skill item in Hot.DicRoleConfig[p_e_RoleName].DicSkill.Keys)
        {
            if (DicSkills.ContainsKey(item))
            {
                DicSkills[item].UpdateImgIsRoleSkill(true);
            }

            Hot.MgrUI_.CreatePanel<PanelCellRoleConfigRoleSkill>
            (false, E_PanelName.PanelCellRoleConfigRoleSkill,
            (panel) =>
            {
                panel.Init(item, RoleSkillContent);
            });
        }
    }
    
    public void ChangeMapSize()
    {
        int ChangeY = int.Parse(IptChangeY.text);
        int ChangeX = int.Parse(IptChangeX.text);
        bool isDone = false;

        (AllContent as RectTransform).sizeDelta = new((Grids[0].Count + ChangeX) * Hot.BodySizeCellMinimap.X, 
                                                      (Grids.Count + ChangeY) * Hot.BodySizeCellMinimap.Y);

        if (ChangeY == 0)
        {
            isDone = true;
        }

        if (ChangeY > 0)
        {
            for (int iy = 0; iy < ChangeY; iy++)
            {
                int tempiy = iy;
                int GridsCount = Grids.Count;

                Grids.Add(new());
                ItemRoot.Add(new());

                GameObject ItemY = Hot.CreateContentStepY(Grids.Count - 1, ItemContent);
                ItemY.transform.SetParent(ItemContent, false);
                GameObject ImgBkY = Hot.CreateContentStepY(Grids.Count - 1, ImgBkContent);
                ImgBkY.transform.SetParent(ImgBkContent, false);
                GameObject ImgStatusY = Hot.CreateContentStepY(Grids.Count - 1, ImgStatusContent);
                ImgStatusY.transform.SetParent(ImgStatusContent, false);

                for (int ix = 0; ix < Grids[0].Count; ix++)
                {
                    int tempix = ix;

                    GameObject objX = Hot.CreateContentStepX(tempix, ItemY.transform);
                    ItemRoot[GridsCount].Add(objX.transform);

                    Hot.MgrUI_.CreatePanel<PanelGridRoleConfig>
                    (false, E_PanelName.PanelGridRoleConfig,
                    (panel) =>
                    {
                        panel.Init(tempix, GridsCount, ComponentRoot);
                        Grids[GridsCount].Add(panel);

                        panel.ImgBk.transform.SetParent(ImgBkY.transform, false);
                        panel.ImgStatus.transform.SetParent(ImgStatusY.transform, false);

                        if (tempiy == ChangeY - 1 && tempix == Grids[0].Count - 1)
                        {
                            isDone = true;

                            if (TogIsReversal.isOn)
                            {
                                for (int i2y = Grids.Count - 1; i2y >= 0; i2y--)
                                {
                                    for (int i2x = Grids[0].Count - 1; i2x >= 0; i2x--)
                                    {
                                        if (Grids[i2y][i2x].Item != null && Grids[i2y][i2x].Item.RootGrid == Grids[i2y][i2x])
                                        {
                                            MoveGrid(Grids[i2y][i2x] as PanelGridRoleConfig, Grids[i2y + ChangeY][i2x] as PanelGridRoleConfig);
                                        }
                                    }
                                }
                            }
                        }
                    });
                }
            }
        }
        else if (ChangeY < 0 && 
                (ChangeY + Grids.Count) > 0 &&
                (ChangeY + Grids.Count) >= Hot.DicRoleConfig[Hot.PanelOtherEditorRoleConfig_.e_ChoseRoleName].SizeBody.Y)
        {
            isDone = true;
            int ReduceUpLength = 0;

            for (int i = 0; i < -ChangeY; i++)
            {
                if (TogIsReversal.isOn)
                {
                    if (JudgeUpCanReduceY())
                    {
                        ReduceUpLength++;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    if (JudgeDownCanReduceY())
                    {
                        DestroyOneDownY(1);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            for (int i2y = 0; i2y < Grids.Count; i2y++)
            {
                for (int i2x = 0; i2x < Grids[0].Count; i2x++)
                {
                    if (Grids[i2y][i2x].Item != null && Grids[i2y][i2x].Item.RootGrid == Grids[i2y][i2x])
                    {
                        MoveGrid(Grids[i2y][i2x] as PanelGridRoleConfig, Grids[i2y - ReduceUpLength][i2x] as PanelGridRoleConfig);
                    }
                }
            }

            DestroyOneDownY(ReduceUpLength);
        }

        UpdateEvent updateEvent = gameObject.AddComponent<UpdateEvent>();
        updateEvent.AddEvent(() =>
        {
            if (!isDone)
            {
                return;
            }

            if (ChangeX > 0)
            {
                for (int iX = 0; iX < ChangeX; iX++)
                {
                    int tempiX = iX;

                    for (int iY = 0; iY < Grids.Count; iY++)
                    {
                        int tempiY = iY;

                        GameObject itemX = Hot.CreateContentStepX(Grids.Count, ItemContent.Find(tempiY.ToString()));
                        ItemRoot[tempiY].Add(itemX.transform);

                        Hot.MgrUI_.CreatePanel<PanelGridRoleConfig>(false, E_PanelName.PanelGridRoleConfig,
                        (panel) =>
                        {
                            Grids[tempiY].Add(panel);
                            panel.Init(Grids[tempiY].Count - 1, tempiY, ComponentRoot);

                            panel.ImgBk.transform.SetParent(ImgBkContent.Find(tempiY.ToString()), false);
                            panel.ImgStatus.transform.SetParent(ImgStatusContent.Find(tempiY.ToString()), false);

                            if (TogIsReversal.isOn && tempiX == ChangeX - 1 && tempiY == Grids.Count - 1)
                            {
                                for (int i2y = Grids.Count - 1; i2y >= 0; i2y--)
                                {
                                    for (int i2x = Grids[0].Count - 1; i2x >= 0; i2x--)
                                    {
                                        if (Grids[i2y][i2x].Item != null && Grids[i2y][i2x].Item.RootGrid == Grids[i2y][i2x])
                                        {
                                            MoveGrid(Grids[i2y][i2x] as PanelGridRoleConfig, Grids[i2y][i2x + ChangeX] as PanelGridRoleConfig);
                                        }
                                    }
                                }
                            }
                        });
                    }
                }
            }
            else if (ChangeX < 0 && 
                    (ChangeX + Grids[0].Count) > 0 &&
                    (ChangeX + Grids[0].Count) >= Hot.DicRoleConfig[Hot.PanelOtherEditorRoleConfig_.e_ChoseRoleName].SizeBody.X)
            {
                int ReduceLeftLength = 0;

                for (int i = 0; i < -ChangeX; i++)
                {
                    if (TogIsReversal.isOn)
                    {
                        if (JudgeLeftCanReduceX())
                        {
                            ReduceLeftLength++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (JudgeRightCanReduceX())
                        {
                            for (int y = 0; y < Grids.Count; y++)
                            {
                                DestroyImmediate(Grids[y][^1].ImgBk.gameObject);
                                DestroyImmediate(Grids[y][^1].ImgStatus.gameObject);
                                DestroyImmediate(ItemContent.Find(y.ToString()).GetChild(ItemContent.Find(y.ToString()).childCount - 1).gameObject);
                                ItemRoot[y].RemoveAt(ItemRoot[y].Count - 1);
                                DestroyImmediate(Grids[y][^1].gameObject);
                                Grids[y].RemoveAt(Grids[y].Count - 1);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                for (int i2y = 0; i2y < Grids.Count; i2y++)
                {
                    for (int i2x = 0; i2x < Grids[0].Count; i2x++)
                    {
                        if (Grids[i2y][i2x].Item != null && Grids[i2y][i2x].Item.RootGrid == Grids[i2y][i2x])
                        {
                            MoveGrid(Grids[i2y][i2x] as PanelGridRoleConfig, Grids[i2y][i2x - ReduceLeftLength] as PanelGridRoleConfig);
                        }
                    }
                }

                for (int i = 0; i < ReduceLeftLength; i++)
                {
                    for (int y = 0; y < Grids.Count; y++)
                    {
                        DestroyImmediate(Grids[y][^1].ImgBk.gameObject);
                        DestroyImmediate(Grids[y][^1].ImgStatus.gameObject);
                        DestroyImmediate(ItemContent.Find(y.ToString()).GetChild(ItemContent.Find(y.ToString()).childCount - 1).gameObject);
                        ItemRoot[y].RemoveAt(ItemRoot[y].Count - 1);
                        DestroyImmediate(Grids[y][^1].gameObject);
                        Grids[y].RemoveAt(Grids[y].Count - 1);
                    }
                }
            }

            updateEvent.Byby();
        });
    }
    public void MoveGrid(PanelGridRoleConfig p_source, PanelGridRoleConfig p_moveTo)
    {
        p_source.Item.RootGrid = p_moveTo;
        p_source.Item.transform.SetParent(ItemRoot[p_moveTo.Y][p_moveTo.X], false);
        p_source.Item.transform.localPosition = new(-20, 20);

        E_RoleName e_RoleName = p_source.Item.e_RoleName;

        if (e_RoleName != E_RoleName.None)
        {
            int sourceX = p_source.X;
            int sourceY = p_source.Y;

            for (int iy = 0; iy < Hot.DicRoleConfig[e_RoleName].SizeBody.Y; iy++)
            {
                for (int ix = 0; ix < Hot.DicRoleConfig[e_RoleName].SizeBody.X; ix++)
                {
                    Grids[sourceY + iy][sourceX + ix].Item = null;
                }
            }
            for (int iy = 0; iy < Hot.DicRoleConfig[e_RoleName].SizeBody.Y; iy++)
            {
                for (int ix = 0; ix < Hot.DicRoleConfig[e_RoleName].SizeBody.X; ix++)
                {
                    Grids[p_moveTo.Y + iy][p_moveTo.X + ix].Item = ItemRoot[p_moveTo.Y][p_moveTo.X].GetComponentInChildren<PanelCellRoleConfig>();
                }
            }
        }
        else
        {
            p_source.Item = null;
            p_moveTo.Item = ItemRoot[p_moveTo.Y][p_moveTo.X].GetComponentInChildren<PanelCellRoleConfig>();
        }
    }
    public void DestroyOneDownY(int p_ReduceUpLength)
    {
        for (int i = 0; i < p_ReduceUpLength; i++)
        {
            DestroyImmediate(ImgBkContent.GetChild(ImgBkContent.childCount - 1).gameObject);
            DestroyImmediate(ImgStatusContent.GetChild(ImgStatusContent.childCount - 1).gameObject);
            DestroyImmediate(ItemContent.GetChild(ItemContent.childCount - 1).gameObject);

            ItemRoot.RemoveAt(ItemRoot.Count - 1);

            for (int c = 0; c < Grids[^1].Count; c++)
            {
                DestroyImmediate(Grids[^1][c].gameObject);
            }

            Grids.RemoveAt(Grids.Count - 1);
        }
    }
    public bool JudgeUpCanReduceY()
    {
        for (int i = 0; i < Grids[^1].Count; i++)
        {
            if (Grids[0][i].Item != null)
            {
                return false;
            }
        }

        return true;
    }
    public bool JudgeDownCanReduceY()
    {
        for (int i = 0; i < Grids[^1].Count; i++)
        {
            if (Grids[^1][i].Item != null)
            {
                return false;
            }
        }

        return true;
    }
    public bool JudgeLeftCanReduceX()
    {
        for (int i = 0; i < Grids.Count; i++)
        {
            if (Grids[i][0].Item != null)
            {
                return false;
            }
        }

        return true;
    }
    public bool JudgeRightCanReduceX()
    {
        for (int i = 0; i < Grids.Count; i++)
        {
            if (Grids[i][^1].Item != null)
            {
                return false;
            }
        }

        return true;
    }
}
