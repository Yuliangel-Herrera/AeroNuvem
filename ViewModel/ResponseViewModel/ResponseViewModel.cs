namespace EFAereoNuvem.ViewModel.ResponseViewModel;
public class ResponseViewModel<T>
{
    public T? Data { get; private set; }
    public List<MessageResponse> Messages { get; private set; } = [];
    public int PageNumber = 1;
    public int PageSize = 25;

    // Construtor principal
    public ResponseViewModel(T data, List<MessageResponse> messages)
    {
        Data = data;
        Messages = messages ?? new List<MessageResponse>();
    }

    // Um dado + uma mensagem
    public ResponseViewModel(T data, MessageResponse message)
    {
        Data = data;
        Messages = new List<MessageResponse> { message };
    }

    // Dados + paginação
    public ResponseViewModel(T? data, int pageNumber = 1, int pageSize = 25)
    {
        Data = data;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    // Apenas mensagens
    public ResponseViewModel(List<MessageResponse> messages)
    {
        Messages = messages ?? [];
    }

    // Uma única mensagem
    public ResponseViewModel(MessageResponse message)
    {
        Messages.Add(message);
    }

    // Lista de erros simples (strings)
    public ResponseViewModel(List<string> errors)
    {
        errors?.ForEach(e =>
        {
            Messages.Add(new MessageResponse(TypeMessage.ERRO, 100, e));
        });
    }

    // Métodos auxiliares
    public bool HasError() =>
        Messages.Any(x => x.TypeMessage == TypeMessage.ERRO);

    public bool HasSuccess() =>
        Messages.Any(x => x.TypeMessage == TypeMessage.SUCESSO) || Messages.Count == 0;

    public bool HasInfo() =>
        Messages.Any(x => x.TypeMessage == TypeMessage.INFORMACAO);

    public bool HasWarning() =>
        Messages.Any(x => x.TypeMessage == TypeMessage.ALERTA);

    public List<MessageResponse> GetErrors() =>
        Messages.Where(x => x.TypeMessage == TypeMessage.ERRO).ToList();

    public List<MessageResponse> GetWarnings() =>
        Messages.Where(x => x.TypeMessage == TypeMessage.ALERTA).ToList();

    public List<MessageResponse> GetInfos() =>
        Messages.Where(x => x.TypeMessage == TypeMessage.INFORMACAO).ToList();

    public List<MessageResponse> GetSuccesses() =>
        Messages.Where(x => x.TypeMessage == TypeMessage.SUCESSO).ToList();
}
