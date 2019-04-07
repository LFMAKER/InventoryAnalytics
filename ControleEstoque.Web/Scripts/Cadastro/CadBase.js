﻿//anti_forgery_token
function add_anti_forgery_token(data) {
    data.__RequestVerificationToken = $('[name=__RequestVerificationToken]').val();
    return data;
}


function formatar_mensagem_aviso(mensagens) {
    var ret = '';
    for (var i = 0; i < mensagens.length; i++) {
        ret += '<li>' + mensagens[i] + '</li>';
    }
    return '<ul>' + ret + '</ul>';
}



function abrir_form(dados) {
    set_dados_form(dados);

    var modal_cadastro = $('#modal_cadastro');
    $('#msg_mensagem_aviso').empty();
    $('#msg_aviso').hide();
    $('#msg_mensagem_aviso').hide();
    $('#msg_erro').hide();
    bootbox.dialog({
        title: 'Cadastro de ' + titulo_pagina,
        message: modal_cadastro
    })
    .on('shown.bs.modal', function () {
        modal_cadastro.show(0, function () {
            set_focus_form();
           
        });
    })
    .on('hidden.bs.modal', function () {
        modal_cadastro.hide().appendTo('body');
    });
}



function criar_linha_grid(dados) {
    var result = set_dados_grid(dados);
    var ret =
         '<tr data-id=' + dados.Id + '>' +
        result +
        '<td>' +
        '<a class="btn btn-primary btn-alterar" style="margin-right: 3px; color: white;" role="button" >Alterar</a>' +
        '<a class="btn btn-danger btn-excluir" role="button" style="margin-right: 3px; color: white;"> Excluir</a>' +
        '</td>' +
        '</tr>';
    return ret;
}


$(document).on('click', '#btn_incluir', function () {
    abrir_form(get_dados_inclusao());
})
.on('click', '.btn-alterar', function () {
    var btn = $(this),
        id = btn.closest('tr').attr('data-id'),
        url = url_alterar,
        param = { 'id': id };
    $.post(url, add_anti_forgery_token(param), function (response) {
        if (response) {
            abrir_form(response);
        }
    });
})
.on('click', '.btn-excluir', function () {
    var btn = $(this),
        tr = btn.closest('tr'),
        id = tr.attr('data-id'),
        url = url_exclusao,
        param = { 'id': id };
    bootbox.confirm({
        message: "Realmente deseja excluir o " + titulo_pagina + "?",
        buttons: {
            confirm: {
                label: 'Sim',
                className: 'btn-danger'
            },
            cancel: {
                label: 'Não',
                className: 'btn-success'
            }
        },
        callback: function (result) {
            if (result) {
                $.post(url, add_anti_forgery_token(param), function (response) {
                    if (response) {
                        tr.remove();
                    }
                });
            }
        }
    });
})



.on('click', '#btn_confirmar', function () {
    var btn = $(this),
        url = url_confirmar,
        param = get_dados_form();
    $.post(url, add_anti_forgery_token(param), function (response) {
        if (response.Resultado == "OK") {
            if (param.Id == 0) {
                param.Id = response.IdSalvo;
                var table = $('#grid_cadastro').find('tbody'),
                    linha = criar_linha_grid(param);
                table.append(linha);
            }
            else {
                var linha = $('#grid_cadastro').find('tr[data-id=' + param.Id + ']').find('td');
                preencher_linha_grid(param, linha);

            }
            $('#modal_cadastro').parents('.bootbox').modal('hide');
        }
        else if (response.Resultado == "ERRO") {
            $('#msg_aviso').hide();
            $('#msg_mensagem_aviso').hide();
            $('#msg_erro').show();
        }
        else if (response.Resultado == "AVISO") {
            $('#msg_mensagem_aviso').html(formatar_mensagem_aviso(response.Mensagens));
            $('#msg_aviso').show();
            $('#msg_mensagem_aviso').show();
            $('#msg_erro').hide();
        }
    });
})
.on('click', '.page-item', function () {
    var btn = $(this),
        tamPag = $('#ddl_tam_pag').val(),
        pagina = btn.text(),
        url = url_page_click,
        param = { 'pagina': pagina, 'tamPag': tamPag };
    //Realizando POST

    $.post(url, add_anti_forgery_token(param), function (response) {
        if (response) {
            var table = $('#grid_cadastro').find('tbody');
            table.empty();

            for (var i = 0; i < response.length; i++) {
                table.append(criar_linha_grid(response[i]));
            }

            btn.siblings().removeClass('active');
            btn.addClass('active');

        }

    });


})
.on('change', '#ddl_tam_pag', function () {
    var ddl = $(this),
        tamPag = ddl.val(),
        pagina = 1,
        url = url_ddl_tam_pag,
        param = { 'pagina': pagina, 'tamPag': tamPag };
    //Realizando POST

    $.post(url, add_anti_forgery_token(param), function (response) {
        if (response) {
            var table = $('#grid_cadastro').find('tbody');
            table.empty();

            for (var i = 0; i < response.length; i++) {
                table.append(criar_linha_grid(response[i]));
            }

            ddl.siblings().removeClass('active');
            ddl.addClass('active');

        }

    });
});
