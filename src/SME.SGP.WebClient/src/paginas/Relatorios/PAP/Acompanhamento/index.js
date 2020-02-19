import React, {
  useState,
  useReducer,
  useMemo,
  useCallback,
  useEffect,
} from 'react';

// Redux
import { useSelector } from 'react-redux';

// Componentes SGP
import { Cabecalho, Ordenacao } from '~/componentes-sgp';

// Componentes
import { Card, Loader, ButtonGroup, Grid } from '~/componentes';
import PeriodosDropDown from './componentes/PeriodosDropDown';
import EixoObjetivo from './componentes/EixoObjetivo';
import BarraNavegacao from './componentes/BarraNavegacao';
import TabelaAlunos from './componentes/TabelaAlunos';
import AlertaSelecionarTurma from './componentes/AlertaSelecionarTurma';

// Estilos
import { Linha } from '~/componentes/EstilosGlobais';

// Serviços
import AcompanhamentoPAPServico from '~/servicos/Paginas/Relatorios/PAP/Acompanhamento';
import { erro, confirmar } from '~/servicos/alertas';
import history from '~/servicos/history';

// Reducer Hook
import Reducer, {
  estadoInicial,
  carregarAlunos,
  carregarPeriodo,
  carregarEixos,
  carregarObjetivos,
  carregarRespostas,
  setarObjetivoAtivo,
} from './reducer';

// Utils
import { valorNuloOuVazio } from '~/utils/funcoes/gerais';

