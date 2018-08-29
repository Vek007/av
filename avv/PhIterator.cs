using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace AV
{
    public static class PhIterator
    {
        public static void IterateAndSave(string rootPath)
        {
            string[] allFiles = Directory.EnumerateFiles(rootPath, "*.JPG", SearchOption.AllDirectories).ToArray();

            foreach (string filename in allFiles)
            {

                List<string> als = GetFoldersNames(Path.GetDirectoryName(filename));

                foreach (string al in als)
                {
                    Data.AddAl(al);
                }

                Ph p = new Ph();
                p.Id = Path.GetFileNameWithoutExtension(filename);
                p.FilePath = filename;
                p.Name = p.Id;

                FileInfo fi = new FileInfo(filename);
                p.CreationDate = fi.LastWriteTime;

                GetMeta(fi, ref p);

                Data.AddPh(p);
                foreach (string al in als)
                {
                    Data.AddAP(al,p.Id);
                }
            }
        }

        public static void GetMeta(FileInfo f, ref Ph p)
        {
            using (FileStream fs = new FileStream(f.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BitmapSource img = BitmapFrame.Create(fs);
                BitmapMetadata md = (BitmapMetadata)img.Metadata;
                p.CreationDate = md.DateTaken == null ? f.CreationTime : DateTime.Parse(md.DateTaken);
                p.Description = md.Subject;
            }
        }

        public static List<string> GetFoldersNames(string path)
        {
            string[] flds = path.Split('\\');
            List<string> tmp =  flds.ToList();
            tmp.RemoveAt(0);
            tmp.RemoveAt(0);
            return tmp;
        }
    }
}
