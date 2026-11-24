DROP DATABASE IF EXISTS dbPomodeEris;

CREATE DATABASE dbPomodeEris;
USE dbPomodeEris;


-- Criando as tabelas que envolvem as pessoas envolvidas no estabelecimento

CREATE TABLE tbPessoa
(
    CPF CHAR(11) PRIMARY KEY, 
    Nome VARCHAR(100) NOT NULL,
    Email VARCHAR(50) NOT NULL,
    Telefone CHAR(11) NOT NULL,
    Senha VARCHAR(70) NOT NULL,
    Situacao varchar(70) NOT NULL
);

CREATE TABLE tbCliente
(
    idCliente INT PRIMARY KEY AUTO_INCREMENT,
    CPF CHAR(11) NOT NULL, 
    dataCadastro DATETIME DEFAULT current_timestamp,
    
    CONSTRAINT fk_tbCliente_CPF_tbPessoa
    FOREIGN KEY (CPF)
    REFERENCES tbPessoa(CPF)
);

CREATE TABLE tbFuncionario
(
    Matricula INT PRIMARY KEY AUTO_INCREMENT,
    CPF CHAR(11) NOT NULL, 
    Funcao VARCHAR(50) NOT NULL,
    
    CONSTRAINT fk_tbFuncionario_CPF_tbPessoa
    FOREIGN KEY (CPF)
    REFERENCES tbPessoa(CPF)
);


-- Criando as tabelas "logísticas" do estabelecimento

CREATE TABLE tbServico
(
    idServico INT PRIMARY KEY AUTO_INCREMENT,
    Nome VARCHAR(50) NOT NULL,
    Descricao VARCHAR(150) NOT NULL
);

CREATE TABLE tbPacote
(
    idPacote INT PRIMARY KEY AUTO_INCREMENT,
    Nome VARCHAR(50) NOT NULL,
    Descricao VARCHAR(150) NOT NULL,
    Valor DECIMAL(7,2) NOT NULL,
    Desconto INT DEFAULT 0,
    Imagem varchar(255) NOT NULL
);

CREATE TABLE tbServicosPacote
(
    idServico INT,
    idPacote INT,
    Quantidade SMALLINT NOT NULL,
    
    PRIMARY KEY(idServico, idPacote),
    
    CONSTRAINT fk_tbServicosPacote_idServico_tbServico
    FOREIGN KEY (idServico)
    REFERENCES tbServico(idServico),
    
    CONSTRAINT fk_tbServicosPacote_idPacote_tbPacote
    FOREIGN KEY (idPacote)
    REFERENCES tbPacote(idPacote)
);

CREATE TABLE tbConsulta
(
    idConsulta INT PRIMARY KEY AUTO_INCREMENT,
    idCliente INT NOT NULL,
    Matricula INT,
    Situacao varchar(50) NOT NULL,
    dataCadastro DATETIME DEFAULT current_timestamp,
    
    CONSTRAINT fk_tbConsulta_idCliente_tbCliente
    FOREIGN KEY (idCliente)
    REFERENCES tbCliente(idcliente),
    
    CONSTRAINT fk_tbConsulta_Matricula_tbFuncionario
    FOREIGN KEY (Matricula)
    REFERENCES tbFuncionario(Matricula)
);

CREATE TABLE tbAgenda(
	idAgenda INT PRIMARY KEY AUTO_INCREMENT,
	tipo varchar(50),
	idConsulta INT,
    DiaHorario datetime,
    
   CONSTRAINT fk_tbAgenda_idConsulta
    FOREIGN KEY (idConsulta)
    REFERENCES tbConsulta(idConsulta)
);
CREATE TABLE tbPacoteConsulta
(
    idConsulta INT,
    idPacote INT,
    Quantidade SMALLINT NOT NULL,
    
    PRIMARY KEY(idConsulta, idPacote),
    
    CONSTRAINT fk_tbPacoteConsulta_idConsulta_tbConsulta
    FOREIGN KEY (idConsulta)
    REFERENCES tbConsulta(idConsulta),
    
    CONSTRAINT fk_tbPacoteConsulta_idPacote_tbPacote
    FOREIGN KEY (idPacote)
    REFERENCES tbPacote(idPacote)
);

