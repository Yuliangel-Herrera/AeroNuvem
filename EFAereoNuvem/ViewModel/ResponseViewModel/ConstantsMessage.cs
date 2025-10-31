namespace EFAereoNuvem.ViewModel.ResponseViewModel;
public class ConstantsMessage
{
    public static readonly MessageResponse OBJETO_NULO = new(TypeMessage.ERRO, 1001, "O objeto informado é nulo.");
    public static readonly MessageResponse ID_DIFERENTE_DO_OBJETO = new(TypeMessage.ERRO, 1004, "O Id informado não confere com o objeto.");
    public static readonly MessageResponse ERRO_SERVIDOR = new(TypeMessage.ERRO, 1003, "Ocorreu um erro no servidor.");
    public static readonly MessageResponse ERRO_CONCORRENCIA_BANCO_DADOS = new(TypeMessage.ERRO, 1004, "Parece que esta alteração está em conflito com outra, tente novamente.");
    public static readonly string NAO_LOCALIZADA = "Não localizada.";
    public static readonly string NAO_LOCALIZADO = "Não localizado.";
    public static readonly MessageResponse ID_E_OBRIGATORIO = new(TypeMessage.ERRO, 1005, "O ID não foi informado.");
    public static readonly MessageResponse ERRO_DE_REQUISICAO_BUSCAR_POR_ID = new(TypeMessage.ERRO, 1006, "Erro ao localizar o item selecionado pelo código.");
    public static readonly MessageResponse ERRO_DE_REQUISICAO_BUSCAR_TODOS = new(TypeMessage.ERRO, 1007, "Erro ao localizar os itens selecionados.");
    public static readonly MessageResponse ERRO_DE_REQUISICAO_CADASTRAR = new(TypeMessage.ERRO, 1008, "Erro ao cadastrar.");
    public static readonly MessageResponse ERRO_DE_REQUISICAO_ALTERAR = new(TypeMessage.ERRO, 1009, "Erro ao alterar o item selecionado.");
    public static readonly MessageResponse ERRO_DE_REQUISICAO_REMOVER = new(TypeMessage.ERRO, 1010, "Erro ao remover o item selecionado.");
    public static readonly MessageResponse ERRO_DE_REQUISICAO_OBTER_OBJETO = new(TypeMessage.ERRO, 1011, "Erro ao obter objeto da requisição.");
    public static readonly MessageResponse MODEL_INVALIDO = new(TypeMessage.ERRO, 1012, "O modelo enviado é inválido.");

    // Flight
    public static readonly MessageResponse VOO_NAO_ENCONTRADO = new(TypeMessage.ERRO, 1001, "Voo com o Id informado não encontrado.");
    public static readonly MessageResponse NENHUM_VOO_DISPONIVEL = new(TypeMessage.ERRO, 1002, "Nenhum voo disponível encontrado.");
    public static readonly MessageResponse NENHUM_VOO_ENCONTRADO = new(TypeMessage.ERRO, 1002, "Nenhum voo encontrado.");
    public static readonly MessageResponse VOO_CADASTRADO_COM_SUCESSO = new(TypeMessage.SUCESSO, 201, "Voo cadastrado com sucesso.");
    public static readonly MessageResponse VOO_ATUALIZADO_COM_SUCESSO = new(TypeMessage.SUCESSO, 200, "Voo atualizado com sucesso.");
    public static readonly MessageResponse VOO_DELETADO_COM_SUCESSO = new(TypeMessage.SUCESSO, 200, "Voo deletado com sucesso.");
    public static readonly MessageResponse VOO_RECUPERADO_COM_SUCESSO = new(TypeMessage.SUCESSO, 200, "Voo recuperado com sucesso.");
    public static readonly MessageResponse VOOS_RECUPERADOS_COM_SUCESSO = new(TypeMessage.SUCESSO, 200, "Voos recuperados com sucesso.");
    public static readonly MessageResponse VOOS_DISPONIVEIS_RECUPERADOS_COM_SUCESSO = new(TypeMessage.SUCESSO, 200, "Voos disponíveis recuperados com sucesso.");

    // Account
    public static readonly MessageResponse EMAIL_JA_CADASTRADO = new(TypeMessage.ERRO, 2001, "Já existe um usuário cadastrado com este e-mail.");
    public static readonly MessageResponse CONTA_CRIADA_COM_SUCESSO = new(TypeMessage.SUCESSO, 201, "Conta criada com sucesso.");
    public static readonly MessageResponse ERRO_AO_CRIAR_CONTA = new(TypeMessage.ERRO, 2002, "Erro ao criar a conta.");
    public static readonly MessageResponse LOGIN_REALIZADO_COM_SUCESSO = new(TypeMessage.SUCESSO, 200, "Login realizado com sucesso.");
    public static readonly MessageResponse TOKEN_GERADO_COM_SUCESSO = new(TypeMessage.SUCESSO, 200, "Token gerado com sucesso.");
    public static readonly MessageResponse ERRO_AO_GERAR_TOKEN = new(TypeMessage.ERRO, 2003, "Erro ao gerar o token.");
    public static readonly MessageResponse ROLE_NAO_ENCONTRADO = new(TypeMessage.ERRO, 2004, "Role não encontrada.");
    public static readonly MessageResponse LOGOUT_REALIZADO_COM_SUCESSO = new(TypeMessage.SUCESSO, 200, "Logout realizado com sucesso.");
    public static readonly MessageResponse ERRO_AO_REALIZAR_LOGOUT = new(TypeMessage.ERRO, 2005, "Erro ao realizar logout.");
}
