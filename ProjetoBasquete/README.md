# Projeto Basquete - Cadastro de Jogadores

Projeto em **C# Windows Forms** com arquitetura em camadas, banco **SQL Server** e **Stored Procedures**.
Tema visual: **Los Angeles Lakers** (Roxo `#552583` e Dourado `#FDB927`).

---

##  Estrutura do Projeto

```
ProjetoBasquete/
├── Banco.sql                    # Script SQL (tabela + stored procedures)
├── Program.cs                   # Ponto de entrada da aplicação
├── ProjetoBasquete.csproj       # Configuração do projeto .NET 6
├── Models/
│   └── Jogador.cs               # Classe modelo (entidade)
├── Data/
│   ├── ConexaoBanco.cs          # Conexão com o banco (credenciais)
│   └── JogadorDAL.cs            # Camada de acesso a dados (CRUD)
└── Views/
    └── FrmCadastroJogador.cs    # Interface gráfica (Windows Forms)
```

---

##  1. Configurar o Banco de Dados (SQL Server)

1. Abra o **SQL Server Management Studio (SSMS)** ou o **Visual Studio** (SQL Server Object Explorer).
2. Execute o script `Banco.sql` para criar:
   - Banco `DB_JogadoresBasquete`
   - Tabela `Jogadores`
   - Stored Procedures: `sp_InserirJogador`, `sp_ListarJogadores`, `sp_BuscarJogadorPorId`, `sp_AtualizarJogador`, `sp_DeletarJogador`, `sp_AlternarPreferido`

---

##  2. Ajustar a String de Conexão

Abra o arquivo `Data/ConexaoBanco.cs` e altere conforme seu ambiente:

```csharp
// Autenticação do Windows (padrão):
private readonly string _stringConexao =
    @"Data Source=localhost\SQLEXPRESS;Initial Catalog=DB_JogadoresBasquete;Integrated Security=True;";

// Ou com usuário e senha do SQL Server:
// private readonly string _stringConexao =
//     @"Data Source=localhost\SQLEXPRESS;Initial Catalog=DB_JogadoresBasquete;User ID=sa;Password=SUA_SENHA;";
```

> Altere `localhost\SQLEXPRESS` para o nome do seu servidor SQL, se necessário.

---

## ▶️ 3. Executar o Projeto

### Com Visual Studio
1. Abra o Visual Studio.
2. Vá em **Arquivo > Abrir > Projeto/Solução** e selecione `ProjetoBasquete.csproj`.
3. Restaure os Pacotes NuGet (clique com o botão direito na solução > **Restaurar Pacotes NuGet**).
4. Pressione **F5** para executar.

### Com .NET CLI
```bash
cd ProjetoBasquete
dotnet restore
dotnet run
```

---

##  Funcionalidades da Tela

| Funcionalidade | Descrição |
|---|---|
| **Cadastrar** | Insere um novo jogador via `sp_InserirJogador` |
| **Atualizar** | Altera dados do jogador selecionado via `sp_AtualizarJogador` |
| **Deletar** | Exclui o jogador selecionado via `sp_DeletarJogador` |
| **Limpar** | Limpa os campos do formulário |
| **Ver Jogadores Preferidos** | Exibe apenas os jogadores marcados como preferidos |
| **CheckBox Preferido** | Marca/desmarca o jogador como favorito (salvo na coluna `Preferido`) |

---

##  Camadas (Arquitetura)

1. **Camada de Apresentação (`Views/`)** — Windows Forms onde o usuário digita os dados.
2. **Camada de Modelo (`Models/`)** — Classe `Jogador` que recebe os dados da interface.
3. **Camada de Conexão (`Data/ConexaoBanco.cs`)** — Define nome do banco, credenciais e abre a conexão SQL.
4. **Camada de Acesso a Dados (`Data/JogadorDAL.cs`)** — Recebe o objeto `Jogador`, mapeia os parâmetros e chama as **Stored Procedures** no banco.

---

## Pronto!

Após configurar o banco e a string de conexão, você poderá cadastrar, listar, atualizar, excluir e marcar jogadores preferidos, tudo com o visual roxo e dourado do Lakers.

Boa sorte no seu projeto final! 