CREATE TABLE tbPagamento
(
    idPagamento INT PRIMARY KEY AUTO_INCREMENT,
    idConsulta INT NOT NULL,
    Valor DECIMAL(7,2) NOT NULL,
    Metodo VARCHAR(20) NOT NULL,
    Situacao VARCHAR(20) NOT NULL,
    
    CONSTRAINT fk_tbPagamento_Matricula_tbConsulta
    FOREIGN KEY (idConsulta)
    REFERENCES tbConsulta(idConsulta)
);


-- PROCEDURES

DELIMITER $$


-- CLIENTE

CREATE PROCEDURE sp_cadastrar_cliente(
    vCPF CHAR(11), 
    vNome VARCHAR(100),
    vEmail VARCHAR(50),
    vTelefone CHAR(11),
    vSenha VARCHAR(70),
    vSituacao varchar(70)
)
BEGIN
    INSERT INTO tbPessoa
    VALUES (vCPF, vNome, vEmail, vTelefone, vSenha, vSituacao);
    
    INSERT INTO tbCliente(CPF)
    VALUES(vCPF);
END $$


CREATE PROCEDURE sp_selecionar_cliente(vIdCliente INT)
BEGIN
    SELECT 
        c.idCliente,
        c.CPF,
        p.Nome,
        p.Email,
        p.Telefone,
        p.Senha
    FROM tbCliente c
    JOIN tbPessoa p
        ON c.CPF = p.CPF
    WHERE c.idCliente = vIdCliente;
END $$

CREATE PROCEDURE sp_selecionar_clienteLogin(vEmail varchar(50), vSenha varchar(70))
BEGIN
    SELECT 
        c.idCliente,
        c.CPF,
        p.Nome,
        p.Email,
        p.Telefone,
        p.Senha,
        p.Situacao
    FROM tbCliente c
    JOIN tbPessoa p
        ON c.CPF = p.CPF
    WHERE c.idCliente = vIdCliente;
END $$

DELIMITER $$


CREATE PROCEDURE  sp_selecionar_todos_clientes()
BEGIN
	SELECT * FROM tbCliente c
	JOIN tbPessoa p
	ON p.CPF = c.CPF;
END $$

CREATE PROCEDURE sp_selecionar_clienteEmail(vEmail varchar(50))
BEGIN
    SELECT 
        c.idCliente,
        c.CPF,
        p.Nome,
        p.Email,
        p.Telefone,
        p.Senha,
        p.Situacao
    FROM tbCliente c
    JOIN tbPessoa p
        ON c.CPF = p.CPF
    WHERE p.Email = vEmail;
END $$

CREATE PROCEDURE sp_selecionar_quantidade_clientes_hoje()
BEGIN
	SELECT COUNT(*) AS quantidadeClientesHoje FROM tbCliente WHERE DATE(DataCadastro) = DATE(CURRENT_TIMESTAMP);
END $$

call sp_selecionar_clienteEmail('teste@teste.com');


-- FUNCIONÁRIO
DELIMITER $$
CREATE PROCEDURE sp_selecionar_funcionarioEmail(vEmail varchar(50))
BEGIN
    SELECT 
        f.Matricula,
        f.CPF,
        f.Funcao,
        p.Nome,
        p.Email,
        p.Telefone,
        p.Senha,
        p.Situacao
    FROM tbfuncionario f
    JOIN tbPessoa p
        ON f.CPF = p.CPF
    WHERE p.Email = vEmail;
END $$

CREATE PROCEDURE sp_cadastrar_funcionario(
    vCPF CHAR(11), 
    vNome VARCHAR(100),
    vEmail VARCHAR(50),
    vSituacao varchar(70),
    vTelefone CHAR(11),
    vSenha VARCHAR(70),
    vFuncao VARCHAR(50)
)
BEGIN
    INSERT INTO tbPessoa
    VALUES (vCPF, vNome, vEmail, vTelefone, vSenha, vSituacao);

    INSERT INTO tbFuncionario(CPF, Funcao)
    VALUES (vCPF, vFuncao);
END $$

