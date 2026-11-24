5-- =============================================
-- Script para MySQL Workbench
-- Projeto: Cadastro de Jogadores de Basquete
-- Padrão: Primeira Parte (constantes, mensagem, bool)
-- =============================================

CREATE DATABASE IF NOT EXISTS DB_JogadoresBasquete
  CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

USE DB_JogadoresBasquete;

-- ---------------------------------------------------------
-- Tabela de posições
-- ---------------------------------------------------------
CREATE TABLE IF NOT EXISTS Posicoes (
    idPosicao INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ---------------------------------------------------------
-- Tabela de jogadores
-- ---------------------------------------------------------
CREATE TABLE IF NOT EXISTS Jogadores (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Altura DECIMAL(4,2) NOT NULL,
    fk_posicao INT NOT NULL,
    NumeroCamisa INT NOT NULL,
    `Time` VARCHAR(100) NOT NULL,
    Nacionalidade VARCHAR(50) NOT NULL,
    Preferido TINYINT(1) NOT NULL DEFAULT 0,
    DataCadastro DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (fk_posicao) REFERENCES Posicoes(idPosicao)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ---------------------------------------------------------
-- Stored Procedures (MySQL)
-- ---------------------------------------------------------

DELIMITER //

-- Listar posições
CREATE PROCEDURE sp_listaPosicoes()
BEGIN
    SELECT idPosicao, Nome
    FROM Posicoes
    ORDER BY Nome;
END //

-- Listar jogadores com INNER JOIN nas posições
CREATE PROCEDURE sp_listaJogadores()
BEGIN
    SELECT 
        j.Id,
        j.Nome,
        j.Altura,
        p.Nome AS Posicao,
        j.NumeroCamisa,
        j.`Time`,
        j.Nacionalidade,
        j.Preferido,
        j.DataCadastro
    FROM Jogadores j
    INNER JOIN Posicoes p
        ON j.fk_posicao = p.idPosicao
    ORDER BY j.Nome;
END //

-- Buscar jogador por ID
CREATE PROCEDURE sp_buscaJogadorPorId(IN p_id INT)
BEGIN
    SELECT 
        j.Id,
        j.Nome,
        j.Altura,
        p.Nome AS Posicao,
        j.NumeroCamisa,
        j.`Time`,
        j.Nacionalidade,
        j.Preferido,
        j.DataCadastro
    FROM Jogadores j
    INNER JOIN Posicoes p
        ON j.fk_posicao = p.idPosicao
    WHERE j.Id = p_id;
END //

-- Inserir jogador
CREATE PROCEDURE sp_insereJogador (
    IN p_nome VARCHAR(100),
    IN p_altura DECIMAL(4,2),
    IN p_posicao INT,
    IN p_camisa INT,
    IN p_time VARCHAR(100),
    IN p_nacionalidade VARCHAR(50)
)
BEGIN
    INSERT INTO Jogadores (Nome, Altura, fk_posicao, NumeroCamisa, `Time`, Nacionalidade, Preferido)
    VALUES (p_nome, p_altura, p_posicao, p_camisa, p_time, p_nacionalidade, 0);
END //

-- Atualizar jogador
CREATE PROCEDURE sp_atualizaJogador (
    IN p_id INT,
    IN p_nome VARCHAR(100),
    IN p_altura DECIMAL(4,2),
    IN p_posicao INT,
    IN p_camisa INT,
    IN p_time VARCHAR(100),
    IN p_nacionalidade VARCHAR(50)
)
BEGIN
    UPDATE Jogadores
    SET Nome = p_nome,
        Altura = p_altura,
        fk_posicao = p_posicao,
        NumeroCamisa = p_camisa,
        `Time` = p_time,
        Nacionalidade = p_nacionalidade
    WHERE Id = p_id;
END //

-- Remover jogador
CREATE PROCEDURE sp_removeJogador(IN p_idjogador INT)
BEGIN
    DELETE FROM Jogadores WHERE Id = p_idjogador;
END //

-- Alternar preferido
CREATE PROCEDURE sp_alternarPreferido (IN p_id INT, IN p_preferido TINYINT(1))
BEGIN
    UPDATE Jogadores SET Preferido = p_preferido WHERE Id = p_id;
END //

DELIMITER ;

-- ---------------------------------------------------------
-- Tabela de gêneros (cadastro de gêneros)
-- ---------------------------------------------------------
CREATE TABLE IF NOT EXISTS Generos (
    idGenero INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(50) NOT NULL UNIQUE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ---------------------------------------------------------
-- Tabela de usuários (login)
-- ---------------------------------------------------------
CREATE TABLE IF NOT EXISTS Usuarios (
    idUsuario INT AUTO_INCREMENT PRIMARY KEY,
    Usuario VARCHAR(50) NOT NULL UNIQUE,
    Senha VARCHAR(100) NOT NULL,
    NomeCompleto VARCHAR(100) NOT NULL,
    Ativo TINYINT(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ---------------------------------------------------------
-- Stored Procedures para Gêneros
-- ---------------------------------------------------------

-- Listar gêneros
CREATE PROCEDURE sp_listaGeneros()
BEGIN
    SELECT idGenero, Nome
    FROM Generos
    ORDER BY Nome;
END //

-- Buscar gênero por ID
CREATE PROCEDURE sp_buscaGeneroPorId(IN p_id INT)
BEGIN
    SELECT idGenero, Nome
    FROM Generos
    WHERE idGenero = p_id;
END //

-- Inserir gênero
CREATE PROCEDURE sp_insereGenero(IN p_nome VARCHAR(50))
BEGIN
    INSERT INTO Generos (Nome) VALUES (p_nome);
END //

-- Atualizar gênero
CREATE PROCEDURE sp_atualizaGenero(IN p_id INT, IN p_nome VARCHAR(50))
BEGIN
    UPDATE Generos SET Nome = p_nome WHERE idGenero = p_id;
END //

-- Remover gênero
CREATE PROCEDURE sp_removeGenero(IN p_id INT)
BEGIN
    DELETE FROM Generos WHERE idGenero = p_id;
END //

-- ---------------------------------------------------------
-- Stored Procedures para Usuários (Login)
-- ---------------------------------------------------------

-- Validar login
CREATE PROCEDURE sp_validarLogin(IN p_usuario VARCHAR(50), IN p_senha VARCHAR(100))
BEGIN
    SELECT idUsuario, Usuario, NomeCompleto, Ativo
    FROM Usuarios
    WHERE Usuario = p_usuario AND Senha = p_senha AND Ativo = 1;
END //

-- Inserir usuário
CREATE PROCEDURE sp_insereUsuario (
    IN p_usuario VARCHAR(50),
    IN p_senha VARCHAR(100),
    IN p_nomeCompleto VARCHAR(100)
)
BEGIN
    INSERT INTO Usuarios (Usuario, Senha, NomeCompleto, Ativo)
    VALUES (p_usuario, p_senha, p_nomeCompleto, 1);
END //

-- Dados de exemplo (opcional)
INSERT INTO Posicoes (Nome) VALUES ('Armador'), ('Ala'), ('Pivô'), ('Ala-Armador'), ('Ala-Pivô');

INSERT INTO Generos (Nome) VALUES ('Rock'), ('Pop'), ('MPB'), ('Sertanejo'), ('Jazz');

INSERT INTO Usuarios (Usuario, Senha, NomeCompleto) VALUES 
('admin', 'admin123', 'Administrador'),
('usuario', '123456', 'Usuario Teste');

/*
CALL sp_insereJogador('LeBron James', 2.06, 2, 23, 'Los Angeles Lakers', 'Estados Unidos');
CALL sp_insereJogador('Anthony Davis', 2.08, 3, 3, 'Los Angeles Lakers', 'Estados Unidos');
*/

SELECT 'Banco MySQL criado com sucesso!' AS Mensagem;