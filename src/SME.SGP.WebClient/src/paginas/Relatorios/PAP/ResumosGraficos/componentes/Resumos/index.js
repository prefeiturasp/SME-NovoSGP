import React, { lazy, useMemo } from 'react';
import PropTypes from 'prop-types';

// Componentes
import { PainelCollapse, LazyLoad } from '~/componentes';

<<<<<<< HEAD
function Resumos({ dados, ciclos, anos, isEncaminhamento }) {
=======
function Resumos({ dados, ciclos, anos, periodo }) {
>>>>>>> c875485f8beb4d02250d1fc803f674b98831d10e
  const TabelaFrequencia = lazy(() => import('./componentes/TabelaFrequencia'));
  const TabelaResultados = lazy(() => import('./componentes/TabelaResultados'));
  const TabelaInformacoesEscolares = lazy(() =>
    import('./componentes/TabelaInformacoesEscolares')
  );
  const TabelaTotalEstudantes = lazy(() =>
    import('./componentes/TabelaTotalEstudantes')
  );

  const filtro = ciclos ? 'ciclos' : 'turma';

  const dadosTabelaFrequencia = useMemo(() => {
    const frequenciaDados = dados && dados.frequencia;
    const dadosFormatados = [];
    const mapa = { turma: 'anos', ciclos: 'ciclos' };

    if (frequenciaDados) {
      frequenciaDados.forEach(frequencia => {
        let quantidade = {
          FrequenciaDescricao: frequencia.frequenciaDescricao,
          TipoDado: 'Quantidade',
        };

        let porcentagem = {
          FrequenciaDescricao: frequencia.frequenciaDescricao,
          TipoDado: 'Porcentagem',
        };

        frequencia.linhas[0][mapa[filtro]].forEach((linha, indice) => {
          quantidade = {
            ...quantidade,
            indice: String(indice),
            Descricao: linha.descricao,
            [linha.chave]: linha.quantidade,
            Total: linha.totalQuantidade,
          };

          porcentagem = {
            ...porcentagem,
            indice: String(indice),
            Descricao: linha.descricao,
            [linha.chave]: `${Math.round(linha.porcentagem, 2)}%`,
            Total: `${Math.round(linha.totalPorcentagem, 2)}%`,
          };
        });

        dadosFormatados.push(quantidade, porcentagem);
      });
    }

    return dadosFormatados;
  }, [dados, filtro]);

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
      {isEncaminhamento ? (
        <PainelCollapse>
          <PainelCollapse.Painel temBorda header="Informações Escolares">
            <LazyLoad>
              <TabelaInformacoesEscolares dados={dados.informacoesEscolares} />
            </LazyLoad>
          </PainelCollapse.Painel>
        </PainelCollapse>
      ) : (
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
      )}
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
  isEncaminhamento: PropTypes.oneOfType([PropTypes.bool]),
};

Resumos.defaultProps = {
  dados: [],
  ciclos: false,
  anos: false,
  isEncaminhamento: false,
};

export default Resumos;
