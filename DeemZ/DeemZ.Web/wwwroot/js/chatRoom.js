class ChatRoom {
    #currentUserId;

    constructor(currentUserId) {
        this.#currentUserId = currentUserId;
    }

    getCurrentUserId() {
        return this.#currentUserId;
    }
}

const temp = document.getElementById('currentUserId');

let chatRoom = new ChatRoom(temp.innerText);

temp.remove();