using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercator.GIS.Map.Tools
{
    public class Shape
    {
        public IntPtr DBFHandle
        {
            get
            {
                return _HDBF;
            }
        }
        private IntPtr _HDBF;

        public IntPtr SHPHandle
        {
            get
            {
                return _HSHP;
            }
        }
        private IntPtr _HSHP;

        public string FileName
        {
            get
            {
                return _FileName;
            }
            set
            {
                _FileName = value;
                Open(_FileName);
            }
        }
        private string _FileName;

        private bool _Opened = false;

        public Shape(string fileName)
        {
            Open(_FileName);
        }

        ~Shape()
        {
            Close();
        }

        public bool Open(string fileName)
        {
            Close();

            if (string.IsNullOrEmpty(fileName)) { return false; }

            var dbfFileName = string.Format(@"{0}.dbf", fileName.Substring(0, fileName.LastIndexOf(@"\.")));

            if (!File.Exists(fileName) || !File.Exists(dbfFileName)) { return false; }
            
            _HSHP = Shapelib.SHPOpen(fileName, "rb+");
            _HDBF = Shapelib.DBFOpen(dbfFileName, "rb+");

            if(null == _HSHP || null == _HDBF) { return false; }

            _Opened = true;

            return true;
        }

        public void Close()
        {
            if (_Opened)
            {
                Shapelib.SHPClose(_HSHP);
                Shapelib.DBFClose(_HDBF);

                _Opened = false;
            }
        }

        public bool Create(string fileName,Shapelib.ShapeType shapeType)
        {
            var dbfFileName = string.Format(@"{0}.dbf", fileName.Substring(0, fileName.LastIndexOf(@"\.")));

            _HSHP = Shapelib.SHPCreate(fileName, shapeType);
            _HDBF = Shapelib.DBFCreate(dbfFileName);

            if (null == _HSHP || null == _HDBF) { return false; }

            return true;
        }
    }
}
