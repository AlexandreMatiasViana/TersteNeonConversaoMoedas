// Write your Javascript code.
var erros;

$(document).ready(function() {

    $("#btnConverter").on("click", function () {

        if (!validarPreenchimentoValor())
            alert(erros);
        else
            converteMoeda();
    });

    populaDropDownMoedas();


});


validarPreenchimentoValor = function () {

    if ($("#valor").val() === "") {
        erros = "Favor preencher o valor";
        return false;
    }

    return true
};

converteMoeda = function() {


    var converterMoeda = {
        "MoedaOrigem": $("#mOrigem").val(),
        "MoedaDestino": $("#mDestino").val(),
        "Valor": $("#valor").val()
    }

    var dados = JSON.stringify(converterMoeda);
    // JSON.stringify({ "MoedaOrigem": "BRL", "MoedaDestino": "USD", "Valor": "10" })

    $.ajax({
        url: "http://localhost:59333/api/ConversaoMoedas/ConverterMoeda",
        type: "POST",
        data: JSON.stringify(converterMoeda),
        contentType: "application/json;charset=utf-8",
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
        },
        success: function (data) {

            if (data.sucesso) {
                var resposta = "";
                resposta += "O valor de ";
                resposta += data.objetoRetorno.valorInformado + "(" + data.objetoRetorno.moedaOrigem + ")";
                resposta += " da moeda ";
                resposta += $("#mOrigem option:selected").text();
                resposta += " convertida na moeda ";
                resposta += $("#mDestino  option:selected").text();
                resposta += " é de  ";
                resposta += data.objetoRetorno.valorConvertido + "(" + data.objetoRetorno.moedaDestino + ")";

                $("#resposta").text(resposta);
            }
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}

populaDropDownMoedas = function () {

    $.ajax({
        type: "GET",
        dataType: "json",
        url: "http://localhost:59333/api/ConversaoMoedas/ListagemMoedas",
        success: function (data) {
            
            if (data.sucesso) {

                var options = "";
                $("#mOrigem, #mDestino").empty();

                $.each(data.objetoRetorno, function() {
                    options += "<option value='" + this["key"] + "'>" + this["value"] +"</option>";
                });

                $("#mOrigem, #mDestino").append(options);
            }
        }
    });

};
