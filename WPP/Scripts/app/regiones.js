function CargarCantones() {
    var provincia = $("#Provincia").val();
    console.log('prov '+provincia);
    var path = "/Region/CargarCantones?idProvincia=" + provincia;
    $.ajax({
        type: 'POST',
        url: path,
        success: function (data) {
            populateDD('Canton', data, "Canton");
            $("#Distrito").empty();
        },
        error: function (xhr, textStatus, errorThrown) {
            $("#Canton").empty();
            $("#Distrito").empty();
        },
        traditional: true

    });

}

function CargarDistritos() {
    var canton = $("#Canton").val();
    console.log('cant '+canton);
    //alert(canton)
    var path = "/Region/CargarDistritos?idCanton=" + canton;
    $.ajax({
        type: 'POST',
        url: path,
        success: function (data) { populateDD('Distrito', data, "Distrito"); },
        error: function (xhr, textStatus, errorThrown) {
            $("#Distrito").empty();
        },
        traditional: true

    });
}

function populateDD(_idPopulate, data, tipo) {
    var dd = $("#" + _idPopulate);
    dd.empty();
    dd.append($('<option></option>').val("").html(""));
    for (var i = 0; i < data.length; i++) {

        dd.append($('<option></option>').val(data[i].Id).html(data[i].Nombre));

    }
}