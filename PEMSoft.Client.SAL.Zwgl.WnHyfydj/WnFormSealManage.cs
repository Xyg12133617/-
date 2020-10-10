using PEMSoft.Client.SYS.WnControl;
using PEMSoft.Client.SYS.WnPlatform;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace PEMSoft.Client.SAL.Zwgl.WnHyfydj
{
    public partial class WnFormSealManage : WnForm
    {
        #region 变量

        /// <summary>
        /// 记录是否进行了反封存操作
        /// </summary>
        internal bool IsModify;

        #endregion

        #region 控件变量及初始化

        private WnToolBar wnToolBarSealManage;
        private WnTbButton tsButtonUnSeal;
        private WnSeparator tsSeparator9;
        private WnTbButton tsButtonExitSealManage;
        private WnGrid wnGridSealManage;

        /// <summary>
        /// 控件变量初始化方法
        /// </summary>
        private void InitializeObject()
        {
            wnToolBarSealManage = this.AllObjects["wnToolBarSealManage"] as WnToolBar;
            tsButtonUnSeal = this.AllObjects["tsButtonUnSeal"] as WnTbButton;
            tsSeparator9 = this.AllObjects["tsSeparator9"] as WnSeparator;
            tsButtonExitSealManage = this.AllObjects["tsButtonExitSealManage"] as WnTbButton;
            wnGridSealManage = this.AllObjects["wnGridSealManage"] as WnGrid;
        }

        #endregion

        #region 窗体方法

        /// <summary>
        /// 构造函数
        /// </summary>
        public WnFormSealManage()
        {
            InitializeComponent();

            /* 注册窗体事件 */
            this.AfterAssemble += WnFormSealManage_AfterAssemble;
        }

        /// <summary>
        /// 响应窗体的AfterAssemble事件
        /// </summary>
        private void WnFormSealManage_AfterAssemble(object sender, CancelEventArgs e)
        {
            /* 调用方法，初始化控件 */
            InitializeObject();

            tsButtonUnSeal.Click += tsButtonUnSeal_Click;
            tsButtonExitSealManage.Click += tsButtonExitSealManage_Click;

            /* 获取数据 */
            Hashtable ht = new Hashtable() { { "DataSetName", "GetSealData" } };
            if (this.DataSource.GetDataSet(ht)["IsSuccess"].Equals("0")) { e.Cancel = true; }
        }

        #endregion

        #region 对象方法

        /// <summary>
        /// 响应“反封存”按钮的Click事件
        /// </summary>
        private void tsButtonUnSeal_Click(object sender, EventArgs e)
        {
            /* 选中一行，若当前行为null，则直接返回 */
            DataRow dr = wnGridSealManage.CurrRow;
            if (dr == null) { return; }

            /* 询问是否确定要进行反封存操作 */
            if (DialogBox.AskYesNo(this.GetCurrLanguageContent("WnFormSealManage.AskUnSealData")) == DialogResult.No) { return; } //Content_CN：是否确定要进行反封存操作？

            /* 获取服务器的当前时间 */
            object currTime = AppServer.GetDateTime();
            if (currTime == null) { return; }

            /* 执行SQL语句，执行反封存操作 */
            Hashtable htUnSeal = new Hashtable();
            htUnSeal["DataSetName"] = "UnSealData";
            htUnSeal["{Guid}"] = dr["Guid"].ToString();
            htUnSeal["{EditUser}"] = AppInfo.UserName;
            htUnSeal["{EditTime}"] = ((DateTime)currTime).ToString("yyyy/MM/dd HH:mm:ss");
            if (this.DataSource.ExecSql(htUnSeal)["IsSuccess"].Equals("0")) { return; }

            /* 标记进行了反封存操作 */
            IsModify = true;

            /* 记录反封存日志 */
            Hashtable htLog = new Hashtable();
            htLog["{Year}"] = dr["Year"].ToString();
            htLog["{Month}"] = dr["Month"].ToString();
            htLog["{HdgsName}"] = dr["HdgsName"].ToString();
            htLog["{Zyg}"] = dr["Zyg"].ToString();
            htLog["{Mdg}"] = dr["Mdg"].ToString();
            htLog["{Yfdj}"] = dr["Yfdj"].ToString();
            htLog["{Dpfy}"] = dr["Dpfy"].ToString();
            htLog["{Ysts}"] = dr["Ysts"].ToString();
            AppServer.SaveModuleOpLog(this.ModuleId, "WnFormMain.UnSealLog", htLog); //Content_CN：反封存了年份为【{Year}】、月份为【{Month}】、货代公司名称为【{HdgsName}】、装运港为【{Zyg}】、目的港为【{Mdg}】、运费单价为【{Yfdj}】、单票费用为【{Dpfy}】、运输天数为【{Ysts}】的海运费用登记信息！

            /* 从界面上删除数据 */
            wnGridSealManage.DeleteRow(dr);
            dr.AcceptChanges();
        }

        /// <summary>
        /// 响应“退出”按钮的Click事件
        /// </summary>
        private void tsButtonExitSealManage_Click(object sender, EventArgs e)
        {
            /* 关闭窗体 */
            this.Close();
        }

        #endregion
    }
}
