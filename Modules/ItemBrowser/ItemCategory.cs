using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DeveloperTools.Modules.ItemBrowser;

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
    public virtual bool Filters(Item item) => true;

    public void UpdateItems(ReadOnlySpan<Item> items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            var item = items[i];
            if (!Filters(item)) continue;
            Items.Add(item);
        }
    }

    public bool Equals(ItemCategory other) => Name.Equals(other.Name);

    public override bool Equals(object obj) => obj is ItemCategory other && Equals(other);

    public override int GetHashCode() => Name.GetHashCode();
}

public class ModItemCategory(Mod mod, Mod targetMod, string name) : ItemCategory(mod, name)
{
    public Mod TargetMod { get; } = targetMod;
    public override string DisplayName => TargetMod.DisplayName;
    public override string Description => $"{TargetMod.DisplayName}";

    public override bool Filters(Item item) => item.ModItem?.Mod == TargetMod;
}

public class SimpleItemCategory(Mod mod, string name, Func<Item, bool> filter) : ItemCategory(mod, name)
{
    public Func<Item, bool> Filter = filter;
    public sealed override bool Filters(Item item) => Filter?.Invoke(item) ?? false;
}