CREATE PROCEDURE sp_selecionar_funcionario(vMatricula INT)
BEGIN
    SELECT 
        f.Matricula,
        f.CPF,
        f.Funcao,
        p.Nome,
        p.Email,
        p.Telefone,
        p.Senha,
        p.Situacao
        
    FROM tbFuncionario f
    JOIN tbPessoa p
        ON f.CPF = p.CPF
    WHERE f.Matricula = vMatricula;
END $$

CREATE PROCEDURE sp_atualizar_funcionario(
    vMatricula INT,
    vCPF CHAR(11),
    vNome VARCHAR(100),
    vEmail VARCHAR(50),
    vSituacao VARCHAR(70),
    vTelefone CHAR(11),
    vSenha VARCHAR(70),
    vFuncao VARCHAR(50)
)
BEGIN

    UPDATE tbPessoa
    SET
        Nome = vNome,
        Email = vEmail,
        Situacao = vSituacao,
        Telefone = vTelefone,
        Senha = COALESCE(NULLIF(vSenha, ''), Senha)
    WHERE CPF = vCPF;

    UPDATE tbFuncionario
    SET
        Funcao = vFuncao
    WHERE Matricula = vMatricula
    AND CPF = vCPF;

END $$

CREATE PROCEDURE sp_selecionar_todos_funcionarios()
BEGIN
	SELECT * FROM tbFuncionario f
    JOIN tbPessoa p
		ON f.CPF = p.CPF;
END $$

-- SERVIÇO

CREATE PROCEDURE sp_cadastrar_servico(
    vNome VARCHAR(50),
    vDescricao VARCHAR(150)
)
BEGIN
    INSERT INTO tbServico (Nome, Descricao)
    VALUES (vNome, vDescricao);
END $$


CREATE PROCEDURE sp_selecionar_servico(vIdServico INT)
BEGIN
    SELECT 
        s.idServico,
        s.Nome,
        s.Descricao
    FROM tbServico s
    WHERE s.idServico = vIdServico;
END $$

CREATE PROCEDURE sp_selecionar_servicos()
BEGIN
	SELECT * FROM tbservico;
END $$


-- PACOTE

CREATE PROCEDURE sp_cadastrar_pacote(
    vNome VARCHAR(50),
    vDescricao VARCHAR(150),
    vValor DECIMAL(7,2),
    vDesconto INT,
    vImagem VARCHAR(255)
)
BEGIN
    INSERT INTO tbPacote (Nome, Descricao, Valor, Desconto, Imagem)
    VALUES (vNome, vDescricao, vValor, vDesconto, vImagem);
END $$


CREATE PROCEDURE sp_selecionar_pacote(vIdPacote INT)
BEGIN
    SELECT 
        p.idPacote,
        p.Nome,
        p.Descricao,
        p.Valor,
        p.Desconto,
        p.Imagem
    FROM tbPacote p
    WHERE p.idPacote = vIdPacote;
END $$
CREATE PROCEDURE sp_atualizar_pacote(
    vIdPacote INT,
    vNome VARCHAR(50),
    vDescricao VARCHAR(150),
    vValor DECIMAL(7,2),
    vDesconto INT,
    vImagem VARCHAR(255)
)
BEGIN
    UPDATE tbPacote
    SET Nome = vNome,
        Descricao = vDescricao,
        Valor = vValor,
        Desconto = vDesconto,
        -- se não veio imagem nova, mantém a que já estava
        Imagem = COALESCE(NULLIF(vImagem, ''), Imagem)
    WHERE idPacote = vIdPacote;
END $$

CREATE PROCEDURE sp_remover_servicos_pacote(vIdPacote INT)
BEGIN
    DELETE FROM tbServicosPacote
    WHERE idPacote = vIdPacote;
END $$

CREATE PROCEDURE sp_pesquisar_pacotes(vPesquisa VARCHAR(255))
BEGIN
    SELECT *
    FROM tbPacote
    WHERE nome LIKE CONCAT('%', vPesquisa, '%');
END $$
-- SERVIÇOS DO PACOTE

CREATE PROCEDURE sp_cadastrar_servicosPacote(
    vIdPacote INT,
    vIdServico INT
)
BEGIN
    INSERT INTO tbServicosPacote
    VALUES (vIdServico, vIdPacote, 1);
