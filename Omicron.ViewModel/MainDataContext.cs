using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BingLibrary.hjb;
using BingLibrary.hjb.Intercepts;
using System.ComponentModel.Composition;
using SxjLibrary;
using System.Windows;
using System.Collections.ObjectModel;
using System.Net;


namespace Omicron.ViewModel
{
    [BingAutoNotify]
    public class MainDataContext : DataSource
    {
        public virtual string AboutPageVisibility { set; get; } = "Collapsed";
        public virtual string HomePageVisibility { set; get; } = "Visible";
        public virtual string Msg { set; get; } = "";
        public virtual bool IsTCPConnect { set; get; }
        public virtual int[] PositionComboBox { set; get; } = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        public virtual int PositionComboBoxSelectedIndex { set; get; } = 0;
        private MessagePrint messagePrint = new MessagePrint();
        private dialog mydialog = new dialog();
        private string ip = "192.168.100.228";
        private TcpIpServer tcpIpServer = new TcpIpServer(IPAddress.Parse("192.168.100.228"), 2001);

        public void ChoseHomePage()
        {
            AboutPageVisibility = "Collapsed";
            HomePageVisibility = "Visible";
            //Msg = messagePrint.AddMessage("111");
        }
        public void ChoseAboutPage()
        {
            AboutPageVisibility = "Visible";
            HomePageVisibility = "Collapsed";
        }
        [Initialize]
        public async void UpdateUI()
        {
            bool f = false;
            while (true)
            {
                IsTCPConnect = tcpIpServer.IsConnect;
                if (f == false && IsTCPConnect)
                {
                    f = true;
                    Msg = messagePrint.AddMessage("检测到客户端连接");
                }
                await Task.Delay(500);
            }
        }
        public async void StartMoveAction()
        {
            if (IsTCPConnect)
            {
                await tcpIpServer.SendAsync((PositionComboBoxSelectedIndex+1).ToString());
                Msg = messagePrint.AddMessage("发送： " + (PositionComboBoxSelectedIndex + 1).ToString());
            }
        }
        [Export(MEF.Contracts.ActionMessage)]
        [ExportMetadata(MEF.Key, "winclose")]
        public async void WindowClose()
        {
            mydialog.changeaccent("Red");
            var r = await mydialog.showconfirm("确定要关闭程序吗？");
            if (r)
            {
                System.Windows.Application.Current.Shutdown();
            }
            else
            {
                mydialog.changeaccent("Cobalt");
            }
        }
    }
}