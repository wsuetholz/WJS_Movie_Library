using System.Globalization;
using System;
using System.Collections.Generic;
using System.IO;
using WJS_Movie_Library.Model;
using WJS_Movie_Library.Shared;
using System.Text.Json;
using WJS_Movie_Library.Model.Interfaces;

namespace WJS_Movie_Library.Services
{
    public class TempMediaService : IMediaService
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        private IList<MediaBase> _mediaList;
        private IList<MediaBase> _newMedia;
        private bool _newFile = false;
        private string _fName;

        public class Media
        {
            public IList<IMedia> MediaList { get; set; }

            public Media(IList<IMedia>mediaList)
            {
                MediaList = (IList<IMedia>)mediaList;
            }

            public Media(IList<MediaBase> mediaList)
            {
                MediaList = (IList<IMedia>)mediaList;
            }

            public Media()
            {
                MediaList = new List<IMedia>();
            }
        }

        public TempMediaService()
        {
            _mediaList = new List<MediaBase>();
            _newMedia = new List<MediaBase>();
            _newFile = false;
            _fName = "";
        }

        public TempMediaService(IList<MediaBase>mediaList)
        {
            _newFile = false;
            _mediaList = mediaList;
            _newMedia = new List<MediaBase>();
        }

        public IList<MediaBase> GetMediaList()
        {
            return _mediaList;
        }

        public IList<MediaBase> GetRangeOfMedia(int startIndex, int length)
        {
            IList<MediaBase> mediaList = new List<MediaBase>();

            for (int idx = startIndex; idx < (startIndex + length); idx++)
            {
                if (idx >= _mediaList.Count)
                    break;
                mediaList.Add(_mediaList[idx]);
            }

            return mediaList;
        }

        public void Add(MediaBase mediaRecord)
        {
            _mediaList.Add(mediaRecord);
            _newMedia.Add(mediaRecord);
        }

        public bool NeedSave()
        {
            return false;
        }

        public void Save()
        {
        }

        public void Save(IList<MediaBase> mediaList)
        {
        }

        public UInt64 GetMaxId()
        {
            UInt64 ret = 0;

            foreach (MediaBase record in _mediaList)
            {
                if (record.Id > ret) ret = record.Id;
            }

            return ret;
        }

        public bool Exists(string title)
        {
            bool result = false;
            string uComp = title.ToUpper();

            foreach (IMedia record in _mediaList)
            {
                string uTitle = record.Title.ToUpper();

                if (uTitle.Equals(uComp))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

    }
}
