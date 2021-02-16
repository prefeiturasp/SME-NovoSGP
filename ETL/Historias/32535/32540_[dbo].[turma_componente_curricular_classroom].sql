
BEGIN TRANSACTION;

CREATE TABLE [dbo].[turma_componente_curricular_classroom] (
    [cd_curso_classroom] VARCHAR(20),
    [cd_componente_curricular] INT NOT NULL,
    [cd_turma_escola] INT
    PRIMARY KEY(cd_curso_classroom)
)
GO;

CREATE  INDEX [ix_cd_componente_curricular] ON [dbo].[turma_componente_curricular_classroom]( [cd_componente_curricular] )
GO;

CREATE  INDEX [ix_cd_turma_escola] ON [dbo].[turma_componente_curricular_classroom]( [cd_turma_escola] )
GO;

CREATE  INDEX [ix_cd_curso_classroom] ON [dbo].[turma_componente_curricular_classroom]( [cd_curso_classroom] )
GO;

COMMIT TRANSACTION;
