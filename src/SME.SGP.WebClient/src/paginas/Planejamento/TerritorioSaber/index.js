import React, { useState, useEffect } from 'react';
import t from 'prop-types';

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

// Componentes internos
const DesenvolvimentoReflexao = React.lazy(() =>
  import('./componentes/DesenvolvimentoReflexao')
);

function TerritorioSaber() {
  const [carregando, setCarregando] = useState(true);
  const [territorioSelecionado, setTerritorioSelecionado] = useState('');
  const [dados, setDados] = useState([]);

  const { turmaSelecionada } = useSelector(estado => estado.usuario);

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
          setDados(data);
          setCarregando(false);
        }
      } catch (error) {
        erro('Não foi possível buscar planejamento.');
        setCarregando(false);
      }
    }
    buscarPlanejamento();
  }, [
    territorioSelecionado,
    turmaSelecionada.anoLetivo,
    turmaSelecionada.turma,
    turmaSelecionada.unidadeEscolar,
  ]);

  return (
    <>
      <AlertaSelecionarTurma />
      <Cabecalho pagina="Planejamento anual do Território do Saber" />
      <Card>
        <ButtonGroup
          somenteConsulta
          permissoesTela={{
            podeIncluir: true,
            podeAlterar: true,
            podeExcluir: true,
            podeInserir: true,
          }}
          onClickVoltar={() => null}
          onClickBotaoPrincipal={() => null}
          onClickCancelar={() => null}
          labelBotaoPrincipal="Salvar"
          desabilitarBotaoPrincipal
        />
        <Grid cols={12}>
          <Linha className="row mb-0">
            <Grid cols={6}>
              <DropDownTerritorios
                territorioSelecionado={territorioSelecionado}
                onChangeTerritorio={valor =>
                  setTerritorioSelecionado(valor || '')
                }
              />
            </Grid>
          </Linha>
        </Grid>
        <Grid className="p-0 m-0 mt-4" cols={12}>
          <Loader loading={carregando} tip="Carregando...">
            <PainelCollapse>
              <PainelCollapse.Painel temBorda header="Primeiro Bimestre">
                <LazyLoad>
                  <DesenvolvimentoReflexao
                    bimestre={1}
                    dadosBimestre={dados?.filter(item => item.bimestre === 1)}
                  />
                </LazyLoad>
              </PainelCollapse.Painel>
              <PainelCollapse.Painel temBorda header="Segundo Bimestre">
                <DesenvolvimentoReflexao
                  bimestre={2}
                  dadosBimestre={dados?.filter(item => item.bimestre === 2)}
                />
              </PainelCollapse.Painel>
              <PainelCollapse.Painel temBorda header="Terceiro Bimestre">
                <DesenvolvimentoReflexao
                  bimestre={3}
                  dadosBimestre={dados?.filter(item => item.bimestre === 3)}
                />
              </PainelCollapse.Painel>
              <PainelCollapse.Painel temBorda header="Quarto Bimestre">
                <DesenvolvimentoReflexao
                  bimestre={4}
                  dadosBimestre={dados?.filter(item => item.bimestre === 4)}
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
