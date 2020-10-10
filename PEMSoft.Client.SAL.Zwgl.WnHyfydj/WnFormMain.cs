/*************************
* 管控对象：海运费用登记
* 管控成员：许勇革
*************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.EditForm.Helpers;
using PEMSoft.Client.SYS.WnControl;
using PEMSoft.Client.SYS.WnPlatform;

namespace PEMSoft.Client.SAL.Zwgl.WnHyfydj
{
    public partial class WnFormMain : WnForm
    {
        #region 变量

        /// <summary>
        /// 记录当前年份
        /// </summary>
        private string _year;

        /// <summary>
        /// 记录当前月份
        /// </summary>
        private string _month;

        /// <summary>
        /// 记录取数是否成功
        /// </summary>
        private bool _isSuccess;

        #endregion

        #region 控件变量及初始化

        private WnToolBar wnToolBarMain;
        private WnTbLabel tsLabelYear;
        private WnCombo tsComboYear;
        private WnTbLabel tsLabelMonth;
        private WnCombo tsComboMonth;
        private WnTbButton tsButtonLoad;
        private WnSeparator tsSeparator1;
        private WnTbButton tsButtonCarry;
        private WnSeparator tsSeparator2;
        private WnTbButton tsButtonAppend;
        private WnTbButton tsButtonImport;
        private WnSeparator tsSeparator3;
        private WnTbButton tsButtonSeal;
        private WnSeparator tsSeparator4;
        private WnTbButton tsButtonSave;
        private WnSeparator tsSeparator5;
        private WnTbButton tsButtonAudit;
        private WnTbButton tsButtonUnAudit;
        private WnSeparator tsSeparator6;
        private WnTbButton tsButtonHelp;
        private WnSeparator tsSeparator7;
        private WnTbButton tsButtonExit;
        private WnTbLabel tsLabelYsh;
        private WnSeparator tsSeparator8;
        private WnTbButton tsButtonSealManage;
        private WnGrid wnGridMain;

        /// <summary>
        /// 控件变量初始化方法
        /// </summary>
        private void InitializeObject()
        {
            wnToolBarMain = this.AllObjects["wnToolBarMain"] as WnToolBar;
            tsLabelYear = this.AllObjects["tsLabelYear"] as WnTbLabel;
            tsComboYear = this.AllObjects["tsComboYear"] as WnCombo;
            tsLabelMonth = this.AllObjects["tsLabelMonth"] as WnTbLabel;
            tsComboMonth = this.AllObjects["tsComboMonth"] as WnCombo;
            tsButtonLoad = this.AllObjects["tsButtonLoad"] as WnTbButton;
            tsSeparator1 = this.AllObjects["tsSeparator1"] as WnSeparator;
            tsButtonCarry = this.AllObjects["tsButtonCarry"] as WnTbButton;
            tsSeparator2 = this.AllObjects["tsSeparator2"] as WnSeparator;
            tsButtonAppend = this.AllObjects["tsButtonAppend"] as WnTbButton;
            tsButtonImport = this.AllObjects["tsButtonImport"] as WnTbButton;
            tsSeparator3 = this.AllObjects["tsSeparator3"] as WnSeparator;
            tsButtonSeal = this.AllObjects["tsButtonSeal"] as WnTbButton;
            tsSeparator4 = this.AllObjects["tsSeparator4"] as WnSeparator;
            tsButtonSave = this.AllObjects["tsButtonSave"] as WnTbButton;
            tsSeparator5 = this.AllObjects["tsSeparator5"] as WnSeparator;
            tsButtonAudit = this.AllObjects["tsButtonAudit"] as WnTbButton;
            tsButtonUnAudit = this.AllObjects["tsButtonUnAudit"] as WnTbButton;
            tsSeparator6 = this.AllObjects["tsSeparator6"] as WnSeparator;
            tsButtonHelp = this.AllObjects["tsButtonHelp"] as WnTbButton;
            tsSeparator7 = this.AllObjects["tsSeparator7"] as WnSeparator;
            tsButtonExit = this.AllObjects["tsButtonExit"] as WnTbButton;
            tsLabelYsh = this.AllObjects["tsLabelYsh"] as WnTbLabel;
            tsSeparator8 = this.AllObjects["tsSeparator8"] as WnSeparator;
            tsButtonSealManage = this.AllObjects["tsButtonSealManage"] as WnTbButton;
            wnGridMain = this.AllObjects["wnGridMain"] as WnGrid;
        }

        #endregion

        #region 窗体方法

        /// <summary>
        /// 构造函数
        /// </summary>
        public WnFormMain()
        {
            InitializeComponent();

            /* 设置该属性为true，确保在窗体的控件按键时，优先触发窗体的按键事件 */
            this.KeyPreview = true;

            /* 注册窗体事件 */
            this.AfterAssemble += WnFormMain_AfterAssemble;
            this.KeyDown += WnFormMain_KeyDown;
            this.FormClosing += WnFormMain_FormClosing;
        }

        /// <summary>
        /// 响应窗体的AfterAssemble事件
        /// </summary>
        private void WnFormMain_AfterAssemble(object sender, CancelEventArgs e)
        {
            /* 初始化控件变量 */
            InitializeObject();

            /* 注册控件事件 */
            tsComboYear.ValueChanging += tsComboYear_ValueChanging;
            tsComboMonth.ValueChanging += tsComboMonth_ValueChanging;
            tsButtonLoad.Click += tsButtonLoad_Click;
            tsButtonCarry.Click += tsButtonCarry_Click;
            tsButtonAppend.Click += tsButtonAppend_Click;
            tsButtonImport.Click += tsButtonImport_Click;
            tsButtonSave.Click += tsButtonSave_Click;
            tsButtonAudit.Click += tsButtonAudit_Click;
            tsButtonUnAudit.Click += tsButtonUnAudit_Click;
            tsButtonSeal.Click += tsButtonSeal_Click;
            tsButtonHelp.Click += tsButtonHelp_Click;
            tsButtonExit.Click += tsButtonExit_Click;
            tsButtonSealManage.Click += tsButtonSealManage_Click;
            wnGridMain.CurrRowChanged += wnGridMain_CurrRowChanged;

            /* 获取当前服务器时间 */
            object currTime = AppServer.GetDateTime();
            if (currTime == null) { e.Cancel = true; return; }

            /* 为tsComboYear控件绑定数据源 */
            tsComboYear.ComboString = ((DateTime)currTime).AddYears(-1).Year + "|" + ((DateTime)currTime).Year + "|" + ((DateTime)currTime).AddYears(+1).Year;
            tsComboYear.SelectedIndex = 1;

            /* 为tsComboMonth控件设置默认值，默认为当前月份 */
            tsComboMonth.SelectedIndex = ((DateTime)currTime).Month - 1;

            /* 设置右键菜单 */
            wnGridMain.ContextMenuStrip = wnToolBarMain.WnMenu;

            /* 设置标准前景色和特殊前景色 */
            wnGridMain.SetForeColor("RowState = '0'|| RowState = '10'|| RowState = '11'", "", Color.FromArgb(0, 128, 0));
            wnGridMain.SetForeColor("AuditUser is not null", "", Color.FromArgb(255, 0, 0)); //已审核的数据显示红色

            /* 获取数据 */
            GetData();
        }

        /// <summary>
        /// 响应窗体的KeyDown事件
        /// </summary>
        private void WnFormMain_KeyDown(object sender, KeyEventArgs e)
        {
            /* 若保存按钮可用时，同时按下Ctrl键和S键，保存数据 */
            if (tsButtonSave.Enabled && e.Control && e.KeyCode == Keys.S)
            {
                tsButtonSave_Click(null, null);
            }
        }

        /// <summary>
        /// 响应窗体的FormClosing事件
        /// </summary>
        private void WnFormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            /* 若保存按钮可用，则在关闭窗体时检查是否有未保存的数据 */
            if (tsButtonSave.Enabled && !SaveData(new List<Act> { Act.AskIsUpdate, Act.SaveData }))
            {
                e.Cancel = true;
            }
        }

        #endregion

        #region 对象方法

        /// <summary>
        /// 响应tsComboYear的ValueChanging事件
        /// </summary>
        private void tsComboYear_ValueChanging(object sender, ValueChangingArgs e)
        {
            /* 若保存按钮可用，检查是否有未保存的数据 */
            if (tsButtonSave.Enabled && !SaveData(new List<Act> { Act.AskIsUpdate, Act.SaveData }))
            {
                tsComboYear.Value = e.OldValue;
                return;
            }

            /* 控制权限 */
            SetControlRight();
        }

        /// <summary>
        /// 响应tsComboMonth的ValueChanging事件
        /// </summary>
        private void tsComboMonth_ValueChanging(object sender, ValueChangingArgs e)
        {
            /* 若保存按钮可用，检查是否有未保存的数据 */
            if (tsButtonSave.Enabled && !SaveData(new List<Act> { Act.AskIsUpdate, Act.SaveData }))
            {
                tsComboMonth.Value = e.OldValue;
                return;
            }

            /* 控制权限 */
            SetControlRight();
        }

        /// <summary>
        /// 响应“加载”按钮的Click事件
        /// </summary>
        private void tsButtonLoad_Click(object sender, EventArgs e)
        {
            /* 若保存按钮可用，检查是否有未保存的数据 */
            if (tsButtonSave.Enabled && !SaveData(new List<Act> { Act.AskIsUpdate, Act.SaveData })) { return; }

            /* 获取数据 */
            GetData();
        }

        /// <summary>
        /// 响应“结转”按钮的Click事件
        /// </summary>
        private void tsButtonCarry_Click(object sender, EventArgs e)
        {
            /* 若保存按钮可用，检查是否有未保存的数据 */
            if (tsButtonSave.Enabled && !SaveData(new List<Act> { Act.AskIsUpdate, Act.SaveData })) { return; }

            /* 如果未获取当前年份、月份，则返回 */
            if (string.IsNullOrWhiteSpace(_year) || string.IsNullOrWhiteSpace(_month)) { return; }

            /* 记录上个月的年份和月份 */
            string lastYear = "";
            string lastMonth = "";

            /* 获取上个月的月份 */
            if (_month.Equals("1"))
            {
                lastMonth = "12";
                lastYear = (Convert.ToInt16(_year) - 1) + "";
            }
            else
            {
                lastYear = _year;
                lastMonth = (Convert.ToInt16(_month) - 1) + "";
            }

            /* 获取上个月份已审核的数据且本月份索引不重复的数据 */
            Hashtable htLastMonth = new Hashtable();
            htLastMonth["DataSetName"] = "GetLastMonthData";
            htLastMonth["{LastYear}"] = lastYear;
            htLastMonth["{LastMonth}"] = lastMonth;
            htLastMonth["{Year}"] = _year;
            htLastMonth["{Month}"] = _month;
            if (this.DataSource.GetDataSet(htLastMonth)["IsSuccess"].Equals("0")) { return; }
            DataTable dtLastData = (this.DataSource.DataSets["GetLastMonthData"] as DataSet).Tables[0];

            /* 如果没有需要插入的数据，则直接返回 */
            if (dtLastData.Rows.Count == 0) { return; }

            /* 将数据插入到界面 */
            foreach (DataRow datarow in dtLastData.Rows)
            {
                wnGridMain.AddRow();
                wnGridMain.CurrRow["Guid"] = Guid.NewGuid();
                wnGridMain.CurrRow["Year"] = _year;
                wnGridMain.CurrRow["Month"] = _month;
                wnGridMain.CurrRow["HdgsGuid"] = datarow["HdgsGuid"];
                wnGridMain.CurrRow["HdgsName"] = datarow["HdgsName"];
                wnGridMain.CurrRow["Zyg"] = datarow["Zyg"];
                wnGridMain.CurrRow["Mdg"] = datarow["Mdg"];
                wnGridMain.CurrRow["Yfdj"] = datarow["Yfdj"];
                wnGridMain.CurrRow["Dpfy"] = datarow["Dpfy"];
                wnGridMain.CurrRow["Ysts"] = datarow["Ysts"];
                wnGridMain.CurrRow["Note"] = datarow["Note"];
                wnGridMain.CurrRow["RowState"] = "10";
            }
        }

        /// <summary>
        /// 响应“新增”按钮的Click事件
        /// </summary>
        private void tsButtonAppend_Click(object sender, EventArgs e)
        {
            /* 清除过滤条件 */
            wnGridMain.FilterString = "";

            /* 追加一行 */
            wnGridMain.AddRow();
        }

        /// <summary>
        /// 响应“导入”按钮的Click事件
        /// </summary>
        private void tsButtonImport_Click(object sender, EventArgs e)
        {
            /* 创建wnGrid接收导入的数据集 */
            WnGrid wnGrid = new WnGrid();
            wnGrid.ImportExcel(false);

            /* 如果导入的数据源为空或者导入的数据源的行数为0，则返回 */
            if (wnGrid.DataSource == null || wnGrid.DataSource.Rows.Count == 0) { return; }

            /* 将导入的表的列名与wnGridMain的列标题相对应 */
            foreach (WnGridColumn dc in wnGridMain.Columns)
            {
                if (wnGrid.DataSource.Columns[dc.Caption.TrimEnd(new char[] { '*' })] != null)
                {
                    wnGrid.DataSource.Columns[dc.Caption.TrimEnd(new char[] { '*' })].ColumnName = dc.ColumnName;
                }
                if (wnGrid.DataSource.Columns[dc.Caption] != null)
                {
                    wnGrid.DataSource.Columns[dc.Caption].ColumnName = dc.ColumnName;
                }
            }

            /* 检查唯一索引重复，若有重复值，则从导入数据中删除 */
            var item = from p in wnGridMain.DataSource.AsEnumerable()
                       join p1 in wnGrid.DataSource.AsEnumerable()
                       on new { hdgsName = p.Field<string>("HdgsName"), zyg = p.Field<string>("Zyg"), mdg = p.Field<string>("Mdg") }
                       equals new { hdgsName = p1.Field<string>("HdgsName"), zyg = p1.Field<string>("Zyg"), mdg = p1.Field<string>("Mdg") }
                       select new { hdgsName = p.Field<string>("HdgsName"), zyg = p.Field<string>("Zyg"), mdg = p.Field<string>("Mdg") };
            if (item.Count() > 0)
            {
                foreach (var obj in item)
                {
                    wnGrid.DataSource.Rows.Remove(wnGrid.DataSource.Select("HdgsName = '" + obj.hdgsName + "' and Zyg = '" + obj.zyg + "' and Mdg= '" + obj.mdg + "'")[0]);
                }
            }

            /* 检查重复之后，再次检查导入的数据是否为空或数据行为0 */
            if (wnGrid.DataSource == null || wnGrid.DataSource.Rows.Count == 0) { return; }

            /* 获取导入数据中所有货代公司名称 */
            string hdgsNames = "'" + string.Join("','", wnGrid.DataSource.AsEnumerable().Select(x => x.Field<string>("HdgsName"))) + "'";
            string hdgsNamesDr = "'" + string.Join(",", wnGrid.DataSource.AsEnumerable().Select(x => x.Field<string>("HdgsName"))) + "'";

            /* 通过货代公司名称获取货代公司内码值，获取失败，直接返回 */
            Hashtable htGetHdgsGuid = new Hashtable();
            htGetHdgsGuid["DataSetName"] = "GetHdgsGuid";
            htGetHdgsGuid["{HdgsNames}"] = hdgsNames;
            htGetHdgsGuid["{HdgsNamesDr}"] = hdgsNamesDr;
            if (this.DataSource.GetDataSet(htGetHdgsGuid)["IsSuccess"].Equals("0")) { return; }

            /* 若未获取到货代公司代码，直接返回 */
            DataTable dtHdgsGuid = ((this.DataSource.DataSets["GetHdgsGuid"]) as DataSet).Tables[0];
            if (dtHdgsGuid.Rows.Count == 0) { return; }

            /* 检查是否存在获取不到内码值的货代公司的信息，删除获取不到内码值的货代公司的信息，并提示 */
            var hdgsNamesDelete = dtHdgsGuid.AsEnumerable().Where(x => string.IsNullOrWhiteSpace(x.Field<string>("HdgsGuid") + "")).Select(x => new { hdgsNameDr = x.Field<string>("HdgsNameDr") });
            if (hdgsNamesDelete.Count() > 0)
            {
                StringBuilder message = new StringBuilder();
                foreach (var obj in hdgsNamesDelete)
                {
                    wnGrid.DataSource.Rows.Remove(wnGrid.DataSource.Select("HdgsName = '" + obj.hdgsNameDr + "'")[0]);
                    message.Append(",").AppendFormat(this.GetCurrLanguageContent("WnFormMain.HdgsNameDr"), obj.hdgsNameDr); //Content_CN：货代公司名称：{0}
                    message.Append('\n');
                }
                DialogBox.ShowWarning(string.Format(this.GetCurrLanguageContent("WnFormMain.NoHdgsGuid"), message.ToString().Substring(1))); //Content_CN：{0}以上导入的数据中货代公司名称不正确！
            }

            /* 如果货代公司内码为空，则直接返回 */
            if (dtHdgsGuid.AsEnumerable().Where(x => !string.IsNullOrWhiteSpace(x.Field<string>("HdgsGuid") + "")).Select(x => new { hdgsNameGuid = x.Field<string>("HdgsGuid") }).Count() == 0) { return; }

            /* 获取导入数据中的装运港 */
            string Zygs = "'" + string.Join("','", wnGrid.DataSource.AsEnumerable().Select(x => x.Field<string>("Zyg"))) + "'";
            string ZygsDr = "'" + string.Join("|", wnGrid.DataSource.AsEnumerable().Select(x => x.Field<string>("Zyg"))) + "'";

            /* 获取装运港信息 */
            Hashtable htGetZyg = new Hashtable();
            htGetZyg["DataSetName"] = "GetZyg";
            htGetZyg["{Zygs}"] = Zygs;
            htGetZyg["{ZygsDr}"] = ZygsDr;
            if (this.DataSource.GetDataSet(htGetZyg)["IsSuccess"].Equals("0")) { return; }

            /* 若未获取到装运港，直接返回 */
            DataTable dtZyg = ((this.DataSource.DataSets["GetZyg"]) as DataSet).Tables[0];
            if (dtZyg.Rows.Count == 0) { return; }

            /* 若获取到的装运港信息不正确，清空导入数据中的装运港信息 */
            var ZygsDelete = dtZyg.AsEnumerable().Where(x => string.IsNullOrWhiteSpace(x.Field<string>("Zyg") + "")).Select(x => new { zygDr = x.Field<string>("ZygDr") });
            if (ZygsDelete.Count() > 0)
            {
                foreach (var obj in ZygsDelete)
                {
                    foreach (DataRow dr in wnGrid.DataSource.Select("Zyg = '" + obj.zygDr + "'"))
                    {
                        dr["Zyg"] = DBNull.Value;
                    }
                }
            }

            /* 获取导入数据中的目的港 */
            string Mdgs = "'" + string.Join("','", wnGrid.DataSource.AsEnumerable().Select(x => x.Field<string>("Mdg"))) + "'";
            string MdgsDr = "'" + string.Join("|", wnGrid.DataSource.AsEnumerable().Select(x => x.Field<string>("Mdg"))) + "'";

            /* 获取目的港信息 */
            Hashtable htGetMdg = new Hashtable();
            htGetMdg["DataSetName"] = "GetMdg";
            htGetMdg["{Mdgs}"] = Mdgs;
            htGetMdg["{MdgsDr}"] = MdgsDr;
            if (this.DataSource.GetDataSet(htGetMdg)["IsSuccess"].Equals("0")) { return; }

            /* 若未获取到目的港，直接返回 */
            DataTable dtMdg = ((this.DataSource.DataSets["GetMdg"]) as DataSet).Tables[0];
            if (dtMdg.Rows.Count == 0) { return; }

            /* 若获取到的目的港信息不正确，清空导入数据中的目的港信息 */
            var MdgsDelete = dtMdg.AsEnumerable().Where(x => string.IsNullOrWhiteSpace(x.Field<string>("Mdg") + "")).Select(x => new { mdgDr = x.Field<string>("MdgDr") });
            if (MdgsDelete.Count() > 0)
            {
                foreach (var obj in MdgsDelete)
                {
                    foreach (DataRow dr in wnGrid.DataSource.Select("Mdg = '" + obj.mdgDr + "'"))
                    {
                        dr["Mdg"] = DBNull.Value;
                    }
                }
            }

            foreach (DataRow datarow in wnGrid.DataSource.Rows)
            {
                /* 新增行，并填入数据 */
                wnGridMain.AddRow();
                wnGridMain.CurrRow["Guid"] = Guid.NewGuid();
                wnGridMain.CurrRow["Year"] = _year;
                wnGridMain.CurrRow["Month"] = _month;
                wnGridMain.CurrRow["HdgsGuid"] = dtHdgsGuid.Select("HdgsName = '" + datarow["HdgsName"].ToString() + "'")[0]["HdgsGuid"];
                wnGridMain.CurrRow["HdgsName"] = datarow["HdgsName"];
                wnGridMain.CurrRow["Zyg"] = datarow["Zyg"];
                wnGridMain.CurrRow["Mdg"] = datarow["Mdg"];
                wnGridMain.CurrRow["Yfdj"] = datarow["Yfdj"];
                wnGridMain.CurrRow["Dpfy"] = datarow["Dpfy"];
                wnGridMain.CurrRow["Ysts"] = datarow["Ysts"];
                wnGridMain.CurrRow["Note"] = datarow["Note"];
                wnGridMain.CurrRow["RowState"] = "10";
            }
        }

        /// <summary>
        /// 响应“封存”按钮的Click事件
        /// </summary>
        private void tsButtonSeal_Click(object sender, EventArgs e)
        {
            /* 保存按钮可用，检查是否有未保存的数据 */
            if (tsButtonSave.Enabled && !SaveData(new List<Act> { Act.AskIsUpdate, Act.SaveData })) { return; }

            /* 若界面没有勾选行并且当前行不为空，则选中当前行，对当前行进行封存 */
            if (wnGridMain.SelectedRowsArray.Length == 0)
            {
                if (wnGridMain.CurrRow == null) { return; }
                wnGridMain.SetRowSelected(wnGridMain.CurrRow);
            }

            /* 询问是否确定要进行封存操作 */
            if (DialogBox.AskYesNo(this.GetCurrLanguageContent("WnFormMain.AskSealData")) == DialogResult.No) { return; } //Content_CN：是否确定要进行封存操作？

            /* 获取当前服务器时间 */
            object currTime = AppServer.GetDateTime();
            if (currTime == null) { return; }

            /* 遍历选中行，获取选中行的Guid、封存人、封存时间 */
            List<string> listSeal = new List<string>();
            List<string> listSealUser = new List<string>();
            List<string> listSealTime = new List<string>();
            foreach (DataRow datarow in wnGridMain.SelectedRowsArray)
            {
                /* 已审核的数据不允许封存,只封存未审核的行 */
                if (!string.IsNullOrWhiteSpace(datarow["AuditUser"] + ""))
                {
                    wnGridMain.SetRowUnSelected(datarow);
                    continue;
                }

                /* 界面的新增行直接删除 */
                if (datarow.RowState == DataRowState.Added)
                {
                    wnGridMain.DeleteRow(datarow);
                    continue;
                }

                /* 将符合条件的信息存入集合 */
                listSeal.Add(datarow["Guid"].ToString());
                listSealUser.Add(AppInfo.UserName);
                listSealTime.Add(Convert.ToDateTime(currTime).ToString("yyyy/MM/dd HH:mm:ss"));
            }

            /* 若集合中无数据，则直接返回 */
            if (listSeal.Count == 0) { return; }

            /* 执行封存语句 */
            Hashtable htSealData = new Hashtable();
            htSealData["DataSetName"] = "SealData";
            htSealData["{Guid}"] = listSeal;
            htSealData["{SealUser}"] = listSealUser;
            htSealData["{SealTime}"] = listSealTime;
            if (this.DataSource.ExecSql(htSealData)["IsSuccess"].Equals("0")) { return; }

            /* 记录日志，并删除界面上的数据 */
            Hashtable htLog = new Hashtable();
            foreach (DataRow dr in wnGridMain.SelectedRowsArray)
            {
                /* 记录封存日志 */
                htLog["{Year}"] = dr["Year"].ToString();
                htLog["{Month}"] = dr["Month"].ToString();
                htLog["{HdgsName}"] = dr["HdgsName"].ToString();
                htLog["{Zyg}"] = dr["Zyg"].ToString();
                htLog["{Mdg}"] = dr["Mdg"].ToString();
                htLog["{Yfdj}"] = dr["Yfdj"].ToString();
                htLog["{Dpfy}"] = dr["Dpfy"].ToString();
                htLog["{Ysts}"] = dr["Ysts"].ToString();
                AppServer.SaveModuleOpLog(this.ModuleId, "WnFormMain.SealLog", htLog); //Content_CN：封存了年份为【{Year}】、月份为【{Month}】、货代公司名称为【{HdgsName}】、装运港为【{Zyg}】、目的港为【{Mdg}】、运费单价为【{Yfdj}】、单票费用为【{Dpfy}】、运输天数为【{Ysts}】的海运费用登记信息！

                wnGridMain.DeleteRow(dr);
                dr.AcceptChanges();
            }
        }

        /// <summary>
        /// 响应“保存”按钮的Click事件
        /// </summary>
        private void tsButtonSave_Click(object sender, EventArgs e)
        {
            /* 保存数据 */
            SaveData(new List<Act> { Act.SaveData });
        }

        /// <summary>
        /// 响应“审核”按钮的Click事件
        /// </summary>
        private void tsButtonAudit_Click(object sender, EventArgs e)
        {
            /* 保存按钮可用，检查是否有未保存的数据 */
            if (tsButtonSave.Enabled && !SaveData(new List<Act> { Act.AskIsUpdate, Act.SaveData })) { return; }

            /* 若未选择行，则提示返回 */
            if (wnGridMain.SelectedRowsArray.Length == 0)
            {
                DialogBox.ShowWarning(GetCurrLanguageContent("WnGridMain.AuditWarn")); //Content_CN：请勾选需要审核的数据！
                return;
            }

            /* 获取当前服务器时间 */
            object currTime = AppServer.GetDateTime();
            if (currTime == null) { return; }

            /* 遍历选中行，获取Guid、审核人、审核时间 */
            List<string> listGuid = new List<string>();
            List<string> listAuditUser = new List<string>();
            List<string> listAuditTime = new List<string>();
            foreach (DataRow datarow in wnGridMain.SelectedRowsArray)
            {
                /* 排除已审核和新增的空行 */
                if (datarow["AuditUser"] != DBNull.Value || datarow["RowState"].Equals("0"))
                {
                    wnGridMain.SetRowUnSelected(datarow);
                    continue;
                }

                /* 将需要审核行的信息添加到集合中 */
                listGuid.Add(datarow["Guid"].ToString());
                listAuditUser.Add(AppInfo.UserName);
                listAuditTime.Add(Convert.ToDateTime(currTime).ToString("yyyy/MM/dd HH:mm:ss"));
            }

            /* 若集合中无数据，则直接返回 */
            if (listGuid.Count == 0) { return; }

            /* 执行审核语句 */
            Hashtable htAudit = new Hashtable();
            htAudit["DataSetName"] = "AuditData";
            htAudit["{Guid}"] = listGuid;
            htAudit["{AuditUser}"] = listAuditUser;
            htAudit["{AuditTime}"] = listAuditTime;
            if (this.DataSource.ExecSql(htAudit)["IsSuccess"].Equals("0")) { return; }

            /* 记录日志，并更新界面数据 */
            Hashtable htLog = new Hashtable();
            foreach (DataRow dr in wnGridMain.SelectedRowsArray)
            {
                /* 记录审核日志 */
                htLog["{Year}"] = dr["Year"].ToString();
                htLog["{Month}"] = dr["Month"].ToString();
                htLog["{HdgsName}"] = dr["HdgsName"].ToString();
                htLog["{Zyg}"] = dr["Zyg"].ToString();
                htLog["{Mdg}"] = dr["Mdg"].ToString();
                htLog["{Yfdj}"] = dr["Yfdj"].ToString();
                htLog["{Dpfy}"] = dr["Dpfy"].ToString();
                htLog["{Ysts}"] = dr["Ysts"].ToString();
                AppServer.SaveModuleOpLog(this.ModuleId, "WnFormMain.AuditLog", htLog); //Content_CN：审核了年份为【{Year}】、月份为【{Month}】、货代公司名称为【{HdgsName}】、装运港为【{Zyg}】、目的港为【{Mdg}】、运费单价为【{Yfdj}】、单票费用为【{Dpfy}】、运输天数为【{Ysts}】的海运费用登记信息！

                dr["AuditUser"] = AppInfo.UserName;
                dr["AuditTime"] = currTime;
                dr.AcceptChanges();
            }

            /* 清空选中行 */
            wnGridMain.SetRowUnSelected(wnGridMain.SelectedRowsArray);

            /* 手动触发CurrRowChanged事件，控制已审核的数据不可编辑 */
            wnGridMain_CurrRowChanged(null, null);
        }

        /// <summary>
        /// 响应“反审核”按钮的Click事件
        /// </summary>
        private void tsButtonUnAudit_Click(object sender, EventArgs e)
        {
            /* 保存按钮可用，检查是否有未保存的数据 */
            if (tsButtonSave.Enabled && !SaveData(new List<Act> { Act.AskIsUpdate, Act.SaveData })) { return; }

            /* 若未选择行，则提示返回 */
            if (wnGridMain.SelectedRowsArray.Length == 0)
            {
                DialogBox.ShowWarning(GetCurrLanguageContent("WnGridMain.UnAuditWarn")); //Content_CN：请勾选需要反审核的数据！
                return;
            }

            /* 遍历选中行，获取选中行的Guid */
            List<string> listGuid = new List<string>();
            foreach (DataRow datarow in wnGridMain.SelectedRowsArray)
            {
                /* 排除选中行为空的行 */
                if (datarow["AuditUser"] == DBNull.Value)
                {
                    wnGridMain.SetRowUnSelected(datarow);
                    continue;
                }

                /* 将获取的Guid添加到集合 */
                listGuid.Add(datarow["Guid"].ToString());
            }

            /* 若集合中无数据，则直接返回 */
            if (listGuid.Count == 0) { return; }

            /* 执行反审核语句 */
            Hashtable htUnAudit = new Hashtable();
            htUnAudit["DataSetName"] = "UnAuditData";
            htUnAudit["{Guid}"] = listGuid;
            if (this.DataSource.ExecSql(htUnAudit)["IsSuccess"].Equals("0")) { return; }

            /* 记录日志，并更新界面数据 */
            Hashtable htLog = new Hashtable();
            foreach (DataRow dr in wnGridMain.SelectedRowsArray)
            {
                /* 记录反审核日志 */
                htLog["{Year}"] = dr["Year"].ToString();
                htLog["{Month}"] = dr["Month"].ToString();
                htLog["{HdgsName}"] = dr["HdgsName"].ToString();
                htLog["{Zyg}"] = dr["Zyg"].ToString();
                htLog["{Mdg}"] = dr["Mdg"].ToString();
                htLog["{Yfdj}"] = dr["Yfdj"].ToString();
                htLog["{Dpfy}"] = dr["Dpfy"].ToString();
                htLog["{Ysts}"] = dr["Ysts"].ToString();
                AppServer.SaveModuleOpLog(this.ModuleId, "WnFormMain.UnAuditLog", htLog); //Content_CN：反审核了年份为【{Year}】、月份为【{Month}】、货代公司名称为【{HdgsName}】、装运港为【{Zyg}】、目的港为【{Mdg}】、运费单价为【{Yfdj}】、单票费用为【{Dpfy}】、运输天数为【{Ysts}】的海运费用登记信息！

                dr["AuditUser"] = DBNull.Value;
                dr["AuditTime"] = DBNull.Value;
                dr.AcceptChanges();
            }

            /* 清空选中行 */
            wnGridMain.SetRowUnSelected(wnGridMain.SelectedRowsArray);

            /* 手动触发CurrRowChanged事件，控制已审核的数据不可编辑 */
            wnGridMain_CurrRowChanged(null, null);
        }

        /// <summary>
        /// 响应“帮助”按钮的Click事件
        /// </summary>
        private void tsButtonHelp_Click(object sender, EventArgs e)
        {
            /* 若具有帮助权限，则弹出帮助窗体且可编辑，否则，只显示帮助窗体 */
            if (this.RightInfo.Contains("EditHelp"))
            {
                HelpInfo.Edit(this.FuncFrameGuid);
            }
            else
            {
                HelpInfo.Show(this.FuncFrameGuid);
            }
        }

        /// <summary>
        /// 响应“退出”按钮的Click事件
        /// </summary>
        private void tsButtonExit_Click(object sender, EventArgs e)
        {
            /* 关闭窗体 */
            this.Close();
        }

        /// <summary>
        /// 响应“封存管理”按钮的Click事件
        /// </summary>
        private void tsButtonSealManage_Click(object sender, EventArgs e)
        {
            /* 询问是否有需要保存的数据没有保存 */
            if (tsButtonSave.Enabled && !SaveData(new List<Act> { Act.AskIsUpdate, Act.SaveData })) { return; }

            /* 打开封存管理界面 */
            WnFormSealManage wnFormSealManage = new WnFormSealManage();
            if (!wnFormSealManage.Assemble(this, "WnFormSealManage")) { return; }
            wnFormSealManage.ShowDialog();

            /* 若进行了反封存操作，则重新获取数据；否则直接返回 */
            if (!wnFormSealManage.IsModify) { return; }

            /* 重新获取数据 */
            GetData();
        }

        /// <summary>
        /// 响应wnGridMain的CurrRowChanged事件
        /// </summary>
        private void wnGridMain_CurrRowChanged(object sender, CurrRowChangedArgs e)
        {
            /* 如果当前行为空，则返回 */
            if (wnGridMain.CurrRow == null) { return; }

            /* 控制界面的编辑权 */
            wnGridMain.IsAllowEdit = wnGridMain.CurrRow["AuditUser"] == DBNull.Value && this.RightInfo.Contains("EditRight");

            /* 根据当前行是否为新增行控制名称列是否可修改，若当前行不是新增行，则货代公司名称、装运港、目的港不能修改 */
            wnGridMain.Columns["HdgsName"].ReadOnly = wnGridMain.Columns["Zyg"].ReadOnly = wnGridMain.Columns["Mdg"].ReadOnly = wnGridMain.CurrRow.RowState != DataRowState.Added;
        }

        #endregion

        #region 内部方法

        ///<summary>
        ///获取数据
        /// </summary>
        private void GetData()
        {
            Hashtable ht = new Hashtable();
            ht["DataSetName"] = "GetData";
            ht["{Year}"] = tsComboYear.Value.ToString();
            ht["{Month}"] = tsComboMonth.Value.ToString();
            ht["{EditUser}"] = AppInfo.UserName;
            ht["{EditTime}"] = DateTime.Now.ToString();
            if (this.DataSource.GetDataSet(ht)["IsSuccess"].Equals("0"))
            {
                /* 记录取数失败 */
                _isSuccess = false;

                /* 控制权限 */
                SetControlRight();
                return;
            }

            /* 取数成功，记录取数成功、当前年份和月份 */
            _isSuccess = true;
            _year = tsComboYear.Value.ToString();
            _month = tsComboMonth.Value.ToString();

            /* 控制权限 */
            SetControlRight();

            return;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        private bool SaveData(List<Act> actionStep)
        {
            /* 提交数据，如果没有数据发生修改直接返回 */
            Hashtable ht = new Hashtable();
            ht["DataSetName"] = "GetData";
            ht["ActionStep"] = new List<Act>() { Act.CommitEdit };
            if (this.DataSource.UpdateDataSet(ht)["IsChanged"].Equals("0")) { return true; }

            /* 若包含AskIsUpdate选项，询问是否保存，根据用户的选择返回相应的值*/
            if (actionStep.Contains(Act.AskIsUpdate))
            {
                ht["ActionStep"] = new List<Act> { Act.AskIsUpdate };
                Hashtable result = DataSource.UpdateDataSet(ht);
                if (result["IsUpdate"].Equals("-1")) { return false; }
                if (result["IsUpdate"].Equals("0"))
                {
                    wnGridMain.RejectChanges();
                    return true;
                }

                /* 移除AskIsUpdate选项 */
                actionStep.Remove(Act.AskIsUpdate);
            }

            /* 检查必录项 */
            ht["ActionStep"] = new List<Act>() { Act.CheckData };
            if (this.DataSource.UpdateDataSet(ht)["IsSuccess"].Equals("0")) { return false; }

            /* 检查唯一索引不重复 */
            var item = from p in wnGridMain.DataSource.AsEnumerable()
                       group p by new { year = p.Field<Int16>("Year"), month = p.Field<byte>("Month"), hdgsGuid = p.Field<string>("HdgsGuid"), hdgsName = p.Field<string>("HdgsName"), zyg = p.Field<string>("Zyg"), mdg = p.Field<string>("Mdg") } into g
                       where g.Count() > 1
                       select new { g.Key.year, g.Key.month, g.Key.hdgsGuid, g.Key.hdgsName, g.Key.zyg, g.Key.mdg };
            if (item.Count() > 0)
            {
                /* 定义字符串，存储重复数据 */
                StringBuilder message = new StringBuilder();

                /* 遍历拼接重复数据，用于提示 */
                foreach (var obj in item)
                {
                    message.Append(",").AppendFormat(this.GetCurrLanguageContent("WnFormMain.CheckRepeat"), obj.year, obj.month, obj.hdgsName, obj.zyg, obj.mdg); //Content_CN：年份：‘{0}’，月份：‘{1}’，货代公司名称：‘{2}’，装运港：‘{3}’，目的港：‘{4}’
                    message.Append('\n');
                }
                DialogBox.ShowWarning(string.Format(this.GetCurrLanguageContent("WnFormMain.RepeatData"), message.ToString().Substring(1))); //Content_CN：{0}的信息存在重复值，请修改后保存！
                return false;
            }

            /* 获取数据库中本年份、本本月的数据 */
            Hashtable htCheckRepeat = new Hashtable();
            htCheckRepeat["DataSetName"] = "GetDataAll";
            htCheckRepeat["{Years}"] = _year;
            htCheckRepeat["{Months}"] = _month;
            if (this.DataSource.GetDataSet(htCheckRepeat)["IsSuccess"].Equals("0")) { return false; }
            DataTable dtAll = (this.DataSource.DataSets["CheckRepeat"] as DataSet).Tables[0];
            if (dtAll.Rows.Count > 0)
            {
                /* 获取重复的年份、月份、货代公司名称、装运港、目的港的信息，并提示 */
                var repeat = from p in dtAll.AsEnumerable()
                             join p1 in wnGridMain.DataSource.Select("RowState = '10'").AsEnumerable()
                             on new { hdgsName = p.Field<string>("HdgsName"), zyg = p.Field<string>("Zyg"), mdg = p.Field<string>("Mdg") }
                             equals new { hdgsName = p1.Field<string>("HdgsName"), zyg = p1.Field<string>("Zyg"), mdg = p1.Field<string>("Mdg") }
                             select new { hdgsName = p.Field<string>("HdgsName"), zyg = p.Field<string>("Zyg"), mdg = p.Field<string>("Mdg") };
                if (repeat.Count() > 0)
                {
                    /* 定义字符串，存储重复数据 */
                    StringBuilder message = new StringBuilder();

                    /* 遍历拼接重复数据，用于提示 */
                    foreach (var obj in repeat)
                    {
                        message.Append(",").AppendFormat(this.GetCurrLanguageContent("WnFormMain.CheckRepeat"), _year, _month, obj.hdgsName, obj.zyg, obj.mdg); //Content_CN：年份：‘{0}’，月份：‘{1}’，货代公司名称：‘{2}’，装运港：‘{3}’，目的港：‘{4}’
                        message.Append('\n');
                    }

                    DialogBox.ShowWarning(string.Format(this.GetCurrLanguageContent("WnFormMain.RepeatData"), message.ToString().Substring(1))); //Content_CN：{0}的信息存在重复值，请修改后保存！
                }
                return false;
            }

            /* 获取服务器的当前时间 */
            object currTime = AppServer.GetDateTime();
            if (currTime == null) { return false; }

            /* 为编辑人、编辑时间赋值 */
            foreach (DataRow dr in wnGridMain.DataSource.Select("RowState = '10' or RowState = '11'"))
            {
                dr["EditUser"] = AppInfo.UserName;
                dr["EditTime"] = ((DateTime)currTime).ToString("yyyy/MM/dd HH:mm:ss");
            }

            /* 保存数据 */
            ht["ActionStep"] = actionStep;
            if (this.DataSource.UpdateDataSet(ht)["IsSuccess"].Equals("0")) { return false; }

            return true;
        }

        ///<summary>
        ///控制权限
        /// </summary>
        private void SetControlRight()
        {
            bool flag = _isSuccess && tsComboYear.Value.Equals(_year) && tsComboMonth.Value.Equals(_month);
            tsButtonCarry.Enabled = tsButtonAppend.Enabled = tsButtonImport.Enabled = tsButtonSeal.Enabled = tsButtonSave.Enabled = flag && this.RightInfo.Contains("EditRight");
            tsButtonAudit.Enabled = tsButtonUnAudit.Enabled = flag && this.RightInfo.Contains("AuditRight");
        }

        #endregion
    }
}
