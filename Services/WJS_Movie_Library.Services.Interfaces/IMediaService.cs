using System.Collections.Generic;
using WJS_Movie_Library.Model;
using WJS_Movie_Library.Model.Interfaces;

namespace WJS_Movie_Library.Services
{
    public interface IMediaService
    {
        void Add(MediaBase mediaRecord);
        bool Exists(string title);
        ulong GetMaxId();
        IList<MediaBase> GetMediaList();
        IList<MediaBase> GetRangeOfMedia(int startIndex, int length);
        bool NeedSave();
        void Save();
    }
}