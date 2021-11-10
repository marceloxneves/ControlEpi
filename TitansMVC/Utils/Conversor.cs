using System;
using System.Data.Entity.Infrastructure;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace TitansMVC.Utils
{
    public class Conversor
    {
        public byte[] ConvertToByte(HttpPostedFileBase file)
        {
            byte[] imageByte = null;
            BinaryReader rdr = new BinaryReader(file.InputStream);
            imageByte = rdr.ReadBytes((int)file.ContentLength);
            return imageByte;
        }

        public byte[] StringToByte(string path, string base64)
        {
            Bitmap bmp;
            byte[] retorno;
            //MemoryStream com o base64 recebido por parâmetro
            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(base64)))
            {
                //Criar um novo Bitmap baseado na MemoryStream
                using (bmp = new Bitmap(ms))
                {                   
                    //Salvar a imagem no formato PNG
                    //bmp.Save(path, ImageFormat.Png);

                    ImageConverter converter = new ImageConverter();
                    retorno = (byte[])converter.ConvertTo(bmp, typeof(byte[]));

                    //_colaboradorRepository.Update(colaborador);
                }
            }

            return retorno;
        }

        public DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();

            // column names
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }
                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) ?? DBNull.Value;
                }

                dtReturn.Rows.Add(dr);
            }

            return dtReturn;
        }
    }
}