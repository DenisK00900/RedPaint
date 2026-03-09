using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public interface IFileBrowser
    {
        public string currDir { get; set; }

        public void UpdateListInfo(string ch = "", bool setMode = false);

        public void FolderUp();
    }
}
