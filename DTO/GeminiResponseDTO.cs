namespace JournalApi.DTO
{
    public class GeminiResponseDTO
    {
        public class GeminiResponse
        {
            public List<Candidate> candidates { get; set; } = new();
        }

        public class Candidate
        {
            public ResponseContent content { get; set; } = new();
        }

        public class ResponseContent
        {
            public List<ResponsePart> parts { get; set; } = new();
        }

        public class ResponsePart
        {
            public string text { get; set; } = string.Empty;
        }
    }
}
