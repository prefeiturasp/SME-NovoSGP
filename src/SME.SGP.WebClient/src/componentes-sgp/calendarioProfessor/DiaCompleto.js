import React, { useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import { Base, Colors } from '~/componentes/colors';
import api from '~/servicos/api';
import history from '~/servicos/history';
import Grid from '~/componentes/grid';
import { Div, Evento, Botao, BotoesAuxiliaresEstilo } from './DiaCompleto.css';
import { store } from '~/redux';
import {
  selecionaDia,
  salvarEventoAulaCalendarioEdicao,
  salvarDadosAulaFrequencia,
} from '~/redux/modulos/calendarioProfessor/actions';
import TiposEventoAulaDTO from '~/dtos/tiposEventoAula';
import RotasDTO from '~/dtos/rotasDto';
import Loader from '~/componentes/loader';
import { SelectComponent } from '~/componentes';
import Alert from '~/componentes/alert';

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

const ForaPerido = () => {
  return (
    <Alert
      alerta={{
        tipo: 'warning',
        id: 'alerta-perido-fechamento',
        mensagem:
          'Apenas é possível consultar este registro pois o período não está em aberto.',
        estiloTitulo: { fontSize: '18px' },
      }}
      className="mb-2"
    />
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

  const usuario = useSelector(state => state.usuario);
  const { turmaSelecionada: turmaSelecionadaStore } = usuario;

  const diaSelecionado = useSelector(
    state => state.calendarioProfessor.diaSelecionado
  );

  let estaAberto = false;

  for (let i = 0; i < dias.length; i += 1)
    if (dias[i] === diaSelecionado) estaAberto = true;

  const [carregandoDia, setCarregandoDia] = useState(false);
  const [dentroPeriodo, setDentroPeriodo] = useState(true);

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
          setCarregandoDia(true);
          api
            .post('v1/calendarios/meses/dias/eventos-aulas', {
              data: diaSelecionado,
              tipoCalendarioId: tipoCalendarioSelecionado,
              EhEventoSME: eventoSme,
              dreId: dreSelecionada,
              ueId: unidadeEscolarSelecionada,
              turmaId: turmaSelecionada,
              todasTurmas,
              turmaHistorico: turmaSelecionadaStore.consideraHistorico,
            })
            .then(resposta => {
              if (resposta.data) {
                setEventosDia(resposta.data);
                setDentroPeriodo(resposta.data.dentroPeriodo);
              } else setEventosDia([]);
              setCarregandoDia(false);
            })
            .catch(() => {
              setEventosDia([]);
              setCarregandoDia(false);
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

  const BotoesAuxiliares = ({ temAula, podeCadastrarAvaliacao }) => {
    return (
      <BotoesAuxiliaresEstilo>
        {temAula && podeCadastrarAvaliacao ? (
          <Botao
            id={shortid.generate()}
            key={shortid.generate()}
            onClick={aoClicarBotaoNovaAvaliacao}
            label="Nova Avaliação"
            color={Colors.Roxo}
            disabled={
              (permissaoTela && !permissaoTela.podeIncluir) || !dentroPeriodo
            }
            className="mr-3"
          />
        ) : null}
        <Botao
          id={shortid.generate()}
          key={shortid.generate()}
          onClick={aoClicarBotaoNovaAula}
          label="Nova Aula"
          color={Colors.Roxo}
          disabled={
            (permissaoTela && !permissaoTela.podeIncluir) || !dentroPeriodo
          }
        />
      </BotoesAuxiliaresEstilo>
    );
  };

  BotoesAuxiliares.propTypes = {
    temAula: PropTypes.number.isRequired,
    podeCadastrarAvaliacao: PropTypes.oneOfType([
      PropTypes.bool,
      PropTypes.number,
    ]).isRequired,
  };

  useEffect(() => {
    estaAberto = false;
    store.dispatch(selecionaDia(undefined));
  }, [filtros]);

  const aoClicarEvento = (id, tipo) => {
    if (permissaoTela && permissaoTela.podeConsultar) {
      salvarDadosEventoAula();
      if (tipo === TiposEventoAulaDTO.Aula || tipo === TiposEventoAulaDTO.CJ) {
        history.push(`${RotasDTO.CADASTRO_DE_AULA}/editar/${id}`);
      } else {
        history.push(`${RotasDTO.EVENTOS}/editar/${id}`);
      }
    }
  };

  const aoClicarEditarAvaliacao = id => {
    if (permissaoTela && permissaoTela.podeConsultar) {
      salvarDadosEventoAula();
      history.push(`${RotasDTO.CADASTRO_DE_AVALIACAO}/editar/${id}`);
    }
  };

  const irParaFrequencia = (disciplinaId, dia) => {
    store.dispatch(salvarDadosAulaFrequencia(disciplinaId, dia));
    history.push(`${RotasDTO.FREQUENCIA_PLANO_AULA}`);
  };

  return (
    estaAberto && (
      <Loader loading={carregandoDia} tip="">      
        <Div className="border-bottom border-top-0 h-100 p-md-1 p-3">
          {eventosDia &&
          eventosDia.eventosAulas &&
          eventosDia.eventosAulas.length > 0 ? (
            <Div className="list-group list-group-flush fade show px-0">
              {!dentroPeriodo ? <ForaPerido /> : ''}
              {eventosDia.eventosAulas.map(evento => {
                return (
                  <Div
                    key={shortid.generate()}
                    className="list-group-item list-group-item-action d-flex p-0"
                  >
                    <Evento
                      className="d-flex rounded w-100 py-2 px-3"
                      style={{ cursor: 'pointer' }}
                      onClick={() =>
                        aoClicarEvento(evento.id, evento.tipoEvento)
                      }
                    >
                      <Div
                        cols={
                          (evento.tipoEvento === TiposEventoAulaDTO.Aula &&
                            1) ||
                          (evento.tipoEvento === TiposEventoAulaDTO.CJ && 1) ||
                          2
                        }
                        className="px-1"
                      >
                        <Botao
                          id={shortid.generate()}
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
                      </Div>
                      <Div
                        cols={
                          evento.tipoEvento === TiposEventoAulaDTO.Aula ||
                          evento.tipoEvento === TiposEventoAulaDTO.CJ
                            ? evento.dadosAula &&
                              evento.dadosAula.atividade.length
                              ? 10 - evento.dadosAula.atividade.length * 2
                              : 10
                            : 11
                        }
                        className="align-self-center font-weight-bold position-relative"
                      >
                        <Div
                          className={`pl-2 stretched-link ${(evento.tipoEvento ===
                            TiposEventoAulaDTO.Aula ||
                            evento.tipoEvento === TiposEventoAulaDTO.CJ) &&
                            'descricao'}`}
                        >
                          {evento.tipoEvento !== TiposEventoAulaDTO.Aula &&
                            evento.tipoEvento !== TiposEventoAulaDTO.CJ &&
                            (evento.descricao ? evento.descricao : 'Evento')}
                          {(evento.tipoEvento === TiposEventoAulaDTO.Aula ||
                            evento.tipoEvento === TiposEventoAulaDTO.CJ) &&
                            evento.dadosAula &&
                            `${evento.dadosAula.turma} - ${evento.dadosAula.modalidade} - ${evento.dadosAula.tipo} - ${evento.dadosAula.unidadeEscolar} - ${evento.dadosAula.disciplina}`}
                        </Div>
                      </Div>
                    </Evento>
                    {evento.dadosAula &&
                    evento.dadosAula.permiteRegistroFrequencia &&
                    turmaSelecionada ? (
                      <Div className="pr-0 d-flex align-items-center px-2 p-x-md-3">
                        <Botao
                          id={shortid.generate()}
                          label="Frequência"
                          color={Colors.Roxo}
                          className="w-100 position-relative btn-sm zIndex"
                          onClick={() =>
                            irParaFrequencia(
                              evento.dadosAula.disciplinaId,
                              diaSelecionado
                            )
                          }
                          height="24px"
                          padding="0 1rem"
                          border
                        />
                      </Div>
                    ) : null}
                    {evento.dadosAula && evento.dadosAula.atividade.length ? (
                      <Grid
                        cols={2}
                        className="pr-0 d-flex align-items-center px-2 p-x-md-3"
                      >
                        <SelectComponent
                          lista={evento.dadosAula.atividade}
                          classNameContainer="w-100"
                          className="fonte-14"
                          onChange={aoClicarEditarAvaliacao}
                          valueOption="id"
                          valueText="nomeAvaliacao"
                          placeholder="Avaliação"
                          size="small"
                          border={Base.Roxo}
                          color={Base.Roxo}
                        />
                      </Grid>
                    ) : null}
                  </Div>
                );
              })}
            </Div>
          ) : (
            <SemEvento />
          )}
          {eventosDia && eventosDia.letivo && turmaSelecionada && (
            <BotoesAuxiliares
              temAula={
                eventosDia.eventosAulas.filter(
                  aula =>
                    aula.tipoEvento === TiposEventoAulaDTO.Aula ||
                    aula.tipoEvento === TiposEventoAulaDTO.CJ
                ).length
              }
              podeCadastrarAvaliacao={
                eventosDia.eventosAulas.filter(
                  aula =>
                    (aula.tipoEvento === TiposEventoAulaDTO.Aula ||
                      aula.tipoEvento === TiposEventoAulaDTO.CJ) &&
                    aula.dadosAula &&
                    aula.dadosAula.podeCadastrarAvaliacao
                ).length
              }
            />
          )}
        </Div>
      </Loader>
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
