using NPOI.HPSF;
using Smart.API.Adapter.BizCore.JD;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using Smart.API.Adapter.Common;

namespace WinTestJD
{
    public partial class LogExport : Form
    {
        public LogExport()
        {
            InitializeComponent();
            LogHelper.RegisterLog4Config(AppDomain.CurrentDomain.BaseDirectory + "\\Config\\Log4net.config");
        }

        private DataTable dtTotal;



        private void btn_Export_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.Description = "请选择保存路径";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    workbook = null;
                    label3.Text = "正在导出...";
                    string foldPath = dialog.SelectedPath;
                    DirectoryInfo theFolder = new DirectoryInfo(foldPath);
                    int totalCount = new VehicleLogSqlBLL().GetCountVehicleLog(startDateTimePicker.Value.Date, endDateTimePicker.Value.Date);
                    if (totalCount > 0)
                    {
                        //int page = totalCount / 1 + 1000;

                        string filePath = theFolder.FullName + "\\log_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                        //for (int i = 0; i < page; i++)
                        //{
                        //    int limitStart = i * 1000;
                        //    int limitEnd = 1000;
                        //    DataTable dt = new VehicleLogSqlBLL().GetDTVehicleLog(startDateTimePicker.Value.Date, endDateTimePicker.Value.Date, limitStart, limitEnd);
                        //    if (i == 0)
                        //    {
                        //        dtTotal = dt.Copy(); ;
                        //    }
                        //    else
                        //    {

                        //        //添加DataTable的数据
                        //        foreach (DataRow dr in dt.Rows)
                        //        {
                        //            dtTotal.ImportRow(dr);
                        //        }
                        //    }

                        //}
                        //DataTable haspicdata = new VehicleLogSqlBLL().GetDTVehicleLogHasPic(startDateTimePicker.Value.Date, endDateTimePicker.Value.Date, 0, 0, false);
                        //string filePath1 = theFolder.FullName + "\\log_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
                        //ExportExcel(haspicdata, filePath1, 0);

                        dtTotal = new VehicleLogSqlBLL().GetDTVehicleLogHasPic(startDateTimePicker.Value.Date, endDateTimePicker.Value.Date, 0, 0, false);
                        if (dtTotal != null)
                        {


                            int inCount = new VehicleLogSqlBLL().GetInCountVehicleLog(startDateTimePicker.Value.Date, endDateTimePicker.Value.Date);
                            int outCount = new VehicleLogSqlBLL().GetOutCountVehicleLog(startDateTimePicker.Value.Date, endDateTimePicker.Value.Date);

                            DataTable dtInOutCount = new DataTable();
                            dtInOutCount.Columns.Add("开始日期");
                            dtInOutCount.Columns.Add("结束日期");
                            dtInOutCount.Columns.Add("进场车辆总数量");
                            dtInOutCount.Columns.Add("出场车辆总数量");

                            dtInOutCount.Rows.Add(startDateTimePicker.Value.Date, endDateTimePicker.Value.Date, inCount, outCount);
                            DataTableToExcel(dtTotal, dtInOutCount, filePath, true);


                            MessageBox.Show("导出成功");
                        }
                        else
                        {
                            MessageBox.Show("未查询到数据");
                            LogHelper.Error("未查询到数据");
                        }
                    }
                    else
                    {
                        MessageBox.Show("未查询到数据");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                LogHelper.Error("Exception1: " + ex.ToString());
            }

            label3.Text = "请选择保存至非桌面的路径";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="path"></param>
        /// <param name="isFirst">0表示第一次创建</param>
        public void ExportExcel(DataTable dt, string filePath, int isFirst)
        {

            //创建文件
            FileStream file = new FileStream(filePath, FileMode.Append, FileAccess.Write);



            //以指定的字符编码向指定的流写入字符
            StreamWriter sw = new StreamWriter(file, Encoding.GetEncoding("GB2312"));

            //StringBuilder strbu = new StringBuilder();
            if (isFirst == 0)
            {
                string title = "";
                //写入标题
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    title = title + dt.Columns[i].ColumnName.ToString() + ",";
                }
                //加入换行字符串
                title = title + Environment.NewLine;
                sw.Write(title);
            }



