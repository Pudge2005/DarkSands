using System.IO;

namespace DevourDev.ProgressSaving
{
    public interface ISavable
    {
        void Save(BinaryWriter bw);

        void Load(BinaryReader br);
    }
}
