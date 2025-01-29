namespace ProductsAPI.Domain.Utils
{
    public class CommandData(string command, dynamic data)
    {
        public string Command { get; set; } = command;
        public dynamic Data { get; set; } = data;
    }
}