namespace Lynx.Models;

// Structu readonly: lives in the Stack, zero fragmentation
public readonly struct FileRecord
{
    public readonly string Name;
    public readonly int PathId;
    public readonly long Size;

    public FileRecord(string name, int pathId, long size)
    {
        Name = name;
        PathId = pathId;
        Size = size;
    }
}