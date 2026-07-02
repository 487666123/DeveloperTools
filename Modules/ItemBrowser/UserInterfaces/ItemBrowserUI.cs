using System.Collections.Generic;
using System.Linq;
using DeveloperTools.Modules.ItemBrowser.UserInterfaces.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace DeveloperTools.Modules.ItemBrowser.UserInterfaces;

[RegisterUI("Vanilla: Radial Hotbars", "Builder: ItemBrowserUI")]
public partial class ItemBrowserUI : BaseBody
{
    public override bool ContainsPoint(Vector2 point)
    {
        if (LeftPanel.ContainsPoint(point)) return true;

        return base.ContainsPoint(point);
    }

    public override IEnumerable<UIView> BlurElements => [MainPanel, LeftPanel];

    protected override void OnInitialize()
    {
        InitializeComponent();

        Header.ControlTarget = this;
        Header.Title.Text = "物品浏览器";
        Header.CloseButton.LeftMouseDown += (_, _) => Enabled = false;

        // 初始化样式
        MainPanel.BorderColor = SUIColor.Border * 1f;
        MainPanel.BackgroundColor = SUIColor.Background * 0.75f;

        LeftTitle.UseDeathText();

        LeftPanel.BorderColor = SUIColor.Border * 1f;
        LeftPanel.BackgroundColor = SUIColor.Background * 0.75f;

        // ItemBrowser 简称 ib
        // 从 system 获取分类列表
        var ibSystem = ItemBrowserSystem.Instance;

        foreach (var (_, itemCategory) in ibSystem.ItemCategories)
        {
            if (itemCategory.Items.Count == 0) continue;
            if (itemCategory is ItemCategoryFromMod) continue;

            var textView = new ItemBrowserCategoryButton(itemCategory).Join(ItemCategoryList.Container);

            textView.LeftMouseDown += (_, _) => UpdateItemTable(itemCategory);

            SUIDividingLine.Horizontal(Color.Black * 0.5f).Join(ItemCategoryList.Container);
        }

        ItemCategoryList.Container.RemoveChild(ItemCategoryList.Container.Children[^1]);

        UpdateItemTable(ibSystem.ItemCategories["All"]);

        new UICategoryGroup().Join(LeftScrollView.Container);
        new UICategoryGroup()
        {
            Title = { Text = "装备类型筛选" }
        }.Join(LeftScrollView.Container);
        new UICategoryGroup()
        {
            Title = { Text = "功能类型筛选" }
        }.Join(LeftScrollView.Container);
        new UICategoryGroup()
        {
            Title = { Text = "其他类型筛选" }
        }.Join(LeftScrollView.Container);
    }

    private void UpdateItemTable(ItemCategory itemCategory)
    {
        var items = itemCategory.Items;

        ItemTable.Container.RemoveAllChildren();

        for (int i = 0; i < items.Count; i++)
        {
            new ItemBrowserSlot(itemCategory, i).Join(ItemTable.Container);
        }
    }

    protected override void UpdateStatus(GameTime gameTime)
    {
        base.UpdateStatus(gameTime);

        var pos = new Vector2(10, 200);
        var pos2 = new Vector2(10, 300);
        foreach (var mod in ModLoader.Mods)
        {
            if (!mod.HasAsset("icon")) continue;
            if (mod.Assets.Request<Texture2D>("icon")?.Value is not { } t2d) continue;

            Main.spriteBatch.Draw(t2d, pos, Color.White);
            pos += new Vector2(100, 0);
            pos2 += new Vector2(100, 0);

            if (mod.HasAsset("icon_small"))
            {
                Main.spriteBatch.Draw(mod.Assets.Request<Texture2D>("icon_small").Value, pos2 - new Vector2(100, 0), Color.White);
            }
            else
            {
                Main.spriteBatch.Draw(ModAsset.default_small.Value, pos2 - new Vector2(100, 0), Color.White);
            }
        }
    }
}
