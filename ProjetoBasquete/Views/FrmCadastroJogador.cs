using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ProjetoBasquete.Data;
using ProjetoBasquete.Models;

namespace ProjetoBasquete.Views
{
    /// <summary>
    /// Tela de cadastro de jogadores de basquete - Segunda Parte.
    /// Segue o padrão da Primeira Parte: ConectaBanco com métodos bool,
    /// campo mensagem, DataTable, filtro RowFilter e exclusão com confirmação.
    /// Tema visual: Los Angeles Lakers (Roxo #552583 e Dourado #FDB927).
    /// </summary>
    public partial class FrmCadastroJogador : Form
    {
        // ==================== CONTROLES ====================
        private DataGridView dgvJogadores;
        private TextBox txtNome;
        private TextBox txtAltura;
        private ComboBox cboPosicao;
        private TextBox txtNumeroCamisa;
        private TextBox txtTime;
        private TextBox txtNacionalidade;
        private CheckBox chkPreferido;
        private TextBox txtBusca;

        private Button btnCadastrar;
        private Button btnAtualizar;
        private Button btnDeletar;
        private Button btnLimpar;
        private Button btnPreferidos;
        private Button btnGerenciarGeneros;

        private Label lblStatus;
        private int _idSelecionado = 0;
        private DataTable _tabelaJogadores;

        // ==================== CORES LAKERS ====================
        private static readonly Color RoxoLakers = ColorTranslator.FromHtml("#552583");
        private static readonly Color DouradoLakers = ColorTranslator.FromHtml("#FDB927");
        private static readonly Color RoxoEscuro = ColorTranslator.FromHtml("#3A1A5E");
        private static readonly Color FundoClaro = ColorTranslator.FromHtml("#F5F5F5");

        public FrmCadastroJogador()
        {
            InitializeComponent();
            CarregarPosicoes();
            CarregarGridJogadores();
        }

        private void InitializeComponent()
        {
            this.Text = "Cadastro de Jogadores de Basquete";
            this.ClientSize = new Size(950, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = FundoClaro;
            this.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            this.MaximizeBox = false;

            // ---------- HEADER ----------
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = RoxoLakers
            };
            var lblTitulo = new Label
            {
                Text = " CADASTRO DE JOGADORES DE BASQUETE",
                ForeColor = DouradoLakers,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 25)
            };
            pnlHeader.Controls.Add(lblTitulo);
            this.Controls.Add(pnlHeader);

