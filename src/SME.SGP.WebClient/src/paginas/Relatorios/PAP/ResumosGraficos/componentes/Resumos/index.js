import React, { lazy, useMemo, useState } from 'react';
import PropTypes from 'prop-types';
import { useSelector } from 'react-redux';

// Componentes
import {
  Button,
  Colors,
  LazyLoad,
  Loader,
  PainelCollapse,
} from '~/componentes';

import { erros, sucesso, ResumosGraficosPAPServico } from '~/servicos';

function Resumos({ dados, ciclos, anos, filtroTela, isEncaminhamento }) {
  const TabelaFrequencia = lazy(() => import('./componentes/TabelaFrequencia'));
  const TabelaResultados = lazy(() => import('./componentes/TabelaResultados'));
  const TabelaInformacoesEscolares = lazy(() =>
    import('./componentes/TabelaInformacoesEscolares')
  );
  const TabelaTotalEstudantes = lazy(() =>
    import('./componentes/TabelaTotalEstudantes')
  );

  const [imprimindo, setImprimindo] = useState(false);
  const { meusDados } = useSelector(state => state.usuario);

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

  const gerarResumosPap = async () => {
    setImprimindo(true);
    const payload = {
      ...filtroTela,
      usuarioNome: meusDados.nome,
      usuarioRf: meusDados.rf,
    };
    await ResumosGraficosPAPServico.gerarResumosPap(payload)
      .then(() => {
        sucesso(
          'Solicitação de geração do relatório gerada com sucesso. Em breve você receberá uma notificação com o resultado.'
        );
      })
      .finally(setImprimindo(false))
      .catch(e => erros(e));
  };

  return (
    <>
      <div className="mb-2 ml-1">
        <Loader loading={imprimindo} ignorarTip>
          <Button
            className="btn-imprimir"
            icon="print"
            color={Colors.Azul}
            border
            onClick={() => gerarResumosPap()}
            id="btn-imprimir-resumos-pap"
          />
        </Loader>
      </div>
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
          <PainelCollapse.Painel temBorda header="Informações escolares">
            <LazyLoad>
              <TabelaInformacoesEscolares
                dados={dados.informacoesEscolares}
                ciclos={ciclos}
                anos={anos}
              />
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
  filtroTela: PropTypes.instanceOf(Object),
};

Resumos.defaultProps = {
  dados: [],
  ciclos: false,
  anos: false,
  isEncaminhamento: false,
  filtroTela: {},
};

export default Resumos;
