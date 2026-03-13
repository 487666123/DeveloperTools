namespace DeveloperTools.UserInterfaces;

[RegisterUI]
public partial class MainController : BaseBody
{
    protected override void OnInitialize()
    {
        InitializeComponent();

        MainPanel.Border = 2;
        MainPanel.BorderColor = SUIColor.Border;
        MainPanel.BackgroundColor = SUIColor.Background;
    }
}
