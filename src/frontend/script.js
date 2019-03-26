function onTemplateChange() {
    var template = document.getElementById("template").value;
    var source = document.getElementById("source").value;
    console.log(template);
    console.log(source);

    console.log(JSON.stringify({ "json": source, "LiquidTemplate": template }));

    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            document.getElementById("result").innerHTML = this.responseText;
        }
    };

    xhttp.open("POST", "https://liquid-transformation-function.azurewebsites.net/api/LiquidTransformation?code=XMAJpZm/HmXTJGqSc9NLBpBFHz3forfrFRYlaUWdXz4BOzt1n6tFNQ==", true);
    xhttp.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    xhttp.send(JSON.stringify({ "json": source, "LiquidTemplate": template }));
}