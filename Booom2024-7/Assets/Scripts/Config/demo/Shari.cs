
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;
using SimpleJSON;


namespace cfg.demo
{
public sealed partial class Shari : Luban.BeanBase
{
    public Shari(JSONNode _buf) 
    {
        { if(!_buf["id"].IsNumber) { throw new SerializationException(); }  Id = _buf["id"]; }
        { if(!_buf["name"].IsString) { throw new SerializationException(); }  Name = _buf["name"]; }
        { if(!_buf["nameEn"].IsString) { throw new SerializationException(); }  NameEn = _buf["nameEn"]; }
        { if(!_buf["description"].IsString) { throw new SerializationException(); }  Description = _buf["description"]; }
        { if(!_buf["price"].IsNumber) { throw new SerializationException(); }  Price = _buf["price"]; }
        { var __json0 = _buf["color"]; if(!__json0.IsArray) { throw new SerializationException(); } int _n0 = __json0.Count; Color = new int[_n0]; int __index0=0; foreach(JSONNode __e0 in __json0.Children) { int __v0;  { if(!__e0.IsNumber) { throw new SerializationException(); }  __v0 = __e0; }  Color[__index0++] = __v0; }   }
        { var __json0 = _buf["flavor"]; if(!__json0.IsArray) { throw new SerializationException(); } int _n0 = __json0.Count; Flavor = new int[_n0]; int __index0=0; foreach(JSONNode __e0 in __json0.Children) { int __v0;  { if(!__e0.IsNumber) { throw new SerializationException(); }  __v0 = __e0; }  Flavor[__index0++] = __v0; }   }
        { var __json0 = _buf["taste"]; if(!__json0.IsArray) { throw new SerializationException(); } int _n0 = __json0.Count; Taste = new int[_n0]; int __index0=0; foreach(JSONNode __e0 in __json0.Children) { int __v0;  { if(!__e0.IsNumber) { throw new SerializationException(); }  __v0 = __e0; }  Taste[__index0++] = __v0; }   }
        { if(!_buf["normality"].IsNumber) { throw new SerializationException(); }  Normality = _buf["normality"]; }
        { if(!_buf["ingredient"].IsString) { throw new SerializationException(); }  Ingredient = _buf["ingredient"]; }
        { if(!_buf["size"].IsNumber) { throw new SerializationException(); }  Size = _buf["size"]; }
        { if(!_buf["tidyness"].IsNumber) { throw new SerializationException(); }  Tidyness = _buf["tidyness"]; }
    }

    public static Shari DeserializeShari(JSONNode _buf)
    {
        return new demo.Shari(_buf);
    }

    /// <summary>
    /// 序号
    /// </summary>
    public readonly int Id;
    /// <summary>
    /// 名字
    /// </summary>
    public readonly string Name;
    /// <summary>
    /// 英文名
    /// </summary>
    public readonly string NameEn;
    /// <summary>
    /// 介绍
    /// </summary>
    public readonly string Description;
    /// <summary>
    /// 价格
    /// </summary>
    public readonly int Price;
    /// <summary>
    /// 白
    /// </summary>
    public readonly int[] Color;
    /// <summary>
    /// 酸
    /// </summary>
    public readonly int[] Flavor;
    /// <summary>
    /// 弹性
    /// </summary>
    public readonly int[] Taste;
    /// <summary>
    /// 正常度
    /// </summary>
    public readonly int Normality;
    /// <summary>
    /// 食材类型
    /// </summary>
    public readonly string Ingredient;
    /// <summary>
    /// 分量
    /// </summary>
    public readonly int Size;
    /// <summary>
    /// 整洁度
    /// </summary>
    public readonly int Tidyness;
   
    public const int __ID__ = 1772426232;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(Tables tables)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "id:" + Id + ","
        + "name:" + Name + ","
        + "nameEn:" + NameEn + ","
        + "description:" + Description + ","
        + "price:" + Price + ","
        + "color:" + Luban.StringUtil.CollectionToString(Color) + ","
        + "flavor:" + Luban.StringUtil.CollectionToString(Flavor) + ","
        + "taste:" + Luban.StringUtil.CollectionToString(Taste) + ","
        + "normality:" + Normality + ","
        + "ingredient:" + Ingredient + ","
        + "size:" + Size + ","
        + "tidyness:" + Tidyness + ","
        + "}";
    }
}

}

