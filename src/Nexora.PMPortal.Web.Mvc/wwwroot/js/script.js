function priceFormatter(value) {
    var returnString = '0';
    if (value) {
        return value.toLocaleString();
    }
    return returnString;
}

$(document).ready(function () {
    //$('[data-mask]').each(function () {
    //    $(this).mask($(this).attr("data-mask"));
    //});
    $(":input").inputmask();

});





//$("#test").inputmask("decimal", {
//    placeholder: "0",
//    digits: 2,
//    digitsOptional: false,
//    radixPoint: ",",
//    groupSeparator: ".",
//    autoGroup: true,
//    allowPlus: false,
//    allowMinus: false,
//    clearMaskOnLostFocus: false,
//    removeMaskOnSubmit: true,
//    autoUnmask: true,
//    onUnMask: function (maskedValue, unmaskedValue) {
//        var x = unmaskedValue.split(',');
//        return x[0].replace(/\./g, '') + '.' + x[1];
//    }
//});