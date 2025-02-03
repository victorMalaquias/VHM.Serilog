namespace VHM.Nuget.Serilog.CommandResult
{
    [Serializable]
    public class Result
    {
        public Result() { }
        public ErrorMessage ErroMessage { get; set; } = new ErrorMessage();

        public void AddError(string message, string property)
        {
            ErroMessage.Errors.Add(new ErrorDescription { Message = message, Property = property });
        }
    }

    [Serializable]
    public class ErrorMessage
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public ICollection<ErrorDescription> Errors { get; set; }

        public ErrorMessage()
        {
            Errors = new List<ErrorDescription>();
        }
    }

    public class ErrorDescription
    {
        public string Message { get; set; }
        public string Property { get; set; }
    }
}
