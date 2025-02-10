using Luban;
using SimpleJSON;
using System.Collections.Generic;

namespace cfg.demo
{
    public abstract class TbBase<T> where T : class
    {
        protected Dictionary<int, T> _dataMap;
        protected List<T> _dataList;

        protected TbBase(JSONNode _buf)
        {
            _dataMap = new Dictionary<int, T>();
            _dataList = new List<T>();
            
            foreach(JSONNode _ele in _buf.Children)
            {
                if(!_ele.IsObject) { throw new SerializationException(); }
                T _v = Deserialize(_ele);
                _dataList.Add(_v);
                _dataMap.Add(GetId(_v), _v);
            }
        }

        // 抽象方法，让子类实现具体的反序列化逻辑
        protected abstract T Deserialize(JSONNode _ele);

        // 抽象方法，让子类实现获取ID的方法
        protected abstract int GetId(T item);

        // 共享方法
        public Dictionary<int, T> DataMap => _dataMap;
        public List<T> DataList => _dataList;

        public T GetOrDefault(int key) => _dataMap.TryGetValue(key, out var v) ? v : null;
        public T Get(int key) => _dataMap[key];
        public int GetCount() => _dataMap.Count;
        public T this[int key] => _dataMap[key];

        // public virtual void ResolveRef(Tables tables)
        // {
        //     foreach(var _v in _dataList)
        //     {
        //         ResolveRefItem(_v, tables);
        //     }
        // }

        // 抽象方法，让子类实现具体的引用解析逻辑
        //protected abstract void ResolveRefItem(T item, Tables tables);
    }
}