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
import Alert from '~/componentes/alert';
import AlertaSelecionarTurma from './componentes/AlertaSelecionarTurma';
import DropDownTerritorios from './componentes/DropDownTerritorios';

// Estilos
import { Linha } from '~/componentes/EstilosGlobais';

// Serviços
import { erro, sucesso, confirmar, erros } from '~/servicos/alertas';
import TerritorioSaberServico from '~/servicos/Paginas/TerritorioSaber';
import history from '~/servicos/history';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';

// Utils
import { valorNuloOuVazio } from '~/utils/funcoes/gerais';

// DTOs
import RotasDto from '~/dtos/rotasDto';
import { URL_HOME } from '~/constantes/url';
import AlertaModalidadeInfantil from '~/componentes-sgp/AlertaModalidadeInfantil/alertaModalidadeInfantil';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';

// Componentes internos
const DesenvolvimentoReflexao = React.lazy(() =>
  import('./componentes/DesenvolvimentoReflexao')
);

function TerritorioSaber() {
  const [modoEdicao, setModoEdicao] = useState(false);
  const [carregando, setCarregando] = useState(false);
  const [bimestreAberto, setBimestreAberto] = useState(false);
  const [somenteConsulta, setSomenteConsulta] = useState(false);
  const [territorioSelecionado, setTerritorioSelecionado] = useState('');

  const [
    mostraMensagemSemTerritorios,
    setMostraMensagemSemTerritorios,
  ] = useState(false);

  const [dados, setDados] = useState({ bimestres: [] });

  const permissoesTela = useSelector(store => store.usuario.permissoes);
  const { turmaSelecionada } = useSelector(estado => estado.usuario);
  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );

  const habilitaCollapse = useMemo(
    () =>
      territorioSelecionado &&
      turmaSelecionada.anoLetivo &&
      turmaSelecionada.turma &&
      turmaSelecionada.unidadeEscolar &&
      !ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada),
    [territorioSelecionado, turmaSelecionada, modalidadesFiltroPrincipal]
  );

  const buscarPlanejamento = useCallback(async () => {
    try {
      setCarregando(true);
      const { data, status } = await TerritorioSaberServico.buscarPlanejamento({
        turmaId: turmaSelecionada.turma,
        ueId: turmaSelecionada.unidadeEscolar,
        anoLetivo: turmaSelecionada.anoLetivo,
        territorioExperienciaId: territorioSelecionado,
      });

      if (data && status === 200) {
        setDados(estado => ({ ...estado, bimestres: data }));
        setCarregando(false);
      } else {
        setDados({ bimestres: [] });
      }
    } catch (error) {
      erro('Não foi possível buscar planejamento.');
      setCarregando(false);
    }
  }, [territorioSelecionado, turmaSelecionada]);

  useEffect(() => {
    if (habilitaCollapse) buscarPlanejamento();

    if (Object.keys(turmaSelecionada).length === 0) {
      setTerritorioSelecionado('');
    }
    if (ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada)) {
      setTerritorioSelecionado('');
    }
  }, [
    buscarPlanejamento,
    habilitaCollapse,
    turmaSelecionada,
    modalidadesFiltroPrincipal,
  ]);

  useEffect(() => {
    const permissoes = permissoesTela[RotasDto.TERRITORIO_SABER];
    const naoSetarSomenteConsultaNoStore = ehTurmaInfantil(
      modalidadesFiltroPrincipal,
      turmaSelecionada
    );
    setSomenteConsulta(
      verificaSomenteConsulta(permissoes, naoSetarSomenteConsultaNoStore)
    );
  }, [turmaSelecionada, permissoesTela, modalidadesFiltroPrincipal]);

  const salvarPlanejamento = useCallback(
    (irParaHome = false) => {
      async function salvar() {
        setCarregando(true);
        const retorno = await TerritorioSaberServico.salvarPlanejamento({
          turmaId: turmaSelecionada.turma,
          escolaId: turmaSelecionada.unidadeEscolar,
          anoLetivo: turmaSelecionada.anoLetivo,
          territorioExperienciaId: territorioSelecionado,
          bimestres: dados.bimestres.filter(
            x =>
              !valorNuloOuVazio(x.desenvolvimento) ||
              !valorNuloOuVazio(x.reflexao)
          ),
        }).catch(e => erros(e));
        if (retorno && retorno.status === 200) {
          sucesso('Planejamento salvo com sucesso.');
          if (irParaHome) {
            history.push(URL_HOME);
          } else {
            setBimestreAberto(false);
            buscarPlanejamento();
            setModoEdicao(false);
          }
        }

        setCarregando(false);
      }

      salvar();
    },
    [
      dados.bimestres,
      territorioSelecionado,
      turmaSelecionada.anoLetivo,
      turmaSelecionada.turma,
      turmaSelecionada.unidadeEscolar,
      buscarPlanejamento,
    ]
  );

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
      setModoEdicao(true);
    },
    [territorioSelecionado]
  );

  const onClickCancelar = async () => {
    if (modoEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja realmente cancelar as alterações?'
      );
      if (confirmou) {
        setBimestreAberto(false);
        setDados({ bimestres: [] });
        buscarPlanejamento();
      }
    }
  };

  const onClickVoltar = async () => {
    if (modoEdicao) {
      const confirmado = await confirmar(
        'Atenção',
        'Suas alterações não foram salvas, deseja salvar agora?'
      );
      if (confirmado) {
        salvarPlanejamento(true);
      } else {
        history.push(URL_HOME);
      }
    } else {
      history.push(URL_HOME);
    }
  };

  const onBuscarTerritorios = response => {
    setMostraMensagemSemTerritorios(response);
  };

  return (
    <>
      <div className="col-md-12">
        {mostraMensagemSemTerritorios &&
        turmaSelecionada &&
        !ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada) ? (
          <Alert
            alerta={{
              tipo: 'warning',
              id: 'territorios-aviso-sem-territori',
              mensagem:
                'Apenas é possível realizar este planejamento para componentes de territórios do saber.',
              estiloTitulo: { fontSize: '18px' },
            }}
            className="mb-2"
          />
        ) : null}
      </div>
      <AlertaModalidadeInfantil />
      <AlertaSelecionarTurma />
      <Cabecalho pagina="Planejamento anual do Território do Saber" />
      <Card>
        <ButtonGroup
          permissoesTela={permissoesTela[RotasDto.TERRITORIO_SABER]}
          onClickVoltar={onClickVoltar}
          onClickBotaoPrincipal={() => salvarPlanejamento()}
          onClickCancelar={onClickCancelar}
          labelBotaoPrincipal="Salvar"
          somenteConsulta={somenteConsulta}
          desabilitarBotaoPrincipal={
            ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada) ||
            !territorioSelecionado ||
            !modoEdicao
          }
          modoEdicao={modoEdicao}
        />
        <Grid cols={12}>
          <Linha className="row mb-0">
            <Grid cols={12}>
              <DropDownTerritorios
                onBuscarTerritorios={onBuscarTerritorios}
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
                header="1º Bimestre"
                key="1"
              >
                {carregando ? (
                  ''
                ) : (
                  <LazyLoad>
                    <DesenvolvimentoReflexao
                      bimestre={1}
                      onChange={onChangeBimestre}
                      dadosBimestre={
                        dados?.bimestres?.filter(item => item.bimestre === 1)[0]
                      }
                    />
                  </LazyLoad>
                )}
              </PainelCollapse.Painel>
              <PainelCollapse.Painel
                disabled={!habilitaCollapse}
                temBorda
                header="2º Bimestre"
                key="2"
              >
                {carregando ? (
                  ''
                ) : (
                  <LazyLoad>
                    <DesenvolvimentoReflexao
                      bimestre={2}
                      onChange={onChangeBimestre}
                      dadosBimestre={
                        dados?.bimestres?.filter(item => item.bimestre === 2)[0]
                      }
                    />
                  </LazyLoad>
                )}
              </PainelCollapse.Painel>
              <PainelCollapse.Painel
                disabled={!habilitaCollapse}
                temBorda
                header="3º Bimestre"
                key="3"
              >
                {carregando ? (
                  ''
                ) : (
                  <LazyLoad>
                    <DesenvolvimentoReflexao
                      bimestre={3}
                      onChange={onChangeBimestre}
                      dadosBimestre={
                        dados?.bimestres?.filter(item => item.bimestre === 3)[0]
                      }
                    />
                  </LazyLoad>
                )}
              </PainelCollapse.Painel>
              <PainelCollapse.Painel
                disabled={!habilitaCollapse}
                temBorda
                header="4º Bimestre"
                key="4"
              >
                {carregando ? (
                  ''
                ) : (
                  <LazyLoad>
                    <DesenvolvimentoReflexao
                      bimestre={4}
                      onChange={onChangeBimestre}
                      dadosBimestre={
                        dados?.bimestres?.filter(item => item.bimestre === 4)[0]
                      }
                    />
                  </LazyLoad>
                )}
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
