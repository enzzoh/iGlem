
CREATE DATABASE DATAtcc;
USE DATAtcc;

drop database DATAtcc


CREATE TABLE usuario (
    idusuario INT IDENTITY(1,1) PRIMARY KEY,
    emailusuario VARCHAR(50),
    senhausuario VARCHAR(40),
    recmail VARCHAR(50), 
    nomeusuario VARCHAR(100),
    telefoneusuario VARCHAR(15),
    IsAdmin BIT DEFAULT 0
);


CREATE TABLE documento (
    documentoid INT IDENTITY(1,1) PRIMARY KEY,
    caminhodocumento VARCHAR(MAX),
    documentonome VARCHAR(100),
	FileData VARBINARY(MAX),
    idusuario INT,
    CONSTRAINT fk_usuario_documento FOREIGN KEY (idusuario) REFERENCES usuario(idusuario)
);



CREATE TABLE palavra_chave (
    id_palavra_chave INT IDENTITY(1,1) PRIMARY KEY,
    palavra VARCHAR(100)
);


CREATE TABLE localizacao (
    id_localizacao INT IDENTITY(1,1) PRIMARY KEY,
    paragrafo INT,
    pagina INT
);


CREATE TABLE documento_palavra_chave (
    id_documento INT,
    id_palavra_chave INT,
    PRIMARY KEY (id_documento, id_palavra_chave),
    CONSTRAINT fk_documento FOREIGN KEY (id_documento) REFERENCES documento(documentoid),
    CONSTRAINT fk_palavra_chave FOREIGN KEY (id_palavra_chave) REFERENCES palavra_chave(id_palavra_chave)
);

CREATE TABLE palavra_chave_localizacao (
    id_palavra_chave INT,
    id_localizacao INT,
    PRIMARY KEY (id_palavra_chave, id_localizacao),
    CONSTRAINT fk_palavra_chave_localizacao FOREIGN KEY (id_palavra_chave) REFERENCES palavra_chave(id_palavra_chave),
    CONSTRAINT fk_localizacao FOREIGN KEY (id_localizacao) REFERENCES localizacao(id_localizacao)
);
