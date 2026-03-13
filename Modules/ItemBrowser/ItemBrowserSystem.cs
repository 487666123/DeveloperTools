using System.Collections.Generic;
using System.Collections.Immutable;
using Terraria;
using Terraria.ModLoader;

namespace DeveloperTools.Modules.ItemBrowser;

/// <summary>
/// 物品浏览器系统
/// </summary>
public class ItemBrowserSystem : ModSystem
{
    public static ItemBrowserSystem Instance => ModContent.GetInstance<ItemBrowserSystem>();

    /// <summary>
    /// 注册的物品分类
    /// </summary>
    private readonly Dictionary<string, ItemCategory> _itemCategories = [];

    /// <summary>
    /// 用于外部只读
    /// </summary>
    public IReadOnlyDictionary<string, ItemCategory> ItemCategories => _itemCategories;

    public ImmutableArray<Item> Items { get; private set; }
    private Item[] _itemCache;

    public override void Load()
    {
        // 全部
        RegisterItemCategory(new ItemCategory(Mod, "All"));

        // 药水
        RegisterItemCategory(new SimpleItemCategory(Mod, "Potions", static (item) =>
        {
            return (item.buffType > 0 && item.buffTime > 0) || item.healLife > 0 || item.healMana > 0;
        }));

        // 武器
        RegisterItemCategory(new SimpleItemCategory(Mod, "Weapon", static (item) =>
        {
            return item.damage > 0;
        }));

        // 近战武器
        RegisterItemCategory(new SimpleItemCategory(Mod, "MeleeWeapon", static (item) =>
        {
            return item.damage > 0 && (item.DamageType == DamageClass.MeleeNoSpeed || item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.SummonMeleeSpeed);
        }));

        // 近战武器
        RegisterItemCategory(new SimpleItemCategory(Mod, "RangeWeapon", static (item) =>
        {
            return item.ammo <= 0 && item.damage > 0 && item.DamageType == DamageClass.Ranged;
        }));

        // 装备
        RegisterItemCategory(new SimpleItemCategory(Mod, "Equip", static (item) =>
        {
            return item.accessory;
        }));

        // 弹药
        RegisterItemCategory(new SimpleItemCategory(Mod, "Ammo", static (item) =>
        {
            return item.ammo > 0;
        }));

        foreach (var mod in ModLoader.Mods)
        {
            // 弹药
            RegisterItemCategory(new ModItemCategory(Mod, mod, $"{mod.Name}.AutoLoad"));
        }
    }

    public bool RegisterItemCategory(ItemCategory itemCategory)
    {
        if (itemCategory is null) return false;
        if (_itemCategories.ContainsKey(itemCategory.Name)) { return false; }

        _itemCategories[itemCategory.Name] = itemCategory;
        return true;
    }

    public override void PostSetupContent()
    {
        _itemCache = new Item[ItemLoader.ItemCount - 1];

        for (int i = 0; i < _itemCache.Length; i++)
        {
            _itemCache[i] = new Item(i + 1);
        }

        Items = [.. _itemCache];

        foreach (var (_, itemCategory) in _itemCategories)
        {
            itemCategory.UpdateItems(Items.AsSpan());
        }
    }
}