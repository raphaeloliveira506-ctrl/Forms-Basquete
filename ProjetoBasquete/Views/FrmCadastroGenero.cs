using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ProjetoBasquete.Data;
using ProjetoBasquete.Models;

namespace ProjetoBasquete.Views
{
    /// <summary>
    /// Tela de cadastro de gêneros.
    /// Segue o padrão da Primeira Parte: ConectaBanco com métodos bool,
    /// campo mensagem, DataTable, filtro RowFilter e exclusão com confirmação.
    /// </summary>
    public partial class FrmCadastroGenero : Form
    {
        private DataGridView dgvGeneros;
        private TextBox txtNome;
        private TextBox txtBusca;

        private Button btnCadastrar;
        private Button btnAtualizar;
        private Button btnDeletar;
        private Button btnLimpar;

        private Label lblStatus;
        private int _idSelecionado = 0;
        private DataTable _tabelaGeneros;

        // Cores Lakers
        private static readonly Color RoxoLakers = ColorTranslator.FromHtml("#552583");
        private static readonly Color DouradoLakers = ColorTranslator.FromHtml("#FDB927");
        private static readonly Color FundoClaro = ColorTranslator.FromHtml("#F5F5F5");

        public FrmCadastroGenero()
        {
            InitializeComponent();
            CarregarGridGeneros();
        }

        private void InitializeComponent()
        {
            this.Text = "Cadastro de Gêneros";
            this.ClientSize = new Size(800, 520);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = FundoClaro;
            this.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            this.MaximizeBox = false;

            // ---------- HEADER ----------
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = RoxoLakers
            };
            var lblTitulo = new Label
            {
                Text = "🎵 CADASTRO DE GÊNEROS",
                ForeColor = DouradoLakers,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 20)
            };
            pnlHeader.Controls.Add(lblTitulo);
            this.Controls.Add(pnlHeader);

