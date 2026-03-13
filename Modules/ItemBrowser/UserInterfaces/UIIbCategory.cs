using Microsoft.Xna.Framework;
using Terraria;

namespace DeveloperTools.Modules.ItemBrowser.UserInterfaces;

/// <summary>
/// 物品浏览器 分类按钮
/// </summary>
public class UIIbCategory : UIElementGroup
{
    public ItemCategory ItemCategory { get; }
    public UITextView NameTextView { get; set; }

    public UIIbCategory(ItemCategory itemCategory)
    {
        ItemCategory = itemCategory;
        FitWidth = false;
        FitHeight = true;
        SetWidth(0f, 1f);
        SetPadding(0f, 6f);

        NameTextView = new UITextView
        {
            FitWidth = false,
            WordWrap = true,
            Text = $"{itemCategory.DisplayName}",
            TextAlign = new Vector2(0.5f),
            TextScale = 0.8f,
        }.Join(this);
        Selected += (_) => NameTextView.TextColor = SUIColor.Highlight;
        Deselected += (_) => NameTextView.TextColor = Color.White;
        NameTextView.SetWidth(0f, 1f);
    }

    protected override void UpdateStatus(GameTime gameTime)
    {
        base.UpdateStatus(gameTime);

        BackgroundColor = Color.Black * HoverTimer.Lerp(IsSelected ? 0.2f : 0.1f, IsSelected ? 0.3f : 0.2f);

        if (IsMouseHovering)
        {
            Main.hoverItemName = ItemCategory.Description;
        }
    }
}
