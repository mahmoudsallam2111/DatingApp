namespace DatingApp.WebApi.SignalR
{
    public class PresenceTracker
    {
        private static readonly Dictionary<string, List<string>> OnlineUser = new();

        public Task<bool> UserConnected(string userName , string connectionId)
        {
            var isonline = false;
            lock (OnlineUser)  // i make a lock cause Dictionary is not a thread safe
            {
                if (OnlineUser.ContainsKey(userName))
                {
                    OnlineUser[userName].Add(connectionId);
                }
                else
                {
                    OnlineUser.Add(userName, new List<string> { connectionId});
                    isonline = true;
                }
            }
            return Task.FromResult(isonline);
        }
        public Task<bool> UserDisConnected(string userName, string connectionId)
        {
            var isOffline = false;
            lock (OnlineUser)
            {
                if (!OnlineUser.ContainsKey(userName)) return Task.FromResult(isOffline);

                OnlineUser[userName].Remove(connectionId);

                if (OnlineUser[userName].Count == 0)
                {
                    OnlineUser.Remove(userName);
                    isOffline = true;
                }
            }
           return Task.FromResult(isOffline);
        }


        public ValueTask<string[]> GetOnlineUsers()
        {
            string[] onlienUser = [];

            lock (OnlineUser)
            {
                onlienUser =  OnlineUser.OrderBy(x => x.Key).Select(x => x.Key).ToArray();
            }

            return ValueTask.FromResult(onlienUser);

        }

        public static Task<List<string>> GetConnectionsForUser(string userName)
        {
            List<string> connectionIds;
            lock (OnlineUser)
            {
                connectionIds = OnlineUser.GetValueOrDefault(userName) ?? new List<string>();
            }
            return Task.FromResult(connectionIds);
        }

    }
}
