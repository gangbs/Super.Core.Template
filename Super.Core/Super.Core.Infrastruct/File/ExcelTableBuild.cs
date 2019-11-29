using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Super.Core.Infrastruct.File
{
   public class ExcelTableBuild
    {
        public static HSSFWorkbook GenTableWithData<T>(List<T> lstData, string[] propertyNames)
        {
            Type tp = typeof(T);

            List<PropertyInfo> lstProperty = new List<PropertyInfo>();
            foreach (var name in propertyNames)
            {
                var p = tp.GetProperty(name);
                if (p != null) lstProperty.Add(p);
            }

            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();

            //创建表头
            IRow headRow = sheet.CreateRow(0);
            for (int i = 0; i < lstProperty.Count(); i++)
            {
                var p = lstProperty[i];
                string headName = p.Name;
                var attr = p.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault();
                if (attr != null)
                {
                    headName = ((DisplayNameAttribute)attr).DisplayName;
                }
                headRow.CreateCell(i).SetCellValue(headName);
            }

            //填充数据
            for (int i = 0; i < lstData.Count; i++)
            {
                IRow dataRow = sheet.CreateRow(i + 1);
                for (int j = 0; j < lstProperty.Count(); j++)
                {
                    var p = lstProperty[j];
                    object cellVal = p.GetValue(lstData[i]);
                    string strCellVal = cellVal != null ? cellVal.ToString() : null;
                    dataRow.CreateCell(j).SetCellValue(strCellVal);
                }
            }
            return workbook;
        }

        public static HSSFWorkbook GenTableWithData<T>(List<T> lstData)
        {
            Type tp = typeof(T);
            var arrPro = tp.GetProperties();

            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();

            //创建表头
            IRow headRow = sheet.CreateRow(0);
            for (int i = 0; i < arrPro.Count(); i++)
            {
                var p = arrPro[i];
                string headName = p.Name;
                var attr = p.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault();
                if (attr != null)
                {
                    headName = ((DisplayNameAttribute)attr).DisplayName;
                }
                headRow.CreateCell(i).SetCellValue(headName);
            }

            //填充数据
            for (int i = 0; i < lstData.Count; i++)
            {
                IRow dataRow = sheet.CreateRow(i + 1);
                for (int j = 0; j < arrPro.Count(); j++)
                {
                    var p = arrPro[j];
                    object cellVal = p.GetValue(lstData[i]);
                    string strCellVal = cellVal != null ? cellVal.ToString() : null;
                    dataRow.CreateCell(j).SetCellValue(strCellVal);
                }
            }

            return workbook;
        }

        public static byte[] Workbook2Byte(HSSFWorkbook workbook)
        {
            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);

            byte[] fileByte = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(fileByte, 0, fileByte.Length);

            return fileByte;
        }
    }
}
