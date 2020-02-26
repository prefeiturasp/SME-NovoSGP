import React, { useState, useEffect } from 'react';
import t from 'prop-types';

// Componentes SGP
import { Cabecalho } from '~/componentes-sgp';

// Componentes
import { Card, ButtonGroup, Grid, PainelCollapse } from '~/componentes';
import Editor from '~/componentes/editor/editor';
import AlertaSelecionarTurma from './componentes/AlertaSelecionarTurma';
import DropDownTerritorios from './componentes/DropDownTerritorios';

// Estilos
import { Linha } from '~/componentes/EstilosGlobais';

function TerritorioSaber() {
  const [territorioSelecionado, setTerritorioSelecionado] = useState(undefined);
  return (
    <>
      <AlertaSelecionarTurma />
      <Cabecalho pagina="Planejamento do Território do saber" />
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
          desabilitarBotaoPrincipal
        />
        <Grid cols={12}>
          <Linha className="row mb-0">
            <Grid cols={4}>
              <DropDownTerritorios
                territorioSelecionado={territorioSelecionado}
                onChangeTerritorio={valor => setTerritorioSelecionado(valor)}
              />
            </Grid>
          </Linha>
        </Grid>
        <Grid className="p-0 m-0 mt-4" cols={12}>
          <PainelCollapse>
            <PainelCollapse.Painel temBorda header="Primeiro Bimestre">
              <Linha className="row ml-1 mr-1">
                <Grid cols={6}>
                  <Editor label="Desenvolvimento das atividades" />
                </Grid>
                <Grid cols={6}>
                  <Editor label="Reflexões sobre a participação dos estudantes, parcerias e avaliação" />
                </Grid>
              </Linha>
            </PainelCollapse.Painel>
          </PainelCollapse>
        </Grid>
        <Grid className="p-0 m-0 mt-4" cols={12}>
          <PainelCollapse>
            <PainelCollapse.Painel temBorda header="Segundo Bimestre">
              Teste
            </PainelCollapse.Painel>
          </PainelCollapse>
        </Grid>
        <Grid className="p-0 m-0 mt-4" cols={12}>
          <PainelCollapse>
            <PainelCollapse.Painel temBorda header="Terceiro Bimestre">
              Teste
            </PainelCollapse.Painel>
          </PainelCollapse>
        </Grid>
        <Grid className="p-0 m-0 mt-4" cols={12}>
          <PainelCollapse>
            <PainelCollapse.Painel temBorda header="Quarto Bimestre">
              Teste
            </PainelCollapse.Painel>
          </PainelCollapse>
        </Grid>
      </Card>
    </>
  );
}

TerritorioSaber.propTypes = {};

TerritorioSaber.defaultProps = {};

export default TerritorioSaber;
