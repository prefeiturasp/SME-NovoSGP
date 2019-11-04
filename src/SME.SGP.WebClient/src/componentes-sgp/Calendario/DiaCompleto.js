import React, { useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import styled from 'styled-components';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import { Base, Colors } from '~/componentes/colors';
import api from '~/servicos/api';
import history from '~/servicos/history';
import Grid from '~/componentes/grid';
import Button from '~/componentes/button';

const Div = styled.div``;
const Evento = styled(Div)`
  &:hover {
    background: ${Base.Roxo};
    color: ${Base.Branco};
  }
`;

const SemEvento = () => {
  return (
    <div
      className="d-flex w-100 h-100 justify-content-center d-flex align-items-center"
      style={{ fontSize: 25, color: Base.CinzaBotao }}
    >
      Sem eventos neste dia
    </div>
  );
};

const DiaCompleto = props => {
  const { dias, mesAtual, filtros } = props;
  const [eventosDia, setEventosDia] = useState([]);

  const diaSelecionado = useSelector(
    state => state.calendarioEscolar.diaSelecionado
  );

  let estaAberto = false;

  for (let i = 0; i < dias.length; i += 1)
    if (dias[i] === diaSelecionado) estaAberto = true;

  useEffect(() => {
    if (diaSelecionado) {
      if (filtros && Object.entries(filtros).length > 0) {
        const {
          tipoCalendarioSelecionado = '',
          eventoSme = true,
          dreSelecionada = '',
          unidadeEscolarSelecionada = '',
        } = filtros;
        api
          .get(
            `v1/calendarios/eventos/meses/${mesAtual}/dias/${diaSelecionado.getDate()}?${dreSelecionada &&
              `DreId=${dreSelecionada}&`}${eventoSme &&
              `EhEventoSme=${eventoSme}&`}${tipoCalendarioSelecionado &&
              `IdTipoCalendario=${tipoCalendarioSelecionado}&`}${unidadeEscolarSelecionada &&
              `UeId=${unidadeEscolarSelecionada}`}`
          )
          .then(resposta => {
            if (resposta.data) setEventosDia(resposta.data);
          })
          .catch(() => {
            setEventosDia([]);
          });
      }
    }
  }, [estaAberto]);

  const aoClicarEvento = id => {
    history.push(`eventos/editar/${id}`);
  };

  return (
    estaAberto && (
      <Div className="border-top border-bottom-0 h-100 p-3">
        {eventosDia && eventosDia.length > 0 ? (
          <Div className="list-group list-group-flush">
            {eventosDia.map(evento => {
              return (
                <Evento
                  key={shortid.generate()}
                  className="list-group-item list-group-item-action d-flex rounded"
                  onClick={() => aoClicarEvento(evento.id)}
                  style={{ cursor: 'pointer' }}
                >
                  <Grid cols={1}>
                    <Button
                      label={evento.tipoEvento}
                      color={Colors.CinzaBotao}
                      border
                      steady
                    />
                  </Grid>
                  <Grid
                    cols={11}
                    className="align-self-center font-weight-bold"
                  >
                    {evento.descricao}
                  </Grid>
                </Evento>
              );
            })}
          </Div>
        ) : (
          <SemEvento />
        )}
      </Div>
    )
  );
};

DiaCompleto.propTypes = {
  dias: PropTypes.oneOfType([PropTypes.array, PropTypes.object]),
  mesAtual: PropTypes.number,
  filtros: PropTypes.oneOfType([PropTypes.array, PropTypes.object]),
};

DiaCompleto.defaultProps = {
  dias: [],
  mesAtual: 0,
  filtros: {},
};

export default DiaCompleto;
