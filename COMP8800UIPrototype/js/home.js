function openPopup() {
    $('section.chat_window').show('fast');

    setTimeout(function() {
        $('section.chat_window .cont .chatting .msg_track > .message_me').show('fast');
    }, 3000); // Waits an additional 3 seconds (6 seconds total)

    setTimeout(function() {
        $('section.chat_window .cont .chatting .msg_track > .message_other').show('fast');
    }, 6000); // Waits an additional 3 seconds (9 seconds total)
}