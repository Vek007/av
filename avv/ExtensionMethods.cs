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

        public static Photo GetPhoto(this ph p)
        {
            return new Photo(p.id, p.name, p.description, p.path, p.infoTags);
        }
    }
}
