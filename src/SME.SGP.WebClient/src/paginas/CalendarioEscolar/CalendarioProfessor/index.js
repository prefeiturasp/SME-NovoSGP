import React, { useEffect, useState, useCallback, useReducer } from 'react';

// Redux
import { useSelector, useDispatch } from 'react-redux';
import shortid from 'shortid';

// Componentes
import { Loader, Card, Grid, Button, Colors } from '~/componentes';

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
import { erro, sucesso } from '~/servicos/alertas';
import history from '~/servicos/history';
import ServicoCalendarios from '~/servicos/Paginas/Calendario/ServicoCalendarios';

// Reducer
import Reducer, { estadoInicial } from './reducer';
import {
  setarEventosMes,
  setarEventosDia,
  setarCarregandoCalendario,
  setarCarregandoMes,
  setarCarregandoDia,
} from './reducer/actions';

// DTOs
import RotasDTO from '~/dtos/rotasDto';
import { selecionaDia } from '~/redux/modulos/calendarioProfessor/actions';

function CalendarioProfessor() {
  const dispatch = useDispatch();
  const { turmaSelecionada, permissoes } = useSelector(
    estado => estado.usuario
  );

  const permissaoTela = permissoes[RotasDTO.CALENDARIO_PROFESSOR];

  const usuarioStore = useSelector(state => state.usuario);
  const [estado, disparar] = useReducer(Reducer, estadoInicial);
  const [tipoCalendarioId, setTipoCalendarioId] = useState(undefined);
  const [podeImprimir, setPodeImprimir] = useState(false);
  const [imprimindo, setImprimindo] = useState(false);

  const onClickMesHandler = useCallback(
    mes => {
      async function buscarEventosMes() {
        try {
          disparar(setarCarregandoMes(true));
          const {
            data,
            status,
          } = await CalendarioProfessorServico.buscarEventosAulasMes({
            numeroMes: mes.numeroMes,
            dre: turmaSelecionada.dre,
            anoLetivo: turmaSelecionada.anoLetivo,
            tipoCalendarioId,
            turma: turmaSelecionada.turma,
            ue: turmaSelecionada.unidadeEscolar,
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
      tipoCalendarioId,
      turmaSelecionada.anoLetivo,
      turmaSelecionada.dre,
      turmaSelecionada.turma,
      turmaSelecionada.unidadeEscolar,
    ]
  );

  const onClickDiaHandler = useCallback(
    dia => {
      dispatch(selecionaDia(dia));
      async function buscarEventosDias() {
        try {
          disparar(setarCarregandoDia(true));
          const {
            data,
            status,
          } = await CalendarioProfessorServico.buscarEventosAulasDia({
            dia: dia.getDate(),
            numeroMes: dia.getMonth() + 1,
            tipoCalendarioId,
            dre: turmaSelecionada.dre,
            ue: turmaSelecionada.unidadeEscolar,
            turma: turmaSelecionada.turma,
            anoLetivo: turmaSelecionada.anoLetivo,
          });

          if (data && status === 200) {
            disparar(setarCarregandoDia(false));
            const mes = {
              estaAberto: false,
              eventos: 0,
              nome: '',
              numeroMes: dia.getMonth() + 1,
            };
            disparar(
              setarEventosMes({
                ...mes,
                dias: data.eventosAulasMes,
              })
            );

            disparar(
              setarEventosDia({
                diaSelecionado: dia,
                dados: data,
              })
            );
          } else if (status === 204) {
            disparar(setarCarregandoDia(false));
          }
        } catch (error) {
          disparar(setarCarregandoDia(false));
          erro('Não foi possível buscar dados do dia.');
        }
      }
      buscarEventosDias();
    },
    [
      tipoCalendarioId,
      turmaSelecionada.anoLetivo,
      turmaSelecionada.dre,
      turmaSelecionada.turma,
      turmaSelecionada.unidadeEscolar,
    ]
  );

  const onChangeTipoCalendarioIdHandler = useCallback(valor => {
    setTipoCalendarioId(valor);
  }, []);

  const gerarRelatorio = async () => {
    setImprimindo(true);
    const payload = {
      DreCodigo: turmaSelecionada.dre,
      UeCodigo: turmaSelecionada.unidadeEscolar,
      TipoCalendarioId: tipoCalendarioId,
      EhSME: usuarioStore.possuiPerfilSmeOuDre,
    };
    await ServicoCalendarios.gerarRelatorio(payload)
      .then(() => {
        sucesso(
          'Solicitação de geração do relatório gerada com sucesso. Em breve você receberá uma notificação com o resultado.'
        );
      })
      .finally(setImprimindo(false))
      .catch(e => erro(e));
  };

  useEffect(() => {
    if (Object.keys(turmaSelecionada).length === 0) {
      setTipoCalendarioId(undefined);
    }
  }, [turmaSelecionada]);

  useEffect(() => {
    setPodeImprimir(tipoCalendarioId);
  }, [tipoCalendarioId]);

  return (
    <>
      <AlertaSelecionarTurma />
      <Cabecalho pagina="Calendário do professor" />
      <Loader loading={false}>
        <Card>
          <div className="col-md-12 d-flex justify-content-between pb-5">
            <Loader loading={imprimindo}>
              <Button
                className="btn-imprimir"
                icon="print"
                color={Colors.Azul}
                border
                onClick={() => gerarRelatorio()}
                disabled={!podeImprimir}
                id="btn-imprimir-relatorio-calendario"
              />
            </Loader>
            <Button
              id={shortid.generate()}
              label="Voltar"
              icon="arrow-left"
              color={Colors.Azul}
              border
              className="btnGroupItem"
              onClick={() => history.push('/')}
            />
          </div>
          <Grid cols={4} className="p-0 m-0">
            <DropDownTipoCalendario
              turmaSelecionada={turmaSelecionada.turma}
              onChange={valor => onChangeTipoCalendarioIdHandler(valor)}
              valor={tipoCalendarioId}
            />
          </Grid>
          <Grid cols={12}>
            <Linha className="row pt-2">
              <Grid cols={12}>
                <Calendario
                  eventos={estado.eventos}
                  onClickMes={mes => onClickMesHandler(mes)}
                  onClickDia={dia => onClickDiaHandler(dia)}
                  carregandoCalendario={estado.carregandoCalendario}
                  carregandoMes={estado.carregandoMes}
                  carregandoDia={estado.carregandoDia}
                  permissaoTela={permissaoTela}
                  tipoCalendarioId={tipoCalendarioId}
                  turmaSelecionada={turmaSelecionada}
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
