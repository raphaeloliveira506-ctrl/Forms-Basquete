using System;
using System.Drawing;
using System.Windows.Forms;
using ProjetoBasquete.Data;
using ProjetoBasquete.Models;

namespace ProjetoBasquete.Views
{
    /// <summary>
    /// Formulário de Login - Acesso ao sistema.
    /// Segue o padrão da Primeira Parte: ConectaBanco com método validarLogin.
    /// </summary>
    public partial class FrmLogin : Form
    {
        private TextBox txtUsuario;
        private TextBox txtSenha;
        private Button btnEntrar;
        private Button btnSair;
        private Label lblStatus;

        public Usuario UsuarioLogado { get; private set; }

        public FrmLogin()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Login - Cadastro de Jogadores de Basquete";
            this.ClientSize = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ColorTranslator.FromHtml("#F5F5F5");
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            var lblTitulo = new Label
            {
                Text = "🏀 LOGIN",
                ForeColor = ColorTranslator.FromHtml("#552583"),
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(150, 30)
            };

            var lblUsuario = new Label
            {
                Text = "Usuário:",
                Location = new Point(60, 90),
                Size = new Size(80, 20),
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            txtUsuario = new TextBox
            {
                Location = new Point(150, 87),
                Size = new Size(180, 26),
                Font = new Font("Segoe UI", 10)
            };

            var lblSenha = new Label
            {
                Text = "Senha:",
                Location = new Point(60, 130),
                Size = new Size(80, 20),
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            txtSenha = new TextBox
            {
                Location = new Point(150, 127),
                Size = new Size(180, 26),
                Font = new Font("Segoe UI", 10),
                UseSystemPasswordChar = true
            };

            btnEntrar = new Button
            {
                Text = "Entrar",
                Location = new Point(100, 180),
                Size = new Size(100, 35),
                BackColor = ColorTranslator.FromHtml("#552583"),
                ForeColor = ColorTranslator.FromHtml("#FDB927"),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnEntrar.Click += BtnEntrar_Click;

            btnSair = new Button
            {
                Text = "Sair",
                Location = new Point(210, 180),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(180, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSair.Click += (s, e) => Application.Exit();

            lblStatus = new Label
            {
                Location = new Point(60, 230),
                Size = new Size(280, 25),
                ForeColor = ColorTranslator.FromHtml("#552583"),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Text = "Pronto"
            };

            this.Controls.AddRange(new Control[]
            {
                lblTitulo, lblUsuario, txtUsuario,
                lblSenha, txtSenha, btnEntrar, btnSair, lblStatus
            });

            this.AcceptButton = btnEntrar;
        }

        private void BtnEntrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                MessageBox.Show("Informe o usuário e a senha.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var con = new ConectaBanco();
            UsuarioLogado = con.validarLogin(txtUsuario.Text.Trim(), txtSenha.Text);

            if (UsuarioLogado != null)
            {
                lblStatus.Text = $"Bem-vindo, {UsuarioLogado.NomeCompleto}!";
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                lblStatus.Text = con.mensagem;
                MessageBox.Show("Usuário ou senha inválidos.", "Erro de Login",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSenha.Clear();
                txtSenha.Focus();
            }
        }
    }
}