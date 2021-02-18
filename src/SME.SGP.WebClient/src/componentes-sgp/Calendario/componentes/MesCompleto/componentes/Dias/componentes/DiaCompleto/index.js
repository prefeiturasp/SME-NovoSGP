import React, { useMemo, useCallback } from 'react';
import shortid from 'shortid';
import t from 'prop-types';

// Ant
import { Tooltip } from 'antd';

// Redux
import { store } from '~/redux';
import { salvarDadosAulaFrequencia } from '~/redux/modulos/calendarioProfessor/actions';
import { useSelector } from 'react-redux';

// Estilos
import { DiaCompletoWrapper, LinhaEvento, Pilula, Linha } from './styles';

// Componentes
import { Loader, Base } from '~/componentes';

// Componentes internos
import AlertaDentroPeriodo from './componentes/AlertaPeriodoEncerrado';
import LabelAulaEvento from './componentes/LabelAulaEvento';
import BotoesAuxiliares from './componentes/BotoesAuxiliares';
import SemEventos from './componentes/SemEventos';
import BotaoAvaliacoes from './componentes/BotaoAvaliacoes';
import BotaoFrequencia from './componentes/BotaoFrequencia';
import DataInicioFim from './componentes/DataInicioFim';

// Utils
import { valorNuloOuVazio } from '~/utils/funcoes/gerais';

// Serviços
import history from '~/servicos/history';

// DTOs
import RotasDTO from '~/dtos/rotasDto';
import { IconeDiaComPendencia } from '../../styles';
import tipoPendenciaAula from '~/dtos/tipoPendenciaAula';

function DiaCompleto({
  dia,
  eventos,
  carregandoDia,
  diasPermitidos,
  permissaoTela,
  tipoCalendarioId,
}) {
  const deveExibir = useMemo(
    () => dia && !!diasPermitidos.find(x => x.toString() === dia.toString()),
    [dia, diasPermitidos]
  );

  const dadosDia = useMemo(() => {
    return eventos.length > 0 && dia instanceof Date
      ? eventos.filter(diaAtual => diaAtual.dia === dia.getDate())[0]
      : null;
  }, [dia, eventos]);

  const usuario = useSelector(state => state.usuario);
  const { turmaSelecionada } = usuario;

  const onClickNovaAulaHandler = useCallback(
    diaSelecionado => {
      history.push(
        `${RotasDTO.CADASTRO_DE_AULA}/novo/${tipoCalendarioId}?diaAula=${diaSelecionado}`
      );
    },
    [tipoCalendarioId]
  );

  const onClickNovaAvaliacaoHandler = useCallback(diaSelecionado => {
    history.push(
      `${RotasDTO.CADASTRO_DE_AVALIACAO}/novo?diaAvaliacao=${diaSelecionado}`
    );
  }, []);

  const onClickFrequenciaHandler = useCallback(
    (disciplinaId, diaSelecionado, aulaId) => {
      store.dispatch(
        salvarDadosAulaFrequencia(disciplinaId, diaSelecionado, aulaId)
      );
      history.push(`${RotasDTO.FREQUENCIA_PLANO_AULA}`);
    },
    []
  );

  const onClickAula = useCallback(item => {
    if (item.ehAula)
      history.push(`${RotasDTO.CADASTRO_DE_AULA}/editar/${item.aulaId}`);
  }, []);

  const obterDescricoesPendencias = pendencias => {
    const obterDesc = id => {
      switch (id) {
        case tipoPendenciaAula.Frequencia:
          return 'Frequência não registrada';
        case tipoPendenciaAula.PlanoAula:
          return 'Plano de aula não registrado';
        case tipoPendenciaAula.DiarioBordo:
          return 'Diário de bordo não registrado';
        case tipoPendenciaAula.Avaliacao:
          return 'Avaliação sem notas/conceitos lançados';
        default:
          return '';
      }
    };
    const conteudo = pendencias.map(item => {
      return <p key={shortid.generate()}>{obterDesc(item)}</p>;
    });

    return (
      <div style={{ fontSize: '12px' }} className="mt-2">
        {conteudo}
      </div>
    );
  };

  return (
    <DiaCompletoWrapper className={`${deveExibir && `visivel`}`}>
      {deveExibir && (
        <Loader loading={carregandoDia} tip="Carregando...">
          <AlertaDentroPeriodo
            exibir={!!dadosDia?.dados?.mensagemPeriodoEncerrado}
          />
          {dadosDia?.dados?.eventosAulas.length > 0
            ? dadosDia.dados.eventosAulas.map(eventoAula => (
                <Linha key={shortid.generate()}>
                  <LinhaEvento
                    className={`${!eventoAula.ehAula && `evento`}`}
                    onClick={() => onClickAula(eventoAula)}
                  >
                    <div className="labelEventoAula">
                      <LabelAulaEvento dadosEvento={eventoAula} />
                    </div>
                    <div className="tituloEventoAula">
                      <div>
                        <Tooltip title={eventoAula.descricao}>
                          {eventoAula.titulo}
                        </Tooltip>
                      </div>
                      <div className="detalhesEvento">
                        {eventoAula.quantidade > 0 && (
                          <span>
                            Quantidade: <strong>{eventoAula.quantidade}</strong>
                          </span>
                        )}
                        {eventoAula.estaAguardandoAprovacao && (
                          <Pilula cor={Base.Branco} fundo={Base.AzulCalendario}>
                            Aguardando aprovação
                          </Pilula>
                        )}
                        {eventoAula.ehReposicao && (
                          <Pilula cor={Base.Branco} fundo={Base.RoxoClaro}>
                            Reposição
                          </Pilula>
                        )}
                        <DataInicioFim dadosAula={eventoAula} />
                      </div>
                    </div>
                    {eventoAula.pendencias && eventoAula.pendencias.length ? (
                      <div
                        className="icone-alerta"
                        style={{ display: 'flex', alignItems: 'center' }}
                      >
                        <Tooltip
                          title={obterDescricoesPendencias(
                            eventoAula.pendencias
                          )}
                        >
                          <IconeDiaComPendencia className="fas fa-exclamation-triangle" />
                        </Tooltip>
                      </div>
                    ) : (
                      ''
                    )}
                  </LinhaEvento>
                  <div className="botoesEventoAula">
                    {eventoAula?.ehAula && eventoAula?.mostrarBotaoFrequencia && (
                      <Tooltip title="Ir para frequência">
                        <BotaoFrequencia
                          onClickFrequencia={() =>
                            onClickFrequenciaHandler(
                              eventoAula.componenteCurricularId,
                              dia,
                              eventoAula.aulaId
                            )
                          }
                        />
                      </Tooltip>
                    )}
                    {eventoAula?.atividadesAvaliativas.length > 0 && (
                      <BotaoAvaliacoes
                        atividadesAvaliativas={eventoAula.atividadesAvaliativas}
                        permissaoTela={permissaoTela}
                      />
                    )}
                  </div>
                </Linha>
              ))
            : !carregandoDia && <SemEventos />}
          <BotoesAuxiliares
            temAula={
              dadosDia?.dados?.eventosAulas.length > 0 &&
              dadosDia.dados.eventosAulas.filter(evento => evento.ehAula)
                .length > 0
            }
            podeCadastrarAula={dadosDia?.dados?.podeCadastrarAula}
            podeCadastrarAvaliacao={
              dadosDia?.dados?.eventosAulas?.filter(
                evento => evento.ehAula && evento.podeCadastrarAvaliacao
              ).length > 0
              && turmaSelecionada.modalidade > 1
            }
            onClickNovaAula={() =>
              onClickNovaAulaHandler(window.moment(dia).format('YYYY-MM-DD'))
            }
            onClickNovaAvaliacao={() =>
              onClickNovaAvaliacaoHandler(
                window.moment(dia).format('YYYY-MM-DD')
              )
            }
            permissaoTela={permissaoTela}
            dentroPeriodo={valorNuloOuVazio(
              dadosDia?.dados?.mensagemPeriodoEncerrado
            )}
            desabilitado={carregandoDia}
          />
        </Loader>
      )}
    </DiaCompletoWrapper>
  );
}

DiaCompleto.propTypes = {
  dia: t.oneOfType([t.any]),
  eventos: t.oneOfType([t.any]),
  diasPermitidos: t.oneOfType([t.any]),
  carregandoDia: t.bool,
  permissaoTela: t.oneOfType([t.any]),
  tipoCalendarioId: t.oneOfType([t.any]),
};

DiaCompleto.defaultProps = {
  dia: {},
  eventos: {},
  diasPermitidos: [],
  carregandoDia: false,
  permissaoTela: {},
  tipoCalendarioId: null,
};

export default DiaCompleto;
