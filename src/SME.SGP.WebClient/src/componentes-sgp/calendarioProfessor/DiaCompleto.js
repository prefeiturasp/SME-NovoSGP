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
const BotoesAuxiliaresEstilo = styled.div`
  display: flex;
  align-items: right;
  justify-content: flex-end;
  width: 100%;
  padding: 16px;
  padding-bottom: 0;
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
        if (filtros && Object.entries(filtros).length > 0) {
          setEventosDia([]);
          const {
            tipoCalendarioSelecionado = '',
            eventoSme = true,
            dreSelecionada = '',
            unidadeEscolarSelecionada = '',
            turmaSelecionada = '',
          } = filtros;
          if (tipoCalendarioSelecionado) {
            api
              .post('http://www.mocky.io/v2/5dd4449f2f000072c3d4fa4f', {
                dia: diaSelecionado.getDate(),
                Mes: mesAtual,
                tipoCalendarioId: tipoCalendarioSelecionado,
                EhEventoSME: eventoSme,
                dreId: dreSelecionada,
                ueId: unidadeEscolarSelecionada,
                turmaId: turmaSelecionada,
              })
              .then(resposta => {
                if (resposta.data) setEventosDia(resposta.data);
                else setEventosDia([]);
              })
              .catch(() => {
                setEventosDia([]);
              });
          } else setEventosDia([]);
        }
      } else setEventosDia([]);
    }
    return () => {
      estado = false;
    };
  }, [diaSelecionado]);

  const BotoesAuxiliares = () => {
    return (
      <BotoesAuxiliaresEstilo>
        <Button
          key={shortid.generate()}
          onClick={() =>
            history.push(
              `calendario-professor/cadastro-aula/novo/${props.filtros.tipoCalendarioSelecionado}`
            )
          }
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
    if (filtros && Object.entries(filtros).length > 0) {
      const {
        tipoCalendarioSelecionado = '',
        eventoSme = true,
        dreSelecionada = '',
        unidadeEscolarSelecionada = '',
        turmaSelecionada = '',
      } = filtros;

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
    }

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
        {eventosDia && eventosDia.length > 0 ? (
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
                    -1 && (
                    <Grid cols={1} className="pl-0">
                      <Botao
                        label={evento.dadosAula.horario}
                        color={Colors.CinzaBotao}
                        className="w-100"
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
                    <Div>
                      {TiposEventoAulaDTO.Evento.indexOf(evento.tipoEvento) >
                        -1 && evento.descricao
                        ? evento.descricao
                        : 'Evento'}
                      {TiposEventoAulaDTO.Evento.indexOf(evento.tipoEvento) ===
                        -1 &&
                        `${evento.dadosAula.turma} - ${evento.dadosAula.modalidade} - ${evento.dadosAula.tipo} - ${evento.dadosAula.unidadeEscolar} - ${evento.dadosAula.disciplina}`}
                    </Div>
                  </Grid>
                </Evento>
              );
            })}
            <Div className="">
              <BotoesAuxiliares />
            </Div>
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
