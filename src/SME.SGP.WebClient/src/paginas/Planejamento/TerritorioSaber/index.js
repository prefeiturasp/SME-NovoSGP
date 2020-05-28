import React, { useState, useEffect, useMemo, useCallback } from 'react';

// Redux
import { useSelector } from 'react-redux';

// Componentes SGP
import { Cabecalho } from '~/componentes-sgp';

// Componentes
import {
  Card,
  ButtonGroup,
  Grid,
  PainelCollapse,
  Loader,
  LazyLoad,
} from '~/componentes';
import AlertaSelecionarTurma from './componentes/AlertaSelecionarTurma';
import DropDownTerritorios from './componentes/DropDownTerritorios';

// Estilos
import { Linha } from '~/componentes/EstilosGlobais';

// Serviços
import { erro, sucesso } from '~/servicos/alertas';
import TerritorioSaberServico from '~/servicos/Paginas/TerritorioSaber';
import history from '~/servicos/history';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';

// Utils
import { valorNuloOuVazio } from '~/utils/funcoes/gerais';

// DTOs
import RotasDto from '~/dtos/rotasDto';

// Componentes internos
const DesenvolvimentoReflexao = React.lazy(() =>
  import('./componentes/DesenvolvimentoReflexao')
);

function TerritorioSaber() {
  const [carregando, setCarregando] = useState(false);
  const [bimestreAberto, setBimestreAberto] = useState(false);
  const [somenteConsulta, setSomenteConsulta] = useState(false);
  const [territorioSelecionado, setTerritorioSelecionado] = useState('');
  const [dados, setDados] = useState({
    bimestres: [],
    id: undefined,
  });

  const permissoesTela = useSelector(store => store.usuario.permissoes);
  const { turmaSelecionada } = useSelector(estado => estado.usuario);

  const habilitaCollapse = useMemo(
    () =>
      territorioSelecionado &&
      turmaSelecionada.anoLetivo &&
      turmaSelecionada.turma &&
      turmaSelecionada.unidadeEscolar,
    [
      territorioSelecionado,
      turmaSelecionada.anoLetivo,
      turmaSelecionada.turma,
      turmaSelecionada.unidadeEscolar,
    ]
  );

  useEffect(() => {
    async function buscarPlanejamento() {
      try {
        setCarregando(true);
        const {
          data,
          status,
        } = await TerritorioSaberServico.buscarPlanejamento({
          turmaId: turmaSelecionada.turma,
          ueId: turmaSelecionada.unidadeEscolar,
          anoLetivo: turmaSelecionada.anoLetivo,
          territorioExperienciaId: territorioSelecionado,
        });

        if (data && status === 200) {
          setDados(estado => ({ ...estado, bimestres: data }));
          setCarregando(false);
        }
      } catch (error) {
        erro('Não foi possível buscar planejamento.');
        setCarregando(false);
      }
    }

    if (habilitaCollapse) buscarPlanejamento();

    if (Object.keys(turmaSelecionada).length === 0) {
      setTerritorioSelecionado('');
    }
  }, [
    habilitaCollapse,
    territorioSelecionado,
    turmaSelecionada,
    turmaSelecionada.anoLetivo,
    turmaSelecionada.turma,
    turmaSelecionada.unidadeEscolar,
  ]);

  useEffect(() => {
    setSomenteConsulta(verificaSomenteConsulta(permissoesTela));
  }, [permissoesTela]);

  const salvarPlanejamento = useCallback(() => {
    async function salvar() {
      try {
        setCarregando(true);
        const {
          data,
          status,
        } = await TerritorioSaberServico.salvarPlanejamento({
          turmaId: turmaSelecionada.turma,
          escolaId: turmaSelecionada.unidadeEscolar,
          anoLetivo: turmaSelecionada.anoLetivo,
          territorioExperienciaId: territorioSelecionado,
          bimestres: dados.bimestres.filter(
            x =>
              !valorNuloOuVazio(x.desenvolvimento) ||
              !valorNuloOuVazio(x.reflexao)
          ),
          id: dados.id,
        });

        if (data || status === 200) {
          setCarregando(false);
          sucesso('Planejamento salvo com sucesso.');
        }
      } catch (error) {
        setCarregando(false);
        erro('Não foi possível salvar planejamento.');
      }
    }

    salvar();
  }, [
    dados.bimestres,
    dados.id,
    territorioSelecionado,
    turmaSelecionada.anoLetivo,
    turmaSelecionada.turma,
    turmaSelecionada.unidadeEscolar,
  ]);

  const onChangeBimestre = useCallback(
    (bimestre, dadosBimestre) => {
      setDados(estadoAntigo => ({
        bimestres: estadoAntigo.bimestres.map(item =>
          item.bimestre === bimestre
            ? {
                ...dadosBimestre,
                territorioExperienciaId: territorioSelecionado,
              }
            : item
        ),
      }));
    },
    [territorioSelecionado]
  );

  return (
    <>
      <AlertaSelecionarTurma />
      <Cabecalho pagina="Planejamento anual do Território do Saber" />
      <Card>
        <ButtonGroup
          permissoesTela={permissoesTela[RotasDto.TERRITORIO_SABER]}
          onClickVoltar={() => history.push('/')}
          onClickBotaoPrincipal={() => salvarPlanejamento()}
          onClickCancelar={() => null}
          labelBotaoPrincipal="Salvar"
          somenteConsulta={somenteConsulta}
          desabilitarBotaoPrincipal={false}
        />
        <Grid cols={12}>
          <Linha className="row mb-0">
            <Grid cols={4}>
              <DropDownTerritorios
                territorioSelecionado={territorioSelecionado}
                onChangeTerritorio={useCallback(
                  valor => setTerritorioSelecionado(valor || ''),
                  []
                )}
              />
            </Grid>
          </Linha>
        </Grid>
        <Grid className="p-0 m-0 mt-4" cols={12}>
          <Loader loading={carregando} tip="Carregando...">
            <PainelCollapse
              onChange={painel => setBimestreAberto(painel)}
              activeKey={habilitaCollapse && bimestreAberto}
            >
              <PainelCollapse.Painel
                disabled={!habilitaCollapse}
                temBorda
                header="Primeiro Bimestre"
                key="1"
              >
                <LazyLoad>
                  <DesenvolvimentoReflexao
                    bimestre={1}
                    onChange={onChangeBimestre}
                    dadosBimestre={
                      dados?.bimestres?.filter(item => item.bimestre === 1)[0]
                    }
                  />
                </LazyLoad>
              </PainelCollapse.Painel>
              <PainelCollapse.Painel
                disabled={!habilitaCollapse}
                temBorda
                header="Segundo Bimestre"
                key="2"
              >
                <DesenvolvimentoReflexao
                  bimestre={2}
                  onChange={onChangeBimestre}
                  dadosBimestre={
                    dados?.bimestres?.filter(item => item.bimestre === 2)[0]
                  }
                />
              </PainelCollapse.Painel>
              <PainelCollapse.Painel
                disabled={!habilitaCollapse}
                temBorda
                header="Terceiro Bimestre"
                key="3"
              >
                <DesenvolvimentoReflexao
                  bimestre={3}
                  onChange={onChangeBimestre}
                  dadosBimestre={
                    dados?.bimestres?.filter(item => item.bimestre === 3)[0]
                  }
                />
              </PainelCollapse.Painel>
              <PainelCollapse.Painel
                disabled={!habilitaCollapse}
                temBorda
                header="Quarto Bimestre"
                key="4"
              >
                <DesenvolvimentoReflexao
                  bimestre={4}
                  onChange={onChangeBimestre}
                  dadosBimestre={
                    dados?.bimestres?.filter(item => item.bimestre === 4)[0]
                  }
                />
              </PainelCollapse.Painel>
            </PainelCollapse>
          </Loader>
        </Grid>
      </Card>
    </>
  );
}

TerritorioSaber.propTypes = {};

TerritorioSaber.defaultProps = {};

export default TerritorioSaber;
