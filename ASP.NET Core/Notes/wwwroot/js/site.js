$(document).ready(function() {
// Add notes...
    $('form').submit( function() {
        var title = $(this).children('input').val();
        if(title.replace(/\s/g, '')) {
            $.ajax({
                method: 'POST',
                url: '/notes',
                data: $(this).serialize(),
                success: (res) => {
                            $('p#error').html("");
                            $('div#notes').html(res);
                        }
            })
        }
        else  $('p#error').html("Please input a title!");
        $(this).children('input').val("");
        return false;
    })
// Delete notes..
    $('div#notes').on('click', 'b', function() {
        if (confirm("Confirm Delete?")) {
            var id = $(this).attr('id');
            $.ajax({
                method: 'GET',
                url: `/notes/${id}`,
                success: (res) => $('div#notes').html(res)
            })
        }
    })
// Update notes...
    var oldValue = "";
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
        var id = $(this).parent().attr('id');
        var data = $(this).parent('form').serialize();
        if($(this).val().replace(/\s/g, '')) {
            $.ajax({
                method: 'POST',
                url: `/notes/${id}`,
                data: data,
                success: (res) => $('div#notes').html(res)
            })
        }
        else  $(this).val(oldValue);
    })

// // Mouseover functions:
    $('div#notes').on('mouseenter', 'input, textarea', function() {
        if($(this).attr('id') != 'focused') $('#tip').fadeIn('slow');
    })

    $('div#notes').on('mouseleave', 'input, textarea', function() {
        $('#tip').hide();
    })

    $('div#notes').on('mousemove', 'input, textarea', function(e) {
        var top = e.pageY + 8;
        var left = e.pageX + 8;
        $('#tip').css({'top' : `${top}px`, 'left' : `${left}px`});
        $(this).focus(function() {
             $('#tip').hide()
        })
    })
})

