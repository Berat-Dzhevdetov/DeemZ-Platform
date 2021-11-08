const userIteration = document.getElementById('user-iteration'),
    askQuestionInput = document.getElementById('ask-question-input'),
    buger = document.getElementById('burger'),
    modalContainer = document.getElementById('modal-container'),
    closeModal = document.getElementById('close-modal');

askQuestionInput.addEventListener('focusin', (e) => {
    e.currentTarget.style.height = '60px';
    userIteration.style.height = '90px';
});

askQuestionInput.addEventListener('focusout', (e) => {
    e.currentTarget.style.height = '45px';
    userIteration.style.height = '75px';
});

buger.addEventListener('click', () => {
    modalContainer.classList.add('show');
});

closeModal.addEventListener('click', () => {
    modalContainer.classList.remove('show');
})

window.addEventListener('keyup', (e) => {
    let pressedKey = e.key;
    switch (pressedKey) {
        case 'Escape':
            if(modalContainer.classList.contains('show'))
                modalContainer.classList.remove('show');
            break;
    }
})

function openReplies(e){
    document.getElementById('reply-modal-container').classList.add('show');
}

function closeModala(){
    document.getElementById('reply-modal-container').classList.remove('show');
}