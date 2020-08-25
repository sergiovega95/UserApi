$.ajaxSetup({
    data: {
        __RequestVerificationToken: document.getElementsByName("__RequestVerificationToken")[0].value
    }
});

function IniciarSesion()
{
    if ($("#formInicioSesion").dxForm("instance").validate().isValid)
    {
        var formularioInicioSesion = $("#formInicioSesion").dxForm("instance").option("formData");

        $.ajax({
            method: "POST",         
            url: "?handler=login"            
        })
        .done(function (msg) 
        {
           
        });
    }
}