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
    <Div
      className="d-flex w-100 h-100 justify-content-center d-flex align-items-center fade show"
      style={{ fontSize: 25, color: Base.CinzaBotao }}
    >
      Sem eventos neste dia
    </Div>
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

  const salvarDadosEventoAula = () => {
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
  };

  const aoClicarBotaoNovaAvaliacao = () => {
    salvarDadosEventoAula();
    history.push(`${RotasDTO.CADASTRO_DE_AVALIACAO}/novo`);
  };

  const aoClicarBotaoNovaAula = () => {
    salvarDadosEventoAula();
    history.push(
      `${RotasDTO.CADASTRO_DE_AULA}/novo/${tipoCalendarioSelecionado}`
    );
  };

  const BotoesAuxiliares = () => {
    return (
      <BotoesAuxiliaresEstilo>
        <Button
          key={shortid.generate()}
          onClick={aoClicarBotaoNovaAvaliacao}
          label="Nova Avaliação"
          color={Colors.Roxo}
          disabled={permissaoTela && !permissaoTela.podeIncluir}
          className="mr-3"
        />
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

    if (tipo === TiposEventoAulaDTO.Aula || tipo === TiposEventoAulaDTO.CJ) {
      history.push(`${RotasDTO.CADASTRO_DE_AULA}/editar/${id}`);
    } else {
      history.push(`/calendario-escolar/eventos/editar/${id}`);
    }
  };

  return (
    estaAberto && (
      <Div className="border-bottom border-top-0 h-100 p-3">
        {eventosDia &&
          eventosDia.eventosAulas &&
          eventosDia.eventosAulas.length > 0 ? (
            <Div className="list-group list-group-flush fade show">
              {eventosDia.eventosAulas.map(evento => {
                return (
                  <Evento
                    key={shortid.generate()}
                    className="list-group-item list-group-item-action d-flex rounded"
                    onClick={() => aoClicarEvento(evento.id, evento.tipoEvento)}
                    style={{ cursor: 'pointer' }}
                  >
                    <Grid
                      cols={
                        (evento.tipoEvento === TiposEventoAulaDTO.Aula && 1) ||
                        (evento.tipoEvento === TiposEventoAulaDTO.CJ && 1) ||
                        2
                      }
                      className="pl-0"
                    >
                      <Botao
                        label={evento.tipoEvento}
                        color={
                          (evento.tipoEvento === TiposEventoAulaDTO.Aula &&
                            Colors.Roxo) ||
                          (evento.tipoEvento === TiposEventoAulaDTO.CJ &&
                            Colors.Laranja) ||
                          Colors.CinzaBotao
                        }
                        className="w-100"
                        height={
                          evento.tipoEvento === TiposEventoAulaDTO.Aula ||
                            evento.tipoEvento === TiposEventoAulaDTO.CJ
                            ? '38px'
                            : 'auto'
                        }
                        border
                        steady
                      />
                    </Grid>
                    {(evento.tipoEvento === TiposEventoAulaDTO.Aula ||
                      evento.tipoEvento === TiposEventoAulaDTO.CJ) &&
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
                        evento.tipoEvento === TiposEventoAulaDTO.Aula ||
                          evento.tipoEvento === TiposEventoAulaDTO.CJ
                          ? 10
                          : 11
                      }
                      className="align-self-center font-weight-bold pl-0"
                    >
                      {evento.tipoEvento !== TiposEventoAulaDTO.Aula &&
                        evento.tipoEvento !== TiposEventoAulaDTO.CJ &&
                        (evento.descricao ? evento.descricao : 'Evento')}
                      {(evento.tipoEvento === TiposEventoAulaDTO.Aula ||
                        evento.tipoEvento === TiposEventoAulaDTO.CJ) &&
                        evento.dadosAula &&
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
        {eventosDia && eventosDia.letivo && turmaSelecionada && (
          <BotoesAuxiliares />
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
