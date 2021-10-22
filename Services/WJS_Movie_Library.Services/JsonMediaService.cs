using System.Globalization;
using System;
using System.Collections.Generic;
using System.IO;
using WJS_Movie_Library.Model;
using WJS_Movie_Library.Shared;
using Newtonsoft.Json;

namespace WJS_Movie_Library.Services
{
    public class JsonMediaService<T> : IMediaService where T : MediaBase, new()
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private static string _extension = ".json";

        private IList<MediaBase> _mediaList;
        private IList<MediaBase> _newMedia;
        private bool _newFile = false;
        private string _fName;

        public JsonMediaService()
        {
            _mediaList = new List<MediaBase>();
            _newMedia = new List<MediaBase>();
            _newFile = false;
            _fName = "";
        }

        public JsonMediaService(string fname)
        {
            _fName = BuildDataFileName.BuildDataFilePath(fname, _extension);

            if (File.Exists(_fName))
            {
                _newFile = false;
                _mediaList = this.LoadFromJSON(_fName);
            }
            else
            {
                _newFile = true;
                _mediaList = new List<MediaBase>();
            }
            _newMedia = new List<MediaBase>();
        }

        public IList<MediaBase> LoadFromJSON(string fname)
        {
            IList<MediaBase> mediaList = new List<MediaBase>();

            using (var sr = new StreamReader(fname))
            {
                var serializer = new JsonSerializer();
                try
                {
                    using (var reader = new JsonTextReader(sr))
                    {
                        T record;
                        do
                        {
                            record = serializer.Deserialize<T>(reader);
                            mediaList.Add(record);
                        } while (record != null);
                    }
                }
                catch (Exception ex)
                {
                    // Log the Exception
                    log.Error($"Problem Reading Data File {fname}.  EX: {ex}");
                }
            }

            return mediaList;
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
            if (_newMedia.Count > 0)
                return true;
            else
                return false;
        }

        public void Save()
        {
            if (NeedSave())
            {
                Save(_mediaList);
            }
        }

        public void Save(IList<MediaBase> mediaList)
        {
            JsonSerializer serializer = JsonSerializer.Create();

            using (var sw = new StreamWriter(_fName, false))
            {
                using (JsonTextWriter jw = new JsonTextWriter(sw))
                {
                    foreach (T record in mediaList)
                    {
                        serializer.Serialize(jw, record);
                    }
                }
            }
        }

        public UInt64 GetMaxId()
        {
            UInt64 ret = 0;

            foreach (T record in _mediaList)
            {
                if (record.Id > ret) ret = record.Id;
            }

            return ret;
        }

        public bool Exists(string title)
        {
            bool result = false;
            string uComp = title.ToUpper();

            foreach (T record in _mediaList)
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
