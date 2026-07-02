using Microsoft.Xna.Framework;
using Terraria.ModLoader.UI;

namespace DeveloperTools.Modules.ItemBrowser.UserInterfaces.Components;

/// <summary>
/// 物品浏览器 分类按钮
/// </summary>
public class ItemBrowserCategoryButton : UIElementGroup
{
    public ItemCategory ItemCategory { get; }
    public UITextView NameView { get; set; }

    public ItemBrowserCategoryButton(ItemCategory itemCategory)
    {
        ItemCategory = itemCategory;
        FitWidth = false;
        FitHeight = true;
        SetWidth(0f, 1f);
        SetPadding(0f, 8f);

        NameView = new UITextView
        {
            FitWidth = false,
            WordWrap = true,
            Text = $"{itemCategory.DisplayName}",
            TextAlign = new Vector2(0.5f),
            TextScale = 0.8f,
        }.Join(this);
        Selected += (_) => NameView.TextColor = SUIColor.Highlight;
        Deselected += (_) => NameView.TextColor = Color.White;
        NameView.SetWidth(0f, 1f);
    }

    protected override void UpdateStatus(GameTime gameTime)
    {
        base.UpdateStatus(gameTime);

        BackgroundColor = Color.Black * HoverTimer.Lerp(0f, 0.2f);

        if (!IsMouseHovering) return;
        UICommon.TooltipMouseText(ItemCategory.Description);
    }
}
