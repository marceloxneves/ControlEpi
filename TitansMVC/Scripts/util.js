function mascara(o, f) {
	v_obj = o;
	v_fun = f;
	setTimeout("execmascara()", 1);
}

function execmascara() {
	v_obj.value = v_fun(v_obj.value);
}

function moeda(v) {
	v = v.replace(/\D/g, ""); // permite digitar apenas numero
	v = v.replace(/(\d{1})(\d{15})$/, "$1.$2"); // coloca ponto antes dos
	// ultimos digitos
	v = v.replace(/(\d{1})(\d{11})$/, "$1.$2"); // coloca ponto antes dos
	// ultimos 11 digitos
	v = v.replace(/(\d{1})(\d{8})$/, "$1.$2"); // coloca ponto antes dos
	// ultimos 8 digitos
	v = v.replace(/(\d{1})(\d{5})$/, "$1.$2"); // coloca ponto antes dos
	// ultimos 5 digitos
	v = v.replace(/(\d{1})(\d{1,2})$/, "$1,$2"); // coloca virgula antes dos
	// ultimos 2 digitos
	return v;
}

function porcentagem(v) {
	v = v.replace(/\D/g, ""); // permite digitar apenas numero
	v = v.replace(/(\d{1})(\d{1,1})$/, "$1,$2"); // coloca virgula antes dos
	// ultimos 2 digitos
	return v;
}

// Maiusculo
function strToUpper(texto) {
	// para usar onkeyup="strToUpper(this)"
	texto.value = texto.value.toUpperCase();
}

// minusculo
function strToLower(texto) {
	// para usar onkeyup="strToLowerCase(this)"
	texto.value = texto.value.toLowerCase();
}

function SomenteNumero(e) {
	var tecla = (window.event) ? event.keyCode : e.which;
	if ((tecla > 47 && tecla < 58))
		return true;
	else {
		if (tecla === 8 || tecla === 0)
			return true;
		else
			return false;
	}
}

function SomenteNumPonto(e) {
	var tecla = (window.event) ? event.keyCode : e.which;
	if ((tecla > 47 && tecla < 58))
		return true;
	else {
		switch (tecla) {
		case 0:
			return true;
		case 8:
			return true;
		case 46:
			return true;
		default:
			return false;
		}
	}
}

function SomenteNumVirgula(e) {
	var tecla = (window.event) ? event.keyCode : e.which;
	if ((tecla > 47 && tecla < 58))
		return true;
	else {
		switch (tecla) {
		case 0:
			return true;
		case 8:
			return true;
		case 44:
			return true;
		default:
			return false;
		}
	}
}

function SomenteNumVirgulaPonto(e) {
	var tecla = (window.event) ? event.keyCode : e.which;
	if ((tecla > 47 && tecla < 58))
		return true;
	else {
		switch (tecla) {
		case 0:
			return true;
		case 8:
			return true;
		case 44:
			return true;
		case 46:
			return true;
		default:
			return false;
		}
	}
}

function SomenteNumPontoTraco(e) {
	var tecla = (window.event) ? event.keyCode : e.which;
	if ((tecla > 47 && tecla < 58))
		return true;
	else {
		switch (tecla) {
		case 0:
			return true;
		case 8:
			return true;
		case 45:
			return true;
		case 46:
			return true;
		default:
			return false;
		}
	}
}

function phoneMaskHandler(event) {
    var target, phone, element;
    target = (event.currentTarget) ? event.currentTarget : event.srcElement;
    phone = target.value.replace(/\D/g, "");
    element = $(target);
    element.unmask();
    if (phone.length > 10) {
        element.mask("(99) 99999-999?9");
    } else {
        element.mask("(99) 9999-9999?9");
    }
}

function getMoney(el) {
    var money = el.replace(".", "");
    money = money.replace(",", ".");
    return parseFloat(money);
}

function formatarCpfSomenteNum(cpf) {
    var newCpf = cpf.replace(".", "");
    newCpf = newCpf.replace(".", "");
    newCpf = newCpf.replace("-", "");
    return newCpf;
}

function getDateTime() {
    var currentdate = new Date();
    var datetime = currentdate.getDate() + "/"
        + (currentdate.getMonth() + 1) + "/"
        + currentdate.getFullYear() + " "
        + currentdate.getHours() + ":"
        + currentdate.getMinutes() + ":"
        + currentdate.getSeconds();
    return datetime;
}

function getFormattedDate(date) {
    var formattedDate = new Date(date);
    var d = formattedDate.getDate();
    var m = formattedDate.getMonth();
    m += 1;  // JavaScript months are 0-11
    var y = formattedDate.getFullYear();

    return d + "/" + m + "/" + y;
}