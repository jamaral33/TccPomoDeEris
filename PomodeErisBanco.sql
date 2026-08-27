	create database db_PomodeEris;
use db_PomodeEris;

-- Criando as tabelas que envolvem as pessoas envolvidas no estabelecimento
create table tbPessoa
(
	CPF char(11) primary key, 
	Nome varchar(100) not null,
    Email varchar(50) not null,
    Telefone varchar(50) not null,
    Senha varchar(70) not null
);

create table tbCliente
(
	idCliente int primary key,
	CPF char(11), 
	Nome varchar(100) not null,
    Email varchar(50) not null,
    Telefone varchar(50) not null,
    Senha varchar(70) not null,
    CONSTRAINT fk_tbCliente_CPF_tbPessoa
     FOREIGN KEY (CPF)
    REFERENCES tbPessoa(CPF)
);

create table tbFuncionario
(
	Matricula int primary key,
	CPF char(11), 
	Nome varchar(100) not null,
    Email varchar(50) not null,
    Telefone varchar(50) not null,
    Senha varchar(70) not null,
    CONSTRAINT fk_tbFuncionario_CPF_tbPessoa
     FOREIGN KEY (CPF)
    REFERENCES tbPessoa(CPF)
);

-- Criando as tabelas "logísticas" do estabelecimento
create table tbConsulta
(
	idConsulta int primary key,
    idCliente int,
    Matricula int,
    dataConsulta datetime not null,
    ServicoContratado varchar(50) not null,
    Estado char(1) not null,
    CONSTRAINT fk_tbConsulta_idCliente_tbCliente
     FOREIGN KEY (idCliente)
    REFERENCES tbCliente(idcliente),
    CONSTRAINT fk_tbConsulta_Matricula_tbFuncionario
     FOREIGN KEY (Matricula)
    REFERENCES tbFuncionario(Matricula)
);

create table tbPagamento
(
	idPagamento int primary key,
    idConsulta int,
    Valor decimal(7,2) not null,
    Metodo varchar(20) not null,
    CONSTRAINT fk_tbPagamento_Matricula_tbConsulta
     FOREIGN KEY (idConsulta)
    REFERENCES tbConsulta(idConsulta)
);


