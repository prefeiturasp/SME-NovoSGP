import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import { Ordenacao } from '~/componentes-sgp';
import { Lista } from './fechamentoFinal.css';
import { Card, Auditoria } from '~/componentes';
import LinhaAluno from './linhaAluno';
import ServicoFechamentoFinal from '~/servicos/Paginas/DiarioClasse/ServicoFechamentoFinal';
import { erros } from '~/servicos/alertas';
import ServicoNotaConceito from '~/servicos/Paginas/DiarioClasse/ServicoNotaConceito';
import ServicoDisciplina from '~/servicos/Paginas/ServicoDisciplina';

const FechamentoFinal = ({
  turmaCodigo,
  disciplinaCodigo,
  ehRegencia,
  turmaPrograma,
}) => {
  const [ehNota, setEhNota] = useState(true);
  const [disciplinaSelecionada, setDisciplinaSelecionada] = useState();
  const [listaConceitos, setListaConceitos] = useState([]);
  const [exibirLista, setExibirLista] = useState(
    (ehRegencia && !!disciplinaSelecionada) || !ehRegencia
  );

  const [disciplinasRegencia, setDisciplinasRegencia] = useState([]);

  const [auditoria, setAuditoria] = useState({});
  const [alunos, setAlunos] = useState([]);

  useEffect(() => {
    setExibirLista((ehRegencia && !!disciplinaSelecionada) || !ehRegencia);
  }, [disciplinaSelecionada, ehRegencia]);

  useEffect(() => {
    if (ehRegencia) {
      ServicoDisciplina.obterDisciplinasPlanejamento(
        disciplinaCodigo,
        turmaCodigo,
        turmaPrograma,
        true
      )
        .then(resposta => {
          setDisciplinasRegencia(resposta.data);
        })
        .catch(e => erros(e));
    }
  }, [disciplinaCodigo, ehRegencia, turmaCodigo, turmaPrograma]);

  useEffect(() => {
    ServicoFechamentoFinal.obter(turmaCodigo, disciplinaCodigo, ehRegencia)
      .then(resposta => {
        setAlunos(resposta.data.alunos);
        setEhNota(resposta.data.ehNota);
      })
      .catch(e => erros(e));
  }, [disciplinaCodigo, ehRegencia, turmaCodigo]);

  useEffect(() => {
    if (!ehNota)
      ServicoNotaConceito.obterTodosConceitos()
        .then(resposta => {
          setListaConceitos(resposta.data);
        })
        .catch(e => erros(e));
  }, [ehNota]);

  const setDisciplinaAtiva = disciplina => {
    const disciplinas = disciplinasRegencia.map(c => {
      c.ativa = c.codigo == disciplina.codigo;
      return c;
    });
    setDisciplinasRegencia([...disciplinas]);
    setDisciplinaSelecionada(disciplina.codigo);
  };

  return (
    <>
      <Lista>
        <div className={`${ehRegencia && 'botao-ordenacao-avaliacao'}`}>
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
          <div className="table-responsive">
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
                        ehNota={ehNota}
                        disciplinaSelecionada={disciplinaSelecionada}
                        listaConceitos={listaConceitos}
                      />
                    </>
                  );
                })}
              </tbody>
            </table>
          </div>
        )}
      </Lista>
      <Auditoria
        criadoPor={auditoria.criadoPor}
        criadoEm={auditoria.criadoEm}
        alteradoPor={auditoria.alteradoPor}
        alteradoEm={auditoria.alteradoEm}
        criadoRf={auditoria.criadoRf}
        alteradoRf={auditoria.alteradoRf}
      />
    </>
  );
};

FechamentoFinal.propTypes = {
  turmaCodigo: PropTypes.string,
  disciplinaCodigo: PropTypes.string,
  ehRegencia: PropTypes.bool,
  turmaPrograma: PropTypes.bool,
};

FechamentoFinal.defaultProps = {
  turmaCodigo: '1',
  disciplinaCodigo: '1',
  ehRegencia: false,
  turmaPrograma: false,
};

export default FechamentoFinal;
