namespace Dealership.MvcClient.ViewModels
{
    public class ErrorViewModel
    {
        private string _message = "";

        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string Message { get => _message; set => _message = value; }
    }
}