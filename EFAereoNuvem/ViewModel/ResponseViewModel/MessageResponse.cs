namespace EFAereoNuvem.ViewModel.ResponseViewModel;
public class MessageResponse(string typeMessage, int code, string message)
{
    public string TypeMessage { get; set; } = typeMessage;
    public int Code { get; set; } = code;
    public string Message { get; set; } = message;

    public override string? ToString()
    {
        return TypeMessage + "-> " + Message;
    }
}