END $$


CREATE PROCEDURE sp_selecionar_servicos_pacote(vIdPacote INT)
BEGIN
    SELECT 
        sp.idPacote,
        sp.idServico,
        sp.Quantidade,
        s.Nome,
        s.Descricao
    FROM tbServicosPacote sp
    JOIN tbServico s
        ON sp.idServico = s.idServico
    WHERE sp.idPacote = vIdPacote;
END $$

CREATE PROCEDURE sp_selecionar_vendas_pacotes()
BEGIN
    SELECT
        p.idPacote,
        p.Nome AS NomePacote,

        COALESCE(SUM(pc.Quantidade), 0) AS Vendas,

		COALESCE(
			SUM(pc.Quantidade * p.Valor * (1 - p.Desconto / 100)),
			0
		) AS Receita
    FROM tbPacote p

    LEFT JOIN tbPacoteConsulta pc
        ON p.idPacote = pc.idPacote

    GROUP BY
        p.idPacote,
        p.Nome

    ORDER BY Receita DESC;

END $$

-- CONSULTA

DELIMITER $$

CREATE PROCEDURE sp_cadastrar_consulta(
    vIdCliente INT,
    vHorario DATETIME
)
BEGIN
    INSERT INTO tbConsulta (idCliente, Situacao)
    VALUES (vIdCliente, 'Agendado');

    SET @idConsulta = LAST_INSERT_ID();

    INSERT INTO tbAgenda (tipo, idConsulta, DiaHorario)
    VALUES ('horario', @idConsulta, vHorario);

    SELECT @idConsulta AS idConsulta;
END $$

DELIMITER $$

CREATE PROCEDURE sp_atualizar_consulta(
    vIdConsulta INT,
    vSituacao VARCHAR(50),
    vMatricula INT,
    vDiaHorario DATETIME,
    vValor DECIMAL(7,2),
    vMetodo VARCHAR(20),
    vSituacaoPagamento VARCHAR(20)
)
BEGIN
    UPDATE tbConsulta
    SET Situacao = vSituacao,
        Matricula = NULLIF(vMatricula, 0)   -- 0 = "Não definido"
    WHERE idConsulta = vIdConsulta;

    UPDATE tbAgenda
    SET DiaHorario = vDiaHorario
    WHERE idConsulta = vIdConsulta;

    UPDATE tbPagamento
    SET Valor = vValor,
        Metodo = vMetodo,
        Situacao = vSituacaoPagamento
    WHERE idConsulta = vIdConsulta;

    DELETE FROM tbPacoteConsulta
    WHERE idConsulta = vIdConsulta;
END $$




CREATE PROCEDURE sp_selecionar_quantidade_consultas_hoje()
BEGIN
    SELECT COUNT(*) AS quantidade 
    FROM tbConsulta 
    WHERE DATE(dataCadastro) = CURRENT_DATE();
END $$
CREATE PROCEDURE sp_selecionar_informacoes_consulta(vIdConsulta int)
BEGIN
	SELECT
		c.idConsulta,
        c.Situacao AS SituacaoConsulta,
        c.dataCadastro AS DataCadastroConsulta,
        
        a.DiaHorario,
        
        cl.idCliente,
        cl.dataCadastro AS DataCadastroCliente,
        
        p.CPF AS CPF_Cliente,
        p.Nome AS  NomeCliente,
        p.Email AS  EmailCliente,
        p.Telefone AS TelefoneCliente,
        p.Situacao AS SituacaoCliente,
        
        
        f.Matricula,
        f.Funcao,
        pf.CPF AS CPF_Funcionario,
        pf.Nome AS  NomeFuncionario,
        pf.Email AS  EmailFuncionario,
        pf.Telefone AS TelefoneFuncionario,
        pf.Situacao AS SituacaoFuncionario,
        
        pg.idPagamento,
        pg.Valor AS ValorPagamento,
        pg.Metodo AS MetodoPagamento,
        pg.Situacao AS SituacaoPagamento

    FROM tbConsulta c
	LEFT JOIN tbAgenda a 
        ON c.idConsulta = a.idConsulta
    LEFT JOIN tbCliente cl 
        ON c.idCliente = cl.idCliente
    LEFT JOIN tbPessoa p 
        ON cl.CPF = p.CPF    
	LEFT JOIN tbFuncionario f
        ON c.Matricula = f.Matricula
    LEFT JOIN tbPessoa pf
        ON f.CPF = pf.CPF
	LEFT JOIN tbPagamento pg
		ON pg.idConsulta = c.idConsulta
	WHERE c.idConsulta = vidConsulta;
