using System;

namespace Tempest.Save
{
    /// <summary>
    /// Metadata about a save file for UI display (slot selector, load menu, etc).
    /// </summary>
    public class SaveFileInfo
    {
        public string slotName;
        public string path;
        public long size;
        public DateTime lastModified;

        public SaveFileInfo(string slotName, string path, long size, DateTime lastModified)
        {
            this.slotName = slotName;
            this.path = path;
            this.size = size;
            this.lastModified = lastModified;
        }

        public override string ToString()
        {
            return $"{slotName} ({size} bytes, {lastModified:yyyy-MM-dd HH:mm:ss})";
        }
    }
}