            // ---------- FORMULÁRIO À ESQUERDA ----------
            var pnlForm = new Panel
            {
                Location = new Point(20, 100),
                Size = new Size(360, 460),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblNome = new Label
            {
                Text = "Nome do Jogador:",
                Location = new Point(15, 15),
                Size = new Size(320, 20),
                ForeColor = RoxoLakers,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            txtNome = new TextBox
            {
                Location = new Point(15, 40),
                Size = new Size(320, 26),
                Font = new Font("Segoe UI", 10)
            };

            var lblAltura = new Label
            {
                Text = "Altura (ex: 1.98):",
                Location = new Point(15, 75),
                Size = new Size(150, 20),
                ForeColor = RoxoLakers,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            txtAltura = new TextBox
            {
                Location = new Point(15, 100),
                Size = new Size(150, 26),
                Font = new Font("Segoe UI", 10)
            };

            var lblPosicao = new Label
            {
                Text = "Posição:",
                Location = new Point(185, 75),
                Size = new Size(100, 20),
                ForeColor = RoxoLakers,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            cboPosicao = new ComboBox
            {
                Location = new Point(185, 100),
                Size = new Size(150, 26),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };

            var lblCamisa = new Label
            {
                Text = "Número da Camisa:",
                Location = new Point(15, 135),
                Size = new Size(150, 20),
                ForeColor = RoxoLakers,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            txtNumeroCamisa = new TextBox
            {
                Location = new Point(15, 160),
                Size = new Size(150, 26),
                Font = new Font("Segoe UI", 10)
            };

            var lblTime = new Label
            {
                Text = "Time:",
                Location = new Point(185, 135),
                Size = new Size(50, 20),
                ForeColor = RoxoLakers,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            txtTime = new TextBox
            {
                Location = new Point(185, 160),
                Size = new Size(150, 26),
                Font = new Font("Segoe UI", 10)
            };

            var lblNacionalidade = new Label
            {
                Text = "Nacionalidade:",
                Location = new Point(15, 195),
                Size = new Size(150, 20),
                ForeColor = RoxoLakers,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            txtNacionalidade = new TextBox
            {
                Location = new Point(15, 220),
                Size = new Size(320, 26),
                Font = new Font("Segoe UI", 10)
            };

            chkPreferido = new CheckBox
            {
                Text = "Marcar como jogador preferido",
                Location = new Point(15, 255),
                Size = new Size(260, 24),
                BackColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 10, FontStyle.Regular)
            };

            btnCadastrar = new Button
            {
                Text = "Cadastrar",
                Location = new Point(15, 295),
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
                Location = new Point(130, 295),
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
                Location = new Point(245, 295),
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
                Location = new Point(15, 340),
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
                lblAltura, txtAltura,
                lblPosicao, cboPosicao,
                lblCamisa, txtNumeroCamisa,
                lblTime, txtTime,
                lblNacionalidade, txtNacionalidade,
                chkPreferido,
                btnCadastrar, btnAtualizar, btnDeletar, btnLimpar
            });
            this.Controls.Add(pnlForm);

            // ---------- BUSCA ----------
            var lblBusca = new Label
            {
                Text = "Buscar por nome:",
                Location = new Point(400, 105),
                Size = new Size(120, 20),
                ForeColor = RoxoLakers,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            txtBusca = new TextBox
            {
                Location = new Point(400, 130),
                Size = new Size(260, 26),
                Font = new Font("Segoe UI", 10)
            };
            txtBusca.TextChanged += TxtBusca_TextChanged;
            this.Controls.Add(lblBusca);
            this.Controls.Add(txtBusca);

            // ---------- GRID ----------
            dgvJogadores = new DataGridView
            {
                Location = new Point(400, 170),
                Size = new Size(520, 340),
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
            dgvJogadores.Columns.Add("Id", "ID");
            dgvJogadores.Columns.Add("Nome", "Nome");
            dgvJogadores.Columns.Add("Altura", "Altura");
            dgvJogadores.Columns.Add("Posicao", "Posição");
            dgvJogadores.Columns.Add("NumeroCamisa", "Camisa");
            dgvJogadores.Columns.Add("Time", "Time");
            dgvJogadores.Columns.Add("Nacionalidade", "Nacionalidade");
            dgvJogadores.Columns.Add("Preferido", "Preferido");

            dgvJogadores.Columns["Id"].Width = 40;
            dgvJogadores.Columns["Preferido"].Width = 80;

            dgvJogadores.CellClick += DgvJogadores_CellClick;
            dgvJogadores.CellFormatting += DgvJogadores_CellFormatting;
            this.Controls.Add(dgvJogadores);

            // ---------- BOTÃO PREFERIDOS ----------
            btnPreferidos = new Button
            {
                Text = "Ver Jogadores Preferidos",
                Location = new Point(400, 525),
                Size = new Size(220, 35),
                BackColor = RoxoEscuro,
                ForeColor = DouradoLakers,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnPreferidos.Click += BtnPreferidos_Click;
            this.Controls.Add(btnPreferidos);

            // ---------- BOTÃO GERENCIAR GÊNEROS ----------
            btnGerenciarGeneros = new Button
            {
                Text = "Gerenciar Gêneros",
                Location = new Point(640, 525),
                Size = new Size(220, 35),
                BackColor = RoxoLakers,
                ForeColor = DouradoLakers,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnGerenciarGeneros.Click += BtnGerenciarGeneros_Click;
            this.Controls.Add(btnGerenciarGeneros);

            // ---------- STATUS ----------
            lblStatus = new Label
            {
                Location = new Point(400, 570),
                Size = new Size(520, 25),
                ForeColor = RoxoLakers,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Text = "Pronto"
            };
            this.Controls.Add(lblStatus);
        }

        // ==================== CARREGAMENTO ====================

        private void CarregarPosicoes()
        {
            try
            {
                var con = new ConectaBanco();
                var dt = con.listaPosicoes();
                if (dt != null)
                {
                    cboPosicao.DataSource = dt;
                    cboPosicao.DisplayMember = "Nome";
                    cboPosicao.ValueMember = "idPosicao";
                    cboPosicao.SelectedIndex = -1;
                }
                else
                {
                    MessageBox.Show(con.mensagem, "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar posições: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarGridJogadores()
        {
            try
            {
                var con = new ConectaBanco();
                _tabelaJogadores = con.listaJogadores();

                if (_tabelaJogadores != null)
                {
                    dgvJogadores.DataSource = _tabelaJogadores;
                    lblStatus.Text = $"{_tabelaJogadores.Rows.Count} jogador(es) encontrado(s)";
                }
                else
                {
                    lblStatus.Text = con.mensagem;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar jogadores: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== FILTRO POR TEXTO (RowFilter) ====================

        private void TxtBusca_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (_tabelaJogadores != null && dgvJogadores.DataSource is DataTable)
                {
                    (dgvJogadores.DataSource as DataTable).DefaultView.RowFilter =
                        string.Format("Nome like '{0}%'", txtBusca.Text);
                }
            }
            catch
            {
                // Ignora erros temporários enquanto digita
            }
        }

        // ==================== EVENTOS DO GRID ====================

        private void DgvJogadores_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var linha = dgvJogadores.Rows[e.RowIndex];
                _idSelecionado = Convert.ToInt32(linha.Cells["Id"].Value);

                txtNome.Text = linha.Cells["Nome"].Value?.ToString();
                txtAltura.Text = linha.Cells["Altura"].Value?.ToString();
                cboPosicao.Text = linha.Cells["Posicao"].Value?.ToString();
                txtNumeroCamisa.Text = linha.Cells["Camisa"].Value?.ToString();
                txtTime.Text = linha.Cells["Time"].Value?.ToString();
                txtNacionalidade.Text = linha.Cells["Nacionalidade"].Value?.ToString();
                chkPreferido.Checked = linha.Cells["Preferido"].Value?.ToString() == " Sim" ||
                                       linha.Cells["Preferido"].Value?.ToString() == "True";

                btnAtualizar.Enabled = true;
                btnDeletar.Enabled = true;
                btnCadastrar.Text = "Cadastrar";
                lblStatus.Text = $"Jogador selecionado: {txtNome.Text}";
            }
        }

        private void DgvJogadores_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvJogadores.Columns[e.ColumnIndex].Name == "Preferido" && e.Value != null)
            {
                if (e.Value.ToString() == "True" || e.Value.ToString() == "1")
                {
                    e.CellStyle.ForeColor = DouradoLakers;
                    e.CellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                    e.Value = " Sim";
                }
                else
                {
                    e.Value = "Não";
                }
            }
        }

        // ==================== MÉTODOS AUXILIARES ====================

        private Jogador ObterDadosFormulario()
        {
            return new Jogador
            {
                Id = _idSelecionado,
                Nome = txtNome.Text.Trim(),
                Altura = string.IsNullOrWhiteSpace(txtAltura.Text) ? 0 : Convert.ToDecimal(txtAltura.Text),
                FkPosicao = cboPosicao.SelectedValue != null ? Convert.ToInt32(cboPosicao.SelectedValue) : 0,
                NumeroCamisa = string.IsNullOrWhiteSpace(txtNumeroCamisa.Text) ? 0 : Convert.ToInt32(txtNumeroCamisa.Text),
                Time = txtTime.Text.Trim(),
                Nacionalidade = txtNacionalidade.Text.Trim(),
                Preferido = chkPreferido.Checked
            };
        }

        private bool ValidarFormulario()
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Informe o nome do jogador.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNome.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtAltura.Text))
            {
                MessageBox.Show("Informe a altura do jogador.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAltura.Focus();
                return false;
            }
            if (cboPosicao.SelectedIndex < 0)
            {
                MessageBox.Show("Selecione a posição.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboPosicao.Focus();
                return false;
            }
            return true;
        }

        private void LimparCampos()
        {
            txtNome.Clear();
            txtAltura.Clear();
            cboPosicao.SelectedIndex = -1;
            txtNumeroCamisa.Clear();
            txtTime.Clear();
            txtNacionalidade.Clear();
            chkPreferido.Checked = false;
            _idSelecionado = 0;
            btnAtualizar.Enabled = false;
            btnDeletar.Enabled = false;
            btnCadastrar.Text = "Cadastrar";
            lblStatus.Text = "Campos limpos";
        }

        // ==================== EVENTOS DOS BOTÕES ====================

        private void BtnCadastrar_Click(object sender, EventArgs e)
        {
            if (!ValidarFormulario()) return;

            var jogador = ObterDadosFormulario();
            var con = new ConectaBanco();

            if (_idSelecionado == 0)
            {
                // Inserir (padrão insereJogador / insereBanda)
                bool sucesso = con.insereJogador(jogador);
                if (sucesso)
                {
                    MessageBox.Show("Jogador cadastrado com sucesso!");
                    CarregarGridJogadores();
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
                // Atualizar
                bool sucesso = con.atualizaJogador(jogador);
                if (sucesso)
                {
                    MessageBox.Show("Jogador atualizado com sucesso!");
                    CarregarGridJogadores();
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
                MessageBox.Show("Selecione um jogador na lista para atualizar.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            BtnCadastrar_Click(sender, e);
        }

        private void BtnDeletar_Click(object sender, EventArgs e)
        {
            if (_idSelecionado == 0)
            {
                MessageBox.Show("Selecione um jogador na lista para excluir.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int linha = dgvJogadores.CurrentRow.Index;
            int id = Convert.ToInt32(dgvJogadores.Rows[linha].Cells["Id"].Value.ToString());

            DialogResult resp = MessageBox.Show("Tem certeza que deseja excluir?",
                "Remover Jogador", MessageBoxButtons.OKCancel);

            if (resp == DialogResult.OK)
            {
                var con = new ConectaBanco();
                bool retorno = con.deletaJogador(id);
                if (retorno)
                {
                    MessageBox.Show("Jogador excluído com sucesso!");
                    CarregarGridJogadores();
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

        private void BtnPreferidos_Click(object sender, EventArgs e)
        {
            try
            {
                var con = new ConectaBanco();
                var dt = con.listaJogadores();

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("Nenhum jogador cadastrado.", "Jogadores Preferidos",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var msg = " JOGADORES PREFERIDOS \n\n";
                foreach (DataRow row in dt.Rows)
                {
                    if (Convert.ToBoolean(row["Preferido"]))
                    {
                        msg += $"• {row["Nome"]} - {row["Time"]} ({row["Posicao"]})\n";
                    }
                }

                if (msg == " JOGADORES PREFERIDOS \n\n")
                {
                    MessageBox.Show("Nenhum jogador marcado como preferido ainda.", "Jogadores Preferidos",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                MessageBox.Show(msg, "Jogadores Preferidos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao listar preferidos: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGerenciarGeneros_Click(object sender, EventArgs e)
        {
            var frmGenero = new FrmCadastroGenero();
            frmGenero.ShowDialog();
        }
    }
}