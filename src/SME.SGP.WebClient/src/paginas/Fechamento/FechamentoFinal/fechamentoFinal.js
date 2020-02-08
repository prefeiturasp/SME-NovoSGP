import React, { useState, useEffect } from 'react';
import { Ordenacao } from '~/componentes-sgp';
import { Lista, MaisMenos } from './fechamentoFinal.css';
import { Card, Auditoria } from '~/componentes';
import CampoNumero from '~/componentes/campoNumero';
import ConceitoFinal from './conceitoFinal';
import LinhaAluno from './linhaAluno';

const FechamentoFinal = () => {
  const [ehNota, setEhNota] = useState(true);
  const [ehRegencia, setEhRegencia] = useState(true);
  const [disciplinaSelecionada, setDisciplinaSelecionada] = useState();
  const [exibirLista, setExibirLista] = useState(
    (ehRegencia && !!disciplinaSelecionada) || !ehRegencia
  );
  const [disciplinasRegencia, setDisciplinasRegencia] = useState([
    {
      nome: 'Português',
      codigo: 123,
      ativa: false,
    },
    {
      nome: 'Matemática',
      codigo: 222,
      ativa: false,
    },
    {
      nome: 'Inglês',
      codigo: 333,
      ativa: false,
    },
    {
      nome: 'Ciencias',
      codigo: 444,
    },
    {
      nome: 'Geografia',
      codigo: 555,
    },
    {
      nome: 'Artes',
      codigo: 666,
    },
    {
      nome: 'História',
      codigo: 777,
    },
  ]);

  useEffect(() => {
    setExibirLista((ehRegencia && !!disciplinaSelecionada) || !ehRegencia);
  }, [disciplinaSelecionada, ehRegencia]);

  const setDisciplinaAtiva = disciplina => {
    const disciplinas = disciplinasRegencia.map(c => {
      c.ativa = c.codigo == disciplina.codigo;
      return c;
    });
    setDisciplinasRegencia([...disciplinas]);
    setDisciplinaSelecionada(disciplina.codigo);
  };

  const [auditoria, setAuditoria] = useState({
    criadoPor: '123',
    criadoEm: '',
    alteradoPor: '123',
    alteradoEm: '123',
    criadoRf: '123',
    alteradoRf: '123',
  });
  const [alunos, setAlunos] = useState([
    {
      nome: 'Alvaro Ramos Grassi',
      numeroChamada: 1,
      totalFaltas: 12,
      totalAusenciasCompensadas: 12,
      frequencia: 70,
      notasConceitoBimestre: [
        {
          notaConceito: 8.5,
          disciplinaCodigo: 123,
          bimestre: 1,
        },
        {
          notaConceito: 7.5,
          disciplinaCodigo: 123,
          bimestre: 1,
        },
        {
          notaConceito: 1.5,
          disciplinaCodigo: 222,
          bimestre: 1,
        },
        {
          notaConceito: 8.5,
          disciplinaCodigo: 123,
          bimestre: 1,
        },
        {
          notaConceito: 3.5,
          disciplinaCodigo: 123,
          bimestre: 2,
        },
        {
          notaConceito: 9,
          disciplinaCodigo: 222,
          bimestre: 2,
        },
      ],
      notasConceitoFinal: [
        {
          notaConceito: 8.5,
          disciplinaCodigo: 123,
          bimestre: 1,
          nomeDisciplina: 'Português',
        },
        {
          notaConceito: 7.5,
          disciplinaCodigo: 123,
          bimestre: 1,
          nomeDisciplina: 'Português',
        },
        {
          notaConceito: 1.5,
          disciplinaCodigo: 222,
          bimestre: 1,
          nomeDisciplina: 'Português',
        },
        {
          notaConceito: 8.5,
          disciplinaCodigo: 123,
          bimestre: 1,
          nomeDisciplina: 'Português asdf asdf asdf',
        },

        {
          notaConceito: 9,
          disciplinaCodigo: 222,
          bimestre: 2,
          nomeDisciplina: 'Português',
        },
      ],
      regenciaExpandida: false,
    },
    {
      nome: 'Alvaro Ramos Grassi',
      numeroChamada: 1,
      totalFaltas: 12,
      totalAusenciasCompensadas: 12,
      frequencia: 70,
      notasConceitoBimestre: [
        {
          notaConceito: 8.5,
          disciplinaCodigo: 123,
          bimestre: 1,
        },
        {
          notaConceito: 7.5,
          disciplinaCodigo: 123,
          bimestre: 1,
        },
        {
          notaConceito: 1.5,
          disciplinaCodigo: 222,
          bimestre: 1,
        },
        {
          notaConceito: 8.5,
          disciplinaCodigo: 123,
          bimestre: 1,
        },
        {
          notaConceito: 3.5,
          disciplinaCodigo: 123,
          bimestre: 2,
        },
        {
          notaConceito: 9,
          disciplinaCodigo: 222,
          bimestre: 2,
        },
      ],
      notasConceitoFinal: [
        {
          notaConceito: 8.5,
          disciplinaCodigo: 123,
          bimestre: 1,
          nomeDisciplina: 'Português',
        },
        {
          notaConceito: 7.5,
          disciplinaCodigo: 123,
          bimestre: 1,
          nomeDisciplina: 'Português',
        },
        {
          notaConceito: 1.5,
          disciplinaCodigo: 222,
          bimestre: 1,
          nomeDisciplina: 'Português',
        },
        {
          notaConceito: 8.5,
          disciplinaCodigo: 123,
          bimestre: 1,
          nomeDisciplina: 'Português',
        },
        {
          notaConceito: 3.5,
          disciplinaCodigo: 123,
          bimestre: 2,
          nomeDisciplina: 'Português',
        },
        {
          notaConceito: 6,
          disciplinaCodigo: 123,
          bimestre: 2,
          nomeDisciplina: 'Português asdfasdfas',
        },
        {
          notaConceito: 9,
          disciplinaCodigo: 222,
          bimestre: 2,
          nomeDisciplina: 'Português',
        },
      ],
      regenciaExpandida: false,
    },
  ]);

  return (
    <Card>
      <div className="col-md-12">
        <Lista>
          <div className="botao-ordenacao-avaliacao">
            <Ordenacao
              conteudoParaOrdenar={alunos}
              ordenarColunaNumero="numeroChamada"
              ordenarColunaTexto="nome"
              retornoOrdenado={retorno => {
                setAlunos(retorno);
              }}
              className="btn-ordenacao"
            />
            {ehRegencia && (
              <div className="lista-disciplinas">
                {disciplinasRegencia.map(disciplina => (
                  <span
                    className={`btn-disciplina ${
                      disciplina.ativa ? 'ativa' : ''
                    }`}
                    onClick={() => setDisciplinaAtiva(disciplina)}
                  >
                    {disciplina.nome}
                  </span>
                ))}
              </div>
            )}
          </div>
          {exibirLista && (
            <table className="table mt-4">
              <thead className="tabela-fechamento-final-thead">
                <tr>
                  <th className="col-nome-aluno" colSpan="2">
                    Nome
                  </th>
                  <th className="sticky-col">{ehNota ? 'Nota' : 'Conceito'}</th>
                  <th className="sticky-col width-120">Total de Faltas</th>
                  <th className="sticky-col">Total de Ausências Compensadas</th>
                  <th className="sticky-col">%Freq.</th>
                  <th className="sticky-col head-conceito">
                    {ehNota ? 'Nota Final' : 'Conceito Final'}
                  </th>
                </tr>
              </thead>
              <tbody className="tabela-fechamento-final-tbody">
                {alunos.map((aluno, i) => {
                  return (
                    <>
                      <LinhaAluno
                        aluno={aluno}
                        ehRegencia={ehRegencia}
                        disciplinaSelecionada={disciplinaSelecionada}
                      />
                    </>
                  );
                })}
              </tbody>
            </table>
          )}
        </Lista>
      </div>
      <Auditoria
        criadoPor={auditoria.criadoPor}
        criadoEm={auditoria.criadoEm}
        alteradoPor={auditoria.alteradoPor}
        alteradoEm={auditoria.alteradoEm}
        criadoRf={auditoria.criadoRf}
        alteradoRf={auditoria.alteradoRf}
      />
    </Card>
  );
};

export default FechamentoFinal;
