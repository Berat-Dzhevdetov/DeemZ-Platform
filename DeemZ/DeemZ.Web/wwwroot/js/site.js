﻿const orangeColor = "#FFA000";
const regalBlueColor = "#234465";
document.querySelectorAll('li.info-item').forEach(li => {
    li.addEventListener('mouseover', e => {
        e.currentTarget.querySelector('span.arrow-right').style.background = regalBlueColor;
    });
    li.addEventListener('mouseout', e => {
        e.currentTarget.querySelector('span.arrow-right').style.background = orangeColor;
    });
})

document.querySelectorAll('div.flex.clickable').forEach(div => {
    div.addEventListener('click', function (e) {
        document.querySelectorAll('div.clicked-lecture').forEach(div => {
            let id = div.getAttribute('data-number');
            let currentTargetId = e.currentTarget.getAttribute('data-number');
            if (currentTargetId != id)
                div.click();
        })
        let div = e.currentTarget;

        div.parentNode.classList.toggle('clicked-lecture');
        div.querySelector('div.plus-close-holder').classList.toggle('close-rotate');
        div.querySelectorAll('.lines').forEach(line => {
            line.classList.toggle('white-background');
        });


        $(this).next(".lecture-description").first().slideToggle(300);
    })
})