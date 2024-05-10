import PropTypes from 'prop-types';
import React, {
  forwardRef,
  useCallback,
  useEffect,
  useImperativeHandle,
  useState,
} from 'react';
import { useDispatch } from 'react-redux';
import shortid from 'shortid';
import { Ordenacao } from '~/componentes-sgp';
import { erros } from '~/servicos/alertas';
import ServicoFechamentoFinal from '~/servicos/Paginas/DiarioClasse/ServicoFechamentoFinal';
import ServicoNotaConceito from '~/servicos/Paginas/DiarioClasse/ServicoNotaConceito';
import ServicoDisciplina from '~/servicos/Paginas/ServicoDisciplina';

import { ContainerAuditoria, Lista } from './fechamentoFinal.css';
import LinhaAluno from './linhaAluno';
import { setExpandirLinha } from '~/redux/modulos/notasConceitos/actions';

const FechamentoFinal = forwardRef((props, ref) => {
  const {
    turmaCodigo,
    disciplinaCodigo,
    ehRegencia,
    turmaPrograma,
    onChange,
    desabilitarCampo,
    carregandoFechamentoFinal,
    bimestreCorrente,
    registraFrequencia,
    semestre,
  } = props;

  const dispatch = useDispatch();

  const [ehNota, setEhNota] = useState(true);
  const [ehSintese, setEhSintese] = useState(false);
  const [disciplinaSelecionada, setDisciplinaSelecionada] = useState();
  const [listaConceitos, setListaConceitos] = useState([]);

  const [disciplinasRegencia, setDisciplinasRegencia] = useState([]);
  const [notasEmEdicao, setNotasEmEdicao] = useState([]);

  const [auditoria, setAuditoria] = useState();
  const [alunos, setAlunos] = useState([]);
  const [dadosFechamentoFinal, setDadosFechamentoFinal] = useState();

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

  const obterListaConceitos = data => {
    return ServicoNotaConceito.obterTodosConceitos(data)
      .then(resposta => {
        setListaConceitos(resposta.data);
      })
      .catch(e => erros(e));
  };

  const resetarComponenteAtivo = () => {
    if (disciplinasRegencia && disciplinasRegencia.length) {
      const lista = disciplinasRegencia.map(c => {
        c.ativa = false;
        return c;
      });
      setDisciplinasRegencia([...lista]);
    }
  };

  const resetarTela = () => {
    resetarComponenteAtivo();
    setDadosFechamentoFinal({});
    setAlunos([]);
    setEhNota(true);
    setEhSintese(false);
    setAuditoria({});
    setDisciplinaSelecionada();
  };

  const obterFechamentoFinal = useCallback(() => {
    setNotasEmEdicao([]);
    dispatch(setExpandirLinha([]));
    carregandoFechamentoFinal(true);
    ServicoFechamentoFinal.obter(
      turmaCodigo,
      disciplinaCodigo,
      ehRegencia,
      semestre
    )
      .then(resposta => {
        if (resposta && resposta.data) {
          resposta.data.alunos.forEach(item => {
            item.notasConceitoFinal.forEach(aluno => {
              aluno.notaConceitoAtual = aluno.notaConceito;
            });
          });
          setDadosFechamentoFinal(resposta.data);
          setAlunos(resposta.data.alunos);
          setEhNota(resposta.data.ehNota);
          setEhSintese(resposta.data.ehSintese);
          setAuditoria({
            auditoriaAlteracao: resposta.data.auditoriaAlteracao,
            auditoriaInclusao: resposta.data.auditoriaInclusao,
          });

          if (!resposta.data.ehNota) {
            obterListaConceitos(resposta.data.eventoData);
          }
        }
        carregandoFechamentoFinal(false);
      })
      .catch(e => {
        resetarTela();
        erros(e);
        carregandoFechamentoFinal(false);
      });
  }, [disciplinaCodigo, ehRegencia, turmaCodigo]);

  useImperativeHandle(ref, () => ({
    cancelar() {
      obterFechamentoFinal();
    },
    salvarFechamentoFinal() {
      obterFechamentoFinal();
    },
  }));

  useEffect(() => {
    if (bimestreCorrente == 'final' && turmaCodigo && disciplinaCodigo) {
      obterFechamentoFinal();
    } else {
      resetarTela();
    }
  }, [bimestreCorrente, disciplinaCodigo]);

  const setDisciplinaAtiva = disciplina => {
    const disciplinas = disciplinasRegencia.map(c => {
      c.ativa =
        c.codigoComponenteCurricular == disciplina.codigoComponenteCurricular;
      return c;
    });
    setDisciplinasRegencia([...disciplinas]);
    setDisciplinaSelecionada(disciplina.codigoComponenteCurricular);
  };

  const onChangeNotaAluno = (aluno, nota, disciplina) => {
    if (nota !== null) {
      const notas = notasEmEdicao;
      const notaEmEdicao = notasEmEdicao.find(
        c =>
          c.alunoRf == aluno.codigo &&
          c.componenteCurricularCodigo == disciplina
      );
      if (notaEmEdicao) {
        notaEmEdicao.conceitoId = ehNota ? '' : Number(nota);
        notaEmEdicao.nota = ehNota ? nota : '';
      } else {
        notas.push({
          alunoRf: aluno.codigo,
          componenteCurricularCodigo: disciplina,
          conceitoId: ehNota ? '' : Number(nota),
          nota: ehNota ? nota : '',
        });
      }

      setNotasEmEdicao([...notas]);
      onChange(notas);
    }
  };
  return (
    <>
      <Lista>
        {alunos?.length && !ehSintese && ehRegencia ? (
          <div>
            <div className="row">
              <div className="col-md-12 d-flex justify-content-end">
                Selecione um componente para consultar as notas ou conceitos dos
                bimestres
              </div>
            </div>
          </div>
        ) : (
          ''
        )}
        {alunos && alunos.length ? (
          <div className="row pb-4" style={{ alignItems: 'center' }}>
            <div className="col-sm-12 col-md-12 col-lg-2 col-xl-3 d-flex justify-content-start">
              <Ordenacao
                conteudoParaOrdenar={alunos}
                ordenarColunaNumero="numeroChamada"
                ordenarColunaTexto="nome"
                retornoOrdenado={retorno => {
                  setAlunos(retorno);
                  dispatch(setExpandirLinha([]));
                }}
                className="btn-ordenacao"
              />
            </div>
            <div className="col-sm-12 col-md-12 col-lg-10 col-xl-9 d-flex justify-content-end">
              {!ehSintese && ehRegencia && (
                <div className="lista-disciplinas">
                  {disciplinasRegencia.map(disciplina => (
                    <span
                      key={shortid.generate()}
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
          </div>
        ) : (
          ''
        )}
        <div className="table-responsive">
          <table className="table mt-4">
            <thead className="tabela-fechamento-final-thead">
              <tr>
                <th className="col-nome-aluno" colSpan="2">
                  Nome
                </th>
                <th>{ehSintese ? 'Síntese' : ehNota ? 'Nota' : 'Conceito'}</th>
                <th className="width-120">Total de Faltas</th>
                <th>Total de Ausências Compensadas</th>
                {ehSintese ? (
                  ''
                ) : (
                  <th className="head-conceito">
                    {ehNota ? 'Nota Final' : 'Conceito Final'}
                  </th>
                )}
                {registraFrequencia ? <th>%Freq.</th> : ''}
              </tr>
            </thead>
            <tbody className="tabela-fechamento-final-tbody">
              {alunos && alunos.length ? (
                alunos.map((aluno, i) => {
                  return (
                    <>
                      <LinhaAluno
                        dados={alunos}
                        aluno={aluno}
                        ehRegencia={ehRegencia}
                        ehNota={ehNota}
                        disciplinaSelecionada={disciplinaSelecionada}
                        listaConceitos={listaConceitos}
                        onChange={onChangeNotaAluno}
                        eventoData={dadosFechamentoFinal.eventoData}
                        notaMedia={dadosFechamentoFinal.notaMedia}
                        frequenciaMedia={dadosFechamentoFinal.frequenciaMedia}
                        indexAluno={i}
                        desabilitarCampo={desabilitarCampo}
                        ehSintese={ehSintese}
                        registraFrequencia={registraFrequencia}
                      />
                    </>
                  );
                })
              ) : (
                <tr>
                  <td colSpan="10" className="text-center">
                    Sem dados
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
        {auditoria ? (
          <div className="row mt-2 mb-2 mt-2">
            <div className="col-md-12">
              <ContainerAuditoria style={{ float: 'left' }}>
                <span>
                  <p>{auditoria.auditoriaInclusao || ''}</p>
                  <p>{auditoria.auditoriaAlteracao || ''}</p>
                </span>
              </ContainerAuditoria>
            </div>
          </div>
        ) : (
          ''
        )}
      </Lista>
    </>
  );
});

FechamentoFinal.propTypes = {
  turmaCodigo: PropTypes.string,
  disciplinaCodigo: PropTypes.string,
  ehRegencia: PropTypes.bool,
  turmaPrograma: PropTypes.bool,
  onChange: PropTypes.func,
  desabilitarCampo: PropTypes.bool,
  carregandoFechamentoFinal: PropTypes.func,
  bimestreCorrente: PropTypes.string,
  registraFrequencia: PropTypes.bool,
  semestre: PropTypes.number,
};

FechamentoFinal.defaultProps = {
  turmaCodigo: '1',
  disciplinaCodigo: '1',
  ehRegencia: false,
  turmaPrograma: false,
  onChange: () => {},
  desabilitarCampo: false,
  carregandoFechamentoFinal: () => {},
  bimestreCorrente: '',
  registraFrequencia: true,
  semestre: null,
};

export default FechamentoFinal;
