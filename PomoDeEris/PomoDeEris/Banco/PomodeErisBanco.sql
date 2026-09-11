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
    Senha VARCHAR(70) NOT NULL
);

CREATE TABLE tbCliente
(
    idCliente INT PRIMARY KEY AUTO_INCREMENT,
    CPF CHAR(11) NOT NULL, 
    
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
    Valor DECIMAL(7,2) NOT NULL
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
    Matricula INT NOT NULL,
    dataHoraConsulta DATETIME NOT NULL,
    Situacao CHAR(1) NOT NULL,
    
    CONSTRAINT fk_tbConsulta_idCliente_tbCliente
    FOREIGN KEY (idCliente)
    REFERENCES tbCliente(idcliente),
    
    CONSTRAINT fk_tbConsulta_Matricula_tbFuncionario
    FOREIGN KEY (Matricula)
    REFERENCES tbFuncionario(Matricula)
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
    vSenha VARCHAR(70)
)
BEGIN
    INSERT INTO tbPessoa
    VALUES (vCPF, vNome, vEmail, vTelefone, vSenha);
    
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


-- FUNCIONÁRIO

CREATE PROCEDURE sp_cadastrar_funcionario(
    vCPF CHAR(11), 
    vNome VARCHAR(100),
    vEmail VARCHAR(50),
    vTelefone CHAR(11),
    vSenha VARCHAR(70),
    vFuncao VARCHAR(50)
)
BEGIN
    INSERT INTO tbPessoa
    VALUES (vCPF, vNome, vEmail, vTelefone, vSenha);

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
        p.Senha
    FROM tbFuncionario f
    JOIN tbPessoa p
        ON f.CPF = p.CPF
    WHERE f.Matricula = vMatricula;
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


-- PACOTE

CREATE PROCEDURE sp_cadastrar_pacote(
    vNome VARCHAR(50),
    vDescricao VARCHAR(150),
    vValor DECIMAL(7,2)
)
BEGIN
    INSERT INTO tbPacote (Nome, Descricao, Valor)
    VALUES (vNome, vDescricao, vValor);
END $$


CREATE PROCEDURE sp_selecionar_pacote(vIdPacote INT)
BEGIN
    SELECT 
        p.idPacote,
        p.Nome,
        p.Descricao,
        p.Valor
    FROM tbPacote p
    WHERE p.idPacote = vIdPacote;
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


-- CONSULTA

CREATE PROCEDURE sp_cadastrar_consulta(
    vIdCliente INT,
    vMatricula INT,
    vDataHoraConsulta DATETIME,
    vSituacao CHAR(1)
)
BEGIN
    INSERT INTO tbConsulta (
        idCliente,
        Matricula,
        dataHoraConsulta,
        Situacao
    )
    VALUES (
        vIdCliente,
        vMatricula,
        vDataHoraConsulta,
        vSituacao
    );
END $$

CREATE PROCEDURE sp_selecionar_consulta(vIdConsulta INT)
BEGIN
    SELECT 
        c.idConsulta,
        c.idCliente,
        c.Matricula,
        c.dataHoraConsulta,
        c.Situacao,

        cl.CPF AS CPF_Cliente,

        pf.CPF AS CPF_Funcionario


    FROM tbConsulta c

    JOIN tbCliente cl
        ON c.idCliente = cl.idCliente

    JOIN tbPessoa pc
        ON cl.CPF = pc.CPF

    JOIN tbFuncionario f
        ON c.Matricula = f.Matricula

    JOIN tbPessoa pf
        ON f.CPF = pf.CPF

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
        p.Valor
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


CREATE PROCEDURE sp_selecionar_pagamento(vIdPagamento INT)
BEGIN
    SELECT 
        pg.idPagamento,
        pg.idConsulta,
        pg.Valor,
        pg.Metodo,
        pg.Situacao,

        c.idCliente,
        c.Matricula,
        c.dataHoraConsulta,
        c.Situacao AS Situacao_Consulta

    FROM tbPagamento pg
    JOIN tbConsulta c
        ON pg.idConsulta = c.idConsulta
    WHERE pg.idPagamento = vIdPagamento;
END $$


DELIMITER ;


-- TESTES


-- CLIENTE

CALL sp_cadastrar_cliente(
    '12345678910',
    'Nilson',
    'nilson@email.com',
    '11987654321',
    '123456'
);

CALL sp_selecionar_cliente(1);


-- FUNCIONÁRIO

CALL sp_cadastrar_funcionario(
    '98765432100',
    'Maria Silva',
    'maria@email.com',
    '11999998888',
    'senha123',
    'Recepcionista'
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
    'Pacote Facial',
    'Pacote completo de cuidados faciais',
    250.00
);

CALL sp_selecionar_pacote(1);


-- SERVIÇOS DO PACOTE

CALL sp_cadastrar_servicosPacote(
    1,
    1
);

CALL sp_selecionar_servicos_pacote(1);


-- CONSULTA

CALL sp_cadastrar_consulta(
    1,
    1,
    '2026-08-25 14:30:00',
    'A'
);

CALL sp_selecionar_consulta(1);


-- PACOTE DA CONSULTA

CALL sp_cadastrar_pacote_consulta(
    1,
    1,
    1
);

CALL sp_selecionar_pacote_consulta(1);


-- PAGAMENTO

CALL sp_cadastrar_pagamento(
    1,
    250.00,
    'PIX',
    'Pago'
);

CALL sp_selecionar_pagamento(1);