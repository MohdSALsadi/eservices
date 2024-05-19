namespace Pattern_of_life.Service
{
    public interface IContextAccessor
    {
        public string UserId();
        public string UserName();
        public string UserEmail();
        public string IPAddress();
    }
}
