using System.Linq;
using Microsoft.Xna.Framework;
using SilkyUIFramework.Layout;

namespace DeveloperTools.Modules.ItemBrowser.UserInterfaces;

[RegisterUI("Vanilla: Radial Hotbars", "Builder: ItemBrowserUI")]
public partial class ItemBrowserUI : BaseBody
{
    public SUIScrollView ItemCategoryList { get; private set; }
    public SUIScrollView ItemTable { get; private set; }

    protected override void OnInitialize()
    {
        InitializeComponent();

        // 初始化样式
        MainPanel.BorderRadius = new Vector4(8f);
        MainPanel.BorderColor = SUIColor.Border * 0.75f;
        MainPanel.BackgroundColor = SUIColor.Background * 0.5f;

        // 物品分类列表
        ItemCategoryList = new SUIScrollView
        {
            Gap = new Size(4f),
            Padding = new Margin(4f),
            Mask = {
                BorderRadius = new Vector4(4f),
                Border = 2f,
                BorderColor = Color.Black * 0.75f,
                BackgroundColor = SUIColor.Background * 0.25f,
            },
            Container =
            {
                Gap = Size.Zero,
                FlexWrap = false,
                FitHeight =true,
                FitWidth = false,
                FlexDirection = FlexDirection.Column,
                MainAlignment = MainAlignment.Start,
                CrossAlignment = CrossAlignment.Start,
            },
        }.Join(ContentContainer);
        ItemCategoryList.SetHeight(0f, 1f);
        ItemCategoryList.SetWidth(180f);

        // ItemBrowser 简称 ib
        // 从 system 获取分类列表
        var ibSystem = ItemBrowserSystem.Instance;
        var itemCategories = ibSystem.ItemCategories.Values.ToArray();

        for (int i = 0; i < itemCategories.Length; i++)
        {
            var itemCategory = itemCategories[i];
            if (itemCategory.Items.Count == 0) continue;

            var textView = new UIIbCategory(itemCategory);
            textView.LeftMouseDown += (_, _) => UpdateItemTable(itemCategory);

            if (i != 0) SUIDividingLine.Horizontal(Color.Black * 0.75f).Join(ItemCategoryList.Container);

            textView.Join(ItemCategoryList.Container);
        }

        SUIDividingLine.Vertical(Color.Black * 0.75f).Join(ContentContainer);

        ItemTable = new SUIScrollView
        {
            Gap = new Size(4f),
            Padding = new Margin(4f),
            FlexGrow = 1f,
            Container =
            {
                FlexWrap = true,
                MainAlignment = MainAlignment.Start,
            },
        }.Join(ContentContainer);
        ItemTable.SetHeight(0f, 1f);

        UpdateItemTable(itemCategories[0]);
    }

    private void UpdateItemTable(ItemCategory itemCategory)
    {
        var items = itemCategory.Items;

        ItemTable.Container.RemoveAllChildren();

        for (int i = 0; i < items.Count; i++)
        {
            new UIIbSlot(itemCategory, i).Join(ItemTable.Container);
        }
    }
}