END $$


CREATE PROCEDURE sp_selecionar_ultimas_7_consultas()
BEGIN
    SELECT 
        c.idConsulta,
        c.Situacao AS SituacaoConsulta,
        c.dataCadastro AS DataCadastroConsulta,
        
        a.DiaHorario,
        
        cl.idCliente,
        cl.dataCadastro AS DataCadastroCliente,
        
        p.CPF,
        p.Nome,
        p.Email,
        p.Telefone,
        p.Situacao AS SituacaoPessoa
        
    FROM tbConsulta c
    LEFT JOIN tbAgenda a 
        ON c.idConsulta = a.idConsulta
    JOIN tbCliente cl 
        ON c.idCliente = cl.idCliente
    JOIN tbPessoa p 
        ON cl.CPF = p.CPF
    ORDER BY c.idConsulta DESC
    LIMIT 7;
END $$

CREATE PROCEDURE sp_selecionar_consultas_dia(vDia DATE)
BEGIN		
    SELECT distinct c.idConsulta FROM 
    tbConsulta c
    INNER JOIN tbAgenda a
    ON c.idConsulta = a.idConsulta
    WHERE vDia = DATE(a.DiaHorario)
	  AND a.tipo = 'horario';
      
END $$

CREATE PROCEDURE sp_atividade_recente(vLimite INT)
BEGIN
    SELECT * FROM (
        SELECT 
            'novoCliente' AS Tipo,
            p.Nome AS NomeCliente,
            NULL AS QuantidadePacotes,
            c.dataCadastro AS DataEvento
        FROM tbCliente c
        INNER JOIN tbPessoa p
            ON c.CPF = p.CPF
        WHERE c.dataCadastro IS NOT NULL

        UNION ALL -- Junta as duas querys

        SELECT 
            'novoAtendimento' AS Tipo,
            p.Nome AS NomeCliente,
            COALESCE(SUM(pc.Quantidade), 0) AS QuantidadePacotes,
            cons.dataCadastro AS DataEvento
        FROM tbConsulta cons
        INNER JOIN tbCliente cli
            ON cons.idCliente = cli.idCliente
        INNER JOIN tbPessoa p
            ON cli.CPF = p.CPF
        LEFT JOIN tbPacoteConsulta pc
            ON cons.idConsulta = pc.idConsulta
        WHERE cons.dataCadastro IS NOT NULL
        GROUP BY
            cons.idConsulta,
            p.Nome,
            cons.dataCadastro
    ) AS Atividades
    ORDER BY DataEvento DESC
    LIMIT vLimite;
END $$

DELIMITER $$

CREATE PROCEDURE sp_selecionar_consulta(vIdConsulta INT)
BEGIN
    SELECT 
        -- Dados da Consulta
        c.idConsulta,
        c.Situacao AS SituacaoConsulta,
        c.dataCadastro AS DataCadastroConsulta,
        
        -- Dados da Agenda
        a.idAgenda,
        a.tipo AS TipoAgenda,
        a.DiaHorario,
        
        -- Cliente
        cl.idCliente,
        cl.dataCadastro AS DataCadastroCliente,
        pc.CPF AS CPF_Cliente,
        pc.Nome AS NomeCliente,
        pc.Email AS EmailCliente,
        pc.Telefone AS TelefoneCliente,
        pc.Situacao AS SituacaoCliente,
        
        -- Funcionário
        f.Matricula,
        f.Funcao AS FuncaoFuncionario,
        pf.CPF AS CPF_Funcionario,
        pf.Nome AS NomeFuncionario,
        pf.Email AS EmailFuncionario,
        pf.Telefone AS TelefoneFuncionario,
        
        -- Pagamento
        pg.idPagamento,
        pg.Valor AS ValorPagamento,
        pg.Metodo AS MetodoPagamento,
        pg.Situacao AS SituacaoPagamento

    FROM tbConsulta c
    LEFT JOIN tbAgenda a 
        ON c.idConsulta = a.idConsulta
    INNER JOIN tbCliente cl 
        ON c.idCliente = cl.idCliente
    INNER JOIN tbPessoa pc 
        ON cl.CPF = pc.CPF
    LEFT JOIN tbFuncionario f 
        ON c.Matricula = f.Matricula
    LEFT JOIN tbPessoa pf 
        ON f.CPF = pf.CPF
    LEFT JOIN tbPagamento pg 
        ON c.idConsulta = pg.idConsulta
    WHERE c.idConsulta = vIdConsulta;
