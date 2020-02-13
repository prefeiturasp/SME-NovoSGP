import React, { lazy, useMemo } from 'react';
import PropTypes from 'prop-types';

// Componentes
import { PainelCollapse, LazyLoad } from '~/componentes';

function Resumos({ dados, ciclos, anos }) {
  const TabelaFrequencia = lazy(() => import('./componentes/TabelaFrequencia'));
  const TabelaResultados = lazy(() => import('./componentes/TabelaResultados'));
  const TabelaInformacoesEscolares = lazy(() =>
    import('./componentes/TabelaInformacoesEscolares')
  );
  const TabelaTotalEstudantes = lazy(() =>
    import('./componentes/TabelaTotalEstudantes')
  );

  const filtroFake = 'turma';

  const dadosTabelaFrequencia = useMemo(() => {
    const frequenciaDados = dados && dados.frequencia;
    const dadosFormatados = [];
    const mapa = { turma: 'anos', ciclos: 'ciclos' };

    if (frequenciaDados) {
      frequenciaDados.forEach(x => {
        let quantidade = {
          FrequenciaDescricao: x.frequenciaDescricao,
          TipoDado: 'Quantidade',
        };

        let porcentagem = {
          FrequenciaDescricao: x.frequenciaDescricao,
          TipoDado: 'Porcentagem',
        };

        x.linhas[0][mapa[filtroFake]].forEach((y, key) => {
          quantidade = {
            ...quantidade,
            key: String(key),
            Descricao: y.descricao,
            [y.chave]: y.quantidade,
            Total: y.totalQuantidade,
          };

          porcentagem = {
            ...porcentagem,
            key: String(key),
            Descricao: y.descricao,
            [y.chave]: `${Math.round(y.porcentagem, 2)}%`,
            Total: `${Math.round(y.totalPorcentagem, 2)}%`,
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
            <TabelaTotalEstudantes
              dados={dados.totalEstudantes}
              ciclos={ciclos}
              anos={anos}
            />
          </LazyLoad>
        </PainelCollapse.Painel>
      </PainelCollapse>
      <PainelCollapse>
        <PainelCollapse.Painel temBorda header="Informações Escolares">
          <LazyLoad>
            <TabelaInformacoesEscolares dados={dados.informacoesEscolares} />
          </LazyLoad>
        </PainelCollapse.Painel>
      </PainelCollapse>
      <PainelCollapse>
        <PainelCollapse.Painel temBorda header="Frequência">
          <LazyLoad>
            <TabelaFrequencia
              dados={dadosTabelaFrequencia}
              ciclos={ciclos}
              anos={anos}
            />
          </LazyLoad>
        </PainelCollapse.Painel>
      </PainelCollapse>
      <PainelCollapse>
        <PainelCollapse.Painel temBorda header="Resultados">
          <LazyLoad>
            <TabelaResultados
              dados={dados.resultados}
              ciclos={ciclos}
              anos={anos}
            />
          </LazyLoad>
        </PainelCollapse.Painel>
      </PainelCollapse>
    </>
  );
}

Resumos.propTypes = {
  dados: PropTypes.oneOfType([PropTypes.any]),
  ciclos: PropTypes.oneOfType([PropTypes.bool]),
  anos: PropTypes.oneOfType([PropTypes.bool]),
};

Resumos.defaultProps = {
  dados: [],
  ciclos: false,
  anos: false,
};

export default Resumos;
