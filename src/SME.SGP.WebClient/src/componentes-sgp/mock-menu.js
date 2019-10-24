
export const getMenu = () => {
  return (
    [
      {
        "codigo": 38,
        "descricao": "Notificação",
        "ehMenu": false,
        "icone": null,
        "menus": [
          {
            "codigo": 38,
            "descricao": "Notificação",
            "podeAlterar": true,
            "podeConsultar": true,
            "podeExcluir": false,
            "podeIncluir": false,
            "subMenus": []
          }
        ],
        "quantidadeMenus": 1
      },
      {
        "codigo": 1,
        "descricao": "Diário de Classe",
        "ehMenu": true,
        "icone": "fas fa-book-reader",
        "menus": [
          {
            "codigo": 17,
            "descricao": "Plano de aula/Frequência",
            "podeAlterar": true,
            "podeConsultar": true,
            "podeExcluir": true,
            "podeIncluir": true,
            "subMenus": []
          },
          {
            "codigo": 22,
            "descricao": "Notas",
            "podeAlterar": true,
            "podeConsultar": true,
            "podeExcluir": true,
            "podeIncluir": true,
            "subMenus": []
          },
          {
            "codigo": 9,
            "descricao": "Boletim",
            "podeAlterar": false,
            "podeConsultar": true,
            "podeExcluir": false,
            "podeIncluir": false,
            "subMenus": []
          },
          {
            "codigo": 1,
            "descricao": "Sondagem",
            "podeAlterar": true,
            "podeConsultar": true,
            "podeExcluir": true,
            "podeIncluir": true,
            "subMenus": []
          }
        ],
        "quantidadeMenus": 4
      },
      {
        "codigo": 5,
        "descricao": "Relatórios",
        "ehMenu": true,
        "icone": "fas fa-file-alt",
        "menus": [
          {
            "codigo": 14,
            "descricao": "Frequência",
            "podeAlterar": false,
            "podeConsultar": true,
            "podeExcluir": true,
            "podeIncluir": true,
            "subMenus": []
          },
          {
            "codigo": 46,
            "descricao": "Relatório Consulta",
            "podeAlterar": false,
            "podeConsultar": false,
            "podeExcluir": false,
            "podeIncluir": false,
            "subMenus": []
          },
          {
            "codigo": 5,
            "descricao": "Relatório de Sondagem",
            "podeAlterar": false,
            "podeConsultar": false,
            "podeExcluir": false,
            "podeIncluir": false,
            "subMenus": []
          }
        ],
        "quantidadeMenus": 3
      },
      {
        "codigo": 26,
        "descricao": "Planejamento",
        "ehMenu": true,
        "icone": "fas fa-list-alt",
        "menus": [
          {
            "codigo": 34,
            "descricao": "Plano de Ciclo",
            "url": "/planejamento/plano-ciclo",
            "podeAlterar": true,
            "podeConsultar": true,
            "podeExcluir": true,
            "podeIncluir": true,
            "subMenus": []
          },
          {
            "codigo": 26,
            "descricao": "Plano Anual",
            "url": "/planejamento/plano-anual",
            "podeAlterar": true,
            "podeConsultar": true,
            "podeExcluir": true,
            "podeIncluir": true,
            "subMenus": []
          }
        ],
        "quantidadeMenus": 2
      },
      {
        "codigo": 10,
        "descricao": "Calendário Escolar",
        "ehMenu": true,
        "icone": "fas fa-calendar-alt",
        "menus": [
          {
            "codigo": 10,
            "descricao": "Calendário Escolar",
            "podeAlterar": false,
            "podeConsultar": true,
            "podeExcluir": false,
            "podeIncluir": false,
            "subMenus": []
          }
        ],
        "quantidadeMenus": 1
      },
      {
        "codigo": 18,
        "descricao": "Gestão",
        "ehMenu": true,
        "icone": "fas fa-user-cog",
        "menus": [
          {
            "codigo": 18,
            "descricao": "Atribuição de CJ",
            "podeAlterar": true,
            "podeConsultar": true,
            "podeExcluir": true,
            "podeIncluir": true,
            "subMenus": []
          }
        ],
        "quantidadeMenus": 1
      },
      {
        "codigo": 47,
        "descricao": "Configurações",
        "ehMenu": true,
        "icone": "fas fa-book-reader",
        "menus": [
          {
            "codigo": 47,
            "descricao": "Usuários",
            "podeAlterar": false,
            "podeConsultar": false,
            "podeExcluir": false,
            "podeIncluir": false,
            "subMenus": [
              {
                "codigo": 47,
                "descricao": "Reiniciar Senha",
                "url": "/usuarios/reiniciar-senha",
                "podeAlterar": true,
                "podeConsultar": false,
                "podeExcluir": false,
                "podeIncluir": false,
                "subMenus": []
              }
            ]
          }
        ],
        "quantidadeMenus": 1
      }
    ])
}

