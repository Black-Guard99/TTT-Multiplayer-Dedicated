using System;

namespace Network_Shared.Attributes {
    [AttributeUsage(AttributeTargets.Class)]
    public class HandlerRegisterAttribute : Attribute {

        public HandlerRegisterAttribute(PacketType type) {
            PacketType = type;
        }
        public PacketType PacketType { get; set; }
    }
}

