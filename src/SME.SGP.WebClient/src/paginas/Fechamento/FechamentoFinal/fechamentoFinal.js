import React, {
  forwardRef,
  useImperativeHandle,
  useState,
  useEffect,
  useCallback,
} from 'react';
import PropTypes from 'prop-types';
import moment from 'moment';
import { Ordenacao } from '~/componentes-sgp';
import { Lista } from './fechamentoFinal.css';
import { Auditoria } from '~/componentes';
import LinhaAluno from './linhaAluno';
import ServicoFechamentoFinal from '~/servicos/Paginas/DiarioClasse/ServicoFechamentoFinal';
import { erros } from '~/servicos/alertas';
import ServicoNotaConceito from '~/servicos/Paginas/DiarioClasse/ServicoNotaConceito';
import ServicoDisciplina from '~/servicos/Paginas/ServicoDisciplina';

const FechamentoFinal = forwardRef((props, ref) => {
  const {
    turmaCodigo,
    disciplinaCodigo,
    ehRegencia,
    turmaPrograma,
    onChange,
  } = props;
  const [ehNota, setEhNota] = useState(true);
  const [disciplinaSelecionada, setDisciplinaSelecionada] = useState();
  const [listaConceitos, setListaConceitos] = useState([]);
  const [exibirLista, setExibirLista] = useState(
    (ehRegencia && !!disciplinaSelecionada) || !ehRegencia
  );

  const [disciplinasRegencia, setDisciplinasRegencia] = useState([]);
  const [notasEmEdicao, setNotasEmEdicao] = useState([]);

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

  const obterFechamentoFinal = useCallback(() => {
    ServicoFechamentoFinal.obter(turmaCodigo, disciplinaCodigo, ehRegencia)
      .then(resposta => {
        setAlunos(resposta.data.alunos);
        setEhNota(resposta.data.ehNota);
      })
      .catch(e => erros(e));
  }, [disciplinaCodigo, ehRegencia, turmaCodigo]);

  useImperativeHandle(ref, () => ({
    cancelar() {
      obterFechamentoFinal();
      setNotasEmEdicao([]);
    },
  }));

  useEffect(() => {
    obterFechamentoFinal();
  }, [obterFechamentoFinal]);

  useEffect(() => {
    if (!ehNota)
      ServicoNotaConceito.obterTodosConceitos(moment().format('YYYY-MM-DD'))
        .then(resposta => {
          setListaConceitos(resposta.data);
        })
        .catch(e => erros(e));
  }, [ehNota]);

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
    const notas = notasEmEdicao;
    const notaEmEdicao = notasEmEdicao.find(
      c =>
        c.alunoRf == aluno.numeroChamada &&
        c.componenteCurricularCodigo == disciplina
    );
    if (notaEmEdicao) {
      notaEmEdicao.conceitoId = ehNota ? 0 : Number(nota);
      notaEmEdicao.nota = ehNota ? nota : 0;
    } else
      notas.push({
        alunoRf: aluno.numeroChamada,
        componenteCurricularCodigo: disciplina,
        conceitoId: ehNota ? 0 : Number(nota),
        nota: ehNota ? nota : 0,
      });
    setNotasEmEdicao([...notas]);
    onChange(notas);
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
                        onChange={onChangeNotaAluno}
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
});

FechamentoFinal.propTypes = {
  turmaCodigo: PropTypes.string,
  disciplinaCodigo: PropTypes.string,
  ehRegencia: PropTypes.bool,
  turmaPrograma: PropTypes.bool,
  onChange: PropTypes.func,
};

FechamentoFinal.defaultProps = {
  turmaCodigo: '1',
  disciplinaCodigo: '1',
  ehRegencia: false,
  turmaPrograma: false,
  onChange: () => {},
};

export default FechamentoFinal;