END $$


-- PACOTE DA CONSULTA

CREATE PROCEDURE sp_cadastrar_pacote_consulta(
    vIdConsulta INT,
    vIdPacote INT,
    vQuantidade SMALLINT
)
BEGIN
    INSERT INTO tbPacoteConsulta (
        idConsulta,
        idPacote,
        Quantidade
    )
    VALUES (
        vIdConsulta,
        vIdPacote,
        vQuantidade
    );
END $$


CREATE PROCEDURE sp_selecionar_pacote_consulta(vIdConsulta INT)
BEGIN
    SELECT 
        pc.idConsulta,
        pc.idPacote,
        pc.Quantidade,
        p.Nome,
        p.Descricao,
        p.Valor,
        p.Desconto,
        p.Imagem
    FROM tbPacoteConsulta pc
    JOIN tbPacote p
        ON pc.idPacote = p.idPacote
    WHERE pc.idConsulta = vIdConsulta;
END $$


-- PAGAMENTO

CREATE PROCEDURE sp_cadastrar_pagamento(
    vIdConsulta INT,
    vValor DECIMAL(7,2),
    vMetodo VARCHAR(20),
    vSituacao VARCHAR(20)
)
BEGIN
    INSERT INTO tbPagamento (
        idConsulta,
        Valor,
        Metodo,
        Situacao
    )
    VALUES (
        vIdConsulta,
        vValor,
        vMetodo,
        vSituacao
    );
END $$

DELIMITER $$

CREATE PROCEDURE sp_selecionar_receita_mes()
BEGIN
    SELECT 
        COALESCE(SUM(p.Valor), 0) AS Receita
    FROM tbPagamento p
    JOIN tbConsulta c
        ON p.idConsulta = c.idConsulta
    WHERE MONTH(c.dataCadastro) = MONTH(CURRENT_DATE())
      AND YEAR(c.dataCadastro) = YEAR(CURRENT_DATE());
END $$

CREATE PROCEDURE sp_selecionar_receita_dia()
BEGIN
    SELECT 
        COALESCE(SUM(p.Valor), 0) AS Receita
    FROM tbPagamento p
    JOIN tbConsulta c
        ON p.idConsulta = c.idConsulta
    WHERE DAY(c.dataCadastro) = DAY(CURRENT_DATE()) AND
	   MONTH(c.dataCadastro) = MONTH(CURRENT_DATE())
      AND YEAR(c.dataCadastro) = YEAR(CURRENT_DATE());
END $$


CREATE PROCEDURE sp_verificar_horario_disponivel(
    vHorario DATETIME,
    vIdConsulta INT
)
BEGIN
    SELECT COUNT(*) AS Ocupados
    FROM tbAgenda a
    LEFT JOIN tbConsulta c ON c.idConsulta = a.idConsulta
    WHERE (a.DiaHorario = vHorario
           OR (a.tipo = 'dia' AND DATE(a.DiaHorario) = DATE(vHorario)))
      AND (c.Situacao IS NULL OR c.Situacao != 'Cancelado')
      AND (a.idConsulta IS NULL OR a.idConsulta != vIdConsulta);
END $$

DELIMITER ;

-- TESTES


-- CLIENTE

CALL sp_cadastrar_cliente(
    '12345678910',
    'Nilson',
    'nilson@email.com',
    '11987654321',
    '123456',
    "Ativo"
);

CALL sp_selecionar_cliente(1);


-- FUNCIONÁRIO

