using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DeveloperTools.Modules.ItemBrowser;

public class ItemCategoryGroup
{
    public IReadOnlyList<ItemCategory> Categories => _categories;
    private readonly List<ItemCategory> _categories = [];

    public event EventHandler<ItemCategory> OnGroupChanged;

    /// <summary>
    /// 能加不能减
    /// </summary>
    public void AddItemCategory(ItemCategory category)
    {
        _categories.Add(category);
        OnGroupChanged?.Invoke(this, category);
    }
}

/// <summary>
/// 物品浏览器的分类<br/>
/// 对开发者自定义物品分类提供支持<br/>
/// 此类用于记录分类的：名字、描述、筛选器<br/>
/// 并且会缓存筛选之后的物品 <see cref="Items"/><br/>
/// 目前是加载时候确定此分类下有什么物品，后续可能会添加一个游戏内刷新分类的功能<br/>
/// HERO's 的物品浏览器是在加载时缓存全程不变的，有些情况下会带来不方便，所以这里要解决<br/>
/// 目前UI上的设计是不带图标的<br/>
/// </summary>
public class ItemCategory : IEquatable<ItemCategory>
{
    /// <summary>
    /// 模组由谁添加的
    /// </summary>
    public Mod Mod { get; }

    /// <summary>
    /// 分类的内部名, 用于冲突检测, 不可相同
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// 显示名称
    /// </summary>
    public virtual string DisplayName => Language.GetText($"Mods.{Mod.Name}.ItemCategories.{Name}.DisplayName").Value;

    /// <summary>
    /// 详细描述
    /// </summary>
    public virtual string Description => Language.GetText($"Mods.{Mod.Name}.ItemCategories.{Name}.Description").Value;

    /// <summary>
    /// 物品缓存
    /// </summary>
    public List<Item> Items { get; } = [];

    public ItemCategory(Mod mod, string name)
    {
        ArgumentNullException.ThrowIfNull(mod);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Mod = mod;
        Name = name;
    }

    /// <summary>
    /// 过滤器
    /// </summary>
    /// <returns>true 代表物品可以出现在这个分类</returns>
    protected virtual bool Matches(Item item) => true;

    private bool _built;
    public void BuildCache(ReadOnlySpan<Item> items)
    {
        if (_built) return; _built = true;

        for (int i = 0; i < items.Length; i++)
        {
            var item = items[i];
            if (!Matches(item)) continue;
            Items.Add(item);
        }
    }

    string MergeName => $"{Mod.Name}.{Name}";

    public bool Equals(ItemCategory other) => MergeName.Equals(other.MergeName);

    public override bool Equals(object obj) => obj is ItemCategory other && Equals(other);

    public override int GetHashCode() => MergeName.GetHashCode();
}

/// <summary>
/// 一个模组对应一个实例
/// </summary>
internal class ItemCategoryFromMod : ItemCategory
{
    public Mod TargetMod { get; }
    public override string DisplayName => TargetMod.DisplayName;
    public override string Description => $"{TargetMod.DisplayName}";

    public ItemCategoryFromMod(Mod mod, Mod targetMod, string name) : base(mod, name)
    {
        ArgumentNullException.ThrowIfNull(targetMod);
        TargetMod = targetMod;
    }

    protected override bool Matches(Item item) => item.ModItem?.Mod == TargetMod;
}

/// <summary>
/// 通过 <see cref="Func{T, TResult}"/> 创建实例
/// </summary>
public class SimpleItemCategory : ItemCategory
{
    private readonly Func<Item, bool> _matches;

    public SimpleItemCategory(Mod mod, string name, Func<Item, bool> matches) : base(mod, name)
    {
        ArgumentNullException.ThrowIfNull(matches);
        _matches = matches;
    }

    protected sealed override bool Matches(Item item) => _matches.Invoke(item);
}