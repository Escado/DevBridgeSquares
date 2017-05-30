namespace DevBridgeSquares.Entities.ApiModels
{
    public class ApiErrorResponse
    {
        public int code { get; set; }
        public string error { get; set; }
        public string field { get; set; }
        public string stackTrace { get; set; }
    }
}