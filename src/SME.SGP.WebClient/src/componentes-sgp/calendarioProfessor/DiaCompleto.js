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
import { store } from '~/redux';
import {
  selecionaDia,
  salvarEventoAulaCalendarioEdicao,
} from '~/redux/modulos/calendarioProfessor/actions';
import TiposEventoAulaDTO from '~/dtos/tiposEventoAula';
import RotasDTO from '~/dtos/rotasDto';

const Div = styled.div``;
const Evento = styled(Div)`
  &:hover {
    background: ${Base.Roxo};
    color: ${Base.Branco};
  }
`;
const Botao = styled(Button)`
  ${Evento}:hover & {
    background: transparent !important;
    border-color: ${Base.Branco} !important;
    color: ${Base.Branco} !important;
  }
`;
const BotoesAuxiliaresEstilo = styled(Div)`
  align-items: right;
  display: flex;
  justify-content: flex-end;
  padding: 16px;
  padding-bottom: 0;
  width: 100%;
`;

const SemEvento = () => {
  return (
    <div
      className="d-flex w-100 h-100 justify-content-center d-flex align-items-center fade show"
      style={{ fontSize: 25, color: Base.CinzaBotao }}
    >
      Sem eventos neste dia
    </div>
  );
};

const DiaCompleto = props => {
  const { dias, mesAtual, filtros } = props;
  const {
    tipoCalendarioSelecionado,
    eventoSme,
    dreSelecionada,
    unidadeEscolarSelecionada,
    turmaSelecionada,
    todasTurmas,
  } = filtros;
  const [eventosDia, setEventosDia] = useState([]);

  const permissaoTela = useSelector(
    state => state.usuario.permissoes[RotasDTO.CALENDARIO_PROFESSOR]
  );
  const diaSelecionado = useSelector(
    state => state.calendarioProfessor.diaSelecionado
  );

  let estaAberto = false;

  for (let i = 0; i < dias.length; i += 1)
    if (dias[i] === diaSelecionado) estaAberto = true;

  useEffect(() => {
    let estado = true;
    if (estado) {
      if (diaSelecionado && estaAberto) {
        setEventosDia([]);
        if (
          tipoCalendarioSelecionado &&
          dreSelecionada &&
          unidadeEscolarSelecionada &&
          (turmaSelecionada || todasTurmas)
        ) {
          api
            .post('v1/calendarios/meses/dias/eventos-aulas', {
              data: diaSelecionado,
              tipoCalendarioId: tipoCalendarioSelecionado,
              EhEventoSME: eventoSme,
              dreId: dreSelecionada,
              ueId: unidadeEscolarSelecionada,
              turmaId: turmaSelecionada,
              todasTurmas,
            })
            .then(resposta => {
              if (resposta.data) setEventosDia(resposta.data);
              else setEventosDia([]);
            })
            .catch(() => {
              setEventosDia([]);
            });
        } else setEventosDia([]);
      } else setEventosDia([]);
    }
    return () => {
      estado = false;
    };
  }, [diaSelecionado]);

  const aoClicarBotaoNovaAula = () => {
    store.dispatch(
      salvarEventoAulaCalendarioEdicao(
        tipoCalendarioSelecionado,
        eventoSme,
        dreSelecionada,
        unidadeEscolarSelecionada,
        turmaSelecionada,
        mesAtual,
        diaSelecionado
      )
    );

    history.push(
      `calendario-professor/cadastro-aula/novo/${tipoCalendarioSelecionado}`
    );
  };

  const BotoesAuxiliares = () => {
    return (
      <BotoesAuxiliaresEstilo>
        <Button
          key={shortid.generate()}
          onClick={aoClicarBotaoNovaAula}
          label="Nova Aula"
          color={Colors.Roxo}
          disabled={permissaoTela && !permissaoTela.podeIncluir}
        />
      </BotoesAuxiliaresEstilo>
    );
  };

  useEffect(() => {
    estaAberto = false;
    store.dispatch(selecionaDia(undefined));
  }, [filtros]);

  const aoClicarEvento = (id, tipo) => {
    store.dispatch(
      salvarEventoAulaCalendarioEdicao(
        tipoCalendarioSelecionado,
        eventoSme,
        dreSelecionada,
        unidadeEscolarSelecionada,
        turmaSelecionada,
        mesAtual,
        diaSelecionado
      )
    );

    if (TiposEventoAulaDTO.Evento.indexOf(tipo) > -1)
      history.push(`calendario-escolar/eventos/editar/${id}`);
    else
      history.push(
        `/calendario-escolar/calendario-professor/cadastro-aula/editar/${id}`
      );
  };

  return (
    estaAberto && (
      <Div className="border-bottom border-top-0 h-100 p-3">
        {eventosDia &&
        eventosDia.eventosAulas &&
        eventosDia.eventosAulas.length > 0 ? (
          <Div className="list-group list-group-flush fade show">
            {eventosDia.map(evento => {
              return (
                <Evento
                  key={shortid.generate()}
                  className="list-group-item list-group-item-action d-flex rounded"
                  onClick={() => aoClicarEvento(evento.id, evento.tipoEvento)}
                  style={{ cursor: 'pointer' }}
                >
                  <Grid cols={1} className="pl-0">
                    <Botao
                      label={evento.tipoEvento}
                      color={
                        (evento.tipoEvento === TiposEventoAulaDTO.Aula &&
                          Colors.Roxo) ||
                        (evento.tipoEvento === TiposEventoAulaDTO.CJ &&
                          Colors.Laranja) ||
                        (TiposEventoAulaDTO.Evento.indexOf(evento.tipoEvento) >
                          -1 &&
                          Colors.CinzaBotao)
                      }
                      className="w-100"
                      border
                      steady
                    />
                  </Grid>
                  {TiposEventoAulaDTO.Evento.indexOf(evento.tipoEvento) ===
                    -1 &&
                    evento.dadosAula && (
                      <Grid cols={1} className="px-0">
                        <Botao
                          label={window
                            .moment(evento.dadosAula.horario, 'HH')
                            .format('HH:mm')}
                          color={Colors.CinzaBotao}
                          className="w-100 px-2"
                          border
                          steady
                        />
                      </Grid>
                    )}
                  <Grid
                    cols={
                      TiposEventoAulaDTO.Evento.indexOf(evento.tipoEvento) > -1
                        ? 11
                        : 10
                    }
                    className="align-self-center font-weight-bold pl-0"
                  >
                    <Div
                      className={`${TiposEventoAulaDTO.Evento.indexOf(
                        evento.tipoEvento
                      ) === -1 && 'pl-3'}`}
                    >
                      {TiposEventoAulaDTO.Evento.indexOf(evento.tipoEvento) >
                        -1 && (evento.descricao ? evento.descricao : 'Evento')}
                      {TiposEventoAulaDTO.Evento.indexOf(evento.tipoEvento) ===
                        -1 &&
                        `${evento.dadosAula.turma} - ${evento.dadosAula.modalidade} - ${evento.dadosAula.tipo} - ${evento.dadosAula.unidadeEscolar} - ${evento.dadosAula.disciplina}`}
                    </Div>
                  </Grid>
                </Evento>
              );
            })}
          </Div>
        ) : (
          <SemEvento />
        )}
        {eventosDia && eventosDia.letivo && <BotoesAuxiliares />}
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
