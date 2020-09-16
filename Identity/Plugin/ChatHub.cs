using Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Plugins
{
    //[Authorize]
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext db;

        public ChatHub(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message).ConfigureAwait(false);
        }

       

        public Task GetErrors(string error)
        {
            return Clients.All.SendAsync("OnErrorsReceived", error);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userid = Context.User.GetUserId();
            var user = db.Users.FirstOrDefault(x=>x.Id==userid);
            user.UserIdentity="";
            db.SaveChanges();

            return Clients.All.SendAsync("OnClientDisconnected", getAppId());
        }

        public override Task OnConnectedAsync()
        {
            return Clients.All.SendAsync("OnClientConnected", getAppId());
        }

        private string getAppId()
        {
            var httpCtx = Context.GetHttpContext();
            return httpCtx.Request.Headers["AppId"].ToString();
        }
    }
}