function RelatorioPAPAcompanhamento() {
  const [estado, disparar] = useReducer(Reducer, estadoInicial);
  const [periodo, setPeriodo] = useState(undefined);
  const [ordenacao, setOrdenacao] = useState(2);
  const [modoEdicao, setModoEdicao] = useState(false);
  const [carregando, setCarregando] = useState(false);
  const [estadoOriginalAlunos, setEstadoOriginalAlunos] = useState(null);
  const { turmaSelecionada } = useSelector(store => store.usuario);

  const dispararAlteracoes = dados => {
    setEstadoOriginalAlunos(dados.periodo.alunos);
    disparar(carregarAlunos(dados.periodo.alunos));
    disparar(carregarPeriodo(dados.periodo));
    disparar(carregarEixos(dados.eixos));
    disparar(
      carregarObjetivos(dados.objetivos.sort((a, b) => a.ordem - b.ordem))
    );
    disparar(carregarRespostas(dados.respostas));
  };

  const salvarAlteracoes = useCallback(
    async objetivo => {
      try {
        setCarregando(true);
        const req = await AcompanhamentoPAPServico.Salvar({
          ordenacao,
          periodo: {
            ...estado.Periodo,
            alunos: estado.Alunos,
          },
        });

        if (req.status === 200) {
          dispararAlteracoes(req.data);
          if (objetivo) {
            disparar(setarObjetivoAtivo(objetivo.id));
          }
          setModoEdicao(false);
          setCarregando(false);
        }
      } catch (err) {
        setCarregando(false);
        erro(`${err.response.data.mensagens[0]}`);
      }
    },
    [estado.Alunos, estado.Periodo, ordenacao]
  );

  const onChangeObjetivoHandler = useCallback(
    async objetivo => {
      salvarAlteracoes(objetivo);
    },
    [salvarAlteracoes]
  );

  const limparTela = useCallback(() => {
    dispararAlteracoes({
      periodo: { alunos: [] },
      eixos: [],
      respostas: [],
      objetivos: [],
    });
    disparar(setarObjetivoAtivo({ id: 0 }));
    setPeriodo(undefined);
    setCarregando(false);
    return false;
  }, []);

  const onChangePeriodoHandler = async valor => {
    try {
      setCarregando(true);

      if (modoEdicao) {
        const confirmou = await confirmar(
          'Atenção',
          'Você não salvou as informações preenchidas.',
          'Deseja realmente cancelar as alterações?'
        );
        if (!confirmou) {
          setCarregando(false);
          return false;
        }
      }

      setModoEdicao(false);
      if (valorNuloOuVazio(valor)) {
        limparTela();
      } else {
        setPeriodo(valor);

        const { data } = await AcompanhamentoPAPServico.ListarAlunos({
          TurmaId: turmaSelecionada.turma,
          PeriodoId: valor,
        });

        if (!data) {
          erro('Não foram encontrados dados para a turma e período selecionados.P');
          setCarregando(false);
          return false;
        }

        dispararAlteracoes(data);
        disparar(setarObjetivoAtivo(estado.Objetivos[0]));
        setCarregando(false);
      }
    } catch (err) {
      setCarregando(false);

      if (err.response) {
        const { data } = err.response;
        if (data) {
          const { mensagens } = data;
          erro(`${mensagens[0]}`);
        } else {
          erro('Não foi possível completar a requisição');
        }
      } else {
        erro('Ocorreu um erro interno, por favor contate o suporte');
      }
    }
    return true;
  };

  const onChangeRespostaHandler = async (aluno, valor) => {
    setModoEdicao(true);
    const alunoCorrente = estado.Alunos.find(
      x => x.codAluno === aluno.codAluno
    );

    const novaResposta = {
      respostaId: String(valor),
      objetivoId: estado.ObjetivoAtivo.id,
    };

    let respostasAluno = [];
    if (!valorNuloOuVazio(valor)) {
      respostasAluno =
        alunoCorrente.respostas && alunoCorrente.respostas.length > 0
          ? [
            ...alunoCorrente.respostas.filter(
              y => y.objetivoId !== estado.ObjetivoAtivo.id
            ),
            novaResposta,
          ]
          : [novaResposta];
    } else {
      respostasAluno =
        alunoCorrente.respostas && alunoCorrente.respostas.length > 0
          ? [
            ...alunoCorrente.respostas.filter(
              y => y.objetivoId !== estado.ObjetivoAtivo.id
            ),
          ]
          : [];
    }

    disparar(
      carregarAlunos(
        estado.Alunos.map(item =>
          item.codAluno === aluno.codAluno
            ? {
              ...aluno,
              respostas: respostasAluno,
            }
            : item
        )
      )
    );
  };

  const objetivosCorrentes = useMemo(() => {
    const objetivos = [];
    const eixos = estado.Eixos.filter(
      x => x.periodoId === Number(periodo) || x.periodoId === 0
    );

    eixos.forEach(item => {
      estado.Objetivos.forEach(obj => {
        if (Number(obj.eixoId) === Number(item.id)) {
          objetivos.push(obj);
        }
      });
    });

    return objetivos;
  }, [estado.Eixos, estado.Objetivos, periodo]);

  const respostasCorrentes = useMemo(() => {
    return (
      estado.ObjetivoAtivo &&
      estado.Respostas.filter(x => x.objetivoId === estado.ObjetivoAtivo.id)
    );
  }, [estado.ObjetivoAtivo, estado.Respostas]);

  const onClickCancelarHandler = useCallback(async () => {
    if (!modoEdicao) return;
    const confirmou = await confirmar(
      'Atenção',
      'Você não salvou as informações preenchidas.',
      'Deseja realmente cancelar as alterações?'
    );
    if (confirmou) {
      setModoEdicao(false);
      disparar(carregarAlunos(estadoOriginalAlunos));
    }
  }, [estadoOriginalAlunos, modoEdicao]);

  const onClickVoltarHandler = useCallback(async () => {
    if (!modoEdicao) {
      history.push('/');
      return;
    }

    const confirmou = await confirmar(
      'Atenção',
      'Você não salvou as informações preenchidas.',
      'Deseja realmente cancelar as alterações?'
    );
    if (confirmou) {
      setModoEdicao(false);
      history.push('/');
    }
  }, [modoEdicao]);

  useEffect(() => {
    limparTela();
  }, [limparTela, turmaSelecionada]);

  return (
    <>
      <AlertaSelecionarTurma />
      <Cabecalho pagina="Relatório de encaminhamento e acompanhamento do PAP" />
      <Loader loading={carregando}>
        <Card mx="mx-0">
          <ButtonGroup
            somenteConsulta
            permissoesTela={{
              podeConsultar: true,
              podeAlterar: true,
              podeIncluir: true,
              podeExcluir: true,
            }}
            modoEdicao={modoEdicao}
            temItemSelecionado
            onClickVoltar={() => onClickVoltarHandler()}
            onClickBotaoPrincipal={() => salvarAlteracoes(estado.ObjetivoAtivo)}
            onClickCancelar={() => onClickCancelarHandler()}
            labelBotaoPrincipal="Salvar"
            desabilitarBotaoPrincipal={!modoEdicao || !periodo}
          />
          <Grid className="p-0" cols={12}>
            <Linha className="row m-0">
              <Grid cols={3}>
                <PeriodosDropDown
                  onChangePeriodo={onChangePeriodoHandler}
                  valor={periodo}
                  desabilitado={
                    turmaSelecionada.turma === null ||
                    turmaSelecionada.turma === undefined
                  }
                />
              </Grid>
            </Linha>
          </Grid>
          <Grid className="p-0 mt-4" cols={12}>
            <BarraNavegacao
              objetivos={objetivosCorrentes}
              objetivoAtivo={estado.ObjetivoAtivo}
              onChangeObjetivo={objetivo => onChangeObjetivoHandler(objetivo)}
            />
          </Grid>
          <Grid className="p-0 mt-2" cols={12}>
            <EixoObjetivo
              eixo={
                estado.ObjetivoAtivo &&
                estado.Eixos.filter(
                  x => x.id === estado.ObjetivoAtivo.eixoId
                )[0]
              }
              objetivo={estado.ObjetivoAtivo}
            />
          </Grid>
          <Grid className="p-0 mt-2" cols={12}>
            <Ordenacao
              retornoOrdenado={retorno => disparar(carregarAlunos(retorno))}
              ordenarColunaNumero="numeroChamada"
              ordenarColunaTexto="nome"
              conteudoParaOrdenar={estado.Alunos}
              desabilitado={estado.Alunos.length <= 0}
              onChangeOrdenacao={valor => setOrdenacao(valor)}
            />
          </Grid>
          <Grid className="p-0 mt-2" cols={12}>
            <TabelaAlunos
              alunos={estado.Alunos}
              objetivoAtivo={estado.ObjetivoAtivo}
              respostas={respostasCorrentes}
              onChangeResposta={onChangeRespostaHandler}
            />
          </Grid>
        </Card>
      </Loader>
    </>
  );
}

export default RelatorioPAPAcompanhamento;
