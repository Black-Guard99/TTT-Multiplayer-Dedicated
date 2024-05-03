namespace TTT.Server.Data {
    public class User {
        public string id { get; set; }
        public string password { get; set; }

        public ushort score { get; set; }
        public bool IsOnline { get; set; }
    }
}
