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

namespace WinTestJD
{
    public partial class LogExport : Form
    {
        public LogExport()
        {
            InitializeComponent();
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
                        int page = totalCount / 1 + 50;

                        string filePath = foldPath + "log_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                        for (int i = 0; i < page; i++)
                        {
                            int limitStart = i * 50;
                            int limitEnd = 50;
                            DataTable dt = new VehicleLogSqlBLL().GetDTVehicleLog(startDateTimePicker.Value.Date, endDateTimePicker.Value.Date, limitStart, limitEnd);
                            if (i == 0)
                            {
                                dtTotal = dt.Copy(); ;
                            }
                            else
                            {

                                //添加DataTable的数据
                                foreach (DataRow dr in dt.Rows)
                                {
                                    dtTotal.ImportRow(dr);
                                }
                            }

                        }
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
                            DataTableToExcel(dtTotal,dtInOutCount, filePath, true);

                          
                            MessageBox.Show("导出成功");
                        }
                        else
                        {
                            MessageBox.Show("未查询到数据");
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

            StringBuilder strbu = new StringBuilder();
            if (isFirst == 0)
            {
                //写入标题
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    strbu.Append(dt.Columns[i].ColumnName.ToString() + "\t");
                }
                //加入换行字符串
                strbu.Append(Environment.NewLine);
            }



            //写入内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    strbu.Append(dt.Rows[i][j].ToString() + "\t");
                }
                strbu.Append(Environment.NewLine);
            }

            sw.Write(strbu.ToString());
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

        public void ExportExcel2(DataTable dt, string filename, int isFirst)
        {
            OleDbConnection conn = null;
            OleDbCommand cmd = null;

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

            Microsoft.Office.Interop.Excel.Workbooks workbooks = excel.Workbooks;

            Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(true);

            try
            {
                //设置区域为当前线程的区域
                dt.Locale = System.Threading.Thread.CurrentThread.CurrentCulture;

                //设置导出文件路径
                // string path = HttpContext.Current.Server.MapPath("Export/");

                //设置新建文件路径及名称
                string savePath = filename;

                //创建文件
                FileStream file = new FileStream(savePath, FileMode.Append, FileAccess.Write);

                //关闭释放流，不然没办法写入数据
                file.Close();
                file.Dispose();

                //由于使用流创建的 excel 文件不能被正常识别，所以只能使用这种方式另存为一下。
                workbook.SaveCopyAs(savePath);


                // Excel 2003 版本连接字符串
                //string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + savePath + "';Extended Properties='Excel 8.0;HDR=Yes;'";

                // Excel 2007 以上版本连接字符串
                string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + savePath + "';Extended Properties='Excel 12.0;HDR=Yes;'";

                //创建连接对象
                conn = new OleDbConnection(strConn);
                //打开连接
                conn.Open();

                //创建命令对象
                cmd = conn.CreateCommand();

                //获取 excel 所有的数据表。
                //new object[] { null, null, null, "Table" }指定返回的架构信息：参数介绍
                //第一个参数指定目录
                //第二个参数指定所有者
                //第三个参数指定表名
                //第四个参数指定表类型
                DataTable dtSheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });

                if (isFirst == 0)
                {
                    //因为后面创建的表都会在最后面，所以本想删除掉前面的表，结果发现做不到，只能清空数据。
                    for (int i = 0; i < dtSheetName.Rows.Count; i++)
                    {
                        cmd.CommandText = "drop table [" + dtSheetName.Rows[i]["TABLE_NAME"].ToString() + "]";
                        cmd.ExecuteNonQuery();
                    }

                    //添加一个表，即 Excel 中 sheet 表
                    cmd.CommandText = "create table " + dt.TableName + " ([序号] INT,[推送时间] VarChar,[推送结果] VarChar,[actionDescId] VarChar,[vehicleNo] VarChar,[parkLotCode] VarChar,[actionPositionCode] VarChar,[LogNo] VarChar,[entryTime] VarChar,[reasonCode] VarChar,[reason] VarChar,[photoStr] VarChar,[photoName] VarChar,[resend] VarChar)";
                    cmd.ExecuteNonQuery();
                }
                //"create table " + dt.TableName + " ([S_Id] INT,[S_StuNo] VarChar,[S_Name] VarChar,[S_Sex] VarChar,[S_Height] VarChar,[S_BirthDate] VarChar,[C_S_Id] INT)";


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string values = "";

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        values += "'" + dt.Rows[i][j].ToString() + "',";
                    }

                    //判断最后一个字符是否为逗号，如果是就截取掉
                    if (values.LastIndexOf(',') == values.Length - 1)
                    {
                        values = values.Substring(0, values.Length - 1);
                    }

                    //写入数据
                    cmd.CommandText = "insert into " + dt.TableName + " (序号,推送时间,推送结果, actionDescId, vehicleNo, parkLotCode,actionPositionCode,actionPosition,actionTime ,LogNo, entryTime,reasonCode,reason,photoStr,photoName,resend) values (" + values + ")";
                    cmd.ExecuteNonQuery();
                }

                conn.Close();
                conn.Dispose();
                cmd.Dispose();

                //加入下面的方法，把保存的 Excel 文件输出到浏览器下载。需要先关闭连接。
                //FileInfo fileInfo = new FileInfo(savePath);
                //OutputClient(fileInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                workbook.Close(false, Type.Missing, Type.Missing);
                workbooks.Close();
                excel.Quit();

                Marshal.ReleaseComObject(workbook);
                Marshal.ReleaseComObject(workbooks);
                Marshal.ReleaseComObject(excel);

                workbook = null;
                workbooks = null;
                excel = null;

                GC.Collect();
            }
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
                Console.WriteLine("Exception: " + ex.Message);
                return -1;
            }
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
