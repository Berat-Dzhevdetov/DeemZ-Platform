const orangeColor = "#FFA000";
const regalBlueColor = "#234465";
document.querySelectorAll('li.info-item').forEach(li => {
    li.addEventListener('mouseover', e => {
        e.currentTarget.querySelector('span.arrow-right').style.background = regalBlueColor;
    });
    li.addEventListener('mouseout', e => {
        e.currentTarget.querySelector('span.arrow-right').style.background = orangeColor;
    });
})

document.querySelectorAll('li.flex.clickable').forEach(li => {
    li.addEventListener('click', e => {
        document.querySelectorAll('li.clicked-lecture').forEach(li => {
            let id = li.getAttribute('data-number');
            let currentTargetId = e.currentTarget.getAttribute('data-number');
            if (currentTargetId != id)
                li.click();
        })
        e.currentTarget.classList.toggle('clicked-lecture');
        e.currentTarget.querySelector('div.plus-close-holder').classList.toggle('close-rotate');
        e.currentTarget.querySelectorAll('.lines').forEach(line => {
            line.classList.toggle('white-background');
        })
    })
})