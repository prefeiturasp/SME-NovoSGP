import React, { useEffect, useState, useCallback } from 'react';
import PropTypes from 'prop-types';
import { useSelector } from 'react-redux';
import styled from 'styled-components';
import Semana from './Semana';
import DiaCompleto from './DiaCompleto';
import { store } from '~/redux';
import { selecionaMes } from '~/redux/modulos/calendarioProfessor/actions';
import api from '~/servicos/api';
import Loader from '~/componentes/loader';

const Div = styled.div``;

const DiaDaSemana = props => {
  const { nomeDia } = props;

  return (
    <Div className="col">
      <Div className="text-muted text-center fonte-12">{nomeDia}</Div>
    </Div>
  );
};

DiaDaSemana.propTypes = {
  nomeDia: PropTypes.string,
};

DiaDaSemana.defaultProps = {
  nomeDia: '',
};

const MesCompleto = props => {
  const { meses, filtros } = props;
  const {
    tipoCalendarioSelecionado,
    eventoSme,
    dreSelecionada,
    unidadeEscolarSelecionada,
    turmaSelecionada,
    todasTurmas,
  } = filtros;

  const mesesLista = meses.split(',');
  const mesesCalendario = useSelector(state => state.calendarioProfessor.meses);

  const [mesSelecionado, setMesSelecionado] = useState(-1);
  const [ultimoUsado, setUltimoUsado] = useState(-1);

  const [diasDaSemana, setDiasDaSemana] = useState([]);
  const [estaAberto, setEstaAberto] = useState([]);

  const eventoAulaCalendarioEdicao = useSelector(
    state => state.calendarioProfessor.eventoAulaCalendarioEdicao
  );

  const usuario = useSelector(state => state.usuario);
  const { turmaSelecionada: turmaSelecionadaStore } = usuario;

  useEffect(() => {
    const abrirMesEventoCalendarioEdicao = setTimeout(() => {
      if (eventoAulaCalendarioEdicao && eventoAulaCalendarioEdicao.mes)
        store.dispatch(selecionaMes(eventoAulaCalendarioEdicao.mes));
    }, 1000);
    return () => clearTimeout(abrirMesEventoCalendarioEdicao);
  }, [eventoAulaCalendarioEdicao]);

  useEffect(() => {
    if (mesesCalendario) {
      if (tipoCalendarioSelecionado) {
        mesesLista.forEach(mes => {
          if (mesesCalendario[mes].estaAberto) {
            setMesSelecionado(parseInt(mes, 10));
          }
        });
      }
    }
  }, [mesesCalendario, mesesLista, tipoCalendarioSelecionado]);

  const [tipoEventosDiaLista, setTipoEventosDiaLista] = useState([]);
  const [carregandoTipos, setCarregandoTipos] = useState(false);

  const obterTipoEventosDia = useCallback(
    async mes => {
      if (mes) {
        if (
          tipoCalendarioSelecionado &&
          dreSelecionada &&
          unidadeEscolarSelecionada &&
          (turmaSelecionada || todasTurmas)
        ) {
          setCarregandoTipos(true);
          await api
            .post(`v1/calendarios/meses/tipos/eventos-aulas`, {
              Mes: mes,
              tipoCalendarioId: tipoCalendarioSelecionado,
              EhEventoSME: eventoSme,
              dreId: dreSelecionada,
              ueId: unidadeEscolarSelecionada,
              turmaId: turmaSelecionada,
              todasTurmas,
            })
            .then(resposta => {
              if (resposta.data) {
                setTipoEventosDiaLista(resposta.data);
              } else setTipoEventosDiaLista([]);
              setCarregandoTipos(false);
            })
            .catch(() => {
              setTipoEventosDiaLista([]);
              setCarregandoTipos(false);
            });
        } else setTipoEventosDiaLista([]);
      }
    },
    [filtros]
  );

  useEffect(() => {
    if (mesSelecionado > 0) {
      const data = new Date(
        turmaSelecionadaStore.anoLetivo,
        mesSelecionado - 1,
        1
      );
      data.setDate(data.getDate() - data.getDay() - 1);

      const diasDaSemanaLista = [];
      for (let numSemanas = 0; numSemanas < 6; numSemanas += 1) {
        diasDaSemanaLista[numSemanas] = [];
        for (let numDias = 0; numDias < 7; numDias += 1) {
          diasDaSemanaLista[numSemanas].push(
            new Date(data.setDate(data.getDate() + 1))
          );
        }
      }

      setDiasDaSemana(diasDaSemanaLista);
      setUltimoUsado(mesSelecionado);
      setEstaAberto(estadoAntigo => {
        return { ...estadoAntigo, [mesSelecionado]: true };
      });
      obterTipoEventosDia(mesSelecionado);
    }
    return () =>
      setEstaAberto(estadoAntigo => {
        return { ...estadoAntigo, [mesSelecionado]: false };
      });
  }, [
    mesSelecionado,
    obterTipoEventosDia,
    turmaSelecionada,
    turmaSelecionadaStore.anoLetivo,
  ]);

  return mesSelecionado > 0 && estaAberto[mesSelecionado] ? (
    <Div
      className={`${mesesCalendario[mesSelecionado].nome} d-none border border-top-0 border-bottom-0 h-100 w-100 fade`}
    >
      <Loader loading={carregandoTipos}>
        <Div className="w-100 d-flex pt-4 pb-3 border-bottom">
          <DiaDaSemana nomeDia="Domingo" />
          <DiaDaSemana nomeDia="Segunda" />
          <DiaDaSemana nomeDia="Terça" />
          <DiaDaSemana nomeDia="Quarta" />
          <DiaDaSemana nomeDia="Quinta" />
          <DiaDaSemana nomeDia="Sexta" />
          <DiaDaSemana nomeDia="Sábado" />
        </Div>
        <Semana
          inicial
          dias={diasDaSemana[0]}
          mesAtual={ultimoUsado}
          filtros={filtros}
          tipoEventosDiaLista={tipoEventosDiaLista}
        />
        <DiaCompleto
          dias={diasDaSemana[0]}
          mesAtual={ultimoUsado}
          filtros={filtros}
        />
        <Semana
          dias={diasDaSemana[1]}
          mesAtual={ultimoUsado}
          filtros={filtros}
          tipoEventosDiaLista={tipoEventosDiaLista}
        />
        <DiaCompleto
          dias={diasDaSemana[1]}
          mesAtual={ultimoUsado}
          filtros={filtros}
        />
        <Semana
          dias={diasDaSemana[2]}
          mesAtual={ultimoUsado}
          filtros={filtros}
          tipoEventosDiaLista={tipoEventosDiaLista}
        />
        <DiaCompleto
          dias={diasDaSemana[2]}
          mesAtual={ultimoUsado}
          filtros={filtros}
        />
        <Semana
          dias={diasDaSemana[3]}
          mesAtual={ultimoUsado}
          filtros={filtros}
          tipoEventosDiaLista={tipoEventosDiaLista}
        />
        <DiaCompleto
          dias={diasDaSemana[3]}
          mesAtual={ultimoUsado}
          filtros={filtros}
        />
        <Semana
          dias={diasDaSemana[4]}
          mesAtual={ultimoUsado}
          filtros={filtros}
          tipoEventosDiaLista={tipoEventosDiaLista}
        />
        <DiaCompleto
          dias={diasDaSemana[4]}
          mesAtual={ultimoUsado}
          filtros={filtros}
        />
        <Semana
          dias={diasDaSemana[5]}
          mesAtual={ultimoUsado}
          filtros={filtros}
          tipoEventosDiaLista={tipoEventosDiaLista}
        />
        <DiaCompleto
          dias={diasDaSemana[5]}
          mesAtual={ultimoUsado}
          filtros={filtros}
        />
      </Loader>
    </Div>
  ) : (
    <Div />
  );
};

MesCompleto.propTypes = {
  meses: PropTypes.string,
  filtros: PropTypes.oneOfType([PropTypes.array, PropTypes.object]),
};

MesCompleto.defaultProps = {
  meses: '',
  filtros: {},
};

export default MesCompleto;
