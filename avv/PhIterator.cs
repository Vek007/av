using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace AV
{
    public static class PhIterator
    {
        public static void IterateAndSave(string rootPath, Main parent)
        {
            string[] allFiles = Directory.EnumerateFiles(rootPath, "*.JPG", SearchOption.AllDirectories).ToArray();

            if (parent != null)
            {
                parent.Invoke((MethodInvoker)(() =>
                {
                    parent.ShowProgressBar(true);
                }));
            }

            foreach (string filename in allFiles)
            {

                List<string> als = GetFoldersNames(Path.GetDirectoryName(filename));

                ph p = new ph();
                p.id = Path.GetFileNameWithoutExtension(filename);
                p.path = filename;
                p.name = p.id;

                FileInfo fi = new FileInfo(filename);
                p.time_stamp = fi.LastWriteTime;

                GetMeta(fi, ref p);

                if (!Data.ExistsAsRecord(p))
                    Data.AddPh(p);
                else
                    Data.AddPhAsDup(p);
            }

            if (parent != null)
            {
                parent.Invoke((MethodInvoker)(() =>
                {
                    parent.ShowProgressBar(false);
                }));

                if (MessageBox.Show("Do you want to refresh the Album tree?", "Refresh", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    parent.Invoke((MethodInvoker)(() =>
                    {
                        parent.RefreshTree();
                    }));
                }
            }

        }

        public static void GetMeta(FileInfo f, ref ph p)
        {
            using (FileStream fs = new FileStream(f.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BitmapSource img = BitmapFrame.Create(fs);
                BitmapMetadata md = (BitmapMetadata)img.Metadata;
                p.time_stamp = md.DateTaken == null ? f.CreationTime : DateTime.Parse(md.DateTaken);
                p.description = md.Subject;
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
