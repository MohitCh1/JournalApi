namespace JournalApi.DTO
{
    public class GeminiRequestDTO
    {
        public class GeminiRequest
        {
            public List<RequestContent> contents { get; set; } = new();
        }

        public class RequestContent
        {
            public List<RequestPart> parts { get; set; } = new();
        }

        public class RequestPart
        {
            public string text { get; set; } = string.Empty;
        }
    }
}
