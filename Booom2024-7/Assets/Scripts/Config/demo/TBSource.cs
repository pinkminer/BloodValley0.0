
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
public partial class TbSource
{
    private readonly System.Collections.Generic.Dictionary<int, demo.Source> _dataMap;
    private readonly System.Collections.Generic.List<demo.Source> _dataList;
    
    public TbSource(JSONNode _buf)
    {
        _dataMap = new System.Collections.Generic.Dictionary<int, demo.Source>();
        _dataList = new System.Collections.Generic.List<demo.Source>();
        
        foreach(JSONNode _ele in _buf.Children)
        {
            demo.Source _v;
            { if(!_ele.IsObject) { throw new SerializationException(); }  _v = demo.Source.DeserializeSource(_ele);  }
            _dataList.Add(_v);
            _dataMap.Add(_v.Id, _v);
        }
    }

    public System.Collections.Generic.Dictionary<int, demo.Source> DataMap => _dataMap;
    public System.Collections.Generic.List<demo.Source> DataList => _dataList;

    public demo.Source GetOrDefault(int key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public demo.Source Get(int key) => _dataMap[key];
    public int GetCount() => _dataMap.Count;
    public demo.Source this[int key] => _dataMap[key];

    public void ResolveRef(Tables tables)
    {
        foreach(var _v in _dataList)
        {
            _v.ResolveRef(tables);
        }
    }

}

}

