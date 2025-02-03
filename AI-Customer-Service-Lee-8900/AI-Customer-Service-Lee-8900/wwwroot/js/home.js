

function openPopup() {
    $('section.chat_window').show('fast');
    var chatTrack = $('section.chat_window .cont .chatting');
    chatTrack[0].scrollTop = chatTrack[0].scrollHeight;
}

function closePopup() {
    $('section.chat_window').hide('fast');
}

function sendMessage() {
    var messageBox = $('section.chat_window .cont .chat_ctrl_panel textarea');
    var messageTrack = $('section.chat_window .cont .chatting .msg_track');

    if (messageBox.val() != null && messageBox.val() != 'undefined' && messageBox.val().length > 0) {
        $(messageTrack).append('<div class="message_me"><div>' + messageBox.val() + '</div></div>');
    }

    var chatTrack = $('section.chat_window .cont .chatting');
    chatTrack[0].scrollTop = chatTrack[0].scrollHeight;

    $.ajax({
        url: '/api/',  // Replace with your actual API route
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(messageBox.val()), // Replace with actual value
        beforeSend: function () {
            $(messageTrack).append('<div class="message_other waiting"><div></div></div>');
        },
        success: function (response) {
            try {
                $('.message_other.waiting > div').html(response[0]);
                $('.message_other.waiting').removeClass('waiting');
                messageBox.removeAttr('disabled');
                messageBox.focus();
                chatTrack[0].scrollTop = chatTrack[0].scrollHeight;

            } catch (e) {
                alert('Something went wrong, please try again later');
            }

        },
        error: function (xhr, status, error) {
            alert('Something went wrong, please try again later');
            console.error("Error:", error);
        }
    });

    messageBox.val('');
    messageBox.attr('disabled', '');


}

$('section.chat_window .cont .chat_ctrl_panel textarea').on("keypress", function (event) {
    if (event.key === "Enter") {
        event.preventDefault();
        sendMessage();
    }
});