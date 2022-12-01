using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using TableDependency.SqlClient.Base.EventArgs;
using TableDependency.SqlClient;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Hubs
{
    public class Chat2Hub : Hub
    {
        public Chat2Hub()
        {
            

            
            var tableDependency = new SqlTableDependency<Chat2>(ConfigurationManager.ConnectionStrings["QuanLyBanHang"].ConnectionString, tableName: "Chat2", schemaName: "dbo", executeUserPermissionCheck: false, includeOldValues: true);
            tableDependency.OnChanged += TableDependency_OnChanged;
            tableDependency.OnError += TableDependency_OnError;
            tableDependency.Start();
        }

        private void TableDependency_OnError(object sender, ErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TableDependency_OnChanged(object sender, RecordChangedEventArgs<Chat2> e)
        {
            Show();
            ShowMessage();
        }
        public static void Show()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<Chat2Hub>();
            context.Clients.All.displayMessage();
        }

        public static void ShowMessage()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<Chat2Hub>();
            context.Clients.All.displayMessageChating();
        }
    }
}