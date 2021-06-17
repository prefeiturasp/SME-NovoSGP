import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { Loader, SelectComponent } from '~/componentes';
import MontarGraficoBarras from '~/paginas/Dashboard/ComponentesDashboard/montarGraficoBarras';
import { erros } from '~/servicos';
import ServicoDashboardRegistroItinerancia from '~/servicos/Paginas/Dashboard/ServicoDashboardRegistroItinerancia';
import ServicoFuncionario from '~/servicos/Paginas/ServicoFuncionario';

const MontarDadosQuantidadeRegistrosPorObjetivo = props => {
  const { anoLetivo, dreId, ueId, mesSelecionado } = props;

  const OPCAO_TODOS = '-99';

  const [carregandoPAAIs, setCarregandoPAAIs] = useState(false);
  const [listaPAAIs, setListaPAAIs] = useState(false);
  const [servidorSelecionado, setServidorSelecionado] = useState();

  const obterFuncionariosPAAIs = useCallback(async () => {
    if (dreId !== OPCAO_TODOS) {
      setCarregandoPAAIs(true);
      const retorno = await ServicoFuncionario.obterFuncionariosPAAIs(dreId)
        .catch(e => erros(e))
        .finally(() => setCarregandoPAAIs(false));

      if (retorno?.data?.length) {
        const lista = retorno.data;

        if (retorno.data.length === 1) {
          setServidorSelecionado(lista[0].codigoRf);
        }

        if (retorno.data.length > 1) {
          lista.unshift({ nomeServidor: 'Todos', codigoRf: OPCAO_TODOS });
          setServidorSelecionado(OPCAO_TODOS);
        }

        setListaPAAIs(retorno.data);
      } else {
        setListaPAAIs([]);
        setServidorSelecionado();
      }
    } else {
      setListaPAAIs([]);
      setServidorSelecionado();
    }
  }, [dreId]);

  useEffect(() => {
    obterFuncionariosPAAIs();
  }, [obterFuncionariosPAAIs]);

  const onChange = rf => {
    setServidorSelecionado(rf);
  };

  return (
    <>
      {dreId !== OPCAO_TODOS ? (
        <div className="col-sm-12 col-md-6 col-lg-4 col-xl-4 mb-2">
          <Loader loading={carregandoPAAIs}>
            <SelectComponent
              id="servidor"
              lista={listaPAAIs}
              valueOption="codigoRf"
              valueText="nomeServidor"
              disabled={listaPAAIs?.length === 1}
              onChange={onChange}
              valueSelect={servidorSelecionado}
              placeholder="PAAIs"
              allowClear={false}
            />
          </Loader>
        </div>
      ) : (
        ''
      )}
      {(dreId !== OPCAO_TODOS && servidorSelecionado) ||
      dreId === OPCAO_TODOS ? (
        <MontarGraficoBarras
          anoLetivo={anoLetivo}
          dreId={dreId}
          ueId={ueId}
          mesSelecionado={mesSelecionado}
          rf={servidorSelecionado}
          nomeIndiceDesc="descricao"
          nomeValor="quantidade"
          ServicoObterValoresGrafico={
            ServicoDashboardRegistroItinerancia.obterQuantidadeRegistrosPorObjetivo
          }
          exibirLegenda
          showAxisBottom={false}
        />
      ) : (
        ''
      )}
    </>
  );
};

MontarDadosQuantidadeRegistrosPorObjetivo.propTypes = {
  anoLetivo: PropTypes.oneOfType(PropTypes.any),
  dreId: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  ueId: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  mesSelecionado: PropTypes.string,
};

MontarDadosQuantidadeRegistrosPorObjetivo.defaultProps = {
  anoLetivo: null,
  dreId: null,
  ueId: null,
  mesSelecionado: '',
};

export default MontarDadosQuantidadeRegistrosPorObjetivo;
