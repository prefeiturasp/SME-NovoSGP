import React, { useMemo, useState, useEffect } from 'react';
import t from 'prop-types';
import shortid from 'shortid';

// Componentes
import { BarraNavegacao, Graficos } from '~/componentes';
import EixoObjetivo from './componentes/EixoObjetivo';

// Estilos
import { Linha } from '~/componentes/EstilosGlobais';

// Funcões
import { removerCaracteresEspeciais } from '~/utils/funcoes/gerais';

function TabGraficos({ dados, periodo, ciclos }) {
  const [itemAtivo, setItemAtivo] = useState(null);
  const [cicloOuAno] = useState(ciclos ? 'ciclos' : 'anos');

  const objetoExistaNaLista = (objeto, lista) => {
    return lista.some(
      elemento => JSON.stringify(elemento) === JSON.stringify(objeto)
    );
  };

  const dadosTabelaFrequencia = useMemo(() => {
    const frequenciaDados = dados.frequencia;
    const dadosFormatados = [];

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

        x.linhas[0][cicloOuAno].forEach((y, key) => {
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
            [y.chave]: Math.round(y.porcentagem, 2),
            Total: Math.round(y.totalPorcentagem, 2),
          };
        });

        dadosFormatados.push(quantidade, porcentagem);
      });
    }

    return dadosFormatados;
  }, [cicloOuAno, dados.frequencia]);

  const dadosTabelaTotalEstudantes = useMemo(() => {
    const montaDados = [];
    const dadoQuantidade = {};
    dadoQuantidade.Id = shortid.generate();
    dadoQuantidade.TipoDado = 'Quantidade';
    dadoQuantidade.FrequenciaDescricao = 'Total';

    if (!dados.totalEstudantes[cicloOuAno]) return false;
    dados.totalEstudantes[cicloOuAno].forEach(ano => {
      dadoQuantidade[
        `${cicloOuAno === 'ciclos' ? ano.cicloDescricao : ano.anoDescricao}`
      ] = ano.quantidade;
    });
    dadoQuantidade.Total = dados.totalEstudantes.quantidadeTotal;

    montaDados.push(dadoQuantidade);

    const dadoPorcentagem = {};
    dadoPorcentagem.Id = shortid.generate();
    dadoPorcentagem.TipoDado = 'Porcentagem';
    dadoPorcentagem.FrequenciaDescricao = 'Total';

    dados.totalEstudantes[cicloOuAno].forEach(ano => {
      dadoPorcentagem[
        `${cicloOuAno === 'ciclos' ? ano.cicloDescricao : ano.anoDescricao}`
      ] = ano.porcentagem; //Math.round(ano.porcentagem, 2);
    });

    dadoPorcentagem.Total = dados.totalEstudantes.porcentagemTotal;
    montaDados.push(dadoPorcentagem);

    return montaDados;
  }, [cicloOuAno, dados.totalEstudantes]);

  const dadosTabelaResultados = useMemo(() => {
    const resultados = [];
    dados.resultados.items.forEach(resultado => {
      let objetivo = {
        eixoDescricao: resultado.eixoDescricao,
        dados: [],
      };

      resultado.objetivos.forEach(obj => {
        objetivo = {
          ...objetivo,
          objetivoDescricao: obj.objetivoDescricao,
          descricao: obj.objetivoDescricao,
        };

        const item = [];
        obj[cicloOuAno].forEach((atual, a) => {
          atual.respostas.forEach((resposta, r) => {
            if (
              !item.find(
                dado => dado.FrequenciaDescricao === resposta.respostaDescricao
              )
            ) {
              item.push({
                eixoDescricao: resultado.eixoDescricao,
                objetivoDescricao: obj.objetivoDescricao,
                descricao: obj.objetivoDescricao,
                FrequenciaDescricao:
                  cicloOuAno === 'ciclos'
                    ? removerCaracteresEspeciais(resposta.respostaDescricao)
                    : resposta.respostaDescricao,
                TipoDado: 'Quantidade',
              });

              item.push({
                eixoDescricao: resultado.eixoDescricao,
                objetivoDescricao: obj.objetivoDescricao,
                descricao: obj.objetivoDescricao,
                FrequenciaDescricao:
                  cicloOuAno === 'ciclos'
                    ? removerCaracteresEspeciais(resposta.respostaDescricao)
                    : resposta.respostaDescricao,
                TipoDado: 'Porcentagem',
              });
            }
          });
        });

        obj[cicloOuAno].forEach(atual => {
          atual.respostas.forEach(resposta => {
            item
              .filter(
                dado =>
                  dado.FrequenciaDescricao === resposta.respostaDescricao &&
                  dado.TipoDado === 'Quantidade'
              )
              .map(dado => {
                dado[
                  cicloOuAno === 'ciclos'
                    ? atual.cicloDescricao
                    : atual.anoDescricao
                ] = resposta.quantidade;
                return dado;
              });
          });
        });

        obj[cicloOuAno].forEach(atual => {
          atual.respostas.forEach(resposta => {
            item
              .filter(
                dado =>
                  dado.FrequenciaDescricao === resposta.respostaDescricao &&
                  dado.TipoDado === 'Porcentagem'
              )
              .map(dado => {
                dado[
                  cicloOuAno === 'ciclos'
                    ? atual.cicloDescricao
                    : atual.anoDescricao
                ] = resposta.porcentagem;
                return dado;
              });
          });
        });

        objetivo = {
          ...objetivo,
          id: shortid.generate(),
          dados: item,
        };

        resultados.push(objetivo);
      });
    });

    return resultados;
  }, [cicloOuAno, dados.resultados.items]);

  const dadosTabelaInformacoesEscolares = useMemo(() => {
    const resultados = [];
    dados.informacoesEscolares.forEach(resultado => {
      let objetivo = {
        eixoDescricao: resultado.eixoDescricao,
        dados: [],
      };

      resultado.objetivos.forEach(obj => {
        objetivo = {
          ...objetivo,
          objetivoDescricao: obj.objetivoDescricao,
          descricao: obj.objetivoDescricao,
        };

        const item = [];
        obj[cicloOuAno].forEach((atual, a) => {
          atual.respostas.forEach((resposta, r) => {
            if (
              !item.find(
                dado => dado.FrequenciaDescricao === resposta.respostaDescricao
              )
            ) {
              item.push({
                eixoDescricao: resultado.eixoDescricao,
                objetivoDescricao: obj.objetivoDescricao,
                descricao: obj.objetivoDescricao,
                FrequenciaDescricao:
                  cicloOuAno === 'ciclos'
                    ? removerCaracteresEspeciais(resposta.respostaDescricao)
                    : resposta.respostaDescricao,
                TipoDado: 'Quantidade',
              });

              item.push({
                eixoDescricao: resultado.eixoDescricao,
                objetivoDescricao: obj.objetivoDescricao,
                descricao: obj.objetivoDescricao,
                FrequenciaDescricao:
                  cicloOuAno === 'ciclos'
                    ? removerCaracteresEspeciais(resposta.respostaDescricao)
                    : resposta.respostaDescricao,
                TipoDado: 'Porcentagem',
              });
            }
          });
        });

        obj[cicloOuAno].forEach(atual => {
          atual.respostas.forEach(resposta => {
            item
              .filter(
                dado =>
                  dado.FrequenciaDescricao === resposta.respostaDescricao &&
                  dado.TipoDado === 'Quantidade'
              )
              .map(dado => {
                dado[
                  cicloOuAno === 'ciclos'
                    ? atual.cicloDescricao
                    : atual.anoDescricao
                ] = resposta.quantidade;
                return dado;
              });
          });
        });

        obj[cicloOuAno].forEach(atual => {
          atual.respostas.forEach(resposta => {
            item
              .filter(
                dado =>
                  dado.FrequenciaDescricao === resposta.respostaDescricao &&
                  dado.TipoDado === 'Porcentagem'
              )
              .map(dado => {
                dado[
                  cicloOuAno === 'ciclos'
                    ? atual.cicloDescricao
                    : atual.anoDescricao
                ] = resposta.porcentagem;
                return dado;
              });
          });
        });

        objetivo = {
          ...objetivo,
          id: shortid.generate(),
          dados: item,
        };

        resultados.push(objetivo);
      });
    });

    return resultados;
  }, [cicloOuAno, dados.informacoesEscolares]);

  const [objetivos, setObjetivos] = useState([]);

  useEffect(() => {
    if (periodo === '1') {
      if (dadosTabelaTotalEstudantes.length > 0) {
        setObjetivos(atual => [
          ...atual,
          {
            id: shortid.generate(),
            eixoDescricao: 'Total',
            objetivoDescricao: 'Total de alunos no PAP',
            descricao: 'Total de alunos no PAP',
            dados: dadosTabelaTotalEstudantes,
          },
        ]);
      }

      setObjetivos(atual => [
        ...atual,
        ...dadosTabelaInformacoesEscolares,
        ...dadosTabelaResultados,
      ]);
    } else {
      if (dadosTabelaTotalEstudantes.length > 0) {
        setObjetivos(atual => [
          ...atual,
          {
            id: shortid.generate(),
            eixoDescricao: 'Total',
            objetivoDescricao: 'Total de alunos no PAP',
            descricao: 'Total de alunos no PAP',
            dados: dadosTabelaTotalEstudantes,
          },
        ]);
      }
      setObjetivos(atual => [...atual, ...dadosTabelaResultados]);
    }
  }, [
    dadosTabelaFrequencia,
    dadosTabelaInformacoesEscolares,
    dadosTabelaResultados,
    dadosTabelaTotalEstudantes,
    periodo,
  ]);

  useEffect(() => {
    setItemAtivo(objetivos[0]);
  }, [objetivos]);

  return (
    <>
      <Linha style={{ marginBottom: '8px' }}>
        <BarraNavegacao
          itens={objetivos}
          itemAtivo={
            itemAtivo
              ? objetivos.filter(x => x.id === itemAtivo.id)[0]
              : objetivos[0]
          }
          onChangeItem={item => setItemAtivo(item)}
        />
      </Linha>
      <Linha style={{ marginBottom: '35px' }}>
        <EixoObjetivo
          eixo={itemAtivo && { descricao: itemAtivo.eixoDescricao }}
          objetivo={itemAtivo && { descricao: itemAtivo.objetivoDescricao }}
        />
      </Linha>
      <Linha style={{ marginBottom: '35px', textAlign: 'center' }}>
        {itemAtivo && itemAtivo.dados && (
          <>
            <h4>Quantidade</h4>
            <div style={{ height: 300 }}>
              <Graficos.Barras
                dados={itemAtivo.dados.filter(x => x.TipoDado === 'Quantidade')}
                indice="FrequenciaDescricao"
                chaves={Object.keys(itemAtivo.dados[0]).filter(
                  x =>
                    [
                      'TipoDado',
                      'FrequenciaDescricao',
                      'key',
                      'Descricao',
                      'Total',
                      'Id',
                      'eixoDescricao',
                      'objetivoDescricao',
                      'descricao',
                    ].indexOf(x) === -1
                )}
              />
            </div>
          </>
        )}
      </Linha>
      <Linha style={{ marginBottom: '35px', textAlign: 'center' }}>
        {itemAtivo && itemAtivo.dados && (
          <>
            <h4>Porcentagem</h4>
            <div style={{ height: 300 }}>
              <Graficos.Barras
                dados={
                  itemAtivo &&
                  itemAtivo.dados.filter(x => x.TipoDado === 'Porcentagem')
                }
                indice="FrequenciaDescricao"
                chaves={Object.keys(itemAtivo && itemAtivo.dados[0]).filter(
                  x =>
                    [
                      'TipoDado',
                      'FrequenciaDescricao',
                      'key',
                      'Descricao',
                      'Total',
                      'Id',
                      'eixoDescricao',
                      'objetivoDescricao',
                      'descricao',
                    ].indexOf(x) === -1
                )}
                porcentagem
              />
            </div>
          </>
        )}
      </Linha>
    </>
  );
}

TabGraficos.propTypes = {
  dados: t.oneOfType([t.any]),
  periodo: t.string,
};

TabGraficos.defaultProps = {
  dados: [],
  periodo: '',
};

export default TabGraficos;
