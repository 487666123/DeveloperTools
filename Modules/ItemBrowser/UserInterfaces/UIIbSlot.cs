using Microsoft.Xna.Framework;
using Terraria;

namespace DeveloperTools.Modules.ItemBrowser.UserInterfaces;

/// <summary>
/// 物品浏览器 物品槽
/// </summary>
public class UIIbSlot : SUIItemSlot
{
    public readonly ItemCategory ItemCategory;
    public readonly int ItemIndex;

    public UIIbSlot(ItemCategory itemCategory, int index)
    {
        ItemIndex = index;
        ItemCategory = itemCategory;

        ItemScale = 0.95f;

        BorderColor = SUIColor.Border * 0.75f;
        BackgroundColor = SUIColor.Background * 0.5f;

        BorderRadius = new Vector4(8f);
        SetSize(48f, 48f);
        SetMaxWidth(54f);

        ItemInside = new Item(itemCategory.Items[ItemIndex].type);

        ItemInteractive = false;
    }

    public override void OnLeftMouseDown(SilkyUIFramework.UIMouseEvent evt)
    {
        base.OnLeftMouseDown(evt);

        Player.OpenInventory();

        if (Main.mouseItem is not { IsAir: true }) return;

        Main.mouseItem = new Item(Item.type);
        Main.mouseItem.stack = Main.mouseItem.maxStack;
    }

    public override void OnRightMouseDown(SilkyUIFramework.UIMouseEvent evt)
    {
        base.OnRightMouseDown(evt);

        Player.OpenInventory();

        if (Main.mouseItem.type == Item.type)
        {
            Main.mouseItem.stack++;
            return;
        }

        if (Main.mouseItem is not { IsAir: true }) return;
        Main.mouseItem = new Item(Item.type);
    }

    protected override void UpdateStatus(GameTime gameTime)
    {
        base.UpdateStatus(gameTime);
    }
}