            // ---------- FORMULÁRIO À ESQUERDA ----------
            var pnlForm = new Panel
            {
                Location = new Point(20, 90),
                Size = new Size(300, 380),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblNome = new Label
            {
                Text = "Nome do Gênero:",
                Location = new Point(15, 15),
                Size = new Size(260, 20),
                ForeColor = RoxoLakers,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            txtNome = new TextBox
            {
                Location = new Point(15, 40),
                Size = new Size(260, 26),
                Font = new Font("Segoe UI", 10)
            };

            btnCadastrar = new Button
            {
                Text = "Cadastrar",
                Location = new Point(15, 90),
                Size = new Size(100, 35),
                BackColor = RoxoLakers,
                ForeColor = DouradoLakers,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCadastrar.Click += BtnCadastrar_Click;

            btnAtualizar = new Button
            {
                Text = "Atualizar",
                Location = new Point(130, 90),
                Size = new Size(100, 35),
                BackColor = RoxoLakers,
                ForeColor = DouradoLakers,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnAtualizar.Click += BtnAtualizar_Click;

            btnDeletar = new Button
            {
                Text = "Deletar",
                Location = new Point(215, 90),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(180, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnDeletar.Click += BtnDeletar_Click;

            btnLimpar = new Button
            {
                Text = "Limpar",
                Location = new Point(15, 135),
                Size = new Size(100, 35),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnLimpar.Click += BtnLimpar_Click;

            pnlForm.Controls.AddRange(new Control[]
            {
                lblNome, txtNome,
                btnCadastrar, btnAtualizar, btnDeletar, btnLimpar
            });
            this.Controls.Add(pnlForm);

            // ---------- BUSCA ----------
            var lblBusca = new Label
            {
                Text = "Buscar por nome:",
                Location = new Point(340, 100),
                Size = new Size(120, 20),
                ForeColor = RoxoLakers,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            txtBusca = new TextBox
            {
                Location = new Point(340, 125),
                Size = new Size(260, 26),
                Font = new Font("Segoe UI", 10)
            };
            txtBusca.TextChanged += TxtBusca_TextChanged;
            this.Controls.Add(lblBusca);
            this.Controls.Add(txtBusca);

            // ---------- GRID ----------
            dgvGeneros = new DataGridView
            {
                Location = new Point(340, 165),
                Size = new Size(440, 300),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = RoxoLakers,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                }
            };
            dgvGeneros.Columns.Add("idGenero", "ID");
            dgvGeneros.Columns.Add("Nome", "Nome");

            dgvGeneros.Columns["idGenero"].Width = 50;
            dgvGeneros.CellClick += DgvGeneros_CellClick;
            this.Controls.Add(dgvGeneros);

            // ---------- STATUS ----------
            lblStatus = new Label
            {
                Location = new Point(340, 480),
                Size = new Size(440, 25),
                ForeColor = RoxoLakers,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Text = "Pronto"
            };
            this.Controls.Add(lblStatus);
        }

        private void CarregarGridGeneros()
        {
            try
            {
                var con = new ConectaBanco();
                _tabelaGeneros = con.listaGeneros();

                if (_tabelaGeneros != null)
                {
                    dgvGeneros.DataSource = _tabelaGeneros;
                    lblStatus.Text = $"{_tabelaGeneros.Rows.Count} gênero(s) encontrado(s)";
                }
                else
                {
                    lblStatus.Text = con.mensagem;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar gêneros: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtBusca_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (_tabelaGeneros != null && dgvGeneros.DataSource is DataTable)
                {
                    (dgvGeneros.DataSource as DataTable).DefaultView.RowFilter =
                        string.Format("Nome like '{0}%'", txtBusca.Text);
                }
            }
            catch
            {
                // Ignora erros temporários
            }
        }

        private void DgvGeneros_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var linha = dgvGeneros.Rows[e.RowIndex];
                _idSelecionado = Convert.ToInt32(linha.Cells["idGenero"].Value);
                txtNome.Text = linha.Cells["Nome"].Value?.ToString();

                btnAtualizar.Enabled = true;
                btnDeletar.Enabled = true;
                btnCadastrar.Text = "Cadastrar";
                lblStatus.Text = $"Gênero selecionado: {txtNome.Text}";
            }
        }

        private Genero ObterDadosFormulario()
        {
            return new Genero
            {
                Id = _idSelecionado,
                Nome = txtNome.Text.Trim()
            };
        }

        private bool ValidarFormulario()
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Informe o nome do gênero.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNome.Focus();
                return false;
            }
            return true;
        }

        private void LimparCampos()
        {
            txtNome.Clear();
            _idSelecionado = 0;
            btnAtualizar.Enabled = false;
            btnDeletar.Enabled = false;
            btnCadastrar.Text = "Cadastrar";
            lblStatus.Text = "Campos limpos";
        }

        private void BtnCadastrar_Click(object sender, EventArgs e)
        {
            if (!ValidarFormulario()) return;

            var genero = ObterDadosFormulario();
            var con = new ConectaBanco();

            if (_idSelecionado == 0)
            {
                bool sucesso = con.insereGenero(genero);
                if (sucesso)
                {
                    MessageBox.Show("Gênero cadastrado com sucesso!");
                    CarregarGridGeneros();
                    LimparCampos();
                }
                else
                {
                    MessageBox.Show(con.mensagem, "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                bool sucesso = con.atualizaGenero(genero);
                if (sucesso)
                {
                    MessageBox.Show("Gênero atualizado com sucesso!");
                    CarregarGridGeneros();
                    LimparCampos();
                }
                else
                {
                    MessageBox.Show(con.mensagem, "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnAtualizar_Click(object sender, EventArgs e)
        {
            if (_idSelecionado == 0)
            {
                MessageBox.Show("Selecione um gênero na lista para atualizar.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            BtnCadastrar_Click(sender, e);
        }

        private void BtnDeletar_Click(object sender, EventArgs e)
        {
            if (_idSelecionado == 0)
            {
                MessageBox.Show("Selecione um gênero na lista para excluir.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult resp = MessageBox.Show("Tem certeza que deseja excluir?",
                "Remover Gênero", MessageBoxButtons.OKCancel);

            if (resp == DialogResult.OK)
            {
                var con = new ConectaBanco();
                bool retorno = con.deletaGenero(_idSelecionado);
                if (retorno)
                {
                    MessageBox.Show("Gênero excluído com sucesso!");
                    CarregarGridGeneros();
                    LimparCampos();
                }
                else
                {
                    MessageBox.Show(con.mensagem);
                }
            }
            else
            {
                MessageBox.Show("Exclusão cancelada");
            }
        }

        private void BtnLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }
    }
}