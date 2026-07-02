using Microsoft.Xna.Framework;
using SilkyUIFramework.Layout;
using Terraria;

namespace DeveloperTools.Modules.ItemBrowser.UserInterfaces.Components;

/// <summary>
/// 分类组
/// </summary>
public partial class UICategoryGroup : UIElementGroup
{
    public UICategoryGroup()
    {
        InitializeComponent();

        var count = Main.rand.Next(1, 6);
        for (int i = 0; i < count; i++)
        {
            new UIItemCategory().Join(ContentContainer);
        }
    }
}

public class UIItemCategory : UIElementGroup
{
    public UIItemCategory()
    {
        SetPadding(8, 0);
        Height = new Dimension(30);

        CrossAlignment = CrossAlignment.Center;
        CrossContentAlignment = CrossContentAlignment.Center;

        BorderRadius = new Vector4(4f);
        BackgroundColor = Color.Black * 0.25f;

        new UITextView()
        {
            Text = "example",
            TextScale = 0.8f
        }.Join(this);
    }
}