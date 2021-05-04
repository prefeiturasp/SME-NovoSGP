import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { Loader, SelectComponent } from '~/componentes';
import GraficoBarras from '~/componentes-sgp/Graficos/graficoBarras';
import { AbrangenciaServico, erros } from '~/servicos';
import ServicoDashboardFrequencia from '~/servicos/Paginas/Dashboard/ServicoDashboardFrequencia';

const GraficoQuantidadeJustificativasPorMotivo = props => {
  const {
    anoLetivo,
    dreId,
    ueId,
    modalidade,
    semestre,
    listaAnosEscolares,
    codigoUe,
    consideraHistorico,
  } = props;

  const [dadosGrafico, setDadosGrafico] = useState([]);
  const [exibirLoader, setExibirLoader] = useState(false);
  const [anoEscolarSelecionado, setAnoEscolarSelecionado] = useState();
  const [listaTurmas, setListaTurmas] = useState([]);
  const [turmaSelecionada, setTurmaSelecionada] = useState();

  const [carregandoTurma, setCarregandoTurma] = useState();

  const OPCAO_TODOS = '-99';

  const obterDadosGrafico = useCallback(async () => {
    setExibirLoader(true);
    const retorno = await ServicoDashboardFrequencia.obterQuantidadeJustificativasMotivo(
      anoLetivo,
      dreId === OPCAO_TODOS ? '' : dreId,
      ueId === OPCAO_TODOS ? '' : ueId,
      modalidade,
      semestre,
      turmaSelecionada
    )
      .catch(e => erros(e))
      .finally(() => setExibirLoader(false));

    if (retorno?.data?.length) {
      setDadosGrafico(retorno.data);
    } else {
      setDadosGrafico([]);
    }
  }, [anoLetivo, dreId, ueId, modalidade, semestre, turmaSelecionada]);

  useEffect(() => {
    if (anoLetivo && dreId && ueId) {
      obterDadosGrafico();
    } else {
      setDadosGrafico([]);
    }
  }, [anoLetivo, dreId, ueId, obterDadosGrafico]);

  useEffect(() => {
    if (listaAnosEscolares?.length) {
      if (listaAnosEscolares?.length === 1) {
        setAnoEscolarSelecionado(listaAnosEscolares[0].valor);
      } else {
        const temTodos = listaAnosEscolares.find(
          item => item.valor === OPCAO_TODOS
        );
        if (temTodos) {
          setAnoEscolarSelecionado(OPCAO_TODOS);
        }
      }
    }
  }, [listaAnosEscolares]);

  const onChangeAnoEscolar = valor => setAnoEscolarSelecionado(valor);

  const obterTurmas = useCallback(async () => {
    setCarregandoTurma(true);
    const resultado = await AbrangenciaServico.buscarTurmas(
      codigoUe,
      modalidade,
      '',
      anoLetivo,
      consideraHistorico
    );
    if (resultado?.data?.length) {
      setListaTurmas(resultado.data);
      if (resultado.data.length === 1) {
        setTurmaSelecionada(resultado.data[0].codigo);
      }

      if (resultado.data.length > 1) {
        resultado.data.unshift({ codigo: OPCAO_TODOS, nome: 'Todas' });
        setTurmaSelecionada(OPCAO_TODOS);
      }
    }
    setCarregandoTurma(false);
  }, [codigoUe, modalidade, anoLetivo, consideraHistorico]);

  useEffect(() => {
    if (modalidade && dreId && ueId && ueId !== OPCAO_TODOS) {
      obterTurmas();
    } else {
      setTurmaSelecionada();
      setListaTurmas([]);
    }
  }, [modalidade]);

  const onChangeTurma = valor => setTurmaSelecionada(valor);

  return (
    <>
      <div className="row">
        <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3 mb-2">
          {ueId && ueId !== OPCAO_TODOS ? (
            <Loader loading={carregandoTurma}>
              <SelectComponent
                id="turma"
                lista={listaTurmas}
                valueOption="codigo"
                valueText="nome"
                label="Turma"
                disabled={!modalidade || listaTurmas?.length === 1}
                valueSelect={turmaSelecionada}
                placeholder="Turma"
                onChange={onChangeTurma}
              />
            </Loader>
          ) : (
            <SelectComponent
              id="ano-escolar"
              lista={listaAnosEscolares}
              valueOption="valor"
              valueText="descricao"
              disabled={listaAnosEscolares?.length === 1}
              valueSelect={anoEscolarSelecionado}
              onChange={onChangeAnoEscolar}
              placeholder="Selecione o ano"
            />
          )}
        </div>
      </div>
      <Loader
        loading={exibirLoader}
        className={exibirLoader ? 'text-center' : ''}
      >
        {dadosGrafico?.length ? (
          <GraficoBarras data={dadosGrafico} />
        ) : !exibirLoader ? (
          'Sem dados'
        ) : (
          ''
        )}
      </Loader>
    </>
  );
};

GraficoQuantidadeJustificativasPorMotivo.propTypes = {
  anoLetivo: PropTypes.oneOfType(PropTypes.any),
  dreId: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  ueId: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  modalidade: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  semestre: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  listaAnosEscolares: PropTypes.oneOfType(PropTypes.array),
  codigoUe: PropTypes.string,
  consideraHistorico: PropTypes.bool,
};

GraficoQuantidadeJustificativasPorMotivo.defaultProps = {
  anoLetivo: null,
  dreId: null,
  ueId: null,
  modalidade: null,
  semestre: null,
  listaAnosEscolares: [],
  codigoUe: '',
  consideraHistorico: false,
};

export default GraficoQuantidadeJustificativasPorMotivo;
