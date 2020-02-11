import React, { lazy, useMemo } from 'react';
import PropTypes from 'prop-types';

// Componentes
import { PainelCollapse, LazyLoad } from '~/componentes';

function Resumos({ dados, filtros }) {
  const filtroFake = 'ciclos';

  const TabelaFrequencia = lazy(() => import('./componentes/TabelaFrequencia'));
  const TabelaTotalEstudantes = lazy(() =>
    import('./componentes/TabelaTotalEstudantes')
  );
  const TabelaResultados = lazy(() => import('./componentes/TabelaResultados'));

  const dadosTabelaFrequencia = useMemo(() => {
    const frequenciaDados = dados.Frequencia;
    const dadosFormatados = [];
    const mapa = { turma: 'Anos', ciclos: 'Ciclos' };

    if (frequenciaDados) {
      frequenciaDados.forEach(x => {
        let quantidade = {
          FrequenciaDescricao: x.FrequenciaDescricao,
          TipoDado: 'Quantidade',
        };

        let porcentagem = {
          FrequenciaDescricao: x.FrequenciaDescricao,
          TipoDado: 'Porcentagem',
        };

        x.Linhas[mapa[filtroFake]].forEach((y, key) => {
          quantidade = {
            ...quantidade,
            key: String(key),
            Descricao: y.Descricao,
            [y.Chave]: y.Quantidade,
            Total: y.TotalQuantidade,
          };

          porcentagem = {
            ...porcentagem,
            key: String(key),
            Descricao: y.Descricao,
            [y.Chave]: `${y.Porcentagem * 10}%`,
            Total: `${y.TotalPorcentagem * 10}%`,
          };
        });

        dadosFormatados.push(quantidade, porcentagem);
      });
    }

    return dadosFormatados;
  }, [dados]);

  return (
    <>
      <PainelCollapse>
        <PainelCollapse.Painel temBorda header="Total de estudantes">
          <LazyLoad>
            <TabelaTotalEstudantes filtros={filtros} />
          </LazyLoad>
        </PainelCollapse.Painel>
      </PainelCollapse>
      <PainelCollapse>
        <PainelCollapse.Painel temBorda header="Frequência">
          <LazyLoad>
            <TabelaFrequencia dados={dadosTabelaFrequencia} />
          </LazyLoad>
        </PainelCollapse.Painel>
      </PainelCollapse>
      <PainelCollapse>
        <PainelCollapse.Painel temBorda header="Resultados">
          <LazyLoad>
            <TabelaResultados filtros={filtros} />
          </LazyLoad>
        </PainelCollapse.Painel>
      </PainelCollapse>
    </>
  );
}

Resumos.propTypes = {
  dados: PropTypes.oneOfType([PropTypes.any]),
  filtros: PropTypes.oneOfType([PropTypes.any]),
};

Resumos.defaultProps = {
  dados: [],
  filtros: [],
};

export default Resumos;
