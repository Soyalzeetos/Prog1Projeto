dps tira os comentários q eu coloquei(eu q coloquei msm) pq é oq vamos usar pra cada função do menu né?

CREATE DATABASE biblioteca;
USE biblioteca;

CREATE TABLE usuarios (
    id int auto_increment PRIMARY KEY,
    nome varchar(100) not null,
    email varchar(100) not null UNIQUE,
    tipo ENUM('Aluno','Professor') not null
);

CREATE TABLE livros (
    id int auto_increment PRIMARY KEY,
    titulo varchar(150) not null,
    autor varchar(100) not null,
    ano int not null,
    disponivel BOOLEAN not null DEFAULT TRUE
);

CREATE TABLE emprestimos (
    id int auto_increment PRIMARY KEY,

    usuarioId int not null,
    livroId int not null,

    data_emprestimo DATE not null,
    data_prevista DATE not null,
    data_devolucao DATE NULL,

    FOREIGN KEY (usuarioId) REFERENCES usuarios(id),

    FOREIGN KEY (livroId) REFERENCES livros(id)
);


INSERT INTO usuarios (nome, email, tipo) VALUES
('Gabriel Javorka', 'gabriel@email.com', 'Aluno'),
('Carlos Henrique', 'carlos@email.com', 'Professor'),
('Ana Beatriz', 'ana@email.com', 'Aluno'),
('Mariana Souza', 'mariana@email.com', 'Professor'),
('João Pedro', 'joao@email.com', 'Aluno'),
('Fernanda Lima', 'fernanda@email.com', 'Aluno'),
('Ricardo Alves', 'ricardo@email.com', 'Professor'),
('Patrícia Gomes', 'patricia@email.com', 'Aluno'),
('Lucas Martins', 'lucas@email.com', 'Aluno'),
('Juliana Costa', 'juliana@email.com', 'Professor');


INSERT INTO livros (titulo, autor, ano, disponivel) VALUES
('Dom Casmurro', 'Machado de Assis', 1899, TRUE),
('Memórias Póstumas de Brás Cubas', 'Machado de Assis', 1881, TRUE),
('O Hobbit', 'J. R. R. Tolkien', 1937, FALSE),
('O Senhor dos Anéis', 'J. R. R. Tolkien', 1954, TRUE),
('1984', 'George Orwell', 1949, FALSE),
('A Revolução dos Bichos', 'George Orwell', 1945, TRUE),
('Harry Potter e a Pedra Filosofal', 'J. K. Rowling', 1997, TRUE),
('Harry Potter e a Câmara Secreta', 'J. K. Rowling', 1998, TRUE),
('Percy Jackson e o Ladrão de Raios', 'Rick Riordan', 2005, TRUE),
('O Nome do Vento', 'Patrick Rothfuss', 2007, TRUE),
('Duna', 'Frank Herbert', 1965, TRUE),
('Neuromancer', 'William Gibson', 1984, TRUE),
('O Pequeno Príncipe', 'Antoine de Saint-Exupéry', 1943, TRUE),
('Código Limpo', 'Robert C. Martin', 2008, TRUE),
('Estruturas de Dados e Algoritmos', 'Narasimha Karumanchi', 2011, TRUE);

INSERT INTO emprestimos (usuarioId, livroId, data_emprestimo, data_prevista, data_devolucao) VALUES

-- Empréstimos em aberto
(1, 3, '2026-06-25', '2026-07-02', NULL),
(2, 5, '2026-06-20', '2026-07-05', NULL),

-- Histórico
(3, 1, '2026-05-01', '2026-05-08', '2026-05-07'),
(4, 2, '2026-05-10', '2026-05-25', '2026-05-20'),
(5, 6, '2026-04-12', '2026-04-19', '2026-04-18'),
(6, 7, '2026-03-15', '2026-03-22', '2026-03-21'),
(7, 8, '2026-02-10', '2026-02-25', '2026-02-23'),
(8, 9, '2026-01-18', '2026-01-25', '2026-01-24'),
(9,10, '2026-01-02', '2026-01-09', '2026-01-08'),
(10,11,'2025-12-15', '2025-12-30', '2025-12-28');

-- consultas para as funções

SELECT * FROM usuarios;
SELECT * FROM livros;

--livros por nome ou autor
SELECT * FROM livros
WHERE titulo LIKE '%hobbit%' OR autor LIKE '%tolkien%';

--livros disponíveis
SELECT *
FROM livros
WHERE disponivel = TRUE;


-- empréstimos em aberto
SELECT
e.id,
u.nome,
l.titulo,
e.data_emprestimo,
e.data_prevista
FROM emprestimos e
JOIN usuarios u ON e.usuarioId = u.id
JOIN livros l ON e.livroId = l.id
WHERE e.data_devolucao IS NULL;


--historico de emprestimos
SELECT e.id, u.nome, l.titulo, e.data_emprestimo, e.data_prevista, e.data_devolucao FROM emprestimos e
JOIN usuarios u ON e.usuarioId = u.id
JOIN livros l ON e.livroId = l.id;

--colocar devolução
UPDATE emprestimos
SET data_devolucao = {date}
WHERE id = 1;
UPDATE livros
SET disponivel = TRUE
WHERE id = 2;