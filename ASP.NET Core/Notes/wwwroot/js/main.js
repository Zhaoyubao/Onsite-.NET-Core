$(document).ready(function() {
// Success Function...
    function showNotes(notes) {
        let html_str = "";
        for(let note of notes) {
            html_str += `<div class='note'><form id='${note.id}' class='note'>`;
            html_str += `<input class='title' name='title' value='${note.title}'>`;
            html_str += `<p class='created'>Updated at: &nbsp;&nbsp;${note.created_at.substring(0,10)}, ${note.created_at.substring(11,16)}</p>`;
            html_str += `<textarea name='description' placeholder='Enter description here...'>${note.description}</textarea>`;
            html_str += `</form><b id='${note.id}'>Delete</b></div>`;
        }
        $('div#notes').html(html_str);
    }

// Add notes...
    $('form').submit(function() {
        let title = $(this).children('input').val();
        if(title.replace(/\s/g, '')) {
            $.ajax({
                method: 'POST',
                url: '/notes',
                data: $(this).serialize(),
                success: (res) => {
                            $('p#error').html("");
                            showNotes(res);
                        }
            })
        }
        else  $('p#error').html("Please input a title!");
        $(this).children('input').val("");
        return false;
    })
// Delete notes...
    $('div#notes').on('click', 'b', function() {
        if (confirm("Confirm Delete?")) {
            let id = $(this).attr('id');
            $.ajax({
                method: 'GET',
                url: `/notes/${id}`,
                success: (res) => showNotes(res)
            })
        }
    })
// Update notes...
    let oldValue = "";
    $('div#notes').on('focusin', 'input, textarea', function() {
        oldValue = $(this).val();
        $(this).attr("id","focused");
        if($(this).attr('name') == 'title')  $(this).css("border-bottom","1px solid silver");
    })
    $('div#notes').on('focusout', 'input, textarea', function() {
        $(this).attr("id","");
        if($(this).attr('name') == 'title')  $(this).css("border-bottom","");
    })

    $('div#notes').on('change', 'input, textarea', function() {
        let id = $(this).parent().attr('id');
        let data = $(this).parent('form').serialize();
        if($(this).val().replace(/\s/g, '')) {
            $.ajax({
                method: 'POST',
                url: `/notes/${id}`,
                data: data,
                success: (res) => showNotes(res)
            })
        }
        else  $(this).val(oldValue);
    })

// Mouseover functions:
    $('div#notes').on('mouseenter', 'input, textarea', function() {
        if($(this).attr('id') != 'focused') $('#tip').fadeIn('slow');
    })

    $('div#notes').on('mouseleave', 'input, textarea', function() {
        $('#tip').hide();
    })

    $('div#notes').on('mousemove', 'input, textarea', function(e) {
        let top = e.pageY + 8;
        let left = e.pageX + 8;
        $('#tip').css({'top' : `${top}px`, 'left' : `${left}px`});
        $(this).focus(function() {
             $('#tip').hide()
        })
    })
})

