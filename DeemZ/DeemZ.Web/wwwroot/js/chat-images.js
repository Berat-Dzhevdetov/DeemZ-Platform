const messageInput = document.getElementById('messageInput');
const messageHolder = document.getElementById('message-holder');
const inputImageHolder = document.getElementById('input-image-holder');
const images = document.querySelectorAll('.input-image');

messageInput.onpaste = function (event) {
    var items = (event.clipboardData || event.originalEvent.clipboardData).items;
    console.log(JSON.stringify(items));
    for (var index in items) {
        var item = items[index];
        if (item.kind === 'file') {
            var blob = item.getAsFile();
            var reader = new FileReader();

            addNewImage(reader);

            reader.readAsDataURL(blob);
        }
    }
}

function addNewImage(reader) {
    let newDiv = document.createElement('div');
    let newImage = document.createElement('img');
    let newI = document.createElement('i');

    newDiv.classList.add('input-image');
    newI.classList.add('clickable');
    newI.classList.add('fas');
    newI.classList.add('fa-times');

    reader.onload = function (event) {
        newImage.src = event.target.result;
    };

    newI.addEventListener('click', (e) => removeImage(e))

    newDiv.appendChild(newImage);
    newDiv.appendChild(newI);

    inputImageHolder.appendChild(newDiv);
}

function removeImage(e) {
    let divToRemove = e.currentTarget.parentNode;

    divToRemove.remove();
}