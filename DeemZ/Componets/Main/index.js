const userIteration = document.getElementById('user-iteration'),
    askQuestionInput = document.getElementById('ask-question-input');

askQuestionInput.addEventListener('focusin', (e) =>{
    e.currentTarget.style.height = '60px';
    userIteration.style.height = '90px';
});

askQuestionInput.addEventListener('focusout', (e) =>{
    e.currentTarget.style.height = '45px';
    userIteration.style.height = '75px';
});