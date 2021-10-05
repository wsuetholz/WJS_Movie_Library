using System.Globalization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using CsvHelper;
using WJS_Movie_Library.Model;
using CsvHelper.Configuration;
using WJS_Movie_Library.Model.Interfaces;

namespace WJS_Movie_Library.Services
{
    public class CsvMediaService<T> : IMediaService where T: MediaBase, new()
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        private IList<MediaBase> _mediaList;
        private IList<MediaBase> _newMedia;
        private bool _newFile = false;
        private string _fName;

        public CsvMediaService()
        {
            _mediaList = new List<MediaBase>();
            _newMedia = new List<MediaBase>();
            _newFile = false;
            _fName = "";
        }

        public CsvMediaService(string fname)
        {
            _fName = fname;

            if (File.Exists(fname))
            {
                _newFile = false;
                _mediaList = this.LoadFromCSV(fname);
            }
            else
            {
                _newFile = true;
                _mediaList = new List<MediaBase>();
            }
            _newMedia = new List<MediaBase>();
        }

        public IList<MediaBase> LoadFromCSV(string fname)
        {
            IList<MediaBase> mediaList = new List<MediaBase>();
            T record = new();

            using (var reader = new StreamReader(fname))
            {
                try
                {
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        record.RegisterCSVMap(csv.Context);     // Should really be a static function.....

                        var records = csv.GetRecords<T>();
                        mediaList = new List<MediaBase>(records);
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
                T rec;
                if (_newMedia.Count > 0)
                {
                    rec = (T)_newMedia[0];
                }
                else
                {
                    rec = new();
                }
                using (var sw = new StreamWriter(_fName, true))
                {
                    using (CsvWriter cw = new CsvWriter(sw, CultureInfo.InvariantCulture))
                    {
                        rec.RegisterCSVMap(cw.Context);     // Should really be a static function.....

                        if (_newFile)
                        {
                            cw.WriteHeader<T>();
                            cw.NextRecord();
                        }

                        // !!TODO!!
                        // Should check if last line in file is a record or a blank line..  If it's a record should do a NextRecord() otherwise will append to that record..

                        foreach (T record in _newMedia)
                        {
                            cw.WriteRecord<T>(record);
                            cw.NextRecord();
                        }
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