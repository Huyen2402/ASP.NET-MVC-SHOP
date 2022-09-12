using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using TableDependency.SqlClient.Base.EventArgs;
using TableDependency.SqlClient;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Hubs
{
    public class ChatHubs : Hub
    {
        public ChatHubs()
        {
            var tableDependency = new SqlTableDependency<Chat>(ConfigurationManager.ConnectionStrings["QuanLyBanHang"].ConnectionString, tableName: "Chat", schemaName: "dbo", executeUserPermissionCheck: false, includeOldValues: true);
            tableDependency.OnChanged += TableDependency_Changed;
            tableDependency.OnError += TableDependency_OnError;

            tableDependency.Start();
        }

        private void TableDependency_Changed(object sender, RecordChangedEventArgs<Chat> e)
        {
            Show();
            ShowMessage();
        }

        private void TableDependency_OnError(object sender, ErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        public static void Show()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ChatHubs>();
            context.Clients.All.displayMessage();
        }

        public static void ShowMessage()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ChatHubs>();
            context.Clients.All.displayMessageChating();
        }
    }
}