            //写入内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string bodytext = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    bodytext = bodytext + dt.Rows[i][j].ToString() + ",";
                }
                bodytext = bodytext + Environment.NewLine;
                sw.Write(bodytext);
            }

            sw.Flush();
            file.Flush();

            sw.Close();
            sw.Dispose();

            file.Close();
            file.Dispose();
        }

        private void LogExport_Load(object sender, EventArgs e)
        {
            this.startDateTimePicker.Value = DateTime.Now.AddDays(-1);
            this.endDateTimePicker.Value = DateTime.Now.AddDays(-1);
        }


        /// <summary>
        /// 创建一个Excel
        /// Yakecan
        /// </summary>
        /// <returns>返回一个空表格</returns>
        public HSSFWorkbook InitializeWorkBook()
        {
            HSSFWorkbook workBook = new HSSFWorkbook();
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();

            dsi.Company = "捷顺科技";
            dsi.Manager = "Office Word 2003/2007";

            si.Author = "jieshun";
            si.Subject = "日志导出";
            si.Title = "日志报表";

            workBook.DocumentSummaryInformation = dsi;
            workBook.SummaryInformation = si;

            return workBook;
        }

        private IWorkbook workbook = null;
        private FileStream fs = null;


        /// <summary>
        /// 将DataTable数据导入到excel中
        /// </summary>
        /// <param name="data">要导入的数据</param>
        /// <param name="isColumnWritten">DataTable的列名是否要导入</param>
        /// <param name="sheetName">要导入的excel的sheet的名称</param>
        /// <returns>导入数据行数(包含列名那一行)</returns>
        public int DataTableToExcel(DataTable data1, DataTable data2, string fileName, bool isColumnWritten)
        {
            int i = 0;
            int j = 0;

            fs = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            if (fileName.IndexOf(".xlsx") > 0 && workbook == null) // 2007版本
                workbook = new XSSFWorkbook();
            else if (fileName.IndexOf(".xls") > 0 && workbook == null) // 2003版本
                workbook = new HSSFWorkbook();
            ISheet sheet = null;
            ISheet sheet2 = null;
            try
            {
                if (workbook != null)
                {
                    sheet = workbook.CreateSheet("日志详情");
                    sheet2 = workbook.CreateSheet("汇总信息");
                }

                int count = 0;
                if (isColumnWritten == true) //写入DataTable的列名
                {
                    IRow row = sheet.CreateRow(0);
                    for (j = 0; j < data1.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(data1.Columns[j].ColumnName);
                    }
                    count = 1;
                }


                for (i = 0; i < data1.Rows.Count; ++i)
                {

                    IRow row = sheet.CreateRow(count);
                    for (j = 0; j < data1.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(data1.Rows[i][j].ToString());
                    }
                    ++count;
                }


                int count2 = 0;
                if (isColumnWritten == true) //写入DataTable的列名
                {
                    IRow row = sheet2.CreateRow(0);
                    for (j = 0; j < data2.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(data2.Columns[j].ColumnName);
                    }
                    count2 = 1;
                }


                for (i = 0; i < data2.Rows.Count; ++i)
                {
                    IRow row = sheet2.CreateRow(count2);
                    for (j = 0; j < data2.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(data2.Rows[i][j].ToString());
                    }
                    ++count2;
                }


                workbook.Write(fs); //写入到excel

                fs.Close();
                fs.Dispose();

                return count;
            }
            catch (Exception ex)
            {
                LogHelper.Error("Exception: " + ex.ToString());
                return -1;
            }
        }

        /// <summary>
        /// 图片在单元格等比缩放居中显示
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="value">图片二进制流</param>
        private void CellImage(ICell cell, byte[] value)
        {
            if (value.Length == 0) return;//空图片处理
            double scalx = 0;//x轴缩放比例
            double scaly = 0;//y轴缩放比例
            int Dx1 = 0;//图片左边相对excel格的位置(x偏移) 范围值为:0~1023,超过1023就到右侧相邻的单元格里了
            int Dy1 = 0;//图片上方相对excel格的位置(y偏移) 范围值为:0~256,超过256就到下方的单元格里了
            bool bOriginalSize = false;//是否显示图片原始大小 true表示图片显示原始大小  false表示显示图片缩放后的大小
            ///计算单元格的长度和宽度
            double CellWidth = 0;
            double CellHeight = 0;
            int RowSpanCount = 1;//合并的单元格行数
            int ColSpanCount = 1;//合并的单元格列数 
            int j = 0;
            for (j = 0; j < RowSpanCount; j++)//根据合并的行数计算出高度
            {
                CellHeight += cell.Sheet.GetRow(cell.RowIndex + j).Height;
            }
            for (j = 0; j < ColSpanCount; j++)
            {
                CellWidth += cell.Row.Sheet.GetColumnWidth(cell.ColumnIndex + j);
            }
            //单元格长度和宽度与图片的长宽单位互换是根据实例得出
            CellWidth = CellWidth / 35;
            CellHeight = CellHeight / 15;
            ///计算图片的长度和宽度
            MemoryStream ms = new MemoryStream(value);
            Image Img = Bitmap.FromStream(ms, true);
            double ImageOriginalWidth = Img.Width;//原始图片的长度
            double ImageOriginalHeight = Img.Height;//原始图片的宽度
            double ImageScalWidth = 0;//缩放后显示在单元格上的图片长度
            double ImageScalHeight = 0;//缩放后显示在单元格上的图片宽度
            if (CellWidth > ImageOriginalWidth && CellHeight > ImageOriginalHeight)//单元格的长度和宽度比图片的大，说明单元格能放下整张图片，不缩放
            {
                ImageScalWidth = ImageOriginalWidth;
                ImageScalHeight = ImageOriginalHeight;
                bOriginalSize = true;
            }
            else//需要缩放，根据单元格和图片的长宽计算缩放比例
            {
                bOriginalSize = false;
                if (ImageOriginalWidth > CellWidth && ImageOriginalHeight > CellHeight)//图片的长和宽都比单元格的大的情况
                {
                    double WidthSub = ImageOriginalWidth - CellWidth;//图片长与单元格长的差距
                    double HeightSub = ImageOriginalHeight - CellHeight;//图片宽与单元格宽的差距
                    if (WidthSub > HeightSub)//长的差距比宽的差距大时,长度x轴的缩放比为1，表示长度就用单元格的长度大小，宽度y轴的缩放比例需要根据x轴的比例来计算
                    {
                        scalx = 1;
                        scaly = (CellWidth / ImageOriginalWidth) * ImageOriginalHeight / CellHeight;//计算y轴的缩放比例,CellWidth / ImageWidth计算出图片整体的缩放比例,然后 * ImageHeight计算出单元格应该显示的图片高度,然后/ CellHeight就是高度的缩放比例
                    }
                    else
                    {
                        scaly = 1;
                        scalx = (CellHeight / ImageOriginalHeight) * ImageOriginalWidth / CellWidth;
                    }
                }
                else if (ImageOriginalWidth > CellWidth && ImageOriginalHeight < CellHeight)//图片长度大于单元格长度但图片高度小于单元格高度，此时长度不需要缩放，直接取单元格的，因此scalx=1，但图片高度需要等比缩放
                {
                    scalx = 1;
                    scaly = (CellWidth / ImageOriginalWidth) * ImageOriginalHeight / CellHeight;
                }
                else if (ImageOriginalWidth < CellWidth && ImageOriginalHeight > CellHeight)//图片长度小于单元格长度但图片高度大于单元格高度，此时单元格高度直接取单元格的，scaly = 1,长度需要等比缩放
                {
                    scaly = 1;
                    scalx = (CellHeight / ImageOriginalHeight) * ImageOriginalWidth / CellWidth;
                }
                ImageScalWidth = scalx * CellWidth;
                ImageScalHeight = scaly * CellHeight;
            }
            Dx1 = Convert.ToInt32((CellWidth - ImageScalWidth) / CellWidth * 1023 / 2);
            Dy1 = Convert.ToInt32((CellHeight - ImageScalHeight) / CellHeight * 256 / 2);
            int pictureIdx = cell.Sheet.Workbook.AddPicture((Byte[])value, PictureType.PNG);
            IClientAnchor anchor = cell.Sheet.Workbook.GetCreationHelper().CreateClientAnchor();
            anchor.AnchorType = AnchorType.MoveDontResize;
            anchor.Col1 = cell.ColumnIndex;
            anchor.Col2 = cell.ColumnIndex + 1;
            anchor.Row1 = cell.RowIndex;
            anchor.Row2 = cell.RowIndex + 1;
            anchor.Dy1 = Dy1;//图片下移量
            anchor.Dx1 = Dx1;//图片右移量，通过图片下移和右移，使得图片能居中显示，因为图片不同文字，图片是浮在单元格上的，文字是钳在单元格里的
            IDrawing patriarch = cell.Sheet.CreateDrawingPatriarch();
            IPicture pic = patriarch.CreatePicture(anchor, pictureIdx);
            if (bOriginalSize)
            {
                pic.Resize();//显示图片原始大小 
            }
            else
            {
                pic.Resize(scalx, scaly);//等比缩放   
            }

            ms.Close();
            ms.Dispose();
        }

        /// <summary>
        /// 把指定的DataTable导出Excel
        /// Yakecan
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="path">导出的路径(包含文件的名称及后缀名)</param>
        /// <param name="tittle">Sheet的名称</param>
        //public void Export(DataTable dt, string path, string tittle)
        //{
        //    HSSFWorkbook workbook = InitializeWorkBook();
        //    ISheet sheet1 = workbook.CreateSheet(tittle);



        //    IRow titleRow = sheet1.CreateRow(0);
        //    titleRow.Height = (short)20 * 25;

        //    ICellStyle titleStyle = workbook.CreateCellStyle();
        //    titleStyle.Alignment = HorizontalAlignment.Center;
        //    titleStyle.VerticalAlignment = VerticalAlignment.Center;
        //    IFont font = workbook.CreateFont();
        //    font.FontName = "宋体";
        //    font.FontHeightInPoints = (short)16;
        //    titleStyle.SetFont(font);

        //    NPOI.SS.Util.CellRangeAddress region = new NPOI.SS.Util.CellRangeAddress(0, 0, 0, dt.Columns.Count);
        //    sheet1.AddMergedRegion(region); // 添加合并区域

        //    ICell titleCell = titleRow.CreateCell(0);
        //    titleCell.CellStyle = titleStyle;
        //    titleCell.SetCellValue(tittle);


        //    IRow headerRow = sheet1.CreateRow(1);
        //    ICellStyle headerStyle = workbook.CreateCellStyle();
        //    headerStyle.Alignment = HorizontalAlignment.Center;
        //    headerStyle.VerticalAlignment = VerticalAlignment.Center;
        //    headerStyle.BorderBottom = BorderStyle.Thin;
        //    headerStyle.BorderLeft = BorderStyle.Thin;
        //    headerStyle.BorderRight = BorderStyle.Thin;
        //    headerStyle.BorderTop = BorderStyle.Thin;
        //    IFont titleFont = workbook.CreateFont();
        //    titleFont.FontHeightInPoints = (short)11;
        //    titleFont.FontName = "宋体";
        //    headerStyle.SetFont(titleFont);

        //    headerRow.CreateCell(0).SetCellValue("序号");
        //    headerRow.GetCell(0).CellStyle = headerStyle;

        //    for (int i = 0; i < dt.Columns.Count; i++)
        //    {
        //        headerRow.CreateCell(i + 1).SetCellValue(dt.Columns[i].ColumnName);
        //        headerRow.GetCell(i + 1).CellStyle = headerStyle;
        //        sheet1.SetColumnWidth(i, 256 * 18);
        //    }

        //    ICellStyle bodyStyle = workbook.CreateCellStyle();
        //    bodyStyle.BorderBottom = BorderStyle.Thin;
        //    bodyStyle.BorderLeft = BorderStyle.Thin;
        //    bodyStyle.BorderRight = BorderStyle.Thin;
        //    bodyStyle.BorderTop = BorderStyle.Thin;
        //    for (int r = 0; r < dt.Rows.Count; r++)
        //    {
        //        IRow bodyRow = sheet1.CreateRow(r + 2);
        //        bodyRow.CreateCell(0).SetCellValue(r + 1);
        //        bodyRow.GetCell(0).CellStyle = bodyStyle;
        //        bodyRow.GetCell(0).CellStyle.Alignment = HorizontalAlignment.Center;

        //        for (int c = 0; c < dt.Columns.Count; c++)
        //        {
        //            bodyRow.CreateCell(c + 1).SetCellValue(dt.Rows[r][c].ToString());
        //            bodyRow.GetCell(c + 1).CellStyle = bodyStyle;
        //        }
        //    }

        //    sheet1.CreateFreezePane(1, 2);

        //    FileStream fs = new FileStream(path, FileMode.Create);
        //    workbook.Write(fs);
        //    fs.Flush();
        //    fs.Position = 0;
        //    sheet1 = null;
        //    headerRow = null;
        //    workbook = null;
        //    //OutPutExcelStreamOnClient(ms, xlsName);
        //    fs.Dispose();
        //}


        //public void OutputClient(FileInfo file)
        //{
        //    HttpResponse response = HttpContext.Current.Response;

        //    response.Buffer = true;

        //    response.Clear();
        //    response.ClearHeaders();
        //    response.ClearContent();

        //    response.ContentType = "application/vnd.ms-excel";

        //    //导出到 .xlsx 格式不能用时，可以试试这个
        //    //HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        //    response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xlsx", DateTime.Now.ToString("yyyy-MM-dd-HH-mm")));

        //    response.Charset = "GB2312";
        //    response.ContentEncoding = Encoding.GetEncoding("GB2312");

        //    response.AddHeader("Content-Length", file.Length.ToString());

        //    response.WriteFile(file.FullName);
        //    response.Flush();

        //    response.Close();
        //}
    }
}
