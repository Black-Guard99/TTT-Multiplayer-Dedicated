namespace TTT.Server.Data {
    public interface IUserRepository : IRepsitory<User> {
        void SetOnline(string id);
        void SetOffline(string id);
    }
}
