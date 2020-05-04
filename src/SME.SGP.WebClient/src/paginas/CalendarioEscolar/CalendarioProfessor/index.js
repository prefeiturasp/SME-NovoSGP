import React, { useEffect, useState, useCallback, useReducer } from 'react';

// Redux
import { useSelector } from 'react-redux';

// Componentes
import { Loader, ButtonGroup, Card, Grid } from '~/componentes';

// Componentes Internos
import DropDownTipoCalendario from './componentes/DropDownTipoCalendario';

// Estilos
import { Linha } from '~/componentes/EstilosGlobais';

// Componentes SGP
import {
  Cabecalho,
  AlertaSelecionarTurma,
  Calendario,
} from '~/componentes-sgp';

// Serviços
import CalendarioProfessorServico from '~/servicos/Paginas/CalendarioProfessor';
import { erro } from '~/servicos/alertas';

// Reducer
import Reducer, { estadoInicial } from './reducer';
import {
  setarEventosMes,
  setarCarregandoCalendario,
  setarCarregandoMes,
  setarCarregandoDia,
} from './reducer/actions';

function CalendarioProfessor() {
  const { turmaSelecionada } = useSelector(estado => estado.usuario);

  const [estado, disparar] = useReducer(Reducer, estadoInicial);

  const onClickMesHandler = useCallback(
    mes => {
      async function buscarEventosMes() {
        try {
          disparar(setarCarregandoMes(true));
          const {
            data,
            status,
          } = await CalendarioProfessorServico.buscarEventosAulasMes({
            EhEventoSME: true,
            Mes: mes.numeroMes,
            dreId: turmaSelecionada.dre,
            tipoCalendarioId: '1',
            todasTurmas: false,
            turmaId: turmaSelecionada.turma,
            ueId: turmaSelecionada.unidadeEscolar,
          });

          if (data && status === 200) {
            disparar(setarCarregandoMes(false));
            disparar(
              setarEventosMes({
                ...mes,
                dias: data,
              })
            );
          } else if (status === 204) {
            disparar(setarCarregandoMes(false));
          }
        } catch (error) {
          disparar(setarCarregandoMes(false));
          erro('Não foi possível buscar dados do mês.');
        }
      }
      buscarEventosMes();
    },
    [
      turmaSelecionada.dre,
      turmaSelecionada.turma,
      turmaSelecionada.unidadeEscolar,
    ]
  );

  return (
    <>
      <AlertaSelecionarTurma />
      <Cabecalho pagina="Calendário do professor" />
      <Loader loading={false}>
        <Card>
          <ButtonGroup />
          <Grid cols={4} className="p-0 m-0">
            <DropDownTipoCalendario onChange={() => console.log('teste')} />
          </Grid>
          <Grid cols={12}>
            <Linha className="row pt-2">
              <Grid cols={12}>
                <Calendario
                  eventos={estado.eventos}
                  onClickMes={mes => onClickMesHandler(mes)}
                  onClickDia={dia => console.log(dia)}
                  carregandoCalendario={estado.carregandoCalendario}
                  carregandoMes={estado.carregandoMes}
                  carregandoDia={estado.carregandoDia}
                />
              </Grid>
            </Linha>
          </Grid>
        </Card>
      </Loader>
    </>
  );
}

CalendarioProfessor.propTypes = {};

CalendarioProfessor.defaultProps = {};

export default CalendarioProfessor;
