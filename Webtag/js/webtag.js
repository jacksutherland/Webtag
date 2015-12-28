var Dashboard = function()
{
    function closeAddLink()
    {
        $("#link-inputs input[type='text']").val("").removeClass("input-validation-error");
        $("#link-inputs").slideToggle();
        $("#link-url-validation").hide();
    }

    function saveLink()
    {
        var valid = true;
        if ($.trim($("#link-title").val()) == "")
        {
            valid = false;
            $("#link-title").addClass("input-validation-error");
        }
        if ($.trim($("#link-url").val()) == "")
        {
            valid = false;
            $("#link-url").addClass("input-validation-error");
        }
        else if (!isUrlValid($("#link-url").val()))
        {
            valid = false;
            $("#link-url").addClass("input-validation-error");
            $("#link-url-validation").show();
        }

        if(valid)
        {
            closeAddLink();
        }
    }

    $(".add-link").click(function()
    {
        $("#link-inputs").slideToggle();
    });

    $("#link-inputs input[type='text']").keyup(function (e)
    {
        $(this).removeClass("input-validation-error");
        $("#link-url-validation").hide();

        if (e.keyCode == 27)
        {
            closeAddLink();
        }
        else if (e.keyCode == 13)
        {
            saveLink();
        }
    });
    
    $("#cancel-link-button").click(function ()
    {
        closeAddLink();
    });

    $("#save-link-button").click(function ()
    {
        saveLink();
    });

    $(".link, .folder").each(function ()
    {
        var node = $(this);
        
        if(node.hasClass("link"))
        {
            // update each link with favicons
            var link = $(this).find("a");
            var faviconURL = link.attr('href').replace(/^(http:\/\/[^\/]+).*$/, '$1') + '/favicon.ico';
            console.log(faviconURL);
            $('<img src="' + faviconURL + '" />').load(function ()
            {
                link.find("i.fa").replaceWith($(this));
            });
        }
        else if (node.hasClass("folder"))
        {
            node.find(">a").click(function (e)
            {
                e.preventDefault();
                node.find(".links").slideToggle();
            });
        }

        node.next(".link-buttons").find(".delete-button").hover(
            function ()
            {
                node.addClass("strike-through");
            },
            function ()
            {
                node.removeClass("strike-through");
            }
        ).click(function ()
        {

        });
    });
}

function isUrlValid(url)
{
    return /^(https?|s?ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/i.test(url);
}
