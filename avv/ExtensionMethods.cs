using AV;
using KaiwaProjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AV
{
    public static class ExtensionMethods
    {
        public static string GetAllAlbumNamesAsString(this ph pht, string delem)
        {
            if (pht.als == null || pht.als.Count <= 0)
            {
                return string.Empty;
            }
            else
            {
                string strAls = string.Empty;
                foreach (al alm in pht.als)
                {
                    if(string.IsNullOrEmpty(alm.name))
                    strAls += (alm.name + delem);
                }

                return strAls;
            }
        }

        public static void UpdatePh(this ph phh)
        {
            string strSql = "update ph set infoTags = '" + phh.infoTags + "' where id = '" + phh.id + "'";
            try
            {
                DBExecutor.ExecuteCommand(strSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Photo> GetAllPhsWithFullPath(this al alm)
        {
            List<Photo> phs = new List<Photo>();

            foreach (ph phh in alm.phs)
            {
                phs.Add(new Photo(phh.id,phh.name,phh.description, phh.path, phh.infoTags));
            }

            return phs;
        }
    }
}