CALL sp_cadastrar_funcionario(
    '98765432100',
    'Maria Silva',
    'maria@email.com',
    'Ativo',
    '11999998888',
    'senha123',
    'Recepcionista'
);

CALL sp_cadastrar_funcionario(
    '12345678901',                                                         
    'Teste Testilson',                                                        
    'teste@teste.com',                                              
    'Ativo',                                                              
    '11999998888',                                                    
    'gwgwa8bw7c9eEmT3fctiuQ==.RS/Wl7XSoIUA7uqk7JMpZXVtfoy4BzGw3dejhb869nU=', 
    'Atendente'                                                            
);

CALL sp_selecionar_funcionario(1);


-- SERVIÇO

CALL sp_cadastrar_servico(
    'Limpeza de Pele',
    'Procedimento para limpeza e renovacao da pele'
);

CALL sp_selecionar_servico(1);


-- PACOTE

CALL sp_cadastrar_pacote(
    'Pacote Manicure',
    'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam suscipit sollicitudin cursus.',
    250.00,
    20,
    '/images/manicure.png'
);

CALL sp_cadastrar_pacote(
    'Pacote de pelos faciais',
    'Pacote completo de testes testes testes e mais testes lorem ipsum sit dolot amet',
    100.00,
    20,
    '/images/pelosFaciais.png'
);

CALL sp_cadastrar_pacote(
    'Pacote HidroRelaxamento',
    'Pacote completo de testes testes testes e mais testes lorem ipsum sit dolot amet',
    250.00,
    20,
    '/images/hidroRelaxamento.png'
);

CALL sp_cadastrar_pacote(
    'Pacote Massagem Relaxante',
    'Pacote completo de testes testes testes e mais testes lorem ipsum sit dolot amet',
    250.00,
    20,
    '/images/massagem-relaxante.png'
);

CALL sp_cadastrar_pacote(
    'Pacote Bronzeamento',
    'Pacote completo de testes testes testes e mais testes lorem ipsum sit dolot amet',
    550.50,
    20,
    '/images/bronzeamento.png'
);

CALL sp_cadastrar_pacote(
    'Pacote MicroPigmentação',
    'Pacote completo de testes testes testes e mais testes lorem ipsum sit dolot amet',
    550.50,
    20,
    '/images/pigmentacao.png'
);
CALL sp_selecionar_pacote(1);


-- SERVIÇOS DO PACOTE

CALL sp_cadastrar_servicosPacote(
    1,
    1
);

CALL sp_selecionar_servicos_pacote(1);


-- CONSULTA

CALL sp_selecionar_consulta(1);

CALL sp_cadastrar_consulta(
    1,
    '2026-11-20 14:00:00'
);

-- Recuperar ID da consulta recém-criada
SET @idConsulta = LAST_INSERT_ID();

-- 2. Cadastrar pacotes da consulta
CALL sp_cadastrar_pacote_consulta(
    1,
    1,
    2
);

CALL sp_cadastrar_pacote_consulta(
    1,
    3,
    1
);

-- 3. Calcular valor total dos pacotes
SELECT SUM(
    p.Valor * (1 - p.Desconto / 100) * pc.Quantidade
)
INTO @valorTotal
FROM tbPacoteConsulta pc
INNER JOIN tbPacote p
    ON pc.idPacote = p.idPacote
WHERE pc.idConsulta = @idConsulta;

-- 4. Cadastrar pagamento
CALL sp_cadastrar_pagamento(
    @idConsulta,
    @valorTotal,
    'Pix',
    'Pago'
);
SELECT * FROM tbConsulta
WHERE idConsulta = @idConsulta;

SELECT * FROM tbAgenda
WHERE idConsulta = @idConsulta;

CALL sp_selecionar_pacote_consulta(@idConsulta);

SELECT * FROM tbPagamento
WHERE idConsulta = @idConsulta;

CALL sp_cadastrar_funcionario(
    '67676789898',
    'Senhor Duriguim',
    'durigam@teste.com',
    'Ativo',
    '11123456789',
    'xGkOwYEGPjRUvorrECuAZA==.cUQVMXi1MJKIEWZUnacXnVUvLO4saXq/jQ0t0F9Zrtw=',
    'Administrador'
);