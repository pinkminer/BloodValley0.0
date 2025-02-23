
using System.IO;
using SimpleJSON;

public class CFG
{
    private static CFG _instance;

    public static CFG Ins()
    {
        if (_instance == null)
        {
            _instance = new CFG();
        }
        return _instance;
    }

    public readonly cfg.Tables Tables;

    CFG()
    {
        string gameConfDir = "Assets\\Scripts\\DataConfig\\JsonData"; // 替换为gen.bat中outputDataDir指向的目录
        Tables = new cfg.Tables(file => JSON.Parse(File.ReadAllText($"{gameConfDir}/{file}.json")));
    }
}