import React, { useState, useReducer, useMemo, useCallback } from 'react';

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

function RelatorioPAPAcompanhamento() {
  const [periodo, setPeriodo] = useState(undefined);
  const [modoEdicao, setModoEdicao] = useState(false);
  const [estadoOriginalAlunos, setEstadoOriginalAlunos] = useState(null);
  const [estado, disparar] = useReducer(Reducer, estadoInicial);
  const [carregando, setCarregando] = useState(false);
  const { turma } = useSelector(store => store.usuario.turmaSelecionada);

  const salvarAlteracoes = useCallback(
    async objetivo => {
      try {
        setCarregando(true);
        const req = await AcompanhamentoPAPServico.Salvar({
          periodo: {
            ...estado.Periodo,
            alunos: estado.Alunos,
          },
        });

        if (req.status === 200) {
          if (objetivo) {
            disparar(setarObjetivoAtivo(objetivo.id));
          }
          setCarregando(false);
        }
      } catch (err) {
        setCarregando(false);
        erro(`${err.response.data.mensagens[0]}`);
      }
    },
    [estado.Alunos, estado.Periodo]
  );

  const onChangeObjetivoHandler = useCallback(
    async objetivo => {
      salvarAlteracoes(objetivo);
    },
    [salvarAlteracoes]
  );

  const onChangePeriodoHandler = async valor => {
    try {
      setCarregando(true);
      setPeriodo(valor);
      const { data } = await AcompanhamentoPAPServico.ListarAlunos({
        TurmaId: turma,
        PeriodoId: valor,
      });

      setEstadoOriginalAlunos(data.periodo.alunos);
      disparar(carregarAlunos(data.periodo.alunos));
      disparar(carregarPeriodo(data.periodo));
      disparar(carregarEixos(data.eixos));
      disparar(carregarObjetivos(data.objetivos));
      disparar(carregarRespostas(data.respostas));
      disparar(setarObjetivoAtivo(estado.Objetivos[0]));
      setCarregando(false);
    } catch (err) {
      setCarregando(false);
      erro(`Não foi possível completar a requisição: ${JSON.stringify(err)}`);
    }
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

    const respostasAluno =
      alunoCorrente.respostas && alunoCorrente.respostas.length > 0
        ? [
            ...alunoCorrente.respostas.filter(
              y => y.objetivoId !== estado.ObjetivoAtivo.id
            ),
            novaResposta,
          ]
        : [novaResposta];

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
    return estado.Respostas.filter(
      x => x.objetivoId === estado.ObjetivoAtivo.id
    );
  }, [estado.ObjetivoAtivo.id, estado.Respostas]);

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

  return (
    <>
      <AlertaSelecionarTurma />
      <Cabecalho pagina="Registro do Projeto de Apoio Pedagógico" />
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
            onClickBotaoPrincipal={() => salvarAlteracoes()}
            onClickCancelar={() => onClickCancelarHandler()}
            labelBotaoPrincipal="Salvar"
          />
          <Grid className="p-0" cols={12}>
            <Linha className="row m-0">
              <Grid cols={3}>
                <PeriodosDropDown
                  onChangePeriodo={onChangePeriodoHandler}
                  valor={periodo}
                  desabilitado={turma === null || turma === undefined}
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
              desabilitado={estado.Alunos.length > 1}
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
