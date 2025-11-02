namespace EFAereoNuvem.ViewModel.ResponseViewModel;
public class ConstantsMessage
{
    public static readonly MessageResponse OBJETO_NULO = new(TypeMessage.ERRO, 1001, "O objeto informado é nulo.");
    public static readonly MessageResponse ID_INVALIDO = new(TypeMessage.ERRO, 1002, "Id informado invalido.");
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
    public static readonly MessageResponse VOO_NAO_ENCONTRADO = new(TypeMessage.ERRO, 2001, "Voo com o Id informado não encontrado.");
    public static readonly MessageResponse NENHUM_VOO_DISPONIVEL = new(TypeMessage.ERRO, 2002, "Nenhum voo disponível encontrado.");
    public static readonly MessageResponse NENHUM_VOO_ENCONTRADO = new(TypeMessage.ERRO, 2003, "Nenhum voo encontrado.");
    public static readonly MessageResponse VOO_CADASTRADO_COM_SUCESSO = new(TypeMessage.SUCESSO, 2004, "Voo cadastrado com sucesso.");
    public static readonly MessageResponse ERRO_CADASTRO_VOO = new(TypeMessage.ERRO, 2005, "Erro ao cadastrar o voo.");
    public static readonly MessageResponse VOO_ATUALIZADO_COM_SUCESSO = new(TypeMessage.SUCESSO, 2006, "Voo atualizado com sucesso.");
    public static readonly MessageResponse ERRO_AO_ATUALIZAR_VOO = new(TypeMessage.ERRO, 2007, "Erro ao atualizar o voo.");
    public static readonly MessageResponse VOO_DELETADO_COM_SUCESSO = new(TypeMessage.SUCESSO, 2008, "Voo deletado com sucesso.");
    public static readonly MessageResponse VOO_RECUPERADO_COM_SUCESSO = new(TypeMessage.SUCESSO, 2009, "Voo recuperado com sucesso.");
    public static readonly MessageResponse VOOS_RECUPERADOS_COM_SUCESSO = new(TypeMessage.SUCESSO, 2010, "Voos recuperados com sucesso.");
    public static readonly MessageResponse VOOS_DISPONIVEIS_RECUPERADOS_COM_SUCESSO = new(TypeMessage.SUCESSO, 2011, "Voos disponíveis recuperados com sucesso.");
    public static readonly MessageResponse ERRO_AO_DELETAR_VOO = new(TypeMessage.ERRO, 2012, "Erro ao deletar o voo.");

    // Account
    public static readonly MessageResponse EMAIL_JA_CADASTRADO = new(TypeMessage.ERRO, 3001, "Já existe um usuário cadastrado com este e-mail.");
    public static readonly MessageResponse CONTA_CRIADA_COM_SUCESSO = new(TypeMessage.SUCESSO, 3002, "Conta criada com sucesso.");
    public static readonly MessageResponse ERRO_AO_CRIAR_CONTA = new(TypeMessage.ERRO, 3003, "Erro ao criar a conta.");
    public static readonly MessageResponse LOGIN_REALIZADO_COM_SUCESSO = new(TypeMessage.SUCESSO, 200, "Login realizado com sucesso.");
    public static readonly MessageResponse TOKEN_GERADO_COM_SUCESSO = new(TypeMessage.SUCESSO, 3004, "Token gerado com sucesso.");
    public static readonly MessageResponse ERRO_AO_GERAR_TOKEN = new(TypeMessage.ERRO, 3005, "Erro ao gerar o token.");
    public static readonly MessageResponse ROLE_NAO_ENCONTRADO = new(TypeMessage.ERRO, 3006, "Role não encontrada.");
    public static readonly MessageResponse LOGOUT_REALIZADO_COM_SUCESSO = new(TypeMessage.SUCESSO, 3007, "Logout realizado com sucesso.");
    public static readonly MessageResponse ERRO_AO_REALIZAR_LOGOUT = new(TypeMessage.ERRO, 3008, "Erro ao realizar logout.");
    public static readonly MessageResponse ERRO_EM_LOGIN = new(TypeMessage.ERRO, 3009, "Erro ao realizar o login. E-mail ou senha invalidos");

    // Airplane
    public static readonly MessageResponse AERONAVE_NAO_ENCONTRADA = new(TypeMessage.ERRO, 4001, "Aeronave com o Id informado não encontrada.");
    public static readonly MessageResponse NENHUMA_AERONAVE_ENCONTRADA = new(TypeMessage.ERRO, 4002, "Nenhuma aeronave encontrada.");
    public static readonly MessageResponse AERONAVE_CADASTRADA_COM_SUCESSO = new(TypeMessage.SUCESSO, 4003, "Aeronave cadastrada com sucesso.");
    public static readonly MessageResponse ERRO_CADASTRO_AERONAVE = new(TypeMessage.ERRO, 4004, "Erro ao cadastrar a aeronave.");
    public static readonly MessageResponse AERONAVE_ATUALIZADA_COM_SUCESSO = new(TypeMessage.SUCESSO, 4005, "Aeronave atualizada com sucesso.");
    public static readonly MessageResponse AERONAVE_EXCLUIDA_COM_SUCESSO = new(TypeMessage.SUCESSO, 4006, "Aeronave excluida com sucesso.");
    public static readonly MessageResponse ERRO_AO_EXCLUIR_AERONAVE = new(TypeMessage.ERRO, 4007, "Erro ao excluir a aeronave.");
    public static readonly MessageResponse ERRO_AO_ATUALIZAR_AERONAVE = new(TypeMessage.ERRO, 4008, "Erro ao atualizar a aeronave.");
    public static readonly MessageResponse ERRO_EXCLUIR_AERONAVE_COM_VOOS = new(TypeMessage.ERRO, 4009, "Não é possível excluir a aeronave pois existem voos associados a ela.");

    // Client
    public static readonly MessageResponse CLIENTE_NAO_ENCONTRADO = new(TypeMessage.ERRO, 5001, "Cliente com o Id informado não encontrado.");
    public static readonly MessageResponse CLIENTE_ENCONTRAD0 = new(TypeMessage.ERRO, 5002, "Nenhum cliente encontrado.");
    public static readonly MessageResponse CLIENTE_CADASTRADO_COM_SUCESSO = new(TypeMessage.SUCESSO, 5003, "Cliente cadastrado com sucesso.");
    public static readonly MessageResponse ERRO_CADASTRO_CLIENTE = new(TypeMessage.ERRO, 5004, "Erro ao cadastrar cliente.");
    public static readonly MessageResponse CLIENTE_ATUALIZADO_COM_SUCESSO = new(TypeMessage.SUCESSO, 5005, "Cliente atualizado com sucesso.");
    public static readonly MessageResponse CLIENTE_EXCLUIDO_COM_SUCESSO = new(TypeMessage.SUCESSO, 5006, "Cliente excluido com sucesso.");
    public static readonly MessageResponse ERRO_AO_EXCLUIR_CLIENTE = new(TypeMessage.ERRO, 5008, "Erro ao excluir cliente.");
    public static readonly MessageResponse ERRO_AO_ATUALIZAR_CLIENTE = new(TypeMessage.ERRO, 5007, "Erro ao atualizar cliente.");
}
