import React, { useState, useEffect, useCallback } from 'react';
import PropTypes from 'prop-types';
import styled from 'styled-components';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import { Base } from '~/componentes/colors';
import { store } from '~/redux';
import {
  selecionaDia,
  salvarEventoCalendarioEdicao,
} from '~/redux/modulos/calendarioEscolar/actions';

const Div = styled.div``;
const TipoEventosLista = styled(Div)`
  bottom: 5px;
  right: 10px;
`;
const TipoEvento = styled(Div)`
  background-color: ${props => (props.cor ? props.cor : Base.Roxo)};
  color: ${Base.Branco};
  font-size: 10px;
  margin-bottom: 2px;
  width: 60px;
  &:last-child {
    margin-bottom: 0;
  }
`;

const Dia = props => {
  const {
    dia,
    mesAtual,
    diaSelecionado,
    tipoEventosDiaLista: tipoLista,
  } = props;
  const [tipoEventosDiaLista, setTipoEventosDiaLista] = useState([]);

  useEffect(() => {
    if (dia && mesAtual && tipoLista.length) {
      const lista = tipoLista.filter(evento => evento.dia === dia.getDate())[0];
      if (lista) setTipoEventosDiaLista(lista);
      return;
    }

    setTipoEventosDiaLista([]);
  }, [dia, mesAtual, tipoLista]);

  const selecionaDiaAberto = useCallback(() => {
    store.dispatch(selecionaDia(dia));
  }, [dia]);

  const eventoCalendarioEdicao = useSelector(
    state => state.calendarioEscolar.eventoCalendarioEdicao
  );

  useEffect(() => {
    const abrirDiaEventoCalendarioEdicao = setTimeout(() => {
      if (
        eventoCalendarioEdicao &&
        eventoCalendarioEdicao.dia &&
        dia &&
        dia.getTime() === eventoCalendarioEdicao.dia.getTime()
      ) {
        selecionaDiaAberto();
        store.dispatch(salvarEventoCalendarioEdicao());
      }
    }, 3000);
    return () => clearTimeout(abrirDiaEventoCalendarioEdicao);
  }, [dia, eventoCalendarioEdicao, selecionaDiaAberto]);

  const style = {
    cursor: 'pointer',
    height: 62,
  };

  if (dia.getDay() === 0) style.backgroundColor = Base.RosaCalendario;
  else if (dia.getDay() === 6) style.backgroundColor = Base.CinzaCalendario;

  const className = `col border border-left-0 border-top-0 position-relative ${dia.getDay() ===
    6 && 'border-right-0'} ${diaSelecionado && 'bg-light'} ${diaSelecionado &&
    dia === diaSelecionado &&
    'border-bottom-0 bg-white'}`;

  let diaFormatado = dia.getDate();
  if (diaFormatado < 10) diaFormatado = `0${diaFormatado}`;

  const diaStyle = {
    color:
      mesAtual === dia.getMonth() + 1 ? Base.Preto : Base.CinzaDesabilitado,
    cursor: 'pointer',
    fontSize: '16px',
    top: 30,
  };

  return (
    <Div className={className} style={style} onClick={selecionaDiaAberto}>
      <Div className="w-100 h-100 d-flex">
        <Div className="position-relative" style={diaStyle}>
          {diaFormatado}
        </Div>
        {mesAtual === dia.getMonth() + 1 &&
          tipoEventosDiaLista &&
          tipoEventosDiaLista.tiposEvento && (
            <TipoEventosLista className="position-absolute">
              {tipoEventosDiaLista?.tiposEvento &&
              tipoEventosDiaLista.tiposEvento?.length ? (
                <TipoEvento
                  key={shortid.generate()}
                  className="d-block badge badge-pill ml-auto mr-0"
                  cor={Base.RoxoEventoCalendario}
                >
                  Evento
                </TipoEvento>
              ) : null}
            </TipoEventosLista>
          )}
      </Div>
    </Div>
  );
};

Dia.propTypes = {
  dia: PropTypes.oneOfType([PropTypes.array, PropTypes.object]),
  mesAtual: PropTypes.number,
  diaSelecionado: PropTypes.oneOfType([
    PropTypes.instanceOf(Date),
    PropTypes.string,
  ]),
  tipoEventosDiaLista: PropTypes.oneOfType([PropTypes.array]),
};

Dia.defaultProps = {
  dia: {},
  mesAtual: 0,
  diaSelecionado: '',
  tipoEventosDiaLista: [],
};

const Semana = props => {
  const diaSelecionado = useSelector(
    state => state.calendarioEscolar.diaSelecionado
  );
  const { inicial, dias, mesAtual, filtros, tipoEventosDiaLista } = props;

  return (
    <Div>
      <Div className="w-100 d-flex">
        <Dia
          className={`${inicial ? '' : 'border-top-0'}`}
          dia={dias[0]}
          mesAtual={mesAtual}
          filtros={filtros}
          diaSelecionado={diaSelecionado}
          tipoEventosDiaLista={tipoEventosDiaLista}
        />
        <Dia
          className={`${inicial ? '' : 'border-top-0'}`}
          dia={dias[1]}
          mesAtual={mesAtual}
          filtros={filtros}
          diaSelecionado={diaSelecionado}
          tipoEventosDiaLista={tipoEventosDiaLista}
        />
        <Dia
          className={`${inicial ? '' : 'border-top-0'}`}
          dia={dias[2]}
          mesAtual={mesAtual}
          filtros={filtros}
          diaSelecionado={diaSelecionado}
          tipoEventosDiaLista={tipoEventosDiaLista}
        />
        <Dia
          className={`${inicial ? '' : 'border-top-0'}`}
          dia={dias[3]}
          mesAtual={mesAtual}
          filtros={filtros}
          diaSelecionado={diaSelecionado}
          tipoEventosDiaLista={tipoEventosDiaLista}
        />
        <Dia
          className={`${inicial ? '' : 'border-top-0'}`}
          dia={dias[4]}
          mesAtual={mesAtual}
          filtros={filtros}
          diaSelecionado={diaSelecionado}
          tipoEventosDiaLista={tipoEventosDiaLista}
        />
        <Dia
          className={`${inicial ? '' : 'border-top-0'}`}
          dia={dias[5]}
          mesAtual={mesAtual}
          filtros={filtros}
          diaSelecionado={diaSelecionado}
          tipoEventosDiaLista={tipoEventosDiaLista}
        />
        <Dia
          className={`${
            inicial ? 'border-right-0' : 'border-top-0 border-right-0'
          }`}
          dia={dias[6]}
          mesAtual={mesAtual}
          filtros={filtros}
          diaSelecionado={diaSelecionado}
          tipoEventosDiaLista={tipoEventosDiaLista}
        />
      </Div>
    </Div>
  );
};

Semana.propTypes = {
  inicial: PropTypes.bool,
  dias: PropTypes.oneOfType([PropTypes.array, PropTypes.object]),
  mesAtual: PropTypes.number,
  filtros: PropTypes.oneOfType([PropTypes.array, PropTypes.object]),
  tipoEventosDiaLista: PropTypes.oneOfType([PropTypes.array]),
};

Semana.defaultProps = {
  inicial: false,
  dias: [],
  mesAtual: 0,
  filtros: {},
  tipoEventosDiaLista: [],
};

export default Semana